using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Extensions;
using AccessPointMap.Application.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger) : base(logger)
        {
            _authenticationService = authenticationService ??
                throw new ArgumentNullException(nameof(authenticationService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Responses.V1.Login), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(V1.Login request)
        {
            _logger.LogAuthenticationRequest(request, Request.GetIpAddressString());

            return Ok(await _authenticationService.Login(request));
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Responses.V1.Refresh), StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh(V1.Refresh request)
        {
            _logger.LogAuthenticationRequest(request, Request.GetIpAddressString());

            var result = await _authenticationService.Refresh(request);

            // TODO: Result object will allow for better handling of such situations...
            return (result is not null)
                ? Ok(result)
                : Forbid();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(V1.Logout request)
        {
            _logger.LogAuthenticationRequest(request, Request.GetIpAddressString());

            await _authenticationService.Logout(request);

            return Ok();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> PasswordReset(V1.PasswordReset request)
        {
            _logger.LogAuthenticationRequest(request, Request.GetIpAddressString());

            await _authenticationService.PasswordReset(request);

            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(V1.Register request)
        {
            _logger.LogAuthenticationRequest(request, Request.GetIpAddressString());

            await _authenticationService.Register(request);

            return Ok();
        }
    }
}
