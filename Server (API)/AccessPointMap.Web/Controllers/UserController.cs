using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccessPointMap.Service;
using AccessPointMap.Service.Handlers;
using AccessPointMap.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessPointMap.Web.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/user")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;
        private readonly IMapper mapper;

        public UserController(
            IUserService userService,
            ILogger<UserController> logger,
            IMapper mapper)
        {
            this.userService = userService ??
                throw new ArgumentNullException(nameof(userService));

            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<ActionResult<IEnumerable<UserGetView>>> GetAllV1()
        {
            try
            {
                var result = await userService.GetAll();

                var usersMapped = mapper.Map<IEnumerable<UserGetView>>(result.Result());

                return Ok(usersMapped);
            }
            catch (Exception e)
            {
                logger.LogError(e, "System failure on getting all users!");
                return Problem();
            }
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<ActionResult<UserGetView>> GetByIdV1(long userId)
        {
            try
            {
                var result = await userService.Get(userId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();
                if (result.Status() == ResultStatus.Sucess)
                {
                    var userMapped = mapper.Map<UserGetView>(result.Result());
                    return Ok(userMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, "System failure on getting user by id!");
                return Problem();
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByIdV1(long userId)
        {
            try
            {
                var result = await userService.Delete(userId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();
                if (result.Status() == ResultStatus.Sucess) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, "System failure on deleting user by id!");
                return Problem();
            }
        }

        [HttpPatch("{userId}/activation")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivationByIdV1(long userId)
        {
            try
            {
                var result = await userService.Activation(userId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();
                if (result.Status() == ResultStatus.NotPermited) return Forbid();
                if (result.Status() == ResultStatus.Sucess) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, "System failure on user activation by id!");
                return Problem();
            }
        }

        [HttpPatch("{userId}/promotion/moderator")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ModeratorPromotionByIdV1(long userId)
        {
            try
            {
                var result = await userService.ModeratorPromotion(userId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();
                if (result.Status() == ResultStatus.Sucess) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, "System failure on user moderator promotion by id!");
                return Problem();
            }
        }

        [HttpPatch("{userId}/promotion/administrator")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdministratorPromotionByIdV1(long userId)
        {
            try
            {
                var result = await userService.AdminPromotion(userId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();
                if (result.Status() == ResultStatus.Sucess) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, "System failure on user administrator promotion by id!");
                return Problem();
            }
        }
    }
}
