using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
    private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Authentication")]
        public async Task<IActionResult> Authentication(AuthenticationRequest authenticationModel, CancellationToken cancellationToken)
        {
            var result = await _accountService.Authenticate(authenticationModel);
            return Ok(result);
        }

        [HttpPost("RegisterUser")]
        public async Task<IActionResult> RegisterUser(RegisterRequest registerModel, CancellationToken cancellationToken)
        {
            var result = await _accountService.RegisterUser(registerModel);
            return Ok(result);
        }

    }
}
