using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskMasterAPI.BLL.Exceptions;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.Models.Clients;
using ValidationException = TaskMasterAPI.BLL.Exceptions.ValidationException;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace TaskMasterAPI.BLL.Services;

public class IdentityService(UserManager<Client> userManager, ILogger<IdentityService> logger)
    : IIdentityService
{
    public async Task ChangeClientPasswordAsync(string clientId, string newPassword)
    {
        var client = await userManager.FindByIdAsync(clientId);
        ThrowExceptionIfNull(clientId, client!);
        var result = await userManager.RemovePasswordAsync(client!);
        ThrowExceptionIfNotSuccess(result);
        var setPasswordResult = await userManager.AddPasswordAsync(client!, newPassword);
        ThrowExceptionIfNotSuccess(setPasswordResult);
    }

    public async Task<string> CreateUserAsync(Client client, string password, bool needChangePassword = true)
    {
        var result = await userManager.CreateAsync(client, password);
        ThrowExceptionIfNotSuccess(result);

        return client.Id;
    }

    public async Task UpdateUserAsync(Client client)
    {
        var result = await userManager.UpdateAsync(client);
        ThrowExceptionIfNotSuccess(result);
    }

    public async Task<ICollection<string>> GetRolesByClientIdAsync(string clientId)
    {
        var client = await userManager.FindByIdAsync(clientId);
        return await userManager.GetRolesAsync(client!);
    }

    public async Task<Client> GetClientByUserNameAsync(string userName)
    {
        return (await userManager.FindByNameAsync(userName))!;
    }

    public async Task LockClientAsync(string clientId, DateTimeOffset? until)
    {
        var client = await userManager.FindByIdAsync(clientId);
        ThrowExceptionIfNull(clientId, client!);
        var result = await userManager.SetLockoutEnabledAsync(client!, true);
        ThrowExceptionIfNotSuccess(result);
        var lockResult = await userManager.SetLockoutEndDateAsync(client!, until);
        ThrowExceptionIfNotSuccess(lockResult);
    }

    public async Task UnlockClientAsync(string clientId)
    {
        var client = await userManager.FindByIdAsync(clientId);
        ThrowExceptionIfNull(clientId, client!);
        var result = await userManager.SetLockoutEndDateAsync(client!, DateTimeOffset.Now.AddDays(-1));
        ThrowExceptionIfNotSuccess(result);
        logger.LogInformation("{UserName} is unlocked", client!.UserName);
    }

    public async Task AddToClaimAsync(string userId, string claimType, string claimValue,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        ThrowExceptionIfNull(userId, user);
        var claims = await userManager.GetClaimsAsync(user);
        if (claims.All(p => p.Type != claimType))
        {
            var result = await userManager.AddClaimAsync(user, new Claim(claimType, claimValue));
            ThrowExceptionIfNotSuccess(result);
        }
    }

    public async Task RemoveClient(string clientId)
    {
        var client = await userManager.FindByIdAsync(clientId);
        var result = await userManager.DeleteAsync(client!);
        ThrowExceptionIfNotSuccess(result);
    }

    private static void ThrowExceptionIfNull(string clientId, Client client)
    {
        if (client == null)
        {
            throw new NotFoundException(nameof(Client), clientId);
        }
    }

    private static void ThrowExceptionIfNotSuccess(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new ValidationException(
                identityResult.Errors.Select(prop => new ValidationFailure(prop.Code, prop.Description)));
        }
    }
}