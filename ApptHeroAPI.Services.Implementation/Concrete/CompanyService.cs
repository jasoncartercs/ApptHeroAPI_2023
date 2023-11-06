using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using ApptHeroAPI.Services.Implementation.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class CompanyService : ICompanyService
    {
        private readonly SqlDbContext _dbContext;


        public CompanyService(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public CompanyBookingRulesModel GetBookingRules(long companyId)
        {
            CompanyBookingRulesModel companyBookingRulesModel = null;
            try
            {
                var companySetting = this._dbContext.CompanySetting.FirstOrDefault(x => x.CompanyId == companyId);

                if (companySetting != null)
                {
                    companyBookingRulesModel = new CompanyBookingRulesModel
                    {
                        MinMinutesBeforeSchedulingAppointment = companySetting.MinMinutesBeforeSchedulingAppointment,
                        WhenShouldAppointmentsStartInMins = companySetting.WhenShouldAppointmentsStartInMins,
                        HowToMaximizeScheduleOption = companySetting.HowToMaximizeScheduleOption,
                        MinMinutesBeforeReschedulingCancellingAppointment = companySetting.MinMinutesBeforeReschedulingCancellingAppointment,
                        AutoApproveAppointments = companySetting.AutoApproveAppointments,
                        ShouldCollectCustomerAddress = companySetting.ShouldCollectCustomerAddress,
                        TravelLimitInMiles = companySetting.TravelLimitInMiles,
                        HowFarInFutureClientsBookAppointments = companySetting.HowFarInFutureBookAppointments,
                        CompanyId = companyId
                    };
                    companySetting = new CompanySetting();
                }

                return companyBookingRulesModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool SaveCalendarStartAndEndTime(CompanySettingsModel companySettingsModel)
        {
            try
            {
                var companySetting = this._dbContext.CompanySetting.FirstOrDefault(x => x.CompanyId == companySettingsModel.CompanyId);

                if (companySetting != null)
                {
                    companySetting.CalendarStartTime = companySettingsModel.CalendarStartTime;
                    companySetting.CalendarEndTime = companySettingsModel.CalendarEndTime;
                    _dbContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool SaveBookingRules(CompanyBookingRulesModel model)
        {
            try
            {
                var companySetting = this._dbContext.CompanySetting.FirstOrDefault(x => x.CompanyId == model.CompanyId);

                if (companySetting == null)
                {
                    companySetting = new CompanySetting();
                }

                companySetting.MinMinutesBeforeSchedulingAppointment = model.MinMinutesBeforeSchedulingAppointment;
                companySetting.WhenShouldAppointmentsStartInMins = model.WhenShouldAppointmentsStartInMins;
                companySetting.HowToMaximizeScheduleOption = model.HowToMaximizeScheduleOption;
                companySetting.MinMinutesBeforeReschedulingCancellingAppointment = model.MinMinutesBeforeReschedulingCancellingAppointment;
                companySetting.AutoApproveAppointments = model.AutoApproveAppointments;
                companySetting.ShouldCollectCustomerAddress = model.ShouldCollectCustomerAddress;
                companySetting.TravelLimitInMiles = model.TravelLimitInMiles;
                companySetting.HowFarInFutureBookAppointments = model.HowFarInFutureClientsBookAppointments;

                if (companySetting.CompanySettingId == 0)
                {
                    this._dbContext.CompanySetting.Add(companySetting);
                }
                this._dbContext.SaveChanges();


                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool  SaveCompanyBufferTimes(long companyId, bool showBufferTimesOnCalendar)
        {
            try
            {
                var companySetting = this._dbContext.CompanySetting.FirstOrDefault(c => c.CompanyId == companyId);

                if (companySetting != null)
                {
                    companySetting.ShowBufferTimesOnCalendar = showBufferTimesOnCalendar;
                    _dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    


        public bool SaveCompanySetting(CompanySettingsModel companyModel)
        {
            try
            {

                var company = this._dbContext.Company.Include(x => x.Address).FirstOrDefault(x => x.CompanyId == companyModel.CompanyId);

                if (company != null)
                {

                    company.Name = companyModel.Name;
                    company.PhoneNumber = PhoneNumberUtility.RemovePhoneNumberFormat(companyModel.PhoneNumber);
                    company.WebsiteUrl = companyModel.WebsiteUrl;
                    company.Email = companyModel.Email;
                    company.Address.AddressLine1 = companyModel.AddressModel.AddressLine1;
                    company.Address.AddressLine2 = companyModel.AddressModel.AddressLine2;
                    company.Address.City = companyModel.AddressModel.City;
                    company.Address.ZipCode = companyModel.AddressModel.ZipCode;
                    company.Address.StateProvinceId = Convert.ToInt32(companyModel.AddressModel.StateProvinceId);
                    this._dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SaveCompany(CompanyModel companyModel)
        {
            try
            {
                var company = this._dbContext.Company.Include(x => x.Address).ThenInclude(x => x.StateProvince).
                    Include(x => x.CompanyCalendars).
                    ThenInclude(cc => cc.Calendar).ThenInclude(t => t.TimeZone).
                    FirstOrDefault(x => x.CompanyId == companyModel.CompanyId);

                if (company != null)
                {

                    company.Name = companyModel.Name;
                    company.PhoneNumber = PhoneNumberUtility.RemovePhoneNumberFormat(companyModel.PhoneNumber);
                    company.WebsiteUrl = companyModel.WebsiteUrl;
                    company.Email = companyModel.Email;
                    company.Address.AddressLine1 = companyModel.AddressModel.AddressLine1;
                    company.Address.AddressLine2 = companyModel.AddressModel.AddressLine2;
                    company.Address.City = companyModel.AddressModel.City;
                    company.Address.ZipCode = companyModel.AddressModel.ZipCode;
                    company.Address.StateProvinceId = Convert.ToInt32(companyModel.AddressModel.StateProvinceId);
                    company.Logo = companyModel.Logo;
                    var calendars = company.CompanyCalendars.ToList();
                    calendars.ForEach(a => a.Calendar.TimeZoneId = companyModel.TimeZoneId);
                    //if there is not a logo
                    if (!string.IsNullOrEmpty(company.Logo))
                    {
                        company.Logo = companyModel.Logo;
                    }
                    this._dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CompanyModel GetCompany(long companyId)
        {
            var companyModel = new CompanyModel();
            var company = this._dbContext.Company.Include(x => x.Address).Include(x => x.CompanyCalendars).
                ThenInclude(x => x.Calendar).ThenInclude(x => x.TimeZone).
                FirstOrDefault(x => x.CompanyId == companyId);
            if (company != null)
            {
                companyModel.Name = company.Name;
                companyModel.PhoneNumber = company.PhoneNumber;
                companyModel.WebsiteUrl = company.WebsiteUrl;
                companyModel.Email = company.Email;
                companyModel.AddressModel = new AddressModel();
                companyModel.AddressModel.AddressId = company.Address.AddressId;
                companyModel.AddressModel.AddressLine1 = company.Address.AddressLine1;
                companyModel.AddressModel.AddressLine2 = company.Address.AddressLine2;
                companyModel.AddressModel.City = company.Address.City;
                companyModel.AddressModel.ZipCode = company.Address.ZipCode;
                companyModel.AddressModel.StateProvinceId = company.Address.StateProvinceId;
                companyModel.Logo = company.Logo;
                var companyCalendar = company.CompanyCalendars.FirstOrDefault();
                if(companyCalendar != null)
                {
                    companyModel.TimeZoneId = companyCalendar.Calendar.TimeZoneId;
                    companyModel.TimeZone = companyCalendar.Calendar.TimeZone.SystemTimeZoneId;
                }
            }
            return companyModel;
        }

        public CompanySettingsModel GetCompanySetting(long companyId)
        {
            try
            {
                var companySettingsModel = new CompanySettingsModel();

                var company = this._dbContext.Company.Include(x => x.Address).FirstOrDefault(x => x.CompanyId == companyId);
                if (company != null)
                {
                    companySettingsModel.Name = company.Name;
                    companySettingsModel.PhoneNumber = company.PhoneNumber;
                    companySettingsModel.WebsiteUrl = company.WebsiteUrl;
                    companySettingsModel.Email = company.Email;
                    companySettingsModel.AddressModel = new AddressModel();
                    companySettingsModel.AddressModel.AddressId = company.Address.AddressId;
                    companySettingsModel.AddressModel.AddressLine1 = company.Address.AddressLine1;
                    companySettingsModel.AddressModel.AddressLine2 = company.Address.AddressLine2;
                    companySettingsModel.AddressModel.City = company.Address.City;
                    companySettingsModel.AddressModel.ZipCode = company.Address.ZipCode;
                    companySettingsModel.AddressModel.StateProvinceId = company.Address.StateProvinceId;
                }
                var companySettings = this._dbContext.CompanySetting.FirstOrDefault(c => c.CompanyId == companyId);
                if (companySettings != null)
                {

                    companySettingsModel.CompanyBookingRulesModel = new CompanyBookingRulesModel
                    {
                        CompanyId = companySettings.CompanyId,
                        MinMinutesBeforeSchedulingAppointment = companySettings.MinMinutesBeforeSchedulingAppointment,
                        WhenShouldAppointmentsStartInMins = companySettings.WhenShouldAppointmentsStartInMins,
                        HowToMaximizeScheduleOption = companySettings.HowToMaximizeScheduleOption,
                        MinMinutesBeforeReschedulingCancellingAppointment = companySettings.MinMinutesBeforeReschedulingCancellingAppointment,
                        AutoApproveAppointments = companySettings.AutoApproveAppointments,
                        ShouldCollectCustomerAddress = companySettings.ShouldCollectCustomerAddress,
                        TravelLimitInMiles = companySettings.TravelLimitInMiles,
                        HowFarInFutureClientsBookAppointments = companySettings.HowFarInFutureBookAppointments,
                    };
                }
                return companySettingsModel;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public CompanySettingsModel GetCompanyCalendarSetting(long companyId)
        {
            var companySettingsModel = new CompanySettingsModel();
            var companySetting = this._dbContext.CompanySetting.FirstOrDefault(c => c.CompanyId == companyId);
            if (companySetting != null)
            {
                companySettingsModel = new CompanySettingsModel
                {
                    CalendarEndTime = companySetting.CalendarEndTime,
                    CalendarStartDayOfWeek = companySetting.CalendarStartDayOfWeek,
                    CalendarStartTime = companySetting.CalendarStartTime,
                    ShowBufferTimesOnCalendar = companySetting.ShowBufferTimesOnCalendar

                };
            }
            return companySettingsModel;
        }

        public List<StateProvinceModel> GetStates()
        {
            List<StateProvinceModel> stateProvinceModels = new List<StateProvinceModel>();
            stateProvinceModels = (from s in _dbContext.StateProvince
                                   select new StateProvinceModel
                                   {
                                       CountryId = s.CountryId,
                                       StateCode = s.StateCode,
                                       StateName = s.StateName,
                                       StateProvinceId = s.StateProvinceId
                                   }).ToList();

            return stateProvinceModels;
        }

     
    }
}

