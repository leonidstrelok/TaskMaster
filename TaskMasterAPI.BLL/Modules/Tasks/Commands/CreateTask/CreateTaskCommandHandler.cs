using AutoMapper;
using MediatR;
using TaskMasterAPI.BLL.Dtos.TaskDtos;
using TaskMasterAPI.DAL.Enums;
using TaskMasterAPI.DAL.Interfaces;
using Task = TaskMasterAPI.Models.Task;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.CreateTask;

public class CreateTaskCommandHandler(IMapper mapper, IApplicationDbContext dbContext)
    : IRequestHandler<CreateTaskCommand, TaskDto>
{
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = mapper.Map<Task>(request);

        await dbContext.SaveChangesAsync(task, SaveChangeType.Add, cancellationToken: cancellationToken);

        return mapper.Map<TaskDto>(task);
    }
}