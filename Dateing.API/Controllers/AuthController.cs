using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dateing.API.Data;
using Dateing.API.DTO;
using Dateing.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace Dateing.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUser registerUser)
        {
            //vlidate request

            registerUser.Username = registerUser.Username.ToLower();

            if (await _repo.UserExist(registerUser.Username))
                return BadRequest("username already exist!");

            var newUser = _mapper.Map<User>(registerUser);

            var createdUser = await _repo.Register(newUser, registerUser.Password);
            var userToReturn = _mapper.Map<UserDetailModel>(createdUser);

            return CreatedAtRoute("GetUser", new { Controller = "User", id = createdUser.Id }, userToReturn);
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(Login loginUser)
        {
            var userFromRepo = await _repo.Login(loginUser.Username.ToLower(), loginUser.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserListModel>(userFromRepo);

            return Ok(new
            {
                Token = tokenHandler.WriteToken(token),
                user = user,
            });
        }
    }
}