using CSharpFunctionalExtensions;
using MediatR;
using TaskMasterAPI.BLL.Dtos.TaskDtos;

namespace TaskMasterAPI.BLL.Modules.Tasks.Queries.GetTaskById;

public class GetTaskByIdQuery(Guid id) : IRequest<Maybe<TaskDto>>
{
    public Guid Id { get; } = id;
}