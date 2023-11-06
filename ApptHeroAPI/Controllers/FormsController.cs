using Microsoft.AspNetCore.Mvc;
using ApptHeroAPI.Services.Abstraction.Contracts;
using System.Net;
using ApptHeroAPI.Services.Abstraction.Models;
using System.Threading.Tasks;
using System;

namespace ApptHeroAPI.Controllers
{
    /// <summary>
    /// Authenticates the user, Generates token and reset the password.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormService _formService;


        public FormsController(IFormService formService)
        {
            _formService = formService;
        }

        [HttpGet]
        [Route("GetFormSettings")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetFormSettings(long companyId)
        {
            try
            {
                FormSettingsModel formSettings = _formService.GetFormSettings(companyId);

                if (formSettings != null)
                {
                    return new OkObjectResult(new ApiResponseModel<FormSettingsModel>()
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
        [Route("GetForms")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

        public void GetForms(long companyId)
        {

        }
    }
}
