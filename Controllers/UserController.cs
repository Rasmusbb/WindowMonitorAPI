using BoilerMonitoringAPI.Data;
using BoilerMonitoringAPI.DTOs;
using BoilerMonitoringAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using System.Security.Cryptography;
using System.Text;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BoilerMonitoringAPI.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class UserController : Controller
    {
        private readonly string DsetNull = "Entity set 'DatabaseContext.User'  is null.";
        private readonly BoilerMonitoringAPIContext _context;
        private readonly IConfiguration _configuration;
        public UserController(BoilerMonitoringAPIContext context, IConfiguration iConfig)
        {
            _context = context;
            _configuration = iConfig;
        }

        [HttpGet("Login")]
        public async Task<ActionResult<string>> Login(string Email, string Password)
        {
            if (_context.Users == null)
            {
                return Problem(DsetNull);
            }
            Email = Email.ToLower();
            User user = _context.Users.FirstOrDefault(u => u.Email == Email);
            if (user != null)
            {
                string hash = Hash(Password + user.UserID);
                if (hash == user.Password)
                {

                    return GenerateJwtToken(user);
                }
                else
                {
                    return NotFound(hash);
                }
            }
            return NotFound();
        }


        [HttpPost("AddUser")]
        public async Task<ActionResult<UserDTO>> AddUser(UserDTO userDTO)
        {
            if (_context.Users == null)
            {
                return Problem(DsetNull);
            }
            try
            {
                userDTO.Email = userDTO.Email.ToLower();
                User User = userDTO.Adapt<User>();
                _context.Users.Add(User);
                User.Password = Hash(userDTO.Password + User.UserID);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { id = User.UserID }, User);
            }
            catch
            {
                return BadRequest();
            }

        }

        [HttpGet("GetUsersHomes")]
        public async Task<ActionResult<List<Home>> GetUserHomes(Guid UserID)
        {
           List<Home> homes = _context.Homes.Where()
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<UserDTOID>> GetUser(Guid UserID)
        {
            if (_context.Homes == null)
            {
                return Problem(DsetNull);
            }
            User User = await _context.Users.FindAsync(UserID);
            UserDTOID UserDTOID = User.Adapt<UserDTOID>();
            return UserDTOID;
        }

        public static string Hash(string password)
        {

            StringBuilder builder = new StringBuilder();
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
            }
            return builder.ToString();
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = new Claim[]
            {
                new Claim("userID", user.UserID.ToString()),
                new Claim("Name", user.UserName.ToString()),
                new Claim("Email",user.Email.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: "BoilerMonitorAPI",
                audience: "BoilerMonitorFrontEnd",
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
