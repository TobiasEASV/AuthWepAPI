using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperSecureWepAPI.DTOs;

namespace SuperSecureWepAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        return Ok();
    }
}