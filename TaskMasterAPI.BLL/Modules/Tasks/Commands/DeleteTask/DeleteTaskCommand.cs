using MediatR;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.DeleteTask;

public class DeleteTaskCommand : IRequest
{
    public required Guid Id { get; set; }
}