using Microsoft.AspNetCore.Mvc;
using LoginWebAPI.Models;
using LoginWebAPI.data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LoginWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public AuthController(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<UserModel>> Getall()
        {
            var allusers = await _db.users.ToListAsync();
            if (allusers == null)
            {
                return NotFound();
            }
            return Ok(allusers);
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserModel>> Register(UserDTO userDTO)
        {
            string passwordHash = 
                BCrypt.Net.BCrypt.HashPassword(userDTO.Password);

            UserModel user = new UserModel()
            {
                Username = userDTO.Username,
                PasswordHash = passwordHash
            };
            await _db.AddAsync(user);
            await _db.SaveChangesAsync();
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserModel>> Login(UserDTO userDTO)
        {
            var user = await _db.users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);
            if (user == null)
            {
                return NotFound("there is no such user");
            }
            if (!BCrypt.Net.BCrypt.Verify(userDTO.Password, user.PasswordHash))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);
            return Ok(token);

        }

        private string CreateToken(UserModel user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSetings:Token").Value!
                ));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
