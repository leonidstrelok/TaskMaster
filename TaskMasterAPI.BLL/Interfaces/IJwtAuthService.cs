using CSharpFunctionalExtensions;
using TaskMasterAPI.BLL.Dtos;

namespace TaskMasterAPI.BLL.Interfaces;

public interface IJwtAuthService
{
    Task<Maybe<TokenDto>> AuthenticationByLogin(string userName, string password);
    Task<TokenDto> AuthenticationByRefreshToken(RefreshDto refresh);
}