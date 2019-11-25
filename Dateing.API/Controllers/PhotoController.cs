using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dateing.API.Data;
using Dateing.API.DTO;
using Dateing.API.Helpers;
using Dateing.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dateing.API.Controllers
{
    [Route("users/{userId}/Photos")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySetting> _cloudineryConfig;
        private Cloudinary _cloudinary;

        public PhotoController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySetting> cloudineryConfig)
        {
            _cloudineryConfig = cloudineryConfig;
            _mapper = mapper;
            _repo = repo;
            Account acc = new Account(_cloudineryConfig.Value.CloudName, _cloudineryConfig.Value.ApiKey, _cloudineryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _repo.GetPhoto(id);
            var returnPhoto = _mapper.Map<ReturnPhotoModel>(photo);
            return Ok(returnPhoto);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhoto(int userId, [FromForm]CreatePhotoModel photoModel)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var SelectedUser = await _repo.GetUser(userId);

            var file = photoModel.File;

            var uploadResult = new ImageUploadResult();
            if (file != null && file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadparam = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _cloudinary.Upload(uploadparam);
                }
            }
            photoModel.Url = uploadResult.Uri.ToString();
            photoModel.PublicID = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoModel);
            if (!SelectedUser.Photos.Any(u => u.IsMain))
            {
                photo.IsMain = true;
            }
            SelectedUser.Photos.Add(photo);

            if (await _repo.SaveAll())
            {
                var photoToReturn = _mapper.Map<ReturnPhotoModel>(photo);
                return CreatedAtRoute("GetPhoto", new { userId = SelectedUser.Id, id = photo.Id }, photoToReturn);
            }
            return BadRequest("could not add the photo");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(long userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var SelectedUser = await _repo.GetUser(userId);

            if (!SelectedUser.Photos.Any(x => x.Id == id))
            {
                return Unauthorized();
            }
            var selectedPhoto = await _repo.GetPhoto(id);
            if (selectedPhoto.IsMain)
                return BadRequest("This is already the main photo.");
            var MainPhoto = await _repo.GetMainPhotoForUser(userId);
            MainPhoto.IsMain = false;
            selectedPhoto.IsMain = true;
            if (await _repo.SaveAll())
                return NoContent();

            return BadRequest("Cannot set main photo.");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(long userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var SelectedUser = await _repo.GetUser(userId);

            if (!SelectedUser.Photos.Any(x => x.Id == id))
            {
                return Unauthorized();
            }
            var deletingPhoto = await _repo.GetPhoto(id);
            if (deletingPhoto.IsMain)
                return BadRequest("Cannot delete your main photo.");

            var deleteParam = new DeletionParams(deletingPhoto.PublicID);
            var request = _cloudinary.Destroy(deleteParam);
            if (request.Result == "ok")
            {
                _repo.Delete(deletingPhoto);
                if (await _repo.SaveAll())
                {
                    return Ok();
                }
            }
            return BadRequest("Could not delete the photo.");
        }
    }
}