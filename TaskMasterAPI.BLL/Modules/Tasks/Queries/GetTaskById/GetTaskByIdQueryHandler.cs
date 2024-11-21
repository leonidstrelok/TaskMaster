using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskMasterAPI.BLL.Dtos.TaskDtos;
using TaskMasterAPI.DAL.Interfaces;
using Task = TaskMasterAPI.Models.Task;

namespace TaskMasterAPI.BLL.Modules.Tasks.Queries.GetTaskById;

public class GetTaskByIdQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetTaskByIdQuery, Maybe<TaskDto>>
{
    public async Task<Maybe<TaskDto>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Set<Task>().FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        return mapper.Map<TaskDto>(task);
    }
}