using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Controllers
{
    [Route("projects/accesspointmap/api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogsRepository logsRepository;

        public UserController(
            IUserRepository userRepository,
            ILogsRepository logsRepository)
        {
            this.userRepository = userRepository;
            this.logsRepository = logsRepository;
        }

        [HttpGet("user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await userRepository.GetAll());
        }

        [HttpGet("user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> GetUsers(int id)
        {
            var user = await userRepository.GetById(id);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUsers(int id)
        {
            if (await userRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("user/activation")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ActivateUsers(UserActivationDto activation)
        {
            if (await userRepository.Activate(activation.Id, activation.Active))
            {
                await logsRepository.Create($"User with id: { activation.Id } activations status set to: { activation.Active }");
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("user")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> PutUsers(User user)
        {
            if (await userRepository.Update(user))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUsers(UserFormDto loginForm)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var loginStatus = await userRepository.Login(loginForm.Email, loginForm.Password, ipAddress);

            if (!loginStatus.Contains("ERROR:"))
            {
                await logsRepository.Create($"User: { loginForm.Email } logged from { ipAddress }");
                return Ok(new { bearerToken = loginStatus });
            }
            else
            {
                switch (loginStatus)
                {
                    case "ERROR:EMAIL": return NotFound();
                    case "ERROR:PASSWORD": return Forbid();
                    case "ERROR:ACTIVE": return Forbid();
                    default: return BadRequest();
                }
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUsers(UserFormDto userForm)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            if (await userRepository.Register(userForm, ipAddress))
            {
                await logsRepository.Create($"User: { userForm.Email } registered from { ipAddress }");
                return Ok();
            }
            return BadRequest();
        }
    }
}
