
using ApptHeroAPI.Common.Enums;
using ApptHeroAPI.Common.Models;
using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Abstraction.Abstracts;
using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Consts;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Enum;
using ApptHeroAPI.Services.Abstraction.Models;
using ApptHeroAPI.Services.Abstraction.Models.MailModels;
using ApptHeroAPI.Services.Implementation.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using Calendar = ApptHeroAPI.Repositories.Context.Entities.Calendar;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class AppointmentService : IAppointmentService
    {
        private readonly SqlDbContext _dbContext;

        private readonly IRepository<Appointment> _appointmentRepository;
        private readonly IRepository<Repositories.Context.Entities.Calendar> _calendarRepository;
        private readonly IRepository<AppointmentSeries> _appointmentSeriesRepository;
        private readonly EMailHelper _eMailHelper;
        private readonly IRepository<Company> _companyRepository;
        private readonly IRepository<ApptHeroAPI.Repositories.Context.Entities.TimeZone> _timeZoneRepository;
        private readonly IRepository<IntakeFormClientSubmission> _intakeFormClientRepository;
        private readonly IRepository<IntakeFormTemplateAppointmentTypes> _intakeFormApptTypeRepository;
        private readonly IRepository<GeneralForm> _generalFormRepository;
        private readonly IRepository<CompanyEmailSetting> _companyEmailSettingReposiory;
        private readonly IRepository<Addon> _addonRepository;
        private string _rootUrl;
        private readonly IConfiguration _configuration;
        private readonly ITimeZoneService _timeZoneService;
        private readonly ServiceRepository _appointmentTypeRepository;
        private readonly IRepository<BlockedOffTime> _blockedOffTimeRepository;
        private readonly IAppointmentEmailService _appointmentEmailService;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<BlockedOffTimeSeries> _blockedOffTimeSeriesRepository;
        private readonly IRepository<OverrideAvailability> _overrideAvailabilityRepository;
        public AppointmentService(IDbContextFactory<SqlDbContext> dbContextFactory, IRepository<Appointment> appointmentRepository, IRepository<Repositories.Context.Entities.Calendar> calendarRepository, IRepository<AppointmentSeries> appointmentSeriesRepository,
            EMailHelper eMailHelper, IRepository<Company> companyRepository, IRepository<ApptHeroAPI.Repositories.Context.Entities.TimeZone> timeZoneRepository,
            IRepository<IntakeFormClientSubmission> intakeFormClientRepository, IRepository<IntakeFormTemplateAppointmentTypes> intakeFormApptTypeRepository, IConfiguration configuration,
            IRepository<GeneralForm> generalRepository, IRepository<CompanyEmailSetting> companyEmailSettingReposiory, IRepository<Addon> addonRepository, ITimeZoneService timeZoneService,
            ServiceRepository appointmentTypeRepository, IRepository<BlockedOffTime> blockedOffTimeRepository, IAppointmentEmailService appointmentEmailService, IRepository<Person> personRepository,
            IRepository<BlockedOffTimeSeries> blockedOffTimeSeriesRepository, IRepository<OverrideAvailability> overrideAvailabilityRepository)
        {
            this._dbContext = dbContextFactory.CreateDbContext();

            this._appointmentRepository = appointmentRepository;
            this._calendarRepository = calendarRepository;
            this._appointmentSeriesRepository = appointmentSeriesRepository;
            this._eMailHelper = eMailHelper;
            this._companyRepository = companyRepository;
            this._configuration = configuration;
            this._intakeFormClientRepository = intakeFormClientRepository;
            this._intakeFormApptTypeRepository = intakeFormApptTypeRepository;
            this._generalFormRepository = generalRepository;
            this._companyEmailSettingReposiory = companyEmailSettingReposiory;
            this._timeZoneRepository = timeZoneRepository;
            this._addonRepository = addonRepository;
            this._timeZoneService = timeZoneService;
            this._appointmentTypeRepository = appointmentTypeRepository;
            this._blockedOffTimeRepository = blockedOffTimeRepository;
            this._appointmentEmailService = appointmentEmailService;
            this._personRepository = personRepository;
            this._blockedOffTimeSeriesRepository = blockedOffTimeSeriesRepository;
            this._overrideAvailabilityRepository = overrideAvailabilityRepository;
        }
        private DateTime ConvertToUtc(DateTime dateTime, TimeZoneInfo timeZone)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
            }

            if (dateTime.Kind == DateTimeKind.Local)
            {
                return TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
            }

            return dateTime;
        }

        public AppointmentViewModel AddAppointment(AppointmentSaveModel model, ref string message)
        {
            AppointmentViewModel returnModel = new AppointmentViewModel();
            try
            {
                Person person;

                //this is a new client
                if (model.ClientId == 0)
                {
                    // This is a new client
                    person = CreateNewClient(model.Client);
                    _dbContext.Person.Add(person);
                    _dbContext.SaveChanges();
                }
                else
                {
                    // Existing client
                    person = new Person
                    {
                        PersonId = model.ClientId
                    };
                }

                if (model.Series != null)
                {
                    AppointmentSeries series = new AppointmentSeries()
                    {
                        CalendarId = model.CalendarId,
                        CreatedDate = DateTime.UtcNow,
                        LastUpdatedDate = DateTime.UtcNow,
                        RecurUntil = model.Series.RecurUntil,
                        TimePeriodId = model.Series.TimePeriodId
                    };
                    var calendar = this._calendarRepository.Get(c => c.CalendarId == model.CalendarId).ConfigureAwait(false).GetAwaiter().GetResult();
                    AppointmentSeries result2 = this._appointmentSeriesRepository.Add(series).ConfigureAwait(false).GetAwaiter().GetResult();
                    int numberOfDays = 0;
                    bool isFirstAppointment = true;

                    while (model.StartTime <= model.Series.RecurUntil.Date.AddDays(1))
                    {
                        //get the number of weeks
                        int numWeeks = numberOfDays / 7;

                        //if the numb
                        if (numWeeks % model.Series.TimePeriodId == 0)
                        {
                            if (isFirstAppointment)
                            {
                                //create the first appointment
                                returnModel = SaveAppointment(model, person, null);
                                isFirstAppointment = false;
                            }
                            else
                            {
                                //create a recurring appointment
                                SaveAppointment(model, person, result2.AppointmentTimeSeriesId);
                            }
                        }
                        numberOfDays += 7;
                        model.StartTime = model.StartTime.AddDays(7);
                        model.EndTime = model.EndTime.AddDays(7);
                    }
                }
                else
                {
                    returnModel = SaveAppointment(model, person, null);
                }

                return returnModel;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                throw;
            }
        }

        private Person CreateNewClient(PersonModel clientModel)
        {
            return new Person
            {
                FirstName = clientModel.FirstName,
                LastName = clientModel.LastName,
                UserRoleId = (int)UserRoles.ClientId,
                Phone = clientModel.Phone,
                EmailAddress = clientModel.EmailAddress,
                IsAccountBanned = false,
                IsAccountEnabled = true,
                IsArchived = false,
                PersonCompany = new PersonCompany
                {
                    CompanyId = clientModel.CompanyId,
                }
            };
        }

        // Function to create a new Appointment object
        private Appointment CreateNewAppointment(AppointmentSaveModel model, long personId, long? appointmentTimeSeriesId)
        {
            return new Appointment
            {
                AppointmentId = model.Id,
                CalendarId = model.CalendarId,
                CreationDate = DateTime.Now,
                StartTime = model.StartTime,
                AppointmentTimeSeriesId = appointmentTimeSeriesId,
                Notes = model.Notes,
                AppointmentTypeId = model.AppointmentTypeId,
                PersonId = personId,
                IsCancelled = false,
                IsNoShow = false,
                IsBlockedOffTime = false,
                HasBeenCheckedOut = false,
                IsAccepted = true,
                IsApproved = 1,
                AppointmentStatus = 1
            };
        }


        private Appointment CreateOrUpdateAppointment(AppointmentSaveModel model, long personId, long? appointmentTimeSeriesId)
        {
            // Try to retrieve an existing appointment from the database
            var appointment = this._appointmentRepository.Get(a => a.AppointmentId == model.Id)
                .ConfigureAwait(false).GetAwaiter().GetResult();

            // If the appointment doesn't exist, create a new one
            if (appointment == null)
            {
                appointment = new Appointment
                {
                    CreationDate = DateTime.UtcNow,
                    IsCancelled = false,
                    IsNoShow = false,
                    IsBlockedOffTime = false,
                    HasBeenCheckedOut = false,
                    IsAccepted = true,
                    IsApproved = 1,
                    AppointmentStatus = 1,
                };
            }

            // Update the properties of the appointment
            appointment.CalendarId = model.CalendarId;
            appointment.LastUpdated = DateTime.UtcNow;
            appointment.StartTime = model.StartTime;
            appointment.AppointmentTimeSeriesId = appointmentTimeSeriesId;
            appointment.Notes = model.Notes;
            appointment.AppointmentTypeId = model.AppointmentTypeId;
            appointment.PersonId = personId;

            return appointment;
        }

        public AppointmentViewModel SaveAppointment(AppointmentSaveModel model, Person person, long? appointmentTimeSeriesId)
        {

            var calendar = this._dbContext.Calendar.Include("TimeZone").FirstOrDefault(x => x.CalendarId == model.CalendarId);

            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(calendar.TimeZone.SystemTimeZoneId);

            Appointment appointment = CreateOrUpdateAppointment(model, person.PersonId, appointmentTimeSeriesId);

            AppointmentType appointmentType = this._appointmentTypeRepository.Get(s => s.AppointmentTypeId == appointment.AppointmentTypeId).ConfigureAwait(false).GetAwaiter().GetResult();

            int duration = 0;
            if (appointmentType != null && appointmentType.AppointmentTypeAddons?.Count > 0)
            {
                duration = appointmentType.Duration;
                appointment.AppointmentAddon = new List<AppointmentAddon>();
                if (model.SelectedAddOns != null && model.SelectedAddOns.Count > 0)
                {
                    foreach (var addon in appointmentType.AppointmentTypeAddons)
                    {
                        var isExist = model.SelectedAddOns.Any(x => x.Id == addon.AddonId);
                        if (isExist)
                        {
                            duration += addon.Addon.Duration;
                        }
                    }

                    foreach (var selectedAddOnId in model.SelectedAddOns)
                    {
                        //get the existing appointment type
                        var appointmentAddon = new AppointmentAddon();
                        appointmentAddon.AddonId = selectedAddOnId.Id;
                        appointmentAddon.AppointmentId = appointment.AppointmentId;
                        appointment.AppointmentAddon.Add(appointmentAddon);
                    }
                }
            }
            appointment.EndTime = appointment.StartTime.AddMinutes(duration);
            appointment.StartTime = TimeZoneInfo.ConvertTimeToUtc(appointment.StartTime, userTimeZone);
            appointment.EndTime = TimeZoneInfo.ConvertTimeToUtc(appointment.EndTime, userTimeZone);

            // Assume that a new appointment has an AppointmentId of 0
            if (appointment.AppointmentId == 0)
            {
                // This is a new appointment, so add it to the repository
                appointment = this._appointmentRepository.Add(appointment).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            else
            {
                // This is an existing appointment, so update it in the repository
                this._appointmentRepository.Update(appointment).ConfigureAwait(false).GetAwaiter().GetResult();
            }

            appointment = this.GetAppointmentEntity(appointment.AppointmentId);
            appointment.StartTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(appointment.StartTime, appointment.Calendar.TimeZone.SystemTimeZoneId);
            appointment.EndTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(appointment.EndTime, appointment.Calendar.TimeZone.SystemTimeZoneId);
            _appointmentEmailService.SendConfirmationMailToTeamMember(appointment);

            if (model.SendMail) SendConfirmationMailToClient(appointment, string.Empty, 3);

            return new AppointmentViewModel
            {
                CalendarId = appointment.CalendarId,
                AppointmentTypeId = appointment.AppointmentTypeId,
                ProductName = appointment.AppointmentType.Product.Name,
                Color = appointment.AppointmentType.Color,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                CreationDate = appointment.CreationDate,
                PersonId = appointment.PersonId.Value,
                Id = appointment.AppointmentId,
                Price = appointment.AppointmentType.Product.Price,
                Notes = appointment.Notes,
                Addons = appointment.AppointmentAddon?.Where(x => x.Addon.IsVisible).Select(s => new AddOnModel()
                {
                    ProductName = s.Addon.Product.Name,
                    ProductId = s.Addon.Product.ProductId,
                    Duration = s.Addon.Duration,
                    Price = s.Addon.Product.Price
                }).ToList(),
                ClientName = appointment.Person.FirstName + " " + appointment.Person.LastName,
                Duration = appointment.AppointmentType.Duration,
                TeamMember = appointment.Calendar.Name,
            };
        }

        public string GetEmailHtml(string name)
        {
            return _appointmentEmailService.GetEmailHtml(name);
        }

        public Appointment GetAppointmentEntity(long appointmentId)
        {
            var appointment = (from a in this._dbContext.Appointment.Include(a => a.AppointmentAddon).Include(a => a.Calendar).Include(a => a.Calendar.Person).Include(a => a.Person).Include(a => a.Person.PersonCompany).Include(a => a.Person.UserRole).Include(a => a.AppointmentAddon).Include(a => a.Calendar.TimeZone).Include(s => s.AppointmentType).Include(x => x.AppointmentType.AppointmentTypeCategory).Include(s => s.AppointmentType.Product)
                               where a.AppointmentId == appointmentId
                               select a
          ).FirstOrDefault();

            return appointment;
        }

        public bool UpdateAppointment(AppointmentSaveModel model)
        {
            try
            {
                var appointment = this._dbContext.Appointment.Where(x => x.AppointmentId == model.Id).Include(x => x.Calendar).Include(x => x.Calendar.TimeZone).FirstOrDefault();

                if (appointment != null)
                {
                    var oldAppoitmentDate = appointment.StartTime.ToString("dd-MM-yyyy") + " at " + appointment.StartTime.ToString("h:mm");
                    var oldCalendarId = appointment.CalendarId;
                    appointment.CalendarId = model.CalendarId;
                    appointment.StartTime = model.StartTime;
                    appointment.Notes = model.Notes;
                    appointment.AppointmentTypeId = model.AppointmentTypeId;
                    appointment.PersonId = model.ClientId;

                    using (SqlConnection con = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                    {
                        con.Open();
                        SqlCommand command = new SqlCommand($"delete from AppointmentAddon where AppointmentId={appointment.AppointmentId}", con);
                        command.ExecuteNonQuery();
                    }

                    var appointmentType = this._appointmentTypeRepository.Get(s => s.AppointmentTypeId == appointment.AppointmentTypeId).ConfigureAwait(false).GetAwaiter().GetResult();

                    int duration = 0;
                    if (appointmentType != null && appointmentType.AppointmentTypeAddons?.Count > 0)
                    {
                        duration = appointmentType.Duration;
                        appointment.AppointmentAddon = new List<AppointmentAddon>();
                        if (model.SelectedAddOns != null && model.SelectedAddOns.Count > 0)
                        {
                            foreach (var addon in appointmentType.AppointmentTypeAddons)
                            {
                                var isExist = model.SelectedAddOns.Any(x => x.Id == addon.AddonId);
                                if (isExist)
                                {
                                    duration += addon.Addon.Duration;
                                }
                            }

                            foreach (var selectedAddOnId in model.SelectedAddOns)
                            {
                                //get the existing appointment type
                                var appointmentAddon = new AppointmentAddon();
                                appointmentAddon.AddonId = selectedAddOnId.Id;
                                appointmentAddon.AppointmentId = appointment.AppointmentId;
                                this._dbContext.AppointmentAddon.Add(appointmentAddon);
                            }
                        }
                    }
                    appointment.EndTime = appointment.StartTime.AddMinutes(duration);
                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appointment.Calendar.TimeZone.SystemTimeZoneId);

                    appointment.StartTime = TimeZoneInfo.ConvertTimeToUtc(appointment.StartTime, userTimeZone);
                    appointment.EndTime = TimeZoneInfo.ConvertTimeToUtc(appointment.EndTime, userTimeZone);
                    this._dbContext.SaveChanges();

                    appointment = this.GetAppointmentEntity(appointment.AppointmentId);
                    if (oldCalendarId == appointment.CalendarId)
                    {
                        _appointmentEmailService.SendUpdateConfirmationMailToTeamMember(appointment, oldAppoitmentDate);
                    }
                    else
                    {
                        _appointmentEmailService.SendAppointmentCancelledEmailToTeamMember(appointment, oldAppoitmentDate);
                        _appointmentEmailService.SendConfirmationMailToTeamMember(appointment);
                    }
                    if (model.SendMail) SendConfirmationMailToClient(appointment, oldAppoitmentDate, 4);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public AppointmentModel GetAppointment(long appointmentId)
        {
            try
            {
                var appointment = (from a in this._dbContext.Appointment.Include(a => a.AppointmentAddon).Include(a => a.Calendar).Include(a => a.Person).Include(a => a.Person.UserRole).Include(a => a.AppointmentAddon).Include(a => a.Calendar.TimeZone).Include(s => s.AppointmentType).Include(x => x.AppointmentType.AppointmentTypeCategory).Include(s => s.AppointmentType.Product)
                                   where a.AppointmentId == appointmentId
                                   select a
                          ).FirstOrDefault();

                var appointmentModel = new AppointmentModel();
                if (appointment != null)
                {
                    appointmentModel.AppointmentStatus = appointment.AppointmentStatus;
                    appointmentModel.AppointmentId = appointment.AppointmentId;
                    appointmentModel.AppointmentName = appointment.AppointmentType.Product.Name;
                    appointmentModel.AppointmentTypeId = appointment.AppointmentTypeId;
                    appointmentModel.CalendarName = appointment.Calendar.Name;
                    appointmentModel.CalendarId = appointment.CalendarId;
                    appointmentModel.AppointmentCreatedById = appointment.AppointmentCreatedById;
                    appointmentModel.CreationDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(appointment.CreationDate, appointment.Calendar.TimeZone.SystemTimeZoneId);
                    appointmentModel.StartTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(appointment.StartTime, appointment.Calendar.TimeZone.SystemTimeZoneId);
                    appointmentModel.EndTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(appointment.EndTime, appointment.Calendar.TimeZone.SystemTimeZoneId);
                    appointmentModel.StartTimeUTC = appointment.StartTime;
                    appointmentModel.EndTimeUTC = appointment.EndTime;
                    appointmentModel.ClientTimeZoneId = appointment.ClientTimeZoneId;
                    appointmentModel.Notes = appointment.Notes;
                    appointmentModel.Color = appointment.AppointmentType.Color;
                    appointmentModel.Description = appointment.AppointmentType.Product.Description;
                    appointmentModel.IsCancelled = appointment.IsCancelled;
                    appointmentModel.IsNoShow = appointment.IsNoShow;
                    appointmentModel.ClientId = appointment.PersonId;
                    appointmentModel.TeammateScheduledId = appointment.Calendar.PersonId;
                    appointmentModel.IsBlockedOffTime = appointment.IsBlockedOffTime;
                    appointmentModel.LastUpdated = appointment.LastUpdated;
                    appointmentModel.HasBeenCheckedOut = appointment.HasBeenCheckedOut;
                    appointmentModel.CalendarName = appointment.Calendar.Name;
                    appointmentModel.SelectedAddOns = (from s in this._dbContext.AppointmentAddon
                                                       join a in this._dbContext.Addon
                                                       on s.AddonId equals a.AddonId
                                                       join p in this._dbContext.Product
                                                       on a.ProductId equals p.ProductId
                                                       where s.AppointmentId == appointmentId
                                                       select new AddOnModel
                                                       {
                                                           Id = s.AddonId,
                                                           Duration = a.Duration,
                                                           IsVisible = a.IsVisible,
                                                           ProductId = a.ProductId,
                                                           ProductModel = new ProductModel
                                                           {
                                                               ServiceName = p.Name,
                                                               Price = p.Price,
                                                               ProductId = p.ProductId
                                                           }
                                                       }).ToList();

                    if (appointment.AppointmentTimeSeriesId.HasValue)
                    {
                        appointmentModel.AppointmentTimeSeriesId = appointment.AppointmentTimeSeriesId;
                        var appointmentSeries = (from a in this._dbContext.AppointmentSeries
                                                 where a.AppointmentTimeSeriesId == appointment.AppointmentTimeSeriesId.Value
                                                 select a).FirstOrDefault();

                        if (appointmentSeries != null)
                        {
                            appointmentModel.AppointmentSeriesModel = new SeriesModel
                            {
                                AppointmentTimeSeriesId = appointment.AppointmentTimeSeriesId.Value,
                                CalendarId = appointment.CalendarId,
                                CreatedDate = appointmentSeries.CreatedDate,
                                LastUpdatedDate = appointmentSeries.LastUpdatedDate,
                                RecurUntil = appointmentSeries.RecurUntil,
                                TimePeriodId = appointmentSeries.TimePeriodId
                            };
                        }
                    }

                    if (appointment.PersonId.HasValue)
                    {
                        appointmentModel.Client = new PersonModel
                        {
                            PersonId = appointment.PersonId.Value,
                            FirstName = appointment.Person.FirstName,
                            LastName = appointment.Person.LastName,
                            Phone = appointment.Person.Phone,
                            EmailAddress = appointment.Person.EmailAddress,
                            RoleId = appointment.Person.UserRoleId,
                            UserRoleName = appointment.Person.UserRole.UserRoleName
                        };
                    }
                    else
                    {
                        appointmentModel.Client = new PersonModel();
                    }

                    long? appointmentCategoryId = null;
                    string categoryName = string.Empty;

                    if (appointment.AppointmentType.AppointmentTypeCategoryId.HasValue)
                    {
                        appointmentCategoryId = appointment.AppointmentType.AppointmentTypeCategoryId.Value;
                        categoryName = appointment.AppointmentType.AppointmentTypeCategory.Name;
                    }
                    appointmentModel.AppointmentType = new AppointmentTypeModel
                    {
                        AccessLevelId = appointment.AppointmentType.AccessLevelId,
                        Color = appointment.AppointmentType.Color,
                        CompanyId = appointment.AppointmentType.Product.CompanyID,
                        AppointmentTypeCategoryId = appointmentCategoryId,
                        AppointmentTypeCategoryName = categoryName,
                        AppointmentTypeId = appointment.AppointmentType.AppointmentTypeId,
                        BlockedOffMinutesAfterAppointment = appointment.AppointmentType.BlockedOffMinutesAfterAppointment,
                        BlockedOffMinutesBeforeAppointment = appointment.AppointmentType.BlockedOffMinutesBeforeAppointment,
                        ConfirmationMessage = appointment.AppointmentType.ConfirmationMessage,
                        DurationInMinutes = appointment.AppointmentType.Duration,
                        AppointmentTypeName = appointment.AppointmentType.Product.Name,
                        ProductId = appointment.AppointmentType.ProductId,
                        ProductModel = new ProductModel
                        {
                            ProductId = appointment.AppointmentType.Product.ProductId,
                            CompanyID = appointment.AppointmentType.Product.CompanyID,
                            Description = appointment.AppointmentType.Product.Description,
                            ImageUrl = appointment.AppointmentType.Product.ImageUrl,
                            Price = appointment.AppointmentType.Product.Price,
                            CreatedDate = appointment.AppointmentType.Product.CreatedDate,
                            ServiceName = appointment.AppointmentType.Product.Name
                        },
                    };
                }
                return appointmentModel;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AppointmentViewModel> GetAppointments(long companyId, string dateTime)
        {
            try
            {
                int year = DateTime.Now.Year;
                DateTime date = Convert.ToDateTime(dateTime);
                List<Appointment> appointments = this._appointmentRepository.GetAll(s => s.AppointmentType.Product.CompanyID == companyId && s.StartTime.Year == date.Year && s.StartTime.Month == date.Month).ConfigureAwait(false).GetAwaiter().GetResult();
                return appointments.Select(s => new AppointmentViewModel
                {
                    Id = s.AppointmentId,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    ProductName = s.AppointmentType.Product.Name,
                    ClientName = s.Person.FirstName + " " + s.Person.LastName,
                    Duration = s.AppointmentType.Duration,
                    Price = s.AppointmentType.Product.Price,
                    TeamMember = s.Calendar.Name,
                    Addons = s.AppointmentAddon?.Where(x => x.Addon.IsVisible).Select(s => new AddOnModel()
                    {
                        ProductName = s.Addon.Product.Name,
                        ProductId = s.Addon.Product.ProductId,
                        Duration = s.Addon.Duration,
                        Price = s.Addon.Product.Price
                    }).ToList()
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void UpdateAppointment(AppointmentViewModel model)
        {
            try
            {
                Appointment appointment = new Appointment()
                {
                    AppointmentId = model.Id,
                    CalendarId = model.CalendarId,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Notes = model.Notes,
                    AppointmentTypeId = model.AppointmentTypeId,
                    PersonId = model.PersonId,
                    IsCancelled = false,
                    IsNoShow = false,
                    IsBlockedOffTime = false,
                    HasBeenCheckedOut = false,
                    IsAccepted = true,
                    IsApproved = 1,
                    AppointmentStatus = 1,
                    LastUpdated = DateTime.Now
                };
                this._appointmentRepository.Update(appointment).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private bool SendConfirmationMailToClient(Appointment appointment, string oldAppointmentDateTime, int companyEmailTypeId)
        {
            try
            {
                Company company = this._companyRepository.Get(s => s.CompanyId == appointment.AppointmentType.Product.CompanyID).ConfigureAwait(false).GetAwaiter().GetResult();
                string covidLink = $"Please click <a href='{this._configuration.GetSection("rootUrl").Value}/CovidScreeningQuestionnaire.aspx?companyId={company.CompanyId}&personId={appointment.PersonId}'>here</a> to fill out your Covid Screening form.";
                string rootUrl = this._configuration.GetSection("rootUrl").Value;

                AppointmentConfirmationClientModel clientModel = new AppointmentConfirmationClientModel()
                {
                    AppointmentDate = appointment.StartTime.ToString("dddd,  MMMM dd, yyyy hh:mm:ss tt"),
                    //AppointmentTypeName=test.
                    OldAppointmentDate = !String.IsNullOrEmpty(oldAppointmentDateTime) ? oldAppointmentDateTime : string.Empty,
                    ClientFirstName = appointment.Person.FirstName,
                    ClientLastName = appointment.Person.LastName,
                    ClientName = appointment.Person.FirstName + " " + appointment.Person.LastName,
                    CompanyName = company.Name,
                    CompanyNumber = company.PhoneNumber,
                    CovidFormLink = covidLink,
                    CompanyLogo = string.IsNullOrEmpty(company.Logo) ? "https://appthero.com/assets/images/cbh.png" : "https://appthero.com/" + company.Logo
                    // GeneralFormLink=

                };
                int? timeZoneId = null;
                if (appointment.ClientTimeZoneId.HasValue) timeZoneId = appointment.ClientTimeZoneId;
                else timeZoneId = appointment?.Calendar?.TimeZoneId;

                Repositories.Context.Entities.TimeZone timeZone = this._timeZoneRepository.Get(s => s.TimeZoneId == timeZoneId).ConfigureAwait(false).GetAwaiter().GetResult();
                clientModel.AppointmentDate += " " + timeZone?.SystemTimeZoneId == null ? " Eastern Standard Time" : timeZone?.SystemTimeZoneId;

                clientModel.ServiceName = appointment.AppointmentType.Product.Name;


                if (company.CompanySetting.ShouldSendCovid19Form)
                {
                    string ovidAdhrefLink = $"Please click <a href='{rootUrl}/CovidScreeningQuestionnaire.aspx?companyId={appointment.Person.PersonCompany.CompanyId}&personId={appointment.PersonId}'>here</a> to fill out your Covid Screening form.";
                    clientModel.CovidScreeningLink = ovidAdhrefLink;
                }
                if (company.CompanySetting.ShouldSendIntakeFormToNewClients)
                {
                    List<Appointment> appointments = this._appointmentRepository.GetAll(s => s.PersonId == appointment.PersonId).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (appointments.Count == 1)
                    {
                        IntakeFormTemplateAppointmentTypes templateAppointmentTypes = this._intakeFormApptTypeRepository.Get(s => s.AppointmentTypeId == appointment.AppointmentTypeId && s.IntakeFormTemplate.CompanyId == appointment.Person.PersonCompany.CompanyId && s.IntakeFormTemplate.IsArchived == false).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (templateAppointmentTypes != null)
                        {
                            clientModel.IntakeFormLink = $"Please click <a href='{rootUrl}/ClientIntakeForm.aspx?companyId={appointment.Person.PersonCompany.CompanyId}&personId={appointment.PersonId}&intakeFormId={templateAppointmentTypes.IntakeFormId}'>here</a> to fill out your Intake form.";
                        }
                    }
                }
                if (company.CompanySetting.ShouldSendPreScreeningForm)
                {
                    //todo fix this issue
                    //GeneralForm generalForm = this._generalFormRepository.Get(s => s.GeneralFormAppointmentTypes.AppointmentTypeId == appointment.AppointmentTypeId && s.CompanyId == appointment.Person.PersonCompany.CompanyId && s.IsArchived == false).ConfigureAwait(false).GetAwaiter().GetResult();
                    //if (generalForm != null && generalForm.GeneralFormId != 0)
                    //{
                    //    clientModel.GeneralFormLink = $"Please click <a href='{rootUrl}/ClientGeneralForm.aspx?companyId={appointment.Person.PersonCompany.CompanyId}&personId={appointment.PersonId}&generalFormId={generalForm.GeneralFormId}'>here</a> to fill out your {generalForm.Name} for your {appointment.AppointmentType.Product.Name} appointment.";
                    //}
                }



                string key = $"{appointment.AppointmentId}{appointment.StartTime}".HashString();
                clientModel.RescheduleCancellationLink = $"<a href='{rootUrl}/LandingPageAppointments.aspx?appointmentId={appointment.AppointmentId}&companyId={appointment.Person.PersonCompany.CompanyId}&key={key}'> Cancel/Reschedule</a>";
                CompanyEmailSetting companyEmail = this._companyEmailSettingReposiory.Get(s => s.CompanyId == company.CompanyId && s.CompanyEmailTypeId == companyEmailTypeId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (companyEmail != null)
                {
                    string subject = clientModel.CreateMailBody(companyEmail.Subject);

                    string messageBody = string.Empty;
                    if (companyEmailTypeId == 4)
                    {
                        messageBody = this._eMailHelper.GetTemplate(clientModel._htmlRescheduledFileName);
                    }
                    else
                    {
                        messageBody = this._eMailHelper.GetTemplate(clientModel._htmlFileName);
                    }
                    messageBody = messageBody.Replace("#EmailBody", companyEmail.Body);
                    messageBody = clientModel.CreateMailBody(messageBody);

                    EmailModel emailModel = new EmailModel()
                    {
                        Subject = subject,
                        Body = messageBody,
                        Recipient = appointment.Person.EmailAddress,
                        ReplyToListEmail = company.Email,
                        ReplyToListName = company.Name
                    };
                    return this._eMailHelper.SendMail(emailModel);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //public AppointmentList GetAppointments(long companyId, int? teamMemberId, DateTime sDate, DateTime eDate)
        //{
        //    try
        //    {
        //        List<Appointment> appointments = new List<Appointment>();

        //        var model = (from a in this._dbContext.Appointment
        //                     .Include(a => a.Calendar)
        //                     .Include(a => a.AppointmentAddon)
        //                     .Include(a => a.Calendar.TimeZone)
        //                     .Include(s => s.AppointmentType)
        //                     .Include(s => s.AppointmentType.Product)
        //                     join p in this._dbContext.Person
        //                     on a.PersonId equals p.PersonId
        //                     join pc in this._dbContext.PersonCompany
        //                     on p.PersonId equals pc.PersonId
        //                     join c in this._dbContext.Company
        //                     on pc.CompanyId equals c.CompanyId
        //                     where pc.CompanyId == companyId && a.IsCancelled == false
        //                     && a.AppointmentStatus == ((int)AppointmentStatus.Approved)
        //                     && a.StartTime.Date >= sDate.Date && a.EndTime.Date <= eDate.Date
        //                     select a
        //                     );

        //        //get all the calendarIds for the company
        //        List<long> calendarIds = this._dbContext.CompanyCalendar.Include(a => a.Calendar).Where(c => c.CompanyId == companyId).Select(c => c.CalendarId)
        //            .ToList();
        //        var blockedOffTime = (from b in this._dbContext.BlockedOffTime.Include(a => a.Calendar)
        //                              where b.ExceptionDateFrom.Date >= sDate.Date && b.ExceptionDateTo.Date <= eDate.Date &&
        //                              calendarIds.Contains(b.CalendarId)
        //                              select b);

        //        var overrideAvailability = (from o in this._dbContext.OverrideAvailability.Include(a => a.Calendar)
        //                                    where o.OverrideFromDate.Date >= sDate.Date && o.OverrideToDate <= eDate.Date &&
        //                                    calendarIds.Contains(o.CalendarId)
        //                                    select o);

        //        if (teamMemberId.HasValue)
        //        {
        //            model = model.Where(x => x.CalendarId == teamMemberId.Value);
        //            blockedOffTime = blockedOffTime.Where(x => x.CalendarId == teamMemberId.Value);
        //            overrideAvailability = overrideAvailability.Where(x => x.CalendarId == teamMemberId.Value);
        //        }

        //        var modelResult = (from a in model
        //                           select new AppointmentViewModel()
        //                           {
        //                               Id = a.AppointmentId,
        //                               StartTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.StartTime, a.Calendar.TimeZone.SystemTimeZoneId),
        //                               EndTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.EndTime, a.Calendar.TimeZone.SystemTimeZoneId),
        //                               ProductName = a.AppointmentType.Product.Name,
        //                               ClientName = a.Person.FirstName + " " + a.Person.LastName,
        //                               Duration = a.AppointmentType.Duration,
        //                               Price = a.AppointmentType.Product.Price,
        //                               TeamMember = a.Calendar.Name,
        //                               CalendarId = a.Calendar.CalendarId,
        //                               TeamMemberId = a.CalendarId,
        //                               Color = a.AppointmentType.Color,
        //                               Addons = a.AppointmentAddon.Where(x => x.Addon.IsVisible).Select(s => new AddOnModel()
        //                               {
        //                                   ProductName = s.Addon.Product.Name,
        //                                   ProductId = s.Addon.Product.ProductId,
        //                                   Duration = s.Addon.Duration,
        //                                   Price = s.Addon.Product.Price
        //                               }).ToList()
        //                           }).ToList();

        //        var blockTimeOffModels = (from b in blockedOffTime
        //                                  select new BlockedOffTimeModel()
        //                                  {
        //                                      BlockOffTimeId = b.BlockedOffTimeId,
        //                                      CalendarId = b.CalendarId,
        //                                      Color = b.Color,
        //                                      Description = b.Description,
        //                                      Notes = b.Notes,
        //                                      StartFromDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(b.ExceptionDateFrom, b.Calendar.TimeZone.SystemTimeZoneId),
        //                                      EndToDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(b.ExceptionDateTo, b.Calendar.TimeZone.SystemTimeZoneId),
        //                                      StartToDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(b.ExceptionDateTo, b.Calendar.TimeZone.SystemTimeZoneId),
        //                                      BlockedOffTimeSeriesId = b.BlockedOffTimeSeriesId,
        //                                  }).ToList();

        //        var overrideAvailabilityModels = (from o in overrideAvailability
        //                                          select new OverrideAvailabilityModel()
        //                                          {
        //                                              CalendarId = o.CalendarId,
        //                                              LastUpdated = o.LastUpdated,
        //                                              OverrideFromDate = o.OverrideFromDate,
        //                                              OverrideToDate = o.OverrideToDate,
        //                                              OverrideId = o.OverrideId
        //                                          }).ToList();

        //        return new AppointmentList
        //        {
        //            AppointmentViewModel = modelResult,
        //            BlockedOffTimeModel = blockTimeOffModels,
        //            OverrideAvailabilityModel = overrideAvailabilityModels
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        public AppointmentList GetAppointments(long companyId, long personId, long? teamMemberId, DateTime sDate, DateTime eDate)
        {
            try
            {
                // Get the CalendarIds associated with the given companyId
                var calendarIds = _dbContext.CompanyCalendar
                    .Where(cc => cc.CompanyId == companyId)
                    .Select(cc => cc.CalendarId)
                    .ToList();

       

                var company = _dbContext.Company.Find(companyId);
                var showBufferTimesOnCalendar = false; // company.CompanySetting.ShowBufferTimesOnCalendar;
                bool isColorCodingByService = false;
                var personCalendar = _dbContext.Calendar.Where(c => c.Person.PersonId == personId).SingleOrDefault();
                if (personCalendar != null)
                {
                   // isColorCodingByService = personCalendar.Person.IsColorCodingByService;
                }

                var query = _dbContext.Appointment
                    .Where(a => calendarIds.Contains(a.CalendarId)  // Filter by CalendarId
                                && !a.IsCancelled
                                && a.AppointmentStatus == (int)AppointmentStatus.Approved
                                && a.StartTime >= sDate && a.EndTime <= eDate);

                if (teamMemberId.HasValue)
                {
                    query = query.Where(a => a.CalendarId == teamMemberId.Value);
                }

                // Apply the Includes after filtering
                query = query
                    .Include(a => a.Calendar)
                        .ThenInclude(cal => cal.TimeZone)
                    .Include(a => a.Calendar)
                        .ThenInclude(cal => cal.Person)
                    .Include(a => a.AppointmentType)
                        .ThenInclude(at => at.Product)
                    .Include(a => a.Person);

             

                var modelResult = query.Select(a => new AppointmentViewModel
                {
                    Id = a.AppointmentId,
                    StartTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.StartTime, a.Calendar.TimeZone.SystemTimeZoneId),
                    EndTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.EndTime, a.Calendar.TimeZone.SystemTimeZoneId),
                    ProductName = a.AppointmentType.Product.Name,
                    ClientName = $"{a.Person.FirstName} {a.Person.LastName}",
                    AppointmentTypeId = a.AppointmentTypeId,
                    Duration = a.AppointmentType.Duration,
                    Price = a.AppointmentType.Product.Price,
                    TeamMember = a.Calendar.Name,
                    CalendarId = a.Calendar.CalendarId,
                    TeamMemberId = a.CalendarId,
                    Color = a.AppointmentType.Color,
                    PersonId = a.PersonId.Value,
                    CreationDate = a.CreationDate,
                    AppointmentTimeSeriesId = a.AppointmentTimeSeriesId.Value,
                    Notes = a.Notes,
                    Addons = a.AppointmentAddon
                        .Where(x => x.Addon.IsVisible)
                        .Select(s => new AddOnModel
                        {
                            ProductName = s.Addon.Product.Name,
                            ProductId = s.Addon.Product.ProductId,
                            Duration = s.Addon.Duration,
                            Price = s.Addon.Product.Price
                        }).ToList()
                }).ToList();

                var blockedOffTime = _dbContext.BlockedOffTime
                    .Where(b => b.ExceptionDateFrom >= sDate && b.ExceptionDateTo <= eDate
                                && calendarIds.Contains(b.CalendarId))
                    .Select(b => new BlockedOffTimeModel
                    {
                        BlockOffTimeId = b.BlockedOffTimeId,
                        CalendarId = b.CalendarId,
                        Color = b.Color,
                        Description = b.Description,
                        Notes = b.Notes,
                        StartFromDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(b.ExceptionDateFrom, b.Calendar.TimeZone.SystemTimeZoneId),
                        EndToDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(b.ExceptionDateTo, b.Calendar.TimeZone.SystemTimeZoneId),
                        StartToDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(b.ExceptionDateTo, b.Calendar.TimeZone.SystemTimeZoneId),
                        BlockedOffTimeSeriesId = b.BlockedOffTimeSeriesId,
                    }).ToList();

                var overrideAvailability = _dbContext.OverrideAvailability
                    .Where(o => o.OverrideFromDate >= sDate && o.OverrideToDate <= eDate
                                && calendarIds.Contains(o.CalendarId))
                    .Select(o => new OverrideAvailabilityModel
                    {
                        CalendarId = o.CalendarId,
                        LastUpdated = o.LastUpdated,
                        OverrideFromDate = o.OverrideFromDate,
                        OverrideToDate = o.OverrideToDate,
                        OverrideId = o.OverrideId
                    }).ToList();

                return new AppointmentList
                {
                    ShowBufferTimesOnCalendar = showBufferTimesOnCalendar,
                    IsColorCodingByService = isColorCodingByService,
                    AppointmentViewModel = modelResult,
                    BlockedOffTimeModel = blockedOffTime,
                    OverrideAvailabilityModel = overrideAvailability
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Tuple<bool, string> CancelAppointment(long appointmentId, long cancelledById, bool sendMail, bool? isNoShow = null, string notes = null)
        {
            try
            {
                Appointment appointment = this._appointmentRepository.Get(s => s.AppointmentId == appointmentId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!appointment.IsCancelled)
                {
                    appointment.IsCancelled = true;
                    appointment.PersonCancellingAppointmentId = cancelledById;

                    if (isNoShow.HasValue) appointment.IsNoShow = isNoShow.Value;
                    if (!string.IsNullOrEmpty(notes)) appointment.Notes = notes;

                    if (this._appointmentRepository.Update(appointment).ConfigureAwait(false).GetAwaiter().GetResult())
                    {
                        if (!sendMail) return new Tuple<bool, string>(true, "Appointment cancelled");
                        if (sendMail) this._appointmentEmailService.SendCancellationEmailToClient(appointment);

                        Person person = this._personRepository.Get(s => s.PersonId == appointment.PersonId).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (person.EmailNotificationPreferenceId == (int)NotificationPreferenceEnum.SendEmailAlerts)
                        {
                            this._appointmentEmailService.SendCancellationEmailToAdmin(appointment);
                        }
                    }
                    return new Tuple<bool, string>(true, "Appointment cancelled");
                }
                return new Tuple<bool, string>(false, "Appointment already cancelled"); ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int GetNumberOfPendingAppointments(long companyId)
        {

            var pendingAppointments = (from c in this._dbContext.Company
                                       join cc in this._dbContext.CompanyCalendar
                                       on c.CompanyId equals cc.CompanyId
                                       join ca in this._dbContext.Calendar
                                       on cc.CalendarId equals ca.CalendarId
                                       join a in this._dbContext.Appointment
                                       on ca.CalendarId equals a.CalendarId
                                       where c.CompanyId == companyId && a.AppointmentStatus == 2
                                       select a
                            ).Count();

            return pendingAppointments;
        }

        public List<PendingAppointmentModel> GetPendingAppointments(long companyId)
        {
            List<PendingAppointmentModel> appointmentModels = new List<PendingAppointmentModel>();
            try
            {

                appointmentModels = (from c in this._dbContext.Company
                                     join cc in this._dbContext.CompanyCalendar
                                     on c.CompanyId equals cc.CompanyId
                                     join ca in this._dbContext.Calendar.Include(a => a.TimeZone)
                                     on cc.CalendarId equals ca.CalendarId
                                     join a in this._dbContext.Appointment.Include(x => x.Person).Include(x => x.AppointmentType).Include(x => x.AppointmentType.Product)
                                     on ca.CalendarId equals a.CalendarId
                                     where c.CompanyId == companyId && a.AppointmentStatus == 2
                                     select new PendingAppointmentModel
                                     {
                                         AppointmentId = a.AppointmentId,
                                         CalendarName = ca.Name,
                                         ClientId = a.PersonId,
                                         ClientName = a.Person.FirstName + " " + a.Person.LastName,
                                         StartTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.StartTime, ca.TimeZone.SystemTimeZoneId),
                                         AppointmentName = a.AppointmentType.Product.Name,
                                         IsFirstTimeAppointment = this._dbContext.Appointment.Where(a => a.PersonId == a.PersonId).Count() == 1

                                     }).ToList();

                foreach (var x in appointmentModels)
                {
                    x.MostRecentIntakeFormDate = GetClientMostRecentIntakeFormDate(Convert.ToInt32(x.ClientId), companyId);
                    x.MostRecentPrescreeningFormSubmission = GetClientMostRecentGeneralFormDate(Convert.ToInt32(x.ClientId), companyId);

                }
            }
            catch (Exception ex)
            {

            }

            return appointmentModels;
        }

        public string GetClientMostRecentIntakeFormDate(int personId, long companyId)
        {
            var model = GetClientMostRecentIntakeForm(personId, companyId);
            if (model != null)
            {
                if (model.SubmissionDate != DateTime.MinValue)
                {
                    return model.SubmissionDate.ToShortDateString();
                }
            }

            return "---";
        }

        public IntakeFormClientSubmission GetClientMostRecentIntakeForm(int personId, long companyId)
        {
            var q = this._dbContext.IntakeFormClientSubmission
                                 .Where(c => c.PersonId == personId && c.CompanyId == companyId)
                                 .OrderByDescending(t => t.SubmissionDate)
                                 .FirstOrDefault();
            return q;
        }

        public string GetClientMostRecentGeneralFormDate(int personId, long companyId)
        {
            var model = this.GetClientMostRecentGeneralForm(personId, companyId);
            if (model != null)
            {
                if (model.SubmissionDate != DateTime.MinValue)
                {
                    return model.SubmissionDate.ToShortDateString();
                }
            }

            return "---";
        }

        public GeneralFormClientSubmission GetClientMostRecentGeneralForm(int personId, long companyId)
        {
            var q = this._dbContext.GeneralFormClientSubmission
                                .Where(c => c.PersonId == personId && c.CompanyId == companyId)
                                .OrderByDescending(t => t.SubmissionDate)
                                .FirstOrDefault();
            return q;
        }

        public bool UpdateAppointmentStatus(int appointmentId, int statusId)
        {
            try
            {
                var appointment = this._appointmentRepository.Get(s => s.AppointmentId == appointmentId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (appointment == null) return false;
                appointment.AppointmentStatus = statusId;
                return this._appointmentRepository.Update(appointment).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool AccpetRejectPendingAppointment(int companyId, int appointmentId, int statusId, bool sendEmail)
        {
            try
            {
                var appointment = this._appointmentRepository.Get(s => s.AppointmentId == appointmentId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (appointment == null) return false;
                appointment.AppointmentStatus = statusId;
                var value = this._appointmentRepository.Update(appointment).ConfigureAwait(false).GetAwaiter().GetResult();
                if (sendEmail)
                {
                    if (statusId == 1)
                    {
                        _appointmentEmailService.SendAcceptPendingAppointmentEmail(appointment, companyId);
                    }
                    else
                    {
                        _appointmentEmailService.SendDeclinePendingAppointmentEmail(appointment, companyId);
                    }
                }
                return value;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool MarkNoShow(long appointmentId)
        {
            try
            {
                Appointment appointment = this._appointmentRepository.Get(s => s.AppointmentId == appointmentId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (!appointment.IsNoShow)
                {
                    appointment.IsNoShow = true;
                    return this._appointmentRepository.Update(appointment).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                return false;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CalendarModel> GetTeamMembers(long companyId)
        {
            try
            {
                var calendars = (from s in this._dbContext.Calendar.Include(x => x.Person).Include(x => x.HoursAvailability)
                                 join cc in this._dbContext.CompanyCalendar
                                 on s.CalendarId equals cc.CalendarId
                                 where cc.CompanyId == companyId && !s.Person.IsArchived &&
                                 s.Person.IsAccountEnabled && !s.Person.IsAccountBanned
                                 select s).ToList();
                List<CalendarModel> calendarModels = calendars.Select(c => new CalendarModel
                {
                    CalendarId = c.CalendarId,
                    CalendarName = c.Name,
                    TimeZoneId = c.TimeZoneId,
                    PersonModel = new PersonModel
                    {
                        FirstName = c.Person.FirstName,
                        LastName = c.Person.LastName,
                        PersonId = c.PersonId,
                        AppointmentColor = c.Person.AppointmentColor,
                        ShowOnCalendar = c.Person.ShowOnCalendar,
                        ShowOnOnlineSchedule = c.Person.ShowOnOnlineSchedule,
                    },
                    BusinessHoursModels = c.HoursAvailability.Select(h => new BusinessHoursModel
                    {
                        BusinessHoursId = h.HoursOfOperationId,
                        CalendarId = h.CalendarId,
                        StartTime = h.StartTime,
                        Day = h.Day,
                        EndTime = h.EndTime
                    }).ToList()
                }).ToList();
                return calendarModels;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<CalendarModel> GetTeamMembers(int companyId, int clientId)
        {
            try
            {
                List<CalendarModel> calendarModels = new List<CalendarModel>();
                DateTime currentDate = DateTime.UtcNow;
                int days = currentDate.DayOfWeek - DayOfWeek.Sunday;
                DateTime weekStart = currentDate.AddDays(-days);
                DateTime weekEnd = weekStart.AddDays(6);

                var calendars = (from s in this._dbContext.Calendar.Include(x => x.Person)
                                 join cc in this._dbContext.CompanyCalendar
                                 on s.CalendarId equals cc.CalendarId
                                 where cc.CompanyId == companyId
                                 select s).ToList();

                var permissions = (from s in this._dbContext.Permission
                                   join p in this._dbContext.TeamMemberPermission
                                   on s.PermissionId equals p.PermissionId
                                   where p.PersonId == clientId
                                   select s).FirstOrDefault();


                var person = this._personRepository.Get(x => x.PersonId == clientId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (person != null && calendars.Count > 0)
                {
                    var c = calendars.ElementAt(0);
                    var timeZone = this._timeZoneRepository.Get(x => x.TimeZoneId == c.TimeZoneId);
                    if (timeZone != null)
                    {
                        //show all calendars
                        if (person.UserRole.UserRoleName == "Admin")
                        {
                            calendarModels = (from calendar in calendars
                                              select new CalendarModel
                                              {
                                                  CalendarId = calendar.CalendarId,
                                                  CalendarName = calendar.Name,
                                                  EmployeeId = calendar.PersonId,
                                                  TimeZoneId = calendar.TimeZoneId,
                                                  Description = calendar.Description,
                                                  PersonModel = new PersonModel
                                                  {
                                                      PersonId = calendar.PersonId,
                                                      FirstName = calendar.Person.FirstName,
                                                      LastName = calendar.Person.LastName,
                                                      IsAccountBanned = calendar.Person.IsAccountBanned,
                                                      IsAccountEnabled = calendar.Person.IsAccountEnabled,
                                                      ShowOnCalendar = calendar.Person.ShowOnCalendar,
                                                      ShowOnOnlineSchedule = calendar.Person.ShowOnOnlineSchedule,
                                                      AppointmentColor = calendar.Person.AppointmentColor,
                                                  },
                                              }).Where(c => c.PersonModel.IsAccountEnabled && !c.PersonModel.IsAccountBanned).ToList();
                        }
                        //only show the calendar for this employee
                        else if (person.UserRole.UserRoleName == "Employee")
                        {
                            var permissonName = permissions.PermissionName;
                            if (permissonName == "Full Access" || permissonName == "Access to all schedules, cannot make changes" || permissonName == "Access to all schedules")
                            {
                                calendarModels = (from calendar in calendars
                                                  select new CalendarModel
                                                  {
                                                      CalendarId = calendar.CalendarId,
                                                      CalendarName = calendar.Name,
                                                      EmployeeId = calendar.PersonId,
                                                      TimeZoneId = calendar.TimeZoneId,
                                                      Description = calendar.Description,

                                                      PersonModel = new PersonModel
                                                      {
                                                          PersonId = calendar.PersonId,
                                                          FirstName = calendar.Person.FirstName,
                                                          LastName = calendar.Person.LastName,
                                                          IsAccountBanned = calendar.Person.IsAccountBanned,
                                                          IsAccountEnabled = calendar.Person.IsAccountEnabled,

                                                      }
                                                  }).Where(c => c.PersonModel.IsAccountEnabled && !c.PersonModel.IsAccountBanned).ToList();
                            }
                            else if (permissonName == "Access to own schedule" || permissonName == "Access to own schedule, cannot make changes")
                            {
                                var employeeCalendar = calendars.Where(c => c.PersonId == person.PersonId);
                                calendarModels = (from calendar in employeeCalendar
                                                  select new CalendarModel
                                                  {
                                                      CalendarId = calendar.CalendarId,
                                                      CalendarName = calendar.Name,
                                                      EmployeeId = calendar.PersonId,
                                                      TimeZoneId = calendar.TimeZoneId,
                                                      Description = calendar.Description,
                                                      PersonModel = new PersonModel
                                                      {
                                                          PersonId = calendar.PersonId,
                                                          FirstName = calendar.Person.FirstName,
                                                          LastName = calendar.Person.LastName,
                                                          IsAccountBanned = calendar.Person.IsAccountBanned,
                                                          IsAccountEnabled = calendar.Person.IsAccountEnabled,

                                                      },
                                                  }
                                                  ).ToList();
                            }
                        }
                    }
                }
                return calendarModels;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public List<AddOnModel> GetUpgradesByAppointmentTypeId(int appointmentTypeId)
        {
            try
            {
                var addons = (from aT in this._dbContext.AppointmentType
                              join aTA in this._dbContext.AppointmentTypeAddon
                              on aT.AppointmentTypeId equals aTA.AppointmentTypeId
                              join a in this._dbContext.Addon
                              on aTA.AddonId equals a.AddonId
                              join p in this._dbContext.Product
                              on a.ProductId equals p.ProductId
                              where aT.AppointmentTypeId == appointmentTypeId && a.IsVisible == true
                              select new AddOnModel()
                              {
                                  Id = a.AddonId,
                                  Duration = a.Duration,
                                  Price = a.Product.Price,
                                  ProductName = p.Name,
                                  ProductId = p.ProductId,
                                  IsVisible = a.IsVisible
                              }).ToList();

                return addons;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ServicePaginatedModel GetAppointmentTypes(long companyId, int pageNumber, int pageSize, string searchString = null)
        {
            try
            {
                int count = 0;
                if (pageNumber != 0) pageNumber--;

                List<AppointmentType> appointmentTypes = new List<AppointmentType>();
                List<ServiceListModel> serviceModel = new List<ServiceListModel>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    appointmentTypes = this._appointmentTypeRepository.GetByPagination((s => s.Product.CompanyID == companyId && s.IsArchived == false && (s.Product.Name.Contains(searchString))), pageNumber, pageSize, out count);
                }
                else
                {
                    appointmentTypes = this._appointmentTypeRepository.GetByPagination((s => s.Product.CompanyID == companyId && s.IsArchived == false), pageNumber, pageSize, out count);
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
                                    }).OrderBy(a => a.ServiceName).ToList();
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



        public List<AddOnModel> GetAddons(long companyId)
        {
            try
            {
                List<Addon> addons = this._addonRepository.GetAll(s => s.Product.CompanyID == companyId && s.IsVisible).ConfigureAwait(false).GetAwaiter().GetResult();
                if (addons == null || addons.Count == 0) return new List<AddOnModel>();
                return addons.Select(s => new AddOnModel()
                {
                    ProductName = s.Product.Name,
                    Price = s.Product.Price,
                    Duration = s.Duration,
                    ProductId = s.Product.ProductId,
                    Id = s.AddonId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public bool CheckoutWithCashOrCheck(string appointmentId, string calendarId, string tipAmount, string discountAmount, string totalAmount, string productIds, string paidBy, string checkNumber, string otherDescription)
        {
            try
            {
                long apptId = long.Parse(appointmentId);
                long cId = long.Parse(calendarId);
                var pIDS = productIds.Split(',');
                if (pIDS.Count() > 0 && !string.IsNullOrEmpty(appointmentId))
                {

                    bool isDiscountApplied = false;
                    if (decimal.TryParse(discountAmount, out decimal dAmount))
                    {
                        isDiscountApplied = true;
                    }

                    if (!float.TryParse(tipAmount, out float tip))
                    {
                        tip = 0;
                    }

                    var appointmentModel = this.GetAppointment(apptId);

                    var total = PaymentUtil.ConvertPriceToInt(decimal.Parse(totalAmount));

                    PaymentOrder paymentOrder = new PaymentOrder
                    {
                        CompanyId = appointmentModel.AppointmentType.CompanyId,
                        OrderDate = DateTime.Now.ToUniversalTime(),
                        PaymentTransactionId = string.Empty,
                        PersonId = appointmentModel.ClientId.Value,
                        Total = total,
                        SoldAt = (int)(PaymentOrderModeEnum.InPerson), // make this enumn 
                        CheckNumber = checkNumber,
                        PaymentMadeWith = paidBy == "cash" ? (int)(PaymentOrderMadeEnum.Cash) : (int)(PaymentOrderMadeEnum.Check),
                        IsDiscountApplied = isDiscountApplied,
                        TipAmount = Convert.ToInt32(tip * 100),
                        AppointmentId = appointmentModel.AppointmentId,
                        PaymentOrderTypeId = 1 // make this enumn 
                    };

                    this._dbContext.PaymentOrder.Add(paymentOrder);
                    this._dbContext.SaveChanges();

                    PaymentOrderMadeWith paymentOrderMadeWith = new PaymentOrderMadeWith
                    {
                        Amount = total,
                        CheckNumber = checkNumber,
                        PaymentMadeWith = paidBy == "cash" ? (int)(PaymentOrderMadeEnum.Cash) : (int)(PaymentOrderMadeEnum.Check),
                        PaymentOrderId = paymentOrder.PaymentOrderId,
                        OtherDescription = otherDescription
                    };
                    this._dbContext.PaymentOrderMadeWith.Add(paymentOrderMadeWith);

                    PaymentOrderDetail paymentOrderDetails = new PaymentOrderDetail
                    {
                        IsDiscountApplied = isDiscountApplied,
                        OrderPrice = appointmentModel.AppointmentType.ProductModel.Price,
                        PaymentOrderId = paymentOrder.PaymentOrderId,
                        ProductId = appointmentModel.AppointmentType.ProductId,
                        TipPercentage = tip
                    };
                    this._dbContext.PaymentOrderDetail.Add(paymentOrderDetails);

                    int numberOfTimesSeenAppointmentsProductId = 0;

                    foreach (var id in pIDS)
                    {
                        long prodId = long.Parse(id);
                        if (prodId == appointmentModel.AppointmentType.ProductModel.ProductId && numberOfTimesSeenAppointmentsProductId == 0)
                        {
                            numberOfTimesSeenAppointmentsProductId++;
                        }
                        else
                        {
                            var productModel = this._dbContext.Product.Where(x => x.ProductId == prodId).FirstOrDefault();
                            paymentOrderDetails = new PaymentOrderDetail
                            {
                                IsDiscountApplied = isDiscountApplied,
                                OrderPrice = productModel.Price,
                                PaymentOrderId = paymentOrder.PaymentOrderId,
                                ProductId = productModel.ProductId,
                                TipPercentage = tip
                            };
                            this._dbContext.PaymentOrderDetail.Add(paymentOrderDetails);
                        }
                    }

                    var appointment = this._dbContext.Appointment.Where(x => x.AppointmentId == appointmentModel.AppointmentId).FirstOrDefault(); ;
                    appointment.HasBeenCheckedOut = true;

                    this._dbContext.SaveChanges();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void SendCancellationMail()
        {

        }


        public Calendar GetCalendar(long calendarId)
        {
            Calendar calendar = this._dbContext.Calendar.FirstOrDefault(x => x.CalendarId == calendarId);
            return calendar;
        }

        public Repositories.Context.Entities.TimeZone GetTimeZone(int timeZoneId)
        {
            Repositories.Context.Entities.TimeZone timeZone = this._timeZoneRepository.Get(s => s.TimeZoneId == timeZoneId).ConfigureAwait(false).GetAwaiter().GetResult();
            return timeZone;
        }

        public TimeZoneInfo GetUsersTimeZone(string systemTimeZoneId)
        {
            TimeZoneInfo userTimeZone = TimeZoneInfo.Utc;
            try
            {
                userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(systemTimeZoneId);
            }
            catch
            {
            }

            return userTimeZone;
        }

        public BlockedOffTimeModel AddBlockedOffTime(BlockedOffTimeModel blockedOffTimeModel)
        {
            try
            {
                Calendar calendar = GetCalendar(blockedOffTimeModel.CalendarId);
                if (calendar == null)
                {
                    return null;
                }

                DateTime startDate, endDate;
                if (!DateTime.TryParse(blockedOffTimeModel.fromDate + " " + blockedOffTimeModel.fromStartTime, out startDate) ||
                    !DateTime.TryParse(blockedOffTimeModel.fromDate + " " + blockedOffTimeModel.fromEndTime, out endDate))
                {
                    return null;
                }

                blockedOffTimeModel.StartFromDate = startDate;
                blockedOffTimeModel.StartToDate = endDate;

                var timeZone = GetTimeZone(calendar.TimeZoneId);
                if (timeZone == null)
                {
                    return null;
                }

                var userTimeZone = GetUsersTimeZone(timeZone.SystemTimeZoneId);

                // add block off time for multiple days
                if (blockedOffTimeModel.EndFromDate.HasValue)
                {
                    return AddBlockOffTimeForMultipleDays(blockedOffTimeModel, userTimeZone);
                }

                // add recurring block off time
                if (blockedOffTimeModel.DoesTimeRecur)
                {
                    return AddBlockedOffTimeRecurring(blockedOffTimeModel, userTimeZone);
                }

                // add single block off time
                BlockedOffTime blockedOffTime = new BlockedOffTime
                {
                    CalendarId = blockedOffTimeModel.CalendarId,
                    ExceptionDateFrom = TimeZoneInfo.ConvertTimeToUtc(blockedOffTimeModel.StartFromDate, userTimeZone),
                    ExceptionDateTo = TimeZoneInfo.ConvertTimeToUtc(blockedOffTimeModel.StartToDate, userTimeZone),
                    Color = blockedOffTimeModel.Color,
                    Description = blockedOffTimeModel.Description,
                    GoogleCalendarId = blockedOffTimeModel.GoogleCalendarId,
                    GoogleEventId = blockedOffTimeModel.GoogleEventId,
                    IsGoogleEvent = blockedOffTimeModel.IsGoogleEvent,
                    LastUpdated = DateTime.UtcNow,
                    Notes = blockedOffTimeModel.Notes
                };
                var addedBlockedOffTime = this._blockedOffTimeRepository.Add(blockedOffTime).ConfigureAwait(false).GetAwaiter().GetResult();

                return GetBlockedOffModel(addedBlockedOffTime, userTimeZone);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private BlockedOffTimeModel GetBlockedOffModel(BlockedOffTime blockedOffTimeModel, TimeZoneInfo userTimeZone)
        {
            var usersExceptionDateFrom = TimeZoneHelper.ConvertTimeToUsersTimeZone(blockedOffTimeModel.ExceptionDateFrom, userTimeZone.StandardName);
            var usersExceptionDateTo = TimeZoneHelper.ConvertTimeToUsersTimeZone(blockedOffTimeModel.ExceptionDateTo, userTimeZone.StandardName);

            return new BlockedOffTimeModel
            {
                CalendarId = blockedOffTimeModel.CalendarId,
                BlockOffTimeId = blockedOffTimeModel.BlockedOffTimeId,
                StartFromDate = usersExceptionDateFrom,
                StartToDate = usersExceptionDateTo,
                Color = blockedOffTimeModel.Color,
                Description = blockedOffTimeModel.Description,
                GoogleCalendarId = blockedOffTimeModel.GoogleCalendarId,
                GoogleEventId = blockedOffTimeModel.GoogleEventId,
                IsGoogleEvent = blockedOffTimeModel.IsGoogleEvent,
                LastUpdated = DateTime.UtcNow,
                Notes = blockedOffTimeModel.Notes,
                BlockedOffTimeSeriesId = blockedOffTimeModel.BlockedOffTimeSeriesId,
            };
        }

        private BlockedOffTimeModel AddBlockedOffTimeRecurring(BlockedOffTimeModel blockedOffTimeModel, TimeZoneInfo userTimeZone)
        {
            BlockedOffTimeModel model = new BlockedOffTimeModel();
            DateTime StartDate = blockedOffTimeModel.StartFromDate;
            DateTime EndDate = blockedOffTimeModel.StartToDate;
            DateTime EndDateRecurring = blockedOffTimeModel.DayUntilRecur.Value.Date;
            int dayOfWeek = Convert.ToInt32(blockedOffTimeModel.DayOfWeekUntil);

            BlockedOffTimeSeries blockedOffTimeSeries = new BlockedOffTimeSeries
            {
                CalendarId = blockedOffTimeModel.CalendarId,
                CreatedDate = DateTime.UtcNow,
                LastUpdatedDate = DateTime.UtcNow
            };

            this._blockedOffTimeSeriesRepository.Add(blockedOffTimeSeries).ConfigureAwait(false).GetAwaiter().GetResult();

            bool isFirstTimeInLoop = true;
            while (StartDate.Date <= EndDateRecurring.Date)
            {
                if ((int)StartDate.DayOfWeek == dayOfWeek)
                {
                    DateTime currentDayStartTime = StartDate;
                    DateTime currentDayEndTime = EndDate;
                    BlockedOffTime blockedOffTime = new BlockedOffTime
                    {
                        CalendarId = blockedOffTimeModel.CalendarId,
                        ExceptionDateFrom = TimeZoneInfo.ConvertTimeToUtc(currentDayStartTime, userTimeZone),
                        ExceptionDateTo = TimeZoneInfo.ConvertTimeToUtc(currentDayEndTime, userTimeZone),
                        Color = blockedOffTimeModel.Color,
                        Description = blockedOffTimeModel.Description,
                        GoogleCalendarId = blockedOffTimeModel.GoogleCalendarId,
                        GoogleEventId = blockedOffTimeModel.GoogleEventId,
                        IsGoogleEvent = blockedOffTimeModel.IsGoogleEvent,
                        LastUpdated = DateTime.UtcNow,
                        Notes = blockedOffTimeModel.Notes,
                        BlockedOffTimeSeriesId = blockedOffTimeSeries.BlockedOffTimeSeriesId
                    };
                    var returnBlockOffModel = this._blockedOffTimeRepository.Add(blockedOffTime).ConfigureAwait(false).GetAwaiter().GetResult();

                    //get the model so we can return it
                    if (isFirstTimeInLoop)
                    {
                        model = GetBlockedOffModel(returnBlockOffModel, userTimeZone);
                        isFirstTimeInLoop = false;
                    }
                }
                StartDate = StartDate.AddDays(1);
                EndDate = EndDate.AddDays(1);
            }

            return model;
        }

        private BlockedOffTimeModel AddBlockOffTimeForMultipleDays(BlockedOffTimeModel blockedOffTimeModel, TimeZoneInfo userTimeZone)
        {
            BlockedOffTimeModel model = new BlockedOffTimeModel();
            int numberOfDaysToBlockOffTime = blockedOffTimeModel.EndFromDate.Value.Subtract(blockedOffTimeModel.StartFromDate).Days + 1;
            bool isFirstTimeInLoop = true;
            for (int index = 0; index < numberOfDaysToBlockOffTime; index++)
            {
                DateTime currentDayStartTime = blockedOffTimeModel.StartFromDate.AddDays(index);
                DateTime currentDayEndTime = blockedOffTimeModel.StartToDate.AddDays(index);
                BlockedOffTime blockedOffTime = new BlockedOffTime
                {
                    CalendarId = blockedOffTimeModel.CalendarId,
                    ExceptionDateFrom = TimeZoneInfo.ConvertTimeToUtc(currentDayStartTime, userTimeZone),
                    ExceptionDateTo = TimeZoneInfo.ConvertTimeToUtc(currentDayEndTime, userTimeZone),
                    Color = blockedOffTimeModel.Color,
                    Description = blockedOffTimeModel.Description,
                    GoogleCalendarId = blockedOffTimeModel.GoogleCalendarId,
                    GoogleEventId = blockedOffTimeModel.GoogleEventId,
                    IsGoogleEvent = blockedOffTimeModel.IsGoogleEvent,
                    LastUpdated = DateTime.UtcNow,
                    Notes = blockedOffTimeModel.Notes
                };

                var returnBlockOffModel = this._blockedOffTimeRepository.Add(blockedOffTime).ConfigureAwait(false).GetAwaiter().GetResult();
                if (isFirstTimeInLoop)
                {
                    model = GetBlockedOffModel(returnBlockOffModel, userTimeZone);
                    isFirstTimeInLoop = false;
                }
            }

            return model;
        }


        public BlockedOffTimeModel GetBlockTimeOffTime(long calendarId, long blockOffTimeId)
        {
            BlockedOffTimeModel blockedOffTimeModel = new BlockedOffTimeModel();
            try
            {
                Calendar calendar = this._dbContext.Calendar.Include("TimeZone").FirstOrDefault(x => x.CalendarId == calendarId);

                if (calendar != null)
                {

                    var blockedOffTime = this._dbContext.BlockedOffTime.FirstOrDefault(x => x.BlockedOffTimeId == blockOffTimeId);
                    if (blockedOffTime != null)
                    {
                        blockedOffTimeModel = new BlockedOffTimeModel()
                        {
                            BlockOffTimeId = blockedOffTime.BlockedOffTimeId,
                            CalendarId = blockedOffTime.CalendarId,
                            Color = blockedOffTime.Color,
                            Description = blockedOffTime.Description,
                            Notes = blockedOffTime.Notes,
                            StartFromDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(blockedOffTime.ExceptionDateFrom, calendar.TimeZone.SystemTimeZoneId),
                            EndToDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(blockedOffTime.ExceptionDateTo, calendar.TimeZone.SystemTimeZoneId),
                            StartToDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(blockedOffTime.ExceptionDateTo, calendar.TimeZone.SystemTimeZoneId),
                        };
                        blockedOffTimeModel.fromDate = blockedOffTimeModel.StartFromDate.ToString("MM/dd/yyyy");
                        blockedOffTimeModel.fromStartTime = blockedOffTimeModel.StartFromDate.ToString("HH:mm");
                        blockedOffTimeModel.fromEndTime = blockedOffTimeModel.StartToDate.ToString("HH:mm");
                    }

                }
            }
            catch (Exception ex)
            {
                blockedOffTimeModel.Description = ex.Message;
            }

            return blockedOffTimeModel;

        }

        public bool UpdateBlockOffTime(BlockedOffTimeModel blockedOffTimeModel)
        {
            try
            {
                var blockedOffTime = this._dbContext.BlockedOffTime.FirstOrDefault(x => x.BlockedOffTimeId == blockedOffTimeModel.BlockOffTimeId);
                if (blockedOffTime != null)
                {
                    Calendar calendar = this._dbContext.Calendar.FirstOrDefault(x => x.CalendarId == blockedOffTimeModel.CalendarId);
                    if (calendar != null)
                    {
                        blockedOffTimeModel.StartFromDate = DateTime.Parse(blockedOffTimeModel.fromDate + " " + blockedOffTimeModel.fromStartTime);
                        blockedOffTimeModel.StartToDate = DateTime.Parse(blockedOffTimeModel.fromDate + " " + blockedOffTimeModel.fromEndTime);
                        ApptHeroAPI.Repositories.Context.Entities.TimeZone timeZone = this._timeZoneRepository.Get(s => s.TimeZoneId == calendar.TimeZoneId).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (timeZone != null)
                        {
                            TimeZoneInfo userTimeZone = TimeZoneInfo.Utc;
                            try
                            {
                                userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZone.SystemTimeZoneId);
                            }
                            catch
                            {
                                userTimeZone = TimeZoneInfo.Utc;
                            }

                            blockedOffTime.CalendarId = blockedOffTimeModel.CalendarId;
                            blockedOffTime.ExceptionDateFrom = TimeZoneInfo.ConvertTimeToUtc(blockedOffTimeModel.StartFromDate, userTimeZone);
                            blockedOffTime.ExceptionDateTo = TimeZoneInfo.ConvertTimeToUtc(blockedOffTimeModel.StartToDate, userTimeZone);
                            blockedOffTime.Notes = blockedOffTimeModel.Notes;
                            this._blockedOffTimeRepository.Update(blockedOffTime).ConfigureAwait(false).GetAwaiter().GetResult();
                            return true;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return false;
        }

        public bool DeleteBlockedOffTime(long blockedOffTimeId)
        {
            try
            {
                bool isDeleted = this._blockedOffTimeRepository.Delete(blockedOffTimeId).ConfigureAwait(false).GetAwaiter().GetResult();
                return isDeleted;
            }
            catch
            {
                throw;
            }
        }
        public OverrideAvailabilityModel UpdateCalendarAvailability(OverrideAvailabilityModel overrideAvailability)
        {
            var calendar = GetCalendar(overrideAvailability.CalendarId);
            if (calendar == null)
            {
                return null;
            }

            var timeZone = GetTimeZone(calendar.TimeZoneId);
            if (timeZone == null)
            {
                return null;
            }

            var usersTimeZone = GetUsersTimeZone(timeZone.SystemTimeZoneId);
            if (usersTimeZone == null)
            {
                return null;
            }

            var savedOverrideAvailability = this._overrideAvailabilityRepository.Add(new OverrideAvailability
            {
                CalendarId = calendar.CalendarId,
                OverrideFromDate = TimeZoneInfo.ConvertTimeToUtc(overrideAvailability.OverrideFromDate, usersTimeZone),
                OverrideToDate = TimeZoneInfo.ConvertTimeToUtc(overrideAvailability.OverrideToDate, usersTimeZone),
                LastUpdated = DateTime.UtcNow
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            return new OverrideAvailabilityModel
            {
                CalendarId = savedOverrideAvailability.CalendarId,
                LastUpdated = savedOverrideAvailability.LastUpdated,
                OverrideFromDate = savedOverrideAvailability.OverrideFromDate,
                OverrideId = savedOverrideAvailability.OverrideId,
                OverrideToDate = savedOverrideAvailability.OverrideToDate
            };
        }
        public bool SaveAvailability(BusinessHoursAvailability businessHoursAvailability)
        {
            try
            {
                Calendar calendar = this._dbContext.Calendar.FirstOrDefault(x => x.CalendarId == businessHoursAvailability.CalendarId);

                if (calendar != null)
                {
                    var todayDate = DateTime.Now.Date;
                    var hoursAvailability = businessHoursAvailability.HoursAvailability.OrderBy(x => x.Day).ToList();
                    for (int i = 0; i < hoursAvailability.Count; i++)
                    {
                        var day = hoursAvailability[i].Day;
                        if (businessHoursAvailability.AppointmentTypeId == 0)
                        {
                            if (hoursAvailability[i].IsActive)
                            {
                                hoursAvailability[i].TimeFrom = todayDate.Add(TimeSpan.Parse(hoursAvailability[i].TimeFromString));
                                hoursAvailability[i].TimeTill = todayDate.Add(TimeSpan.Parse(hoursAvailability[i].TimeTillString));
                            }
                            var hours = this._dbContext.HoursOfOperation.Where(a => a.Day == day && a.CalendarId == businessHoursAvailability.CalendarId).ToList();

                            if (hours.Count() == 0)
                            {
                                hours = new List<HoursOfOperation>();
                                hours.Add(new HoursOfOperation());
                            }

                            var hour = hours.SingleOrDefault();

                            hour.StartTime = (hoursAvailability[i].IsActive) ? hoursAvailability[i].TimeFrom.Value.TimeOfDay : (TimeSpan?)null;
                            hour.EndTime = (hoursAvailability[i].IsActive) ? hoursAvailability[i].TimeTill.Value.TimeOfDay : (TimeSpan?)null;
                            hour.CalendarId = businessHoursAvailability.CalendarId;
                            hour.Day = (byte)day;
                            if (hour.HoursOfOperationId == 0)
                            {
                                this._dbContext.HoursOfOperation.Add(hour);
                            }
                        }
                    }

                    this._dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool IsColorCodingByService(long personId)
        {
            bool isColorCodingByService = true;
            var person = this._personRepository.Get(p => p.PersonId == personId).ConfigureAwait(false).GetAwaiter().GetResult();
            if (person != null)
            {
                isColorCodingByService = person.IsColorCodingByService;
            }

            return isColorCodingByService;
        }
        public List<HoursAvailability> GetRegularBusinessHours(long calendarId)
        {
            List<HoursAvailability> businessHoursAvailabilities = new List<HoursAvailability>();

            var hoursOfOperation = this._dbContext.HoursOfOperation.Where(x => x.CalendarId == calendarId).ToList();
            if (hoursOfOperation.Count > 0)
            {
                for (int i = 0; i < hoursOfOperation.Count; i++)
                {
                    var dayOfWeek = hoursOfOperation.Where(d => d.Day == i).SingleOrDefault();
                    HoursAvailability businessHoursAvailability = new HoursAvailability
                    {
                        IsActive = false,
                        TimeFromString = null,
                        TimeTillString = null,
                        Day = i
                    };

                    if (dayOfWeek != null)
                    {
                        if (dayOfWeek.StartTime != null && dayOfWeek.EndTime != null)
                        {
                            DateTime startDateTime = DateTime.Now.Date;
                            DateTime endDateTime = DateTime.Now.Date;

                            startDateTime += dayOfWeek.StartTime.Value;
                            endDateTime += dayOfWeek.EndTime.Value;
                            businessHoursAvailability.IsActive = true;

                            businessHoursAvailability.TimeFromString = startDateTime.ToString("HH:mm");
                            businessHoursAvailability.TimeTillString = endDateTime.ToString("HH:mm");
                        }
                    }

                    businessHoursAvailabilities.Add(businessHoursAvailability);
                }
            }


            return businessHoursAvailabilities;
        }

    }
}
