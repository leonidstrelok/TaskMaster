using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.DAL.Interfaces;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Modules.Clients.Commands.RegistrationClient;

public class RegistrationClientCommandValidators : AbstractValidator<RegistrationClientCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public RegistrationClientCommandValidators(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(p => p.Email)
            .NotNull()
            .EmailAddress()
            .MustAsync(UniqEmail);

        RuleFor(p => p.UserName)
            .NotNull()
            .NotEmpty()
            .MustAsync(UniqUserName);

        RuleFor(p => p)
            .Must(p => p.Password == p.ConfirmedPassword);
    }

    private async Task<bool> UniqEmail(string email, CancellationToken ct) =>
        await _dbContext.Set<Client>().AllAsync(p => p.Email != email, ct);

    private async Task<bool> UniqUserName(string userName, CancellationToken ct) =>
        await _dbContext.Set<Client>().AllAsync(p => p.UserName != userName, ct);
}