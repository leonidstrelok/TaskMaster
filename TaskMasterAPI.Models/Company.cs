using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.Models;

public class Company : Bases.Base
{
    public string NameCompany { get; set; } = null!;
    public string? DescriptionCompany { get; set; }
    public CompanyIndustryType CompanyIndustry { get; set; }
    public int QuantityEmployees { get; set; }
    public ICollection<Task>? Tasks { get; set; }
    public ICollection<Client>? Clients { get; set; }
}