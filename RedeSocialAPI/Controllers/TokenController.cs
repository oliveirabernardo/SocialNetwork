using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RedeSocialBLL.Models;
using RedeSocialBLL.Models.Identity;
using RedeSocialDAL;

namespace RedeSocialAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        public readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly ApplicationDbContext _context;

        public TokenController(UserManager<Usuario> userManager,
            SignInManager<Usuario> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
            _passwordHasher = passwordHasher;

        }

        private async Task<IActionResult> CriarToken(string UserEmail)
        {

            if (UserEmail != null)
            {

                if (UserEmail != "")
                {
                    var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim("Email", UserEmail)
                   };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Falhou");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(AuthenticateRequest userData)
        {
            var login = await _signInManager.PasswordSignInAsync(userData.Email, userData.Password, isPersistent: false, lockoutOnFailure: false);
            if (login.Succeeded)
            {
                return await CriarToken(userData.Email);
            }
            else
            {
                return BadRequest("Login Invalido");
            }
        }
    }
}
