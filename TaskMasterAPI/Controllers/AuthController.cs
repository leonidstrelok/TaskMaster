using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMasterAPI.BLL.Dtos;
using TaskMasterAPI.BLL.Interfaces;
using TaskMasterAPI.BLL.Modules.Clients.Commands.AuthByLogin;
using TaskMasterAPI.BLL.Modules.Clients.Commands.RegistrationClient;

namespace TaskMasterAPI.Controllers;

[ApiController]
[Consumes("application/json")]
[Produces("application/json")]
[Route("api/v1/[controller]")]
[AllowAnonymous]
public class AuthController(IMediator mediator, IJwtAuthService jwtAuthService) : ControllerBase
{
    [HttpPost, Route("login")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login([FromBody] AuthByLoginCommand request)
    {
        var result = await mediator.Send(request);
        if (result.HasValue)
            return Ok(result.Value);

        return Unauthorized("Invalid login or password");
    }

    [HttpPost, Route("register")]
    public async Task<IActionResult> Registration(RegistrationClientCommand request)
    {
        var result = await mediator.Send(request);

        if (result)
        {
            return Ok();
        }

        return BadRequest();
    }

    [HttpPost, Route("refresh-token")]
    [ProducesResponseType(typeof(TokenDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshDto request)
    {
        try
        {
            var result = await jwtAuthService.AuthenticationByRefreshToken(request);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}