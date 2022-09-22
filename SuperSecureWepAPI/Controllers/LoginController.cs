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
    public ActionResult<TokenDTO> Login([FromBody] LoginDto loginDto)
    {
        string token;
        if (_authenticationService.ValidateUser(loginDto.userName, loginDto.password, out token))
        {
            return Ok(new TokenDTO(true, "You rock", token));
        }
        else
        {
            return Ok(new TokenDTO(false, "Incorrect credentials", token));
        }
    }
}