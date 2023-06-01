using AutoMapper;
using CarRental.Auth;
using CarRental.Dto;
using CarRental.Models;
using CarRental.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using CarRental.Auth;
using Microsoft.AspNetCore.Authorization;

namespace CarRental.Controllers
{
    public class UserController : ApiBaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly Authentication _authentication;

        public UserController(IUserService userService, IMapper mapper, IConfiguration configuration)
        {
            _userService = userService;
            _mapper = mapper;
            _configuration = configuration;
            _authentication = new Authentication(configuration);
        }

        [HttpGet("all"), Authorize( Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetUsers();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(users);
        }

        [HttpGet("{userId}"), Authorize]
        public async Task<IActionResult> GetUser(int userId) 
        { 
            var userExists = await _userService.UserExists(userId);
            if (!userExists) 
            {
                return NotFound();
            }

            var user = await _userService.GetUser(userId);
            var userDto = _mapper.Map<UserDto>(user);

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            return Ok(userDto);
        }

        [HttpPut("{userId}"), Authorize]
        public async Task<IActionResult> UpdateUser(int userId, UpdateUserDto updatedUser)
        {

            if (updatedUser == null)
            {
                return BadRequest(ModelState);
            }


            if (!await _userService.UserExists(userId))
            {
                return NotFound();
            }

            var userMap = _mapper.Map<UpdateUserDto>(updatedUser);
            
            if (!await _userService.UpdateUser(userMap, userId))
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }


        [HttpDelete("{userId}"), Authorize]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var userExists = await _userService.UserExists(userId);
            if (!userExists) 
            {
                return NotFound();
            }

            var userToDelete = await _userService.GetUser(userId);

            if(!await _userService.DeleteUser(userToDelete)) 
            {
                ModelState.AddModelError("", "Something went wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully removed");
        }

    }
}
