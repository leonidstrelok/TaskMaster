using MediatR;
using TaskMasterAPI.BLL.Dtos.TaskDtos;

namespace TaskMasterAPI.BLL.Modules.Tasks.Queries.GetAllTasks;

public class GetAllTaskQuery(int pageNumber, int take) : IRequest<IEnumerable<TaskDto>>
{
    public int PageNumber { get; set; } = pageNumber;
    public int Take { get; set; } = take;
}