using CSharpFunctionalExtensions;
using MediatR;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.BLL.Interfaces;

namespace TaskMasterAPI.BLL.Modules.Clients.Commands.AuthByLogin;

public class AuthByLoginCommandHandler : IRequestHandler<AuthByLoginCommand, Maybe<TokenDto>>
{
    private readonly IJwtAuthService _jwtAuthService;

    public AuthByLoginCommandHandler(IJwtAuthService jwtAuthService)
    {
        _jwtAuthService = jwtAuthService;
    }

    public async Task<Maybe<TokenDto>> Handle(AuthByLoginCommand request, CancellationToken cancellationToken)
    {
        return await _jwtAuthService.AuthenticationByLogin(request.Login, request.Password);
    }
}