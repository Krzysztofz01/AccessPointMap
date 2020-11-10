using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ms_accesspointmap_api.Models;
using ms_accesspointmap_api.Repositories;

namespace ms_accesspointmap_api.Controllers
{
    [Route("projects/accesspointmap/api/auth")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UsersController(
            IUserRepository userRepository,
            ILogsRepository logsRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return Ok(await userRepository.GetAll());
        }

        [HttpGet("user/{id}")]
        public async Task<ActionResult<Users>> GetUsers(int id)
        {
            var user = await userRepository.GetById(id);
            
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpDelete("user/{id}")]
        public async Task<ActionResult> DeleteUsers(int id)
        {
            if(await userRepository.Delete(id))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("user/activation")]
        public async Task<ActionResult> ActivateUsers(Activation activation)
        {
            if(await userRepository.Activate(activation.Id, activation.Activate))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPut("user")]
        public async Task<ActionResult> PutUsers(Users user)
        {
            if(await userRepository.Update(user))
            {
                return Ok();
            }
            return NotFound();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUsers(LoginForm loginForm)
        {
            var ipAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            var loginStatus = await userRepository.Login(loginForm.Email, loginForm.Password, ipAddress);

            if(!loginStatus.Contains("ERROR:"))
            {
                return Ok(new { bearerToken = loginStatus });
            }
            else
            {
                switch(loginStatus)
                {
                    case "ERROR:EMAIL": return NotFound(); break;
                    case "ERROR:PASSWORD": return Forbid(); break;
                    case "ERROR:ACTIVE": return Forbid(); break;
                    default: return BadRequest(); break;
                }
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUsers(Users user)
        {
            if(await userRepository.Register(user))
            {
                return Ok();
            }
            return BadRequest();
        }
    }

    public class Activation
    {
        public int Id { get; set; }
        public bool Activate { get; set; }
    }

    public class LoginForm
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
