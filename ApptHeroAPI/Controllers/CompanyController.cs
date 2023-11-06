using ApptHeroAPI.Repositories.Context.Entities;
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
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            this._companyService = companyService;
        }



        [HttpPost]
        [Route("SaveCompanySetting")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SaveCompanySetting(CompanySettingsModel model)
        {
            try
            {
                bool isAdded = this._companyService.SaveCompanySetting(model);
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
            }
        }

        [HttpPost]
        [Route("SaveCompanyBufferTimes")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SaveCompanyBufferTimes(CompanySettingsModel model)
        {
            try
            {
                bool isAdded = this._companyService.SaveCompanyBufferTimes(model.CompanyId, model.ShowBufferTimesOnCalendar);
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
            }
        }

        [HttpPost]
        [Route("SaveCompany")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SaveCompany(CompanyModel model)
        {
            try
            {
                bool isAdded = this._companyService.SaveCompany(model);
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
            }
        }

        [HttpPost]
        [Route("SaveBookingRules")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SaveBookingRules(CompanyBookingRulesModel model)
        {
            try
            {
                bool isAdded = this._companyService.SaveBookingRules(model);
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
            }
        }

        [HttpGet]
        [Route("SaveCalendarStartAndEndTime")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SaveCalendarStartAndEndTime(CompanySettingsModel model)
        {
            try
            {
                bool isAdded = _companyService.SaveCalendarStartAndEndTime(model);
                if (model != null)
                {
                    if (isAdded) return new StatusCodeResult((int)HttpStatusCode.Created);
                    return new BadRequestObjectResult(new ApiResponseModel()
                    {
                        Success = false,
                        Message = "Something went wrong. Please check the values passed."
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
        [Route("GetBookingRules")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetBookingRules(long companyId)
        {
            try
            {
                var model  = _companyService.GetBookingRules(companyId);
                if (model != null)
                {
                    return new OkObjectResult(new ApiResponseModel<CompanyBookingRulesModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = model
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
        [Route("GetCompanySetting")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCompanySetting(long companyId)
        {
            try
            {
                var companySetting = this._companyService.GetCompanySetting(companyId);

                if (companySetting != null)
                {
                    return new OkObjectResult(new ApiResponseModel<CompanySettingsModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = companySetting
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
        [Route("GetCompany")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCompany(long companyId)
        {
            try
            {
                var company = this._companyService.GetCompany(companyId);

                if (company != null)
                {
                    return new OkObjectResult(new ApiResponseModel<CompanyModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = company
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
        [Route("GetCompanyCalendarSetting")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetCompanyCalendarSetting(long companyId)
        {
            try
            {
                var calendarSetting = this._companyService.GetCompanyCalendarSetting(companyId);

                if (calendarSetting != null)
                {
                    return new OkObjectResult(new ApiResponseModel<CompanySettingsModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = calendarSetting
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
        [Route("GetStates")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                var states = this._companyService.GetStates();

                if (states != null && states.Count > 0)
                {
                    return new OkObjectResult(new ApiResponseModel<List<StateProvinceModel>>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = states
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
