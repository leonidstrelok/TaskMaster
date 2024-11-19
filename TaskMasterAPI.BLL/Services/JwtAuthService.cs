using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.BLL.Exceptions;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.DAL.Interfaces;
using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Services;

public class JwtAuthService : IJwtAuthService
{
    private readonly UserManager<Client> _userManager;
    private readonly SignInManager<Client> _signInManager;
    private readonly IIdentityService _identityService;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _dbContext;

    public JwtAuthService(UserManager<Client> userManager, SignInManager<Client> signInManager,
        IIdentityService identityService, IConfiguration configuration, IApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _identityService = identityService;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<TokenDto> AuthenticationByLogin(string userName, string password)
    {
        var isExist = await IsExistsClient(userName, password, true);

        var user = await _identityService.GetClientByUserNameAsync(userName);
        if (user != null && isExist)
        {
            return await AuthenticationBase(user);
        }

        return null;
    }

    public async Task<TokenDto> AuthenticationByRefreshToken(RefreshDto refresh)
    {
        if (refresh is null)
            throw new ValidationException("Invalid client request");

        var accessToken = refresh.AccessToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal is null)
            throw new ValidationException("Invalid access token or refresh token"); ;

        var newAccessToken = CreateToken(principal.Claims.ToList());
        var newRefreshToken = GenerateRefreshToken();

        return new TokenDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = newRefreshToken,
            ValidTo = newAccessToken.ValidTo
        };
    }

    private async Task<bool> IsExistsClient(string userName, string password, bool rememberMe)
    {
        var result = await _signInManager.PasswordSignInAsync(userName, password, rememberMe, true);

        return result.Succeeded;
    }

    private async Task<TokenDto> AuthenticationBase(Client client)
    {
        var userRoles = await _userManager.GetRolesAsync(client);

        var authClaims = new List<Claim>
        {
            new("ClientId", client.Id),
            new(ClaimTypes.NameIdentifier, client.Id),
            new(ClaimTypes.Name, client.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var roles = new List<RoleType>();

        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            roles.Add(Enum.Parse<RoleType>(userRole));
        }

        if (!int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out var refreshTokenValidityInDays))
        {
            throw new ArgumentNullException();
        }

        var jwtSecurityToken = CreateToken(authClaims);
        var refreshToken = GenerateRefreshToken();

        return new TokenDto()
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
            RefreshToken = refreshToken,
            ValidTo = jwtSecurityToken.ValidTo,
            ClientId = client.Id,
            Roles = roles
        };
    }
    
    private JwtSecurityToken CreateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:Secret"]));

        if (!int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes))
        {
            throw new ArgumentNullException();
        }
        

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Audience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;

    }
}