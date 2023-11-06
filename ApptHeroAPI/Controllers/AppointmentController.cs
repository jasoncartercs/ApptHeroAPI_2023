using ApptHeroAPI.Middleware;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Enum;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApptHeroAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ReusableActionFilter))]
    public class AppointmentController : ReusableBaseController
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService, IEnumerable<string> claimNames) : base(claimNames)
        {
            this._appointmentService = appointmentService;
        }

        [HttpGet]
        [Route("GetAppointments")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAppointments(int? teamMemberId, string startDate, string endDate)
        {
            try
            {
                DateTime sDate = Convert.ToDateTime(startDate);
                DateTime eDate = Convert.ToDateTime(endDate);

                var claims = GetUserClaims();

                if(claims.CompanyId == null || claims.UserId == null)
                {
                    return BadRequest(new { message = "Invalid company Id or userId claim." });
                }
                
                AppointmentList appointments = this._appointmentService.GetAppointments(claims.CompanyId.Value, claims.UserId.Value, teamMemberId, sDate, eDate);

                if (appointments != null)
                {
                    return new OkObjectResult(new ApiResponseModel<AppointmentList>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = appointments
                    });
                }
                return new StatusCodeResult((int)HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                throw;
                // return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("GetAppointmentbyId")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAppointmentbyId(long appointmentId)
        {
            try
            {
                var appointment = this._appointmentService.GetAppointment(appointmentId);

                if (appointment != null)
                {
                    return new OkObjectResult(new ApiResponseModel<AppointmentModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = appointment
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
        [Route("GetTeamMembers")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTeamMembers(int companyId, int clientId)
        {
            try
            {
                List<CalendarModel> models = this._appointmentService.GetTeamMembers(companyId, clientId);
                if (models.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<CalendarModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = models
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        [Route("GetTeamMembersByCompanyId")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetTeamMembers()
        {
            try
            {
                var claims = GetUserClaims();
                if (claims.CompanyId == null )
                {
                    return BadRequest(new { message = "Invalid claims." });
                }

                List<CalendarModel> models = this._appointmentService.GetTeamMembers(claims.CompanyId.Value);
                if (models.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<CalendarModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = models
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        [Route("GetUpgradesByAppointmentTypeId")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUpgradesByAppointmentTypeId(int appointmentTypeId)
        {
            try
            {
                List<AddOnModel> models = this._appointmentService.GetUpgradesByAppointmentTypeId(appointmentTypeId);
                if (models.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<AddOnModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = models
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetAppointmentTypes")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAppointmentTypes(long companyId, int pageNumber, int pageSize, string search)
        {
            try
            {
                ServicePaginatedModel paginatedModel = this._appointmentService.GetAppointmentTypes(companyId, pageNumber, pageSize, search);

                if (paginatedModel != null && paginatedModel.Services.Count > 0) return new OkObjectResult(new ApiResponseModel<ServicePaginatedModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = paginatedModel
                });
                return new NoContentResult();

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetUpgrades")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUpgrades(long companyId)
        {
            try
            {
                List<AddOnModel> models = this._appointmentService.GetAddons(companyId);
                if (models.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<AddOnModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = models
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("AddAppointment")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AddAppointment(AppointmentSaveModel model)
        {
            string message = string.Empty;
            try
            {
                AppointmentViewModel appointmentViewModel = this._appointmentService.AddAppointment(model, ref message);

                if (appointmentViewModel == null) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<AppointmentViewModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = appointmentViewModel
                });

                //if (isAdded) return new StatusCodeResult((int)HttpStatusCode.Created);
                //return new BadRequestObjectResult(new ApiResponseModel()
                //{
                //    Success = false,
                //    Message = "Something went wrong. Please check the values passed."
                //});
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = message + " exception : " + ex.Message
                }); ;
            }
        }

        [HttpPost]
        [Route("UpdateAppointment")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAppointment(AppointmentSaveModel model)
        {
            try
            {
                bool isAdded = this._appointmentService.UpdateAppointment(model);
                if (isAdded) return new StatusCodeResult((int)HttpStatusCode.Created);
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Something went wrong. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
                // return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("CancelAppointment")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelAppointment(int appointmntId, int cancelledById, bool sendMail, bool? isNoShow = null, string notes = null)
        {
            try
            {
                var response = this._appointmentService.CancelAppointment(appointmntId, cancelledById, sendMail, isNoShow, notes);
                if (response.Item1) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = response.Item2
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = response.Item2
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetPendingAppointments")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPendingAppointments(long companyId)
        {
            try
            {
                List<PendingAppointmentModel> models = this._appointmentService.GetPendingAppointments(companyId);
                if (models.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<PendingAppointmentModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = models
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetNumberOfPendingAppointments")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetNumberOfPendingAppointments(long companyId)
        {
            try
            {
                int number= this._appointmentService.GetNumberOfPendingAppointments(companyId);
                return new OkObjectResult(new ApiResponseModel<int>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = number
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        [Route("UpdateAppointmentStatus")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAppointmentStatus(int appointmentId, int statusId)
        {
            try
            {
                bool result = this._appointmentService.UpdateAppointmentStatus(appointmentId, statusId);
                if (result) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "update successfully." })
                { StatusCode = (int)HttpStatusCode.Created };
                return new BadRequestObjectResult(new ApiResponseModel() { Success = false, Message = "Unable to update data." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("CheckoutWithCashOrCheck")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckoutWithCashOrCheck(string appointmentId, string calendarId, string tipAmount, string discountAmount, string totalAmount, string productIds, string paidBy, string checkNumber, string otherDescription)
        {
            try
            {
                bool result =  this._appointmentService.CheckoutWithCashOrCheck(appointmentId, calendarId, tipAmount, discountAmount, totalAmount, productIds, paidBy, checkNumber, otherDescription);
                if (result) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "Checkout successfully." })
                { StatusCode = (int)HttpStatusCode.Created };
                return new BadRequestObjectResult(new ApiResponseModel() { Success = false, Message = "Unable to checkout." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("CreateBlockOffTime")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateBlockOffTime(BlockedOffTimeModel blockedOffTimeModel)
        {
            try
            {
                BlockedOffTimeModel model = this._appointmentService.AddBlockedOffTime(blockedOffTimeModel);
                if (model == null) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<BlockedOffTimeModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = model
                });
                //if (result) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "Created successfully." })
                //{ StatusCode = (int)HttpStatusCode.Created };
                //return new BadRequestObjectResult(new ApiResponseModel() { Success = false, Message = "Unable to add data." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("UpdateBlockOffTime")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateBlockOffTime(BlockedOffTimeModel blockedOffTimeModel)
        {
            try
            {
                bool result = this._appointmentService.UpdateBlockOffTime(blockedOffTimeModel);
                if (result) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "update successfully." })
                { StatusCode = (int)HttpStatusCode.Created };
                return new BadRequestObjectResult(new ApiResponseModel() { Success = false, Message = "Unable to update data." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetBlockTimeOffTime")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBlockTimeOffTime(long calendarId, long blockOffTimeId)
        {
            try
            {
                BlockedOffTimeModel model = this._appointmentService.GetBlockTimeOffTime(calendarId, blockOffTimeId);
                if (model == null) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<BlockedOffTimeModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = model
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("DeleteBlockedOffTime")]
        public IActionResult DeleteBlockedOffTime(int blockedOffTimeId)
        {
            try
            {
                bool isDeleted = this._appointmentService.DeleteBlockedOffTime(blockedOffTimeId);
                if (isDeleted) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Data deleted successfully."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Unable to delete Blocked Off Time.Please check the values passed."
                });
            }
            catch (Exception)
            { throw; }
        }

        [HttpPost]
        [Route("UpdateCalendarAvailability")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateCalendarAvailability(OverrideAvailabilityModel overrideAvailabilityModel)
        {
            try
            {
                var model = this._appointmentService.UpdateCalendarAvailability(overrideAvailabilityModel);
                if (model == null) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<OverrideAvailabilityModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = model
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("SaveAvailability")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SaveAvailability(BusinessHoursAvailability businessHoursAvailability)
        {
            try
            {
                bool result = this._appointmentService.SaveAvailability(businessHoursAvailability);
                if (result) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "Added successfully." })
                { StatusCode = (int)HttpStatusCode.Created };
                return new BadRequestObjectResult(new ApiResponseModel() { Success = false, Message = "Unable to Add data." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetRegularBusinessHours")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetRegularBusinessHours(long calendarId)
        {
            try
            {
                var model = this._appointmentService.GetRegularBusinessHours(calendarId);
                if (model == null) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<HoursAvailability>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = model
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("AccpetRejectPendingAppointment")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AccpetRejectPendingAppointment(int companyId, int appointmentId, int statusId, bool isEmailSend)
        {
            try
            {
                bool result = this._appointmentService.AccpetRejectPendingAppointment(companyId, appointmentId, statusId, isEmailSend);
                if (result) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "update successfully." })
                { StatusCode = (int)HttpStatusCode.Created };
                return new BadRequestObjectResult(new ApiResponseModel() { Success = false, Message = "Unable to update data." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetEmailTemplateData")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetEmailTemplateData(string templateName)
        {
            try
            {
                var model = this._appointmentService.GetEmailHtml(templateName);
                if (model == null) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<string>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = model
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        [Route("IsColorCodingByService")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> IsColorCodingByService(long personId)
        {
            try
            {
                return new OkObjectResult(new ApiResponseModel<bool>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = this._appointmentService.IsColorCodingByService(personId)
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
