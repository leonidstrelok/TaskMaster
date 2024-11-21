using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.BLL.Dtos.TaskDtos;
using TaskMasterAPI.BLL.Helpers;
using TaskMasterAPI.DAL.Interfaces;
using Task = TaskMasterAPI.Models.Task;

namespace TaskMasterAPI.BLL.Modules.Tasks.Queries.GetAllTasks;

public class GetAllTaskQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetAllTaskQuery, IEnumerable<TaskDto>>
{
    public async Task<IEnumerable<TaskDto>> Handle(GetAllTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Set<Task>().AsNoTracking().PaginationAsync(request.PageNumber, request.Take);

        return mapper.Map<IEnumerable<TaskDto>>(task);
    }
}