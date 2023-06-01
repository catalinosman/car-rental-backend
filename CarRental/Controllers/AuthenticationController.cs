using CarRental.Auth;
using CarRental.Dto;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using AutoMapper;
using CarRental.Services;

namespace CarRental.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly Authentication _authentication;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private IConfiguration _configuration;

        public AuthenticationController(IMapper mapper, IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration; 
            _authentication = new Authentication(configuration);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _authentication.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = _mapper.Map<User>(request);

            user.UserRole = "Customer";
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            await _userService.CreateUser(user);

            return new JsonResult(user, options);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO request)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (!_authentication.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }

            string token = await Task.FromResult(_authentication.CreateToken(user));
            return Ok(new AuthToken(token));
        }
    }
}
