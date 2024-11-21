using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Interfaces;

public interface IRoleMemberService
{
    Task<ICollection<string>> GetRole(Client? applicationUser);
    Task<ICollection<Client?>> GetUsersInRoleAsync(string roleName);
    Task GrantRole(string userId, string role);
    Task GrantRoleIfNotExists(string userId, string role);
    Task<bool> IsInRole(string userId, string role);
    Task RemoveFromRole(string userId, string role);
    Task RemoveFromRoleIfExists(string userId, string role);
}