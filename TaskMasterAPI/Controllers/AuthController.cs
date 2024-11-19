using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TaskMasterAPI.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [Route("login/{username}")]
    [HttpGet]
    public IActionResult Login(string username)
    {
        var claims = new List<Claim>() { new(ClaimTypes.Name, username) };
        var algorithm = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:Secret"]));
        var jwt = new JwtSecurityToken(_configuration["ApplicationSettings:Issuer"],
            _configuration["ApplicationSettings:Audience"],
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(algorithm,
                SecurityAlgorithms.HmacSha256));

        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }
}