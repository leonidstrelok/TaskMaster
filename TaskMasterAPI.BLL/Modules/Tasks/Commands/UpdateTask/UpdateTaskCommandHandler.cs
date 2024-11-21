using AutoMapper;
using MediatR;
using TaskMasterAPI.BLL.Dtos.TaskDtos;
using TaskMasterAPI.DAL.Enums;
using TaskMasterAPI.DAL.Interfaces;
using Task = TaskMasterAPI.Models.Task;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.UpdateTask;

public class UpdateTaskCommandHandler(IMapper mapper, IApplicationDbContext dbContext)
    : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = mapper.Map<Task>(request);

        await dbContext.SaveChangesAsync(task, SaveChangeType.Update, cancellationToken: cancellationToken);

        return mapper.Map<TaskDto>(task);
    }
}