using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.Authentication;
using AccessPointMap.Application.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using static AccessPointMap.Application.Authentication.AuthenticationErrors;
using static AccessPointMap.Application.Authentication.Requests;

namespace AccessPointMap.API.Controllers
{
    [ProducesResponseType((int)HttpStatusCode.Forbidden)]
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
        public async Task<IActionResult> Login(
            V1.Login request,
            CancellationToken cancellationToken)
        {
            LogIncomingAuthenticationRequest(request);

            var result = await _authenticationService.Login(request, cancellationToken);
            if (result.IsFailure) return GetFailureResponse(result.Error);


            return Ok(result.Value);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(Responses.V1.Refresh), StatusCodes.Status200OK)]
        public async Task<IActionResult> Refresh(
            V1.Refresh request,
            CancellationToken cancellationToken)
        {
            LogIncomingAuthenticationRequest(request);

            var result = await _authenticationService.Refresh(request, cancellationToken);
            if (result.IsFailure) return GetFailureResponse(result.Error);

            return Ok(result.Value);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(
            V1.Logout request,
            CancellationToken cancellationToken)
        {
            LogIncomingAuthenticationRequest(request);

            var result = await _authenticationService.Logout(request, cancellationToken);
            if (result.IsFailure) return GetFailureResponse(result.Error);

            return Ok();
        }

        [HttpPost("reset")]
        public async Task<IActionResult> PasswordReset(
            V1.PasswordReset request,
            CancellationToken cancellationToken)
        {
            LogIncomingAuthenticationRequest(request);

            var result = await _authenticationService.PasswordReset(request, cancellationToken);
            if (result.IsFailure) return GetFailureResponse(result.Error);

            return Ok();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(
            V1.Register request,
            CancellationToken cancellationToken)
        {
            LogIncomingAuthenticationRequest(request);

            var result = await _authenticationService.Register(request, cancellationToken);
            if (result.IsFailure) return GetFailureResponse(result.Error);

            return Ok();
        }

        private IActionResult GetFailureResponse(Error error)
        {
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(HttpContext);

            problemDetails.Title = error.Message;
            problemDetails.Detail = null;

            if (error is AuthenticationCredentialsError)
            {
                problemDetails.Status = (int)HttpStatusCode.Forbidden;
                return StatusCode((int)HttpStatusCode.Forbidden, problemDetails);
            }

            if (error is AuthenticationIdentityError)
            {
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                return StatusCode((int)HttpStatusCode.BadRequest, problemDetails);
            }

            problemDetails.Status = (int)HttpStatusCode.BadRequest;
            return StatusCode((int)HttpStatusCode.BadRequest, problemDetails);
        }
    }
}
