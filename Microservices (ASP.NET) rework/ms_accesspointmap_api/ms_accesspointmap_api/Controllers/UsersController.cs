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
            await userRepository.Delete(id);
            int rowsAffected = await userRepository.SaveChanges();

            if(rowsAffected < 1)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("user/activation")]
        public async Task<ActionResult> ActivateUsers(Activation activation)
        {
            await userRepository.Activate(activation.Id, activation.Activate);
            int rowsAffected = await userRepository.SaveChanges();

            if(rowsAffected < 1)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPut("user")]
        public async Task<ActionResult> PutUsers(Users user)
        {
            await userRepository.Update(user);
            int rowsAffected = await userRepository.SaveChanges();

            if(rowsAffected < 1)
            {
                return BadRequest();
            }
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LoginUsers(LoginForm loginForm)
        {
            var token = await userRepository.Login(loginForm.Email, loginForm.Password);

            if(token == null)
            {
                return Unauthorized();
            }
            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUsers(Users user)
        {
            bool created = await userRepository.Register(user);
            if(created)
            {
                int rowsAffected = await userRepository.SaveChanges();
                if(rowsAffected < 1)
                {
                    return BadRequest();
                }
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
