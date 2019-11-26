using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dateing.API.Data;
using Dateing.API.DTO;
using Dateing.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dateing.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UserController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var mappedUsers = _mapper.Map<IEnumerable<UserListModel>>(users);
            return Ok(mappedUsers);
        }
        [HttpGet("{id}" , Name="GetUser")]
        public async Task<IActionResult> GetUser(long id)
        {
            var user = await _repo.GetUser(id);
            if (user == null)
            {
                return BadRequest();
            }
            var mappedUser = _mapper.Map<UserDetailModel>(user);
            return Ok(mappedUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(long id, UserUpdateModel userUpdate)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var SelectedUser = await _repo.GetUser(id);
            _mapper.Map(userUpdate, SelectedUser);
            if (await _repo.SaveAll())
                return NoContent();
            throw new System.Exception("update user infomation failed.");
        }
    }
}