using Microsoft.AspNetCore.Identity;
using TaskMasterAPI.Models.Base;

namespace TaskMasterAPI.Models.Clients;

public class Client : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime DeletedAt { get; set; }
    public GenderType Gender { get; set; }
    public ICollection<Task>? Tasks { get; set; }
    public Guid? CompanyId { get; set; }
}