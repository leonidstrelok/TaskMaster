using AutoMapper;
using MediatR;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.DAL.Interfaces;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Modules.Clients.Commands.RegistrationClient;

public class RegistrationClientCommandHandler(
    IApplicationDbContext dbContext,
    IIdentityService identityService,
    IRoleMemberService roleMemberService,
    IMapper mapper)
    : IRequestHandler<RegistrationClientCommand, bool>
{
    public async Task<bool> Handle(RegistrationClientCommand request, CancellationToken cancellationToken)
    {
        const string claimType = "UserIdentifier";
        var isSuccessCreateClient = false;
        await dbContext.InvokeTransactionAsync(async () =>
        {
            var user = mapper.Map<Client>(request);
            var userId = await identityService.CreateUserAsync(user, request.Password);
            await roleMemberService.GrantRole(userId, request.Role.ToString());
            await identityService.AddToClaimAsync(userId, claimType, userId, cancellationToken);
            isSuccessCreateClient = true;
        }, cancellationToken);
        return isSuccessCreateClient;
    }
}