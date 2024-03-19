using Microsoft.AspNetCore.Mvc;

namespace EventPlanner.Server.Features.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto user)
    {
        var response = await _service.Register(user);
        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);

    }


}