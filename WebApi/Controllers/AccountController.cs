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

        [HttpGet("Confirm-Email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token, CancellationToken cancellationToken)
        {
            var result = await _accountService.ConfirmEmail(userId, token);
            return Ok(result);
        }

        [HttpGet("Resend-Confirmation-Email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromQuery] string email, CancellationToken cancellationToken)
        {
            var result = await _accountService.ResendConfirmationEmailAsync(email);
            return Ok(result);
        }
    }
}
