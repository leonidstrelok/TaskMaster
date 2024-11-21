using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.BLL.Helpers;
using TaskMasterAPI.DAL.Interfaces;
using TaskMasterAPI.Models.Clients;

namespace TaskMasterAPI.BLL.Modules.Clients.Queries.GetAllUsers;

public class GetAllUserQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetAllUserQuery, IEnumerable<RegistrationDto>>
{
    public async Task<IEnumerable<RegistrationDto>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var users = await dbContext.Set<Client>().AsNoTracking().PaginationAsync(request.PageNumber, request.Take);
        return mapper.Map<IQueryable<RegistrationDto>>(users);
    }
}