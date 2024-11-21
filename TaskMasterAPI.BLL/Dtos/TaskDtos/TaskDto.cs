using TaskMasterAPI.Models.Bases;
using TaskMasterAPI.Models.Bases.Enums;

namespace TaskMasterAPI.BLL.Dtos.TaskDtos;

public class TaskDto : Base
{
    public string Title { get; set; }
    public string Description { get; set; }
    public StatusType Status { get; set; }
    public DateTime ExpiredAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public PriorityType Priority { get; set; }
    public string? ClientId { get; set; }
    public string AuthorId { get; set; }
    public string? TesterId { get; set; }
    public LabelType Label { get; set; }
    public Guid CompanyId { get; set; }
    public Guid? HistoryTaskId { get; set; }
    public ICollection<CommentTaskDto> Comments { get; set; }
}