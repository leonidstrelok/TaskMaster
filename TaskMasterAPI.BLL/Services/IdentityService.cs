using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using TaskMasterAPI.BLL.Exceptions;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.Models.Clients;
using ValidationException = TaskMasterAPI.BLL.Exceptions.ValidationException;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace TaskMasterAPI.BLL.Services;

public class IdentityService : IIdentityService
{
    private readonly UserManager<Client> _userManager;
    private readonly ILogger<IdentityService> _logger;

    public IdentityService(UserManager<Client> userManager, ILogger<IdentityService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task ChangeClientPasswordAsync(string clientId, string newPassword)
    {
        var client = await _userManager.FindByIdAsync(clientId);
        ThrowExceptionIfNull(clientId, client!);
        var result = await _userManager.RemovePasswordAsync(client!);
        ThrowExceptionIfNotSuccess(result);
        var setPasswordResult = await _userManager.AddPasswordAsync(client!, newPassword);
        ThrowExceptionIfNotSuccess(setPasswordResult);
    }

    public async Task<string> CreateUserAsync(Client client, string password, bool needChangePassword = true)
    {
        var result = await _userManager.CreateAsync(client, password);
        ThrowExceptionIfNotSuccess(result);

        return client.Id;
    }

    public async Task UpdateUserAsync(Client client)
    {
        var result = await _userManager.UpdateAsync(client);
        ThrowExceptionIfNotSuccess(result);
    }

    public async Task<ICollection<string>> GetRolesByClientIdAsync(string clientId)
    {
        var client = await _userManager.FindByIdAsync(clientId);
        return await _userManager.GetRolesAsync(client!);
    }

    public async Task<Client> GetClientByUserNameAsync(string userName)
    {
        return (await _userManager.FindByNameAsync(userName))!;
    }

    public async Task LockClientAsync(string clientId, DateTimeOffset? until)
    {
        var client = await _userManager.FindByIdAsync(clientId);
        ThrowExceptionIfNull(clientId, client!);
        var result = await _userManager.SetLockoutEnabledAsync(client!, true);
        ThrowExceptionIfNotSuccess(result);
        var lockResult = await _userManager.SetLockoutEndDateAsync(client!, until);
        ThrowExceptionIfNotSuccess(lockResult);
    }
    
    public async Task UnlockClientAsync(string clientId)
    {
        var client = await _userManager.FindByIdAsync(clientId);
        ThrowExceptionIfNull(clientId, client!);
        var result = await _userManager.SetLockoutEndDateAsync(client!, DateTimeOffset.Now.AddDays(-1));
        ThrowExceptionIfNotSuccess(result);
        _logger.LogInformation("{UserName} is unlocked", client!.UserName);
    }

    public async Task RemoveClient(string clientId)
    {
        var client = await _userManager.FindByIdAsync(clientId);
        var result = await _userManager.DeleteAsync(client!);
        ThrowExceptionIfNotSuccess(result);
    }

    private static void ThrowExceptionIfNull(string clientId, Client client)
    {
        if (client == null)
        {
            throw new NotFoundException(nameof(Client), clientId);
        }
    }

    public static void ThrowExceptionIfNotSuccess(IdentityResult identityResult)
    {
        if (!identityResult.Succeeded)
        {
            throw new ValidationException(
                identityResult.Errors.Select(prop => new ValidationFailure(prop.Code, prop.Description)));
        }
    }
}