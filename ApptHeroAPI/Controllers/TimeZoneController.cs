using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System;
using ApptHeroAPI.Services.Abstraction.Contracts;

namespace ApptHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeZoneController : ControllerBase
    {
        private readonly ITimeZoneService _timeZoneService;
        public TimeZoneController(ITimeZoneService timeZoneService)
        {
            _timeZoneService = timeZoneService;
        }

        [HttpGet]
        [Route("GetTimeZones")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTimeZones()
        {
            try
            {
                var timeZones = _timeZoneService.GetTimeZones();

                if (timeZones != null && timeZones.Count > 0)
                {
                    return new OkObjectResult(new ApiResponseModel<List<TimeZoneModel>>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = timeZones
                    });
                }
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetUSTimeZones")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUSTimeZones()
        {
            try
            {
                var timeZones = _timeZoneService.GetUSTimeZones();

                if (timeZones != null && timeZones.Count > 0)
                {
                    return new OkObjectResult(new ApiResponseModel<List<TimeZoneModel>>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = timeZones
                    });
                }
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
