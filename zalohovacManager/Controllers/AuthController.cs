using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using zalohovacManager.Database;
using zalohovacManager.DTOs;

namespace zalohovacManager.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private DatabaseContext _context;
        private IConfiguration _config;

        // Vytáhneme si databázi a konfiguraci (abychom měli přístup k tajnému heslu z appsettings)
        public AuthController(DatabaseContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // 1. Zkusíme najít uživatele v databázi
            var user = _context.Accounts.FirstOrDefault(a => a.Username == request.Username && a.Password == request.Password);

            // 2. Pokud neexistuje nebo má špatné heslo, vyhodíme chybu
            if (user == null)
            {
                return Unauthorized("Špatné přihlašovací jméno nebo heslo.");
            }

            // 3. Údaje sedí! Jdeme vyrobit JWT token.
            // Vezmeme náš super tajný klíč z appsettings.json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Nastavíme pravidla tokenu (kdo ho vydal, pro koho je a že platí 2 hodiny)
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            // 4. Pošleme hotový token zpátky uživateli
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = tokenString });
        }
    }
}