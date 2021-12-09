using AccessPointMap.Application.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static AccessPointMap.Application.Authentication.Requests;

namespace AccessPointMap.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService ??
                throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        public async Task<IActionResult> Login(V1.Login request)
        {
            return Ok(await _authenticationService.Login(request));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(V1.Refresh request)
        {
            return Ok(await _authenticationService.Refresh(request));
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout(V1.Logout request)
        {
            await _authenticationService.Logout(request);

            return Ok();
        }

        [HttpPost("reset")]
        [Authorize]
        public async Task<IActionResult> PasswordReset(V1.PasswordReset request)
        {
            await _authenticationService.PasswordReset(request);

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(V1.Register request)
        {
            await _authenticationService.Register(request);

            return Ok();
        }
    }
}
