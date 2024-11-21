using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using TaskMasterAPI.BLL.Exceptions;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Services;

public class RoleMemberService(UserManager<Client?> userManager) : IRoleMemberService
{
    public async Task<ICollection<string>> GetRole(Client? applicationUser)
    {
        var roles = await userManager.GetRolesAsync(applicationUser);
        return roles;
    }

    public async Task<ICollection<Client?>> GetUsersInRoleAsync(string roleName)
    {
        return await userManager.GetUsersInRoleAsync(roleName);
    }

    public async Task GrantRole(string userId, string role)
    {
        var user = await GetUser(userId);
        var result = await userManager.AddToRoleAsync(user, role);
        ThrowExceptionIfNotSuccess(result);
    }

    public async Task GrantRoleIfNotExists(string userId, string role)
    {
        var isInRole = await IsInRole(userId, role);
        if (!isInRole)
        {
            await GrantRole(userId, role);
        }
    }

    public async Task<bool> IsInRole(string userId, string role)
    {
        var user = await GetUser(userId);
        return await userManager.IsInRoleAsync(user, role);
    }

    public async Task RemoveFromRole(string userId, string role)
    {
        var user = await GetUser(userId);
        var result = await userManager.RemoveFromRoleAsync(user, role);
        ThrowExceptionIfNotSuccess(result);
    }

    public async Task RemoveFromRoleIfExists(string userId, string role)
    {
        var isInRole = await IsInRole(userId, role);
        if (isInRole)
        {
            await RemoveFromRole(userId, role);
        }
    }

    private async Task<Client?> GetUser(string userId)
    {
        return await userManager.FindByIdAsync(userId);
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