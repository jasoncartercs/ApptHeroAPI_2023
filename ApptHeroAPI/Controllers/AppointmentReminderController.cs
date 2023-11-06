using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
//using ApptHeroAPI.Services.Implementation.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ApptHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentReminderController : ControllerBase
    {
        private readonly IAppointmentReminderService _appointmentReminderService;
        public AppointmentReminderController(IAppointmentReminderService appointmentReminderService)
        {
            _appointmentReminderService = appointmentReminderService;
        }


        [HttpGet]
        [Route("GetAppointmentRemindersState")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public void GetAppointmentRemindersState(long companyId)
        {

        }

        [HttpGet]
        [Route("GetAppointmentReminders")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAppointmentReminders(long companyId)
        {
            try
            {
                AllAppointmentReminderModels appointmentReminderModels = _appointmentReminderService.GetAppointmentReminders(companyId);

                if (appointmentReminderModels != null)
                {
                    return new OkObjectResult(new ApiResponseModel<AllAppointmentReminderModels>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = appointmentReminderModels
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
