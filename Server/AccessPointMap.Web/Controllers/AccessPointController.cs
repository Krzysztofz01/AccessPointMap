using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessPointMap.Service;
using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using AccessPointMap.Web.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AccessPointMap.Web.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/accesspoint")]
    [ApiVersion("1.0")]
    public class AccessPointController : ControllerBase
    {
        private readonly IAccessPointService accessPointSerivce;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<AccessPointController> logger;

        public AccessPointController(
            IAccessPointService accessPointSerivce,
            IUserService userService,
            IMapper mapper,
            ILogger<AccessPointController> logger)
        {
            this.accessPointSerivce = accessPointSerivce ??
                throw new ArgumentNullException(nameof(accessPointSerivce));

            this.userService = userService ??
                throw new ArgumentNullException(nameof(userService));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            this.logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostManyV1(IEnumerable<AccessPointPostView> accessPoints)
        {
            try
            {
                var userId = userService.GetUserIdFromPayload(User.Claims);

                var accesspointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);

                var result = await accessPointSerivce.Add(accesspointsMapped, userId);

                if (result.Status() == ResultStatus.Sucess) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on posting accesspoints.");
                return Problem();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccessPointGetView>>> GetAllPublicV1()
        {
            try
            {
                var result = await accessPointSerivce.GetAllPublic();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointsMapped = mapper.Map<IEnumerable<AccessPointGetView>>(result.Result());

                    return Ok(accessPointsMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting all public accesspoints.");
                return Problem();
            }
        }

        [HttpGet("{accessPointId}")]
        public async Task<ActionResult<AccessPointGetView>> GetPublicByIdV1(long accessPointId)
        {
            try
            {
                var result = await accessPointSerivce.GetByIdPublic(accessPointId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointMapped = mapper.Map<AccessPointGetView>(result.Result());

                    return Ok(accessPointMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpDelete("{accessPointId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByIdV1(long accessPointId)
        {
            try
            {
                var result = await accessPointSerivce.Delete(accessPointId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess) return Ok();

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on deleting accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpPatch("{accessPointId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateByIdV1(AccessPointPatchView accessPoint, long accessPointId)
        {
            try
            {
                var accessPointMapped = mapper.Map<AccessPointDto>(accessPoint);

                var result = await accessPointSerivce.Update(accessPointId, accessPointMapped);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess) return Ok();

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on updating accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpPatch("{accessPointId}/display")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeDisplayByIdV1(long accessPointId)
        {
            try
            {
                var result = await accessPointSerivce.ChangeDisplay(accessPointId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess) return Ok();

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on changing display for accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpPatch("{accessPointId}/merge")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MergeByIdV1(long accessPointId)
        {
            try
            {
                var result = await accessPointSerivce.MergeById(accessPointId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess) return Ok();

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on merging accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpGet("master")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<ActionResult<IEnumerable<AccessPointGetView>>> GetAllMasterV1()
        {
            try
            {
                var result = await accessPointSerivce.GetAllMaster();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointsMapped = mapper.Map<IEnumerable<AccessPointGetView>>(result.Result());

                    return Ok(accessPointsMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting all master accesspoints.");
                return Problem();
            }
        }

        [HttpPost("master")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PostManyMasterV1(IEnumerable<AccessPointPostView> accessPoints)
        {
            try
            {
                var userId = userService.GetUserIdFromPayload(User.Claims);

                var accesspointsMapped = mapper.Map<IEnumerable<AccessPointDto>>(accessPoints);

                var result = await accessPointSerivce.AddMaster(accesspointsMapped, userId);

                if (result.Status() == ResultStatus.Sucess) return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on posting accesspoints to master group.");
                return Problem();
            }
        }

        [HttpGet("master/{accessPointId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<ActionResult<AccessPointGetView>> GetMasterByIdV1(long accessPointId)
        {
            try
            {
                var result = await accessPointSerivce.GetByIdMaster(accessPointId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointMapped = mapper.Map<AccessPointGetView>(result.Result());

                    return Ok(accessPointMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpGet("queue")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<ActionResult<IEnumerable<AccessPointGetView>>> GetAllQueueV1()
        {
            try
            {
                var result = await accessPointSerivce.GetAllQueue();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointsMapped = mapper.Map<IEnumerable<AccessPointGetView>>(result.Result());

                    return Ok(accessPointsMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting all queue accesspoints.");
                return Problem();
            }
        }

        [HttpGet("queue/{accessPointId}")]
        [Authorize(Roles = "Admin, Mod")]
        public async Task<ActionResult<AccessPointGetView>> GetQueueByIdV1(long accessPointId)
        {
            try
            {
                var result = await accessPointSerivce.GetByIdQueue(accessPointId);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointMapped = mapper.Map<AccessPointGetView>(result.Result());

                    return Ok(accessPointMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting accesspoint with id: {accessPointId}.");
                return Problem();
            }
        }

        [HttpGet("search/{ssid}")]
        public async Task<ActionResult<IEnumerable<AccessPointGetView>>> SearchBySsidV1(string ssid)
        {
            try
            {
                var result = await accessPointSerivce.SearchBySsid(ssid);

                if (result.Status() == ResultStatus.NotFound) return NotFound();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var accessPointsMapped = mapper.Map<IEnumerable<AccessPointGetView>>(result.Result());

                    return Ok(accessPointsMapped);
                }

                return BadRequest();
            }
            catch(Exception e)
            {
                logger.LogError(e, $"System failure on ssid search with phrase: {ssid}");
                return Problem();
            }
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<AccessPointStatisticsGetView>> GetStatsV1()
        {
            try
            {
                var result = await accessPointSerivce.GetStats();

                if (result.Status() == ResultStatus.Sucess)
                {
                    var statsMapped = mapper.Map<AccessPointStatisticsGetView>(result.Result());
                    return Ok(statsMapped);
                }

                return BadRequest();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"System failure on geting statistics!");
                return Problem();
            }
        }

        //TODO: Endpoints to get users posted and modified accesspoints
    }
}
