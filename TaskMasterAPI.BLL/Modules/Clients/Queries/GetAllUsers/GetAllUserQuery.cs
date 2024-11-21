using MediatR;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Modules.Clients.Queries.GetAllUsers;

public class GetAllUserQuery : IRequest<IEnumerable<RegistrationDto>>
{
    public int PageNumber { get; set; }
    public int Take { get; set; }
}