using AutoMapper;
using TaskMasterAPI.BLL.Dtos.TaskDtos;
using TaskMasterAPI.BLL.Modules.Tasks.Commands.CreateTask;
using Task = TaskMasterAPI.Models.Task;

namespace TaskMasterAPI.BLL.MappingProfiles;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<Task, CreateTaskCommand>();
        CreateMap<Task, TaskDto>()
            .ReverseMap();
    }
}