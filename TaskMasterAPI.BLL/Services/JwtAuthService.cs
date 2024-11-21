using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.BLL.Exceptions;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Bases.Enums;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Services;

public class JwtAuthService(
    UserManager<Client> userManager,
    SignInManager<Client> signInManager,
    IIdentityService identityService,
    IConfiguration configuration)
    : IJwtAuthService
{
    public async Task<Maybe<TokenDto>> AuthenticationByLogin(string userName, string password)
    {
        var isExist = await IsExistsClient(userName, password, true);

        var user = await identityService.GetClientByUserNameAsync(userName);
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
        var result = await signInManager.PasswordSignInAsync(userName, password, rememberMe, true);

        return result.Succeeded;
    }

    private async Task<TokenDto> AuthenticationBase(Client client)
    {
        var userRoles = await userManager.GetRolesAsync(client);

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
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"]));

        if (!int.TryParse(configuration["ApplicationSettings:TokenValidityInMinutes"], out var tokenValidityInMinutes))
        {
            throw new ArgumentNullException();
        }
        

        var token = new JwtSecurityToken(
            issuer: configuration["ApplicationSettings:Issuer"],
            audience: configuration["ApplicationSettings:Audience"],
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["ApplicationSettings:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;

    }
}