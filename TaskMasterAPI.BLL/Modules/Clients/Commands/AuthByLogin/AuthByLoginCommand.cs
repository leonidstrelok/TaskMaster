using CSharpFunctionalExtensions;
using MediatR;
using TaskMasterAPI.BLL.Dtos;

namespace TaskMasterAPI.BLL.Modules.Clients.Commands.AuthByLogin;

public class AuthByLoginCommand : IRequest<Maybe<TokenDto>>
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}