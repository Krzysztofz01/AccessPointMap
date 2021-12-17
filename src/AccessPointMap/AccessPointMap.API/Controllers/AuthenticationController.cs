using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using static AccessPointMap.Application.Authentication.Requests;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    public class AuthenticationController : CommandController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService) : base()
        {
            _authenticationService = authenticationService ??
                throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(V1.Login request)
        {
            return Ok(await _authenticationService.Login(request));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(V1.Refresh request)
        {
            return Ok(await _authenticationService.Refresh(request));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(V1.Logout request)
        {
            await _authenticationService.Logout(request);

            return Ok();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> PasswordReset(V1.PasswordReset request)
        {
            await _authenticationService.PasswordReset(request);

            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(V1.Register request)
        {
            await _authenticationService.Register(request);

            return Ok();
        }
    }
}
