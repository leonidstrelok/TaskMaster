using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Bases.Enums;

namespace TaskMasterAPI.BLL.Dtos;

public class TokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ValidTo { get; set; }
    public string ClientId { get; set; }
    public ICollection<RoleType> Roles { get; set; }
}