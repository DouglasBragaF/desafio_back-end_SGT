using GestaoTarefas.Application.Dtos;
using GestaoTarefas.Application.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
  private readonly JwtTokenService _jwtTokenService;

  public AuthController(JwtTokenService jwtTokenService)
  {
    _jwtTokenService = jwtTokenService;
  }

  [HttpPost("login")]
  [ProducesResponseType(200, Type = typeof(string))]
  [ProducesResponseType(401)]
  public IActionResult Login([FromBody] UserLoginDto request)
  {
    if (request.User == "user" && request.Password == "password")
    {
      var token = _jwtTokenService.GenerateToken(request.User);
      return Ok(new { token });
    }

    return Unauthorized();
  }
}
