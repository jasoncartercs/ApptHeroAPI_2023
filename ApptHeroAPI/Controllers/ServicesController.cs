using ApptHeroAPI.Middleware;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ApptHeroAPI.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ReusableActionFilter))]
    //[Authorize]
    public class ServicesController : ReusableBaseController
    {
        private readonly IAppointmentTypeService _appointmentService;
        public ServicesController(IAppointmentTypeService appointmentService, IEnumerable<string> claimNames) : base(claimNames)
        {
            this._appointmentService = appointmentService;
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetCategorizedServices")]
        public IActionResult GetCategorizedServices()
        {
            try
            {
                var claims = GetUserClaims();

                if (claims.CompanyId == null)
                {
                    return BadRequest(new { message = "Invalid company Id claim." });
                }
                List<CategoryWiseServiceModel> models = this._appointmentService.GetApointmentCategories(claims.CompanyId.Value);
                if (models.Count == 0) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<List<CategoryWiseServiceModel>>()
                {
                    Success = true,
                    Message = "Data feteched successfully.",
                    Data = models
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet]
        //[ApiVersion("1.0")]
        //[Route("GetCategorizedServicesByCalendarId")]
        //public IActionResult GetCategorizedServicesByCalendarId(long calendarId)
        //{
        //    try
        //    {
        //        List<CategoryWiseServiceModel> models = this._appointmentService.GetApointmentCategoriesByCalendarId(calendarId);
        //        if (models.Count == 0) return new NoContentResult();
        //        return new OkObjectResult(new ApiResponseModel<List<CategoryWiseServiceModel>>()
        //        {
        //            Success = true,
        //            Message = "Data feteched successfully.",
        //            Data = models
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetCategories")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetCategories(long companyId)
        {
            try
            {
                List<CategoryModel> categories = this._appointmentService.GetCategoris(companyId);
                if (categories != null && categories.Count > 0) return new OkObjectResult(new ApiResponseModel<List<CategoryModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = categories
                });
                else return new OkObjectResult(new ApiResponseModel() { Success = false, Message = "No data available." });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetServices")]
        public IActionResult GetServices(long companyId, int pageNumber, int pageSize, string search)
        {
            try
            {
                ServicePaginatedModel paginatedModel = this._appointmentService.GetServices(companyId, pageNumber, pageSize, search);

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
        [ApiVersion("1.0")]
        [Route("GetServiceDetails")]
        public IActionResult GetServiceDetails(int serviceId)
        {
            try
            {
                ServiceModel details = this._appointmentService.GetServiceDetails(serviceId);
                if (details != null)
                {
                    return new OkObjectResult(new ApiResponseModel<ServiceModel>()
                    {
                        Success = true,
                        Message = "Data fetched successfully.",
                        Data = details
                    });
                }
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }




        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetServiceUpgrades")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetServiceUpgrades(int companyId, int serviceId, string searchStrng)
        {
            try
            {
                List<AddOnModel> models = this._appointmentService.GetServiceUpgrades(companyId, serviceId, searchStrng);
                if (models.Count == 0) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<List<AddOnModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = models
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetAddonsByCalendarIdForProvider")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetServiceUpgradesByCalendarIdForProvider(long calendarId, long serviceId)
        {
            try
            {
                List<AddOnModel> models = await this._appointmentService.GetServiceUpgradesByCalendarIdForProvider(calendarId, serviceId);
                if (models.Count == 0)
                {
                    return new NoContentResult();
                }

                return new OkObjectResult(new ApiResponseModel<List<AddOnModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = models
                });
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ApiVersion("1.0")]
        [Route("CreateCategory")]
        public IActionResult CreateCategory(string categoryName, int companyId, int sortOrder)
        {
            try
            {
                bool isSaved = this._appointmentService.CreateCategory(categoryName, companyId, sortOrder);
                if (isSaved) return new StatusCodeResult((int)HttpStatusCode.Created);
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Unable to create category. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPut]
        [ApiVersion("1.0")]
        [Route("UpdateCategory")]
        public IActionResult UpdateCategory(CategoryModel categoryModel)
        {
            try
            {
                bool isSaved = this._appointmentService.UpdateCategory(categoryModel);
                if (isSaved) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Data updated."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Unable to update category. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("CreateService")]
        public IActionResult CreateService(ServiceModel serviceModel)
        {
            try
            {
                bool isSuccess = this._appointmentService.CreateService(serviceModel);
                if (isSuccess) return new StatusCodeResult((int)HttpStatusCode.Created);
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Failed to create the service. Service Name is already exist. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        [ApiVersion("1.0")]
        [Route("UpdateService")]
        public IActionResult UpdateService(ServiceModel serviceModel)
        {
            try
            {
                bool isSuccess = this._appointmentService.UpdateService(serviceModel);
                if (isSuccess) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Data updated."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Failed to update the service. Please check the values passed."
                });
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("CreateUpgrade")]
        public IActionResult CreateUpgrade(AddOnModel addOnModel)
        {
            try
            {
                bool isSuccess = this._appointmentService.CreatedUpgrades(addOnModel);
                if (isSuccess) return new StatusCodeResult((int)HttpStatusCode.Created);
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Failed to create the service. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Route("UpdateUpgrades")]
        public IActionResult UpdateUpgrades(AddOnModel addOnModel)
        {
            try
            {
                bool isSuccess = this._appointmentService.UpdateUpgrades(addOnModel);
                if (isSuccess) return new StatusCodeResult((int)HttpStatusCode.Created);
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Failed to create the upgrades. Please check the values passed."
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetUpgradeById")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public IActionResult GetUpgradeById(int upgradeId)
        {
            try
            {
                List<AddOnModel> models = this._appointmentService.GetUpgradeById(upgradeId);
                if (models.Count == 0) return new NoContentResult();
                return new OkObjectResult(new ApiResponseModel<List<AddOnModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = models
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [Route("GetAppointmentTypesByAccessLevel")]
        public IActionResult GetServicesListByAccessLevel(long companyId, int accessLevel)
        {
            try
            {
                var lst = this._appointmentService.GetAppointmentTypesByAccessLevel(companyId, accessLevel);

                if (lst != null && lst.Count > 0) return new OkObjectResult(new ApiResponseModel<List<AppointmentTypeModel>>()
                {
                    Success = true,
                    Message = "Data fetched successfully.",
                    Data = lst
                });
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpDelete]
        [ApiVersion("1.0")]
        [Route("DeleteService")]
        public IActionResult DeleteService(int id)
        {
            try
            {
                bool isDeleted = this._appointmentService.ArchiveAppointmentType(id);
                if (isDeleted) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Data deleted successfully."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Unable to delete service.Please check the values passed."
                });
            }
            catch (Exception)
            { throw; }
        }

        [HttpDelete]
        [ApiVersion("1.0")]
        [Route("DeleteCategory")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                bool isDeleted = this._appointmentService.ArchiveAppointmentCategory(id);
                if (isDeleted) return new OkObjectResult(new ApiResponseModel()
                {
                    Success = true,
                    Message = "Data deleted successfully."
                });
                return new BadRequestObjectResult(new ApiResponseModel()
                {
                    Success = false,
                    Message = "Unable to delete category.Please check the values passed."
                });
            }
            catch (Exception)
            { throw; }
        }
    }
}
