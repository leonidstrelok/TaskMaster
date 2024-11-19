using TaskMasterAPI.BLL.Dtos;

namespace TaskMasterAPI.BLL.Interfaces;

public interface IJwtAuthService
{
    Task<TokenDto> AuthenticationByLogin(string userName, string password);
    Task<TokenDto> AuthenticationByRefreshToken(RefreshDto refresh);
}