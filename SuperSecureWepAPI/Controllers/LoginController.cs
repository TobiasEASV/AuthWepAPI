using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperSecureWepAPI.DTOs;
using SuperSecureWepAPI.Services;

namespace SuperSecureWepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        if (_authenticationService.ValidateUser(loginDto.userName, loginDto.password))
        {
            return Ok();
        }
        else
        {
            return BadRequest("Failed login attempt");
        }
    }
}