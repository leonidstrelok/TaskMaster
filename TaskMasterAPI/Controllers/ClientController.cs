using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskMasterAPI.Controllers;

[Authorize]
[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
public class ClientController : ControllerBase
{
    
}