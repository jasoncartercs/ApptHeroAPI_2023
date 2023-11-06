using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using ApptHeroAPI.Extensions;
using ApptHeroAPI.Middleware;
using System.Threading.Tasks;

namespace ApptHeroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ReusableActionFilter))]

    public class ClientController : ReusableBaseController
    {
        private readonly IClientService<PersonModel> _clientService;
        public ClientController(IClientService<PersonModel> clientService, IEnumerable<string> claimNames) : base(claimNames)
        {
            this._clientService = clientService; 
        }

        /// <summary>
        /// List of clients available under given CompanyId
        /// </summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>List of clients</returns>
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClients")]
        public IActionResult GetClients(int pageNumber,int pageSIze, string search)
        {
            try {
                var claims = GetUserClaims();

                if (claims.CompanyId == null)
                {
                    return BadRequest(new { message = "Invalid company Id claim." });
                }
                ClientPaginatedModel paginatedModel= this._clientService.GetClients(claims.CompanyId.Value, pageNumber,pageSIze,search);

                if(paginatedModel != null && paginatedModel.Clients.Count>0) return new OkObjectResult(new ApiResponseModel<ClientPaginatedModel>()
                {
                    Success=true,
                    Message="Data fetched successfully.",
                    Data= paginatedModel 
                });
                return new NoContentResult();
            }
            catch(Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// List of clients available under given CompanyId
        /// </summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>List of clients</returns>
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientsForExport")]
        public IActionResult GetClients(long companyId)
        {
            try
            {
                List<PersonModel> clients = this._clientService.GetClients(companyId);

                if (clients != null && clients.Count > 0) return new OkObjectResult(new ApiResponseModel<List<PersonModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = clients
                });
                return new NoContentResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// List of clients available under given CompanyId
        /// </summary>
        /// <param name="companyId">Company ID</param>
        /// <returns>List of clients</returns>
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientsFilter")]
        public IActionResult GetClientsFilter(long companyId, string query, int pageNumber, int pageSize,
               string dateFilterFrom, string dateFilterTo, string teammateFilter, bool lastAppointmentChecked, int lastAppointmentDays,
               bool hasFutureAppointmentChecked, bool hadServiceChecked, string categoryServicesFilter,
               bool hadServiceCategoryChecked, string categoryFilter)
        {
            try
            {
                ClientPaginatedModel paginatedModel = this._clientService.GetClients(companyId,query,pageNumber,pageSize,
                    dateFilterFrom,dateFilterTo,teammateFilter,lastAppointmentChecked,lastAppointmentDays, hasFutureAppointmentChecked, hadServiceChecked,
                    categoryServicesFilter, hadServiceCategoryChecked, categoryFilter);

                if (paginatedModel != null && paginatedModel.Clients.Count > 0) return new OkObjectResult(new ApiResponseModel<ClientPaginatedModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = paginatedModel
                });
                return new NoContentResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientsByTag")]
        public IActionResult GetClientsByTag(long companyId, long tagId)
        {
            try
            {
                List<PersonModel> personModels = this._clientService.GetClients(companyId, tagId);

                if (personModels != null && personModels.Count > 0) return new OkObjectResult(new ApiResponseModel<List<PersonModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = personModels
                });
                return new NoContentResult();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Client details for the given id.
        /// </summary>
        /// <param name="id">Id of the client</param>
        /// <returns>Client details</returns>
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClient")]
        public IActionResult GetClient(long id)
        {
            try
            {
                PersonModel personModel= this._clientService.GetClient(id);

                if (personModel == null) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<PersonModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = personModel
                });
            }
            catch(Exception)
            { throw; }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientAppointments")]
        public IActionResult GetClientAppointments(long id)
        {
            try
            {
              ClientModel model = this._clientService.GetClientAppointments(id);

               if (model == null) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<ClientModel>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = model
                });
            }
            catch (Exception)
            { throw; }
        }
        /// <summary>
        /// Changes or updates the ban status of the client.
        /// </summary>
        /// <param name="id">Client Id</param>
        /// <param name="status">Ban status that needs to be updated</param>
        /// <returns></returns>
        
        




        [HttpPut]
        [ApiVersion("1.0")]
        [Route("UpdateBanStatus")]
        public IActionResult UpdateBanStatus(long id, bool status)
        {
            try
            {
                bool isUpdated = this._clientService.ChangeBanStatus(id,status);
                if (!isUpdated) return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Unable to update the status. Please check the values passed."
                });
                return new OkObjectResult(new ApiResponseModel()
                {
                    Success=true,
                    Message="Status updated."
                });
            }
            catch (Exception)
            { throw; }
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("CreateClient")]
        public IActionResult CreateClient([FromBody]PersonModel personModel)
        {
            try
            {
                bool isCreated= this._clientService.CreateClient(personModel);
                if (isCreated) return new ObjectResult(new ApiResponseModel() { Success = true, Message = "Created successfully." }) { StatusCode=(int)HttpStatusCode.Created};
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success=false,
                    Message="Unable to create client. Please check the values passed." 
                });
            }
            catch(Exception)
            {
                throw;
            }
        }

      


        [HttpPut]
        [ApiVersion("1.0")]
        [Route("UpdateClient")]
        public IActionResult UpdateClient([FromBody] PersonModel personModel)
        {
            try
            {
                bool isUpdated=this._clientService.UpdateClient(personModel);
                if (isUpdated) return new OkObjectResult(new ApiResponseModel()
                { 
                    Success=true,
                    Message="Data updated successfully."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success=false,
                    Message="Unable to update client.Please check the values passed."
                });
            }
            catch(Exception ex)
            { throw; }
        }

        [HttpDelete]
        [ApiVersion("1.0")]
        [Route("DeleteClient")]
        public IActionResult DeleteClient(int id)
        {
            try
            {
                bool isDeleted = this._clientService.DeleteClient(id);
                if (isDeleted) return new OkObjectResult(new ApiResponseModel()
                {
                    Success=true,
                    Message="Data deleted successfully."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success=false,
                    Message="Unable to delete client.Please check the values passed."
                });
            }
            catch (Exception)
            { throw; }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetTags")]
        public IActionResult GetTags(long companyId)
        {
            try
            {
                List<TagModel> tags = this._clientService.GetTags(companyId);
                if (tags.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<TagModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = tags
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientMessageHistory")]
        public IActionResult GetClientMessageHistory(long personId, int pageNumber, int pageSize)
        {
            try
            {
                var claims = GetUserClaims();

                if (claims.CompanyId == null)
                {
                    return BadRequest(new { message = "Invalid company Id claim." });
                }

              var clientMessages =  _clientService.GetClientMessageHistory(claims.CompanyId.Value, personId, pageNumber, pageSize);
                if (clientMessages.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<MessageLogModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = clientMessages
                });
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientPackages")]
        public async Task<IActionResult> GetClientPackages(long personId, int pageNumber, int pageSize)
        {
            try
            {
                var clientPackages = await _clientService.GetClientPackages(personId, pageNumber, pageSize);
                if (clientPackages.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<PersonPackageModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = clientPackages
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetClientForms")]
        public IActionResult GetClientForms(long personId)
        {
            try
            {
                var claims = GetUserClaims();

                if (claims.CompanyId == null)
                {
                    return BadRequest(new { message = "Invalid company Id claim." });
                }
                List<AllFormsModel> forms = this._clientService.GetClientForms(claims.CompanyId.Value, personId);
                if (forms.Count == 0) return new StatusCodeResult((int)HttpStatusCode.NoContent);
                return new OkObjectResult(new ApiResponseModel<List<AllFormsModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully",
                    Data = forms
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
