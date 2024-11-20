using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Interfaces;

public interface IIdentityService
{
    Task ChangeClientPasswordAsync(string clientId, string newPassword);
    Task<string> CreateUserAsync(Client client, string password, bool needChangePassword = true);
    Task UpdateUserAsync(Client client);
    Task<ICollection<string>> GetRolesByClientIdAsync(string clientId);
    Task<Client> GetClientByUserNameAsync(string userName);
    Task LockClientAsync(string clientId, DateTimeOffset? until);
    Task RemoveClient(string clientId);
    Task UnlockClientAsync(string clientId);
}