using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.Models;

public class Command : Base
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ClosedAt { get; set; }
    public ICollection<Client>? Clients { get; set; }
}