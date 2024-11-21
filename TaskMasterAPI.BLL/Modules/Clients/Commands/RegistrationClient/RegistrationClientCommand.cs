using MediatR;
using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Bases.Enums;

namespace TaskMasterAPI.BLL.Modules.Clients.Commands.RegistrationClient;

public class RegistrationClientCommand : IRequest<bool>
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string ConfirmedPassword { get; set; }
    public required RoleType Role { get; set; }
}