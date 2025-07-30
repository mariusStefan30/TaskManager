using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Models;
using Microsoft.AspNetCore.Authorization;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInMgr;
        private readonly UserManager<IdentityUser> _userMgr;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<IdentityUser> userMgr,
            SignInManager<IdentityUser> signInMgr,
            IConfiguration config
        )
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
            _config = config;
        }

        //Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userMgr.FindByNameAsync(dto.Username);
            if (user == null) return Unauthorized("User not found");

            var results = await _signInMgr.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!results.Succeeded) return Unauthorized("Invalid credentials");

            //Claims
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.UtcNow.AddMinutes(
                double.Parse(_config["Jwt:ExpiresInMinutes"]));


            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Aduience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                epiration = expires
            });

        }
    }
}

public record LoginDto(string Username, string Password);