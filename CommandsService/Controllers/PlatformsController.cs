using CommandsService.Contracts.Platforms;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController(ICommandRespository commandRespository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllPlatforms()
    {
        var platforms = await commandRespository.GetAllPlatformAsync();

        var result = platforms.Adapt<IEnumerable<PlatformResponse>>();

        return Ok(result);
    }
}
