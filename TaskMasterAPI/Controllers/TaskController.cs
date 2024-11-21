using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMasterAPI.BLL.Modules.Tasks.Queries.GetAllTasks;
using TaskMasterAPI.BLL.Modules.Tasks.Queries.GetTaskById;

namespace TaskMasterAPI.Controllers;

[Authorize]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class TaskController(IMediator mediator) : ControllerBase
{
    [HttpGet, Route("get-all")]
    public async Task<IActionResult> GetData(int pageNumber = 0, int take = 0)
    {
        var tasks = await mediator.Send(new GetAllTaskQuery(pageNumber, take));
        return Ok(tasks);
    }

    [HttpGet, Route("get-by-id/{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetTaskByIdQuery(id));
        if (result.HasValue)
            return Ok(result);
        return NotFound();
    }
}