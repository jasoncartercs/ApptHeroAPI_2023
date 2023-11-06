using ApptHeroAPI.Repositories.Abstraction.Abstracts;
using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Calendar = ApptHeroAPI.Repositories.Context.Entities.Calendar;

namespace ApptHeroAPI.Services.Concrete.Implementation
{
    public class AppointmentTypeService : IAppointmentTypeService
    {
        private readonly ServiceRepository _appointmentTypeRepository;
        private readonly SqlDbContext _dbContext;
        private readonly IAppointmentCategoryRepository _appointmentCategoryRepository;
        private readonly IRepository<AppointmentTypeCategory> _appointmentTypeCategoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Addon> _addOnRepository;
        private readonly IRepository<AppointmentTypeCategory> _appttTypeCategoryRepository;
        private readonly IRepository<AppointmentTypeAddon> _appointmentTypeAddonRepository;
        private readonly IRepository<TeammateAddons> _teammateAddOnRepository;
        private readonly IRepository<Calendar> _calendarRepository;
        private readonly IConfiguration _configuration;

        public AppointmentTypeService(IConfiguration configuration, IDbContextFactory<SqlDbContext> dbContextFactory, ServiceRepository appointmentTypeRepository, IAppointmentCategoryRepository appointmentCategoryRepository, IRepository<AppointmentTypeCategory> appointmentTypeCategoryRepository, IRepository<Product> productRepository
            , IRepository<Addon> addOnRepository, IRepository<AppointmentTypeCategory> apptTypeCategoryRepository
            , IRepository<AppointmentTypeAddon> appointmentTypeAddonRepository, IRepository<TeammateAddons> teammateAddOnRepository,
            IRepository<Calendar> calendarRepository)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
            this._configuration = configuration;
            this._appointmentCategoryRepository = appointmentCategoryRepository;
            this._appointmentTypeCategoryRepository = appointmentTypeCategoryRepository;
            this._productRepository = productRepository;
            this._appointmentTypeRepository = appointmentTypeRepository;
            this._addOnRepository = addOnRepository;
            this._appttTypeCategoryRepository = appointmentTypeCategoryRepository;
            this._appointmentTypeAddonRepository = appointmentTypeAddonRepository;
            this._teammateAddOnRepository = teammateAddOnRepository;
            this._calendarRepository = calendarRepository;
        }

        public bool CreateCategory(string name, int companyId, int sortOrder)
        {
            try
            {
                AppointmentTypeCategory appointmentTypeCategory = new AppointmentTypeCategory()
                {
                    IsArchived = false,
                    CompanyID = companyId,
                    Name = name,
                    SortOrder = sortOrder
                };
                AppointmentTypeCategory result = this._appointmentTypeCategoryRepository.Add(appointmentTypeCategory).ConfigureAwait(false).GetAwaiter().GetResult();
                if (result == null) return false;
                return true; ;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CreatedUpgrades(AddOnModel addOnModel)
        {
            try
            {
                var product = new Product
                {
                    Price = addOnModel.ProductModel.Price,
                    Name = addOnModel.ProductModel.ServiceName,
                    CompanyID = addOnModel.ProductModel.CompanyID,
                    CreatedDate = DateTime.Now.ToUniversalTime()
                };

                var productModel = this._productRepository.Add(product).ConfigureAwait(false).GetAwaiter().GetResult();

                var Addon = new Addon()
                {
                    Duration = addOnModel.Duration,
                    IsVisible = addOnModel.IsVisible,
                    ProductId = productModel.ProductId
                };
                Addon result = this._addOnRepository.Add(Addon).ConfigureAwait(false).GetAwaiter().GetResult();

                List<AppointmentTypeAddon> appointmentTypeAddon = new List<AppointmentTypeAddon>();

                foreach (var at in addOnModel.AppointmentTypeId)
                {
                    appointmentTypeAddon.Add(new AppointmentTypeAddon() { AddonId = result.AddonId, AppointmentTypeId = at });

                }
                this._appointmentTypeAddonRepository.AddList(appointmentTypeAddon).ConfigureAwait(false).GetAwaiter().GetResult();

            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool UpdateUpgrades(AddOnModel addOnModel)
        {
            try
            {
                using (var transaction = this._dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var addon = this._addOnRepository.Get(s => s.AddonId == addOnModel.Id).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (addon == null) return false;

                        addon.Duration = addOnModel.Duration;
                        addon.IsVisible = addOnModel.IsVisible;
                        this._addOnRepository.Update(addon).ConfigureAwait(false).GetAwaiter().GetResult();


                        var product = this._productRepository.Get(s => s.ProductId == addon.ProductId).ConfigureAwait(false).GetAwaiter().GetResult();

                        product.Price = addOnModel.ProductModel.Price;
                        product.Name = addOnModel.ProductModel.ServiceName;

                        this._productRepository.Update(product).ConfigureAwait(false).GetAwaiter().GetResult();

                        using (SqlConnection con = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                        {
                            con.Open();
                            SqlCommand command = new SqlCommand($"delete from AppointmentTypeAddon where AddonId={addon.AddonId}", con);
                            command.ExecuteNonQuery();
                        }


                        List<AppointmentTypeAddon> appointmentTypeAddon = new List<AppointmentTypeAddon>();

                        foreach (var at in addOnModel.AppointmentTypeId)
                        {
                            appointmentTypeAddon.Add(new AppointmentTypeAddon() { AddonId = addon.AddonId, AppointmentTypeId = at });

                        }
                        this._appointmentTypeAddonRepository.AddList(appointmentTypeAddon).ConfigureAwait(false).GetAwaiter().GetResult();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public List<AddOnModel> GetUpgradeById(int upgradeId)
        {
            var addOnModel = new List<AddOnModel>();
            try
            {
                addOnModel = (from a in this._dbContext.Addon
                              join p in this._dbContext.Product
                              on a.ProductId equals p.ProductId
                              where a.AddonId == upgradeId
                              select new AddOnModel
                              {
                                  Id = a.AddonId,
                                  Duration = a.Duration,
                                  IsVisible = a.IsVisible,
                                  ProductId = a.ProductId,
                                  ProductModel = new ProductModel
                                  {
                                      Price = p.Price,
                                      ServiceName = p.Name,
                                      CompanyID = p.CompanyID
                                  },
                                  AppointmentTypeId = this._dbContext.AppointmentTypeAddon.Where(x => x.AddonId == upgradeId).Select(x => x.AppointmentTypeId).ToList()
                              }).ToList();


                return addOnModel;


            }
            catch
            {
                return addOnModel;
            }
        }

        public List<AppointmentTypeModel> GetAppointmentTypesByAccessLevel(long companyId, int accessId)
        {
            List<AppointmentTypeModel> appointmentTypeModels = new List<AppointmentTypeModel>();

            appointmentTypeModels = (from appointmentType in this._dbContext.AppointmentType.Include(x => x.Product)
                                     where appointmentType.Product.CompanyID == companyId && appointmentType.IsArchived == false &&
                                     appointmentType.AccessLevelId == accessId
                                     select new AppointmentTypeModel
                                     {
                                         AccessLevelId = appointmentType.AccessLevelId,
                                         Color = appointmentType.Color,
                                         AppointmentTypeId = appointmentType.AppointmentTypeId,
                                         BlockedOffMinutesAfterAppointment = appointmentType.BlockedOffMinutesAfterAppointment,
                                         BlockedOffMinutesBeforeAppointment = appointmentType.BlockedOffMinutesBeforeAppointment,
                                         AppointmentTypeCategoryId = appointmentType.AppointmentTypeCategoryId,
                                         ConfirmationMessage = appointmentType.ConfirmationMessage,
                                         DurationInMinutes = appointmentType.Duration,
                                         AppointmentTypeName = appointmentType.Product.Name,

                                         ProductModel = new ProductModel
                                         {
                                             CompanyID = appointmentType.Product.CompanyID,
                                             Description = appointmentType.Product.Description,
                                             ImageUrl = appointmentType.Product.ImageUrl,
                                             Price = appointmentType.Product.Price,
                                             ServiceName = appointmentType.Product.Name,
                                             CreatedDate = appointmentType.Product.CreatedDate,
                                             ProductId = appointmentType.Product.ProductId
                                         }
                                     }).ToList();


            return appointmentTypeModels;
        }

        public bool CreateService(ServiceModel serviceModel)
        {
            try
            {
                Product product = this._productRepository.Get(s => s.Name == serviceModel.ServiceName).ConfigureAwait(false).GetAwaiter().GetResult();
                if (product == null)
                {
                    AppointmentType appointmentType = new AppointmentType()
                    {
                        AccessLevelId = serviceModel.AccessId,
                        AppointmentTypeCategoryId = serviceModel.CategoryId,
                        BlockedOffMinutesAfterAppointment = serviceModel.BlockedOffMinutesAfterAppointment,
                        BlockedOffMinutesBeforeAppointment = serviceModel.BlockedOffMinutesBeforeAppointment,
                        Color = serviceModel.Color,
                        ConfirmationMessage = serviceModel.ConfirmationMessage,
                        Duration = serviceModel.Duration,
                        IsArchived = false,
                        //IsZoomMeeting/// need to discuss...
                        Product = new Product()
                        {
                            Price = serviceModel.Price,
                            CompanyID = serviceModel.CompanyId,
                            CreatedDate = DateTime.Now,
                            Description = serviceModel.Description,
                            ImageUrl = serviceModel.ImageUrl,
                            Name = serviceModel.ServiceName
                        }
                        //SortOrder=serviceModel. //need to discuss

                    };

                    AppointmentType appointTypeResult = this._appointmentTypeRepository.Add(appointmentType).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (appointTypeResult == null) return false;
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CategoryWiseServiceModel> GetApointmentCategories(long companyId)
        {
            try
            {
                List<AppointmentType> appts = this._appointmentTypeRepository.GetAll(s => s.Product.CompanyID == companyId && !s.IsArchived && !s.AppointmentTypeCategory.IsArchived).ConfigureAwait(false).GetAwaiter().GetResult();

                var apptGroups = appts.GroupBy(s => s.AppointmentTypeCategory.Name);

                List<CategoryWiseServiceModel> models = new List<CategoryWiseServiceModel>();
                List<AppointmentType> types = new List<AppointmentType>();
                foreach (var group in apptGroups)
                {
                    types = group.ToList();
                    models.Add(new CategoryWiseServiceModel()
                    {

                        CategoryName = group.Key,
                        CategoryId = types.Select(s => s.AppointmentTypeCategoryId).FirstOrDefault().Value,
                        Products = types.Select(s => new ProductModel()
                        {
                            ServiceName = s.Product.Name,
                            Duration = s.Duration,
                            Price = s.Product.Price,
                            AppointmentTypeId = s.AppointmentTypeId,
                            ProductId = s.ProductId
                        }).ToList()
                    });
                }
                return models;
                //DataTable dataTable = this._appointmentCategoryRepository.GetServicesNCategories(companyId).ConfigureAwait(false).GetAwaiter().GetResult();
                //string serializedData = JsonConvert.SerializeObject(dataTable);
                //List<CategoryWiseServiceModel> appointmentCategoryModels = JsonConvert.DeserializeObject<List<CategoryWiseServiceModel>>(serializedData);
                //return appointmentCategoryModels;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CategoryModel> GetCategoris(long companyId)
        {
            try
            {
                List<AppointmentTypeCategory> categories = this._appttTypeCategoryRepository.GetAll(s => s.CompanyID == companyId && !s.IsArchived).ConfigureAwait(false).GetAwaiter().GetResult();
                return categories.Select(s => new CategoryModel()
                {
                    CompanyId = s.CompanyID,
                    Id = s.AppointmentTypeCategoryId,
                    SortOrder = s.SortOrder,
                    Name = s.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool ArchiveAppointmentCategory(long appointmentCategoryId)
        {
            try
            {
                AppointmentTypeCategory typeCategory = this._appointmentTypeCategoryRepository.Get(s => s.AppointmentTypeCategoryId == appointmentCategoryId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (typeCategory == null) return false;

                typeCategory.IsArchived = true;
                bool isUpdated = this._appointmentTypeCategoryRepository.Update(typeCategory).ConfigureAwait(false).GetAwaiter().GetResult();
                return isUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ArchiveAppointmentType(long appointmentTypeId)
        {
            try
            {
                var appointmentType = this._appointmentTypeRepository.Get(s => s.AppointmentTypeId == appointmentTypeId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (appointmentType == null) return false;

                appointmentType.IsArchived = true;
                bool isUpdated = this._appointmentTypeRepository.Update(appointmentType).ConfigureAwait(false).GetAwaiter().GetResult();
                return isUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ServicePaginatedModel GetServices(long companyId, int pageNumber, int pageSize, string searchString = null)
        {
            try
            {
                int count = 0;
                if (pageNumber != 0) pageNumber--;

                List<AppointmentType> appointmentTypes = new List<AppointmentType>();
                List<ServiceListModel> serviceModel = new List<ServiceListModel>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    appointmentTypes = this._appointmentTypeRepository.GetByPagination((s => s.Product.CompanyID == companyId && (s.Product.Name.Contains(searchString))), pageNumber, pageSize, out count);
                }
                else
                {
                    appointmentTypes = this._appointmentTypeRepository.GetByPagination((s => s.Product.CompanyID == companyId), pageNumber, pageSize, out count);
                }

                if (appointmentTypes != null && appointmentTypes.Count > 0)
                {
                    serviceModel = (from s in appointmentTypes
                                    select new ServiceListModel
                                    {
                                        Id = s.AppointmentTypeId,
                                        ServiceName = s.Product.Name,
                                        Duration = s.Duration,
                                        Price = s.Product.Price
                                    }).ToList();
                }
                return new ServicePaginatedModel()
                {
                    Services = serviceModel,
                    PageNumber = pageNumber + 1,
                    RecordsReturned = serviceModel.Count,
                    TotalCount = count
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ServiceModel GetServiceDetails(int serviceId)
        {
            try
            {
                var appointmentType = this._appointmentTypeRepository.Get(x => x.AppointmentTypeId == serviceId).ConfigureAwait(false).GetAwaiter().GetResult();
                var serviceModel = new ServiceModel();
                if (appointmentType != null)
                {
                    serviceModel = new ServiceModel()
                    {
                        AccessId = appointmentType.AccessLevelId,
                        CategoryId = appointmentType.AppointmentTypeCategoryId.Value,
                        CategoryName = appointmentType.AppointmentTypeCategory.Name,
                        BlockedOffMinutesAfterAppointment = appointmentType.BlockedOffMinutesAfterAppointment,
                        BlockedOffMinutesBeforeAppointment = appointmentType.BlockedOffMinutesBeforeAppointment,
                        Color = appointmentType.Color,
                        ConfirmationMessage = appointmentType.ConfirmationMessage,
                        Duration = appointmentType.Duration,
                        Price = appointmentType.Product.Price,
                        CompanyId = appointmentType.Product.CompanyID,
                        Description = appointmentType.Product.Description,
                        ImageUrl = appointmentType.Product.ImageUrl,
                        ServiceName = appointmentType.Product.Name,
                        ProductId = appointmentType.ProductId,
                        TypeId = appointmentType.AppointmentTypeId
                    };
                }
                return serviceModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<AddOnModel>> GetServiceUpgradesByCalendarIdForProvider(long calendarId, long serviceId)
        {
            try
            {
                List<AppointmentTypeAddon> appointmentTypeAddons = new List<AppointmentTypeAddon>();

                var addons = await this._addOnRepository.GetAll();
                var calendar = await this._calendarRepository.Get(x => x.CalendarId == calendarId);
                var teammateAddons = await this._teammateAddOnRepository.GetAll(x => x.PersonId == calendar.PersonId);

                var results = from addon in addons
                              join teammateAddon in teammateAddons on addon.AddonId equals teammateAddon.AddonId
                              select new AddOnModel
                              {
                                  Id = addon.AddonId,
                                  Duration = addon.Duration,
                                  IsVisible = addon.IsVisible,
                                  ProductId = addon.ProductId,
                                  ProductName = addon.Product.Name,
                                  ProductModel = new ProductModel
                                  {
                                      ServiceName = addon.Product.Name,
                                      ProductId = addon.ProductId,
                                      Price = teammateAddon.Price ?? addon.Product.Price
                                  },
                              };

                return results.ToList();


            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<AddOnModel> GetServiceUpgrades(long companyId, int serviceId, string searchString)
        {
            try
            {
                List<AppointmentTypeAddon> appointmentTypeAddons = new List<AppointmentTypeAddon>();
                List<Addon> addOns = new List<Addon>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    if (serviceId > 0)
                    {
                        addOns = this._addOnRepository.GetAll(s => s.Product.CompanyID == companyId && s.ProductId == serviceId && s.Product.Name.Contains(searchString)).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else
                    {
                        addOns = this._addOnRepository.GetAll(s => s.Product.CompanyID == companyId && s.Product.Name.Contains(searchString)).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }
                else
                {
                    if (serviceId > 0)
                    {
                        addOns = this._addOnRepository.GetAll(s => s.Product.CompanyID == companyId && s.ProductId == serviceId).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                    else
                    {
                        addOns = this._addOnRepository.GetAll(s => s.Product.CompanyID == companyId).ConfigureAwait(false).GetAwaiter().GetResult();
                    }
                }

                return addOns.Select(s => new AddOnModel()
                {
                    Id = s.AddonId,
                    Duration = s.Duration,
                    Price = s.Product.Price,
                    ProductName = s.Product.Name,
                    ProductId = s.Product.ProductId,
                    IsVisible = s.IsVisible
                }).ToList();
                //if (!string.IsNullOrEmpty(searchString))
                //{
                //    searchString = searchString.ToLower();
                //    appointmentTypeAddons = this._appointmentTypeAddonRepository.GetAll(s => s.Addon.Product.CompanyID == companyId && s.Addon.Product.Name.ToLower().Contains(searchString)).ConfigureAwait(false).GetAwaiter().GetResult();
                //}
                //else
                //appointmentTypeAddons = this._appointmentTypeAddonRepository.GetAll(s => s.Addon.Product.CompanyID == companyId).ConfigureAwait(false).GetAwaiter().GetResult();

                //return appointmentTypeAddons.Select(s => new AppointmentTypeAddonModel()
                //{
                //    AddonId = s.AddonId,
                //    AppointmentTyepId = s.AppointmentTypeId,
                //    Duration = s.Addon.Duration,
                //    IsVisible = s.Addon.IsVisible,
                //    Price = s.Addon.Product.Price,
                //    ProductName = s.Addon.Product.Name
                //}).ToList();

                //DataTable dataTable = this._appointmentCategoryRepository.GetServicesNUpgrades(companyId).ConfigureAwait(false).GetAwaiter().GetResult();
                //string serializedData = JsonConvert.SerializeObject(dataTable);
                //List<ServiceUpgradesModel> serviceUpgrades = JsonConvert.DeserializeObject<List<ServiceUpgradesModel>>(serializedData);
                //return serviceUpgrades;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateCategory(CategoryModel categoryModel)
        {
            try
            {
                AppointmentTypeCategory typeCategory = this._appointmentTypeCategoryRepository.Get(s => s.AppointmentTypeCategoryId == categoryModel.Id).ConfigureAwait(false).GetAwaiter().GetResult();
                if (typeCategory == null) return false;

                //  IsArchived = categoryModel.IsArchived,
                typeCategory.CompanyID = categoryModel.CompanyId;
                typeCategory.Name = categoryModel.Name;
                typeCategory.SortOrder = categoryModel.SortOrder;

                bool isUpdated = this._appointmentTypeCategoryRepository.Update(typeCategory).ConfigureAwait(false).GetAwaiter().GetResult();
                return isUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateService(ServiceModel serviceModel)
        {
            try
            {
                Product product = this._productRepository.Get(s => s.ProductId == serviceModel.ProductId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (product == null) return false;
                product = new Product();

                AppointmentType appointmentType = new AppointmentType()
                {
                    AppointmentTypeId = serviceModel.TypeId,
                    AccessLevelId = serviceModel.AccessId,
                    AppointmentTypeCategoryId = serviceModel.CategoryId,
                    BlockedOffMinutesAfterAppointment = serviceModel.BlockedOffMinutesAfterAppointment,
                    BlockedOffMinutesBeforeAppointment = serviceModel.BlockedOffMinutesBeforeAppointment,
                    Color = serviceModel.Color,
                    ConfirmationMessage = serviceModel.ConfirmationMessage,
                    Duration = serviceModel.Duration,
                    IsArchived = false,
                    //IsZoomMeeting/// need to discuss...
                    ProductId = serviceModel.ProductId,
                    //SortOrder=serviceModel. //need to discuss
                    Product = new Product()
                    {
                        ProductId = serviceModel.ProductId,
                        Price = serviceModel.Price,
                        CompanyID = serviceModel.CompanyId,
                        CreatedDate = DateTime.Now,
                        Description = serviceModel.Description,
                        ImageUrl = serviceModel.ImageUrl,
                        Name = serviceModel.ServiceName
                    }
                };

                bool isUpdated = this._appointmentTypeRepository.Update(appointmentType).ConfigureAwait(false).GetAwaiter().GetResult();
                return isUpdated;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
