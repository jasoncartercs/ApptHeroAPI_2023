using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using ApptHeroAPI.Services.Implementation.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApptHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailSetupController : ControllerBase
    {
        private readonly ICustomizeEmailService _customizeEmailService;
        public EmailSetupController(ICustomizeEmailService customizeEmailService)
        {
            _customizeEmailService = customizeEmailService;
        }


        [HttpGet]
        [Route("GetEmailTypes")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetEmailTypes()
        {
            try
            {
                List<CompanyEmailTypesModel> formSettings = _customizeEmailService.GetEmailTypes();

                if (formSettings != null)
                {
                    return new OkObjectResult(new ApiResponseModel<List<CompanyEmailTypesModel>>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = formSettings
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
        [Route("GetEmailTypeSubjectAndBody")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetEmailTypeSubjectAndBody(CompanyEmailTypesModel companyEmailTypesModel)
        {
            try
            {
                CompanyEmailSettingsModel formSettings = _customizeEmailService.GetEmailTypeSubjectAndBody(companyEmailTypesModel);

                if (formSettings != null)
                {
                    return new OkObjectResult(new ApiResponseModel<CompanyEmailSettingsModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = formSettings
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
    }
}
