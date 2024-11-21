using MediatR;
using TaskMasterAPI.BLL.Dtos.TaskDtos;
using TaskMasterAPI.Models.Bases.Enums;

namespace TaskMasterAPI.BLL.Modules.Tasks.Commands.UpdateTask;

public class UpdateTaskCommand : IRequest<TaskDto>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public PriorityType Priority { get; set; }
    public string? ClientId { get; set; }
    public string? TesterId { get; set; }
    public LabelType Label { get; set; }
}