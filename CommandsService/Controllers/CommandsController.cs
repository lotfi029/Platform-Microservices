using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms/{platformId:guid}/[controller]")]
[ApiController]
public class CommandsController(ICommandRespository commandRespository) : ControllerBase
{
    [HttpPost("")]
    public async Task<ActionResult<Guid>> CreateCommand([FromRoute] Guid platformId, [FromBody] CreateCommandRequest command)
    {
        var exists = await commandRespository.PlatformExist(platformId);
        if (!exists.IsSuccess)
            return Ok(exists.Error);

        var commandRequest = new Command
        {
            HowTo = command.HowTo,
            CommandLine = command.CommandLine,
            PlatformId = platformId,
        };

        var result = await commandRespository.CreateCommand(platformId, commandRequest);
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        var commandResponse = result.Value.Adapt<CommandResponse>();

        return CreatedAtAction(nameof(GetCommand), new { platformId, commandId = result.Value }, commandResponse);
    }

    [HttpGet("{commandId:guid}")]
    public async Task<ActionResult<Command>> GetCommand([FromRoute] Guid platformId, [FromRoute] Guid commandId)
    {
        var result = await commandRespository.GetCommand(platformId, commandId);
        if (!result.IsSuccess)
            return NotFound(result.Error);

        var response = result.Value.Adapt<CommandResponse>();

        return Ok(response);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllCommandsByPlatformId([FromRoute] Guid platformId)
    {
        var exists = await commandRespository.PlatformExist(platformId);
        if (!exists.IsSuccess)
            return NotFound();

        var commands = await commandRespository.GetAllCommandsByPlatfromIdAsync(platformId);

        return Ok(commands.Adapt<IEnumerable<CommandResponse>>());
    }
}