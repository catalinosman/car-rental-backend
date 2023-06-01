using CarRental.Dto;
using CarRental.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarRental.Services
{
    public interface IUserService
    {
        Task<ICollection<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> GetUserByEmail(string email);
        Task<bool> UserExists(int id);
        Task<bool> CreateUser(User user);
        Task<bool> UpdateUser(UpdateUserDto user, int userId);
        Task<bool> DeleteUser(User user);
    }
}

