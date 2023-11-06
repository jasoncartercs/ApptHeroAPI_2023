using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Consts;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class TeammateService : ITeammateService
    {

        private readonly SqlDbContext _dbContext;
        public TeammateService(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }

        public bool SaveTeamMember(PersonModel personModel, PersonModel adminModel, List<AddOnModel> addonModels)
        {
            bool didSave = false;


            Company company = _dbContext.Company.Include(c => c.CompanyCalendars).Where(p => p.CompanyId == personModel.CompanyId).FirstOrDefault();

            if (company != null)
            {
                //insert person
                if (personModel.PersonId == 0)
                {
                    var userRole = _dbContext.UserRole.Where(i => i.UserRoleName == UserRoleNameConst.Employee).SingleOrDefault();
                    var permission = _dbContext.Permission.Find(personModel.PermissionModel.PermissionId); 
                    Calendar adminCalendar = _dbContext.Calendar.Where(cc => cc.PersonId == adminModel.PersonId).SingleOrDefault();
                    if (userRole != null && permission != null && adminCalendar != null)
                    {
                        Person person = new Person
                        {
                            FirstName = personModel.FirstName,
                            LastName = personModel.LastName,
                            EmailAddress = personModel.EmailAddress,
                            Phone = personModel.Phone,
                            Password = personModel.Password,
                            UserRole = userRole,
                            IsAccountBanned = false,
                            IsAccountEnabled = true,
                            AppointmentColor = personModel.AppointmentColor,
                            SMSNotificationPreferenceId = personModel.SMSNotificationPreferenceId,
                            EmailNotificationPreferenceId = personModel.EmailNotificationPreferenceId,
                            ShowOnOnlineSchedule = personModel.ShowOnOnlineSchedule,
                            ShowOnCalendar = personModel.ShowOnCalendar,
                        };

                        foreach (var at in personModel.AppointmentTypeModels)
                        {
                            //get the existing appointment type
                            var appointmentType = _dbContext.AppointmentType.Find(at.AppointmentTypeId);

                            if (appointmentType != null)
                            {
                                var teammateAppointmentType = new TeamMemberAppointmentType
                                {
                                    Price = at.TeammatePrice,
                                    AppointmentTypeId = appointmentType.AppointmentTypeId
                                };
                                person.TeamMemberAppointmentTypes.Add(teammateAppointmentType);
                            }
                        }

                        foreach (var addon in addonModels)
                        {
                            var a = _dbContext.Addon.Find(addon.Id); // unitOfWork.AddonRepository.GetById(addon.AddonId);
                            var teammmateAddon = new TeammateAddons
                            {
                                Price = addon.TeammatePrice,
                                AddonId = a.AddonId
                            };
                            person.TeammateAddons.Add(teammmateAddon);
                        }

                        Calendar personCalendar = new Calendar
                        {
                            Name = personModel.FullName,
                            PersonId = person.PersonId,
                            TimeZoneId = adminCalendar.TimeZoneId,
                            Description = "",
                            IsDefault = false
                        };

                        //company.Calendars.Add(personCalendar);
                        //person.Calendars.Add(personCalendar);
                        //person.Companies.Add(company);

                     
                        person.TeamMemberPermissions.Add(new TeamMemberPermission
                        {
                            PermissionId = permission.PermissionId,
                            PersonId = person.PersonId,
                        });
                        //didSave = true;
                        
                        _dbContext.Person.Add(person);
                        _dbContext.SaveChanges();
                        //unitOfWork.PersonRepository.Insert(person);
                    }
                }
                else
                {
                    //var person = company.People.Where(p => p.PersonId == personModel.PersonId).SingleOrDefault();
                    //if (person != null)
                    //{
                    //    person.FirstName = personModel.FirstName;
                    //    person.LastName = personModel.LastName;
                    //    person.EmailAddress = personModel.EmailAddress;
                    //    person.Phone = personModel.Phone;
                    //    person.AppointmentColor = personModel.AppointmentColor;
                    //    person.EmailNotificationPreferenceId = personModel.EmailNotificationPreferenceId;
                    //    person.SMSNotificationPreferenceId = personModel.SMSNotificationPreferenceId;
                    //    person.ShowOnOnlineSchedule = personModel.ShowOnOnlineSchedule;
                    //    person.ShowOnCalendar = personModel.ShowOnCalendar;
                    //    if (person.Calendars.Count() == 1)
                    //    {
                    //        person.Calendars.First().Name = personModel.Name;
                    //    }
                    //}

                    //if (person.Permissions.Count == 0)
                    //{
                    //    var permission = unitOfWork.PermissionRepository.GetById(personModel.PermissionModel.PermissionId);

                    //    if (permission != null)
                    //    {
                    //        person.Permissions.Add(permission);
                    //    }
                    //}
                    //else if (person.Permissions.Count == 1)
                    //{
                    //    var permission = person.Permissions.ElementAt(0);

                    //    if (permission != null)
                    //    {
                    //        person.Permissions.Remove(permission);
                    //        permission = unitOfWork.PermissionRepository.GetById(personModel.PermissionModel.PermissionId);
                    //        person.Permissions.Add(permission);
                    //    }
                    //}

                    //var personAppoinmentTypesCurrentlyInDB = person.TeamMemberAppointmentTypes;

                    ////in the database, but not in the list
                    //var inDatabaseButNotInList = personAppoinmentTypesCurrentlyInDB.Where(database => !personModel.AppointmentTypeModels.Any(list => list.AppointmentTypeId == database.AppointmentTypeId)).ToList();
                    //foreach (var item in inDatabaseButNotInList)
                    //{
                    //    var appointmentTypeToRemove = person.TeamMemberAppointmentTypes.Single(i => i.AppointmentTypeId == item.AppointmentTypeId);

                    //    //remove the item
                    //    person.TeamMemberAppointmentTypes.Remove(appointmentTypeToRemove);
                    //}

                    //var personAddonsCurrentInDb = person.TeammateAddons;
                    ////in the database, but ont in the list
                    //var inDatabaseButNotInListAddons = personAddonsCurrentInDb.Where(database => !addonModels.Any(list => list.AddonId == database.AddonId)).ToList();
                    //foreach (var item in inDatabaseButNotInListAddons)
                    //{
                    //    var addOnsToRemove = person.TeammateAddons.Single(i => i.AddonId == item.AddonId);
                    //    person.TeammateAddons.Remove(addOnsToRemove);
                    //}

                    //foreach (var item in personModel.AppointmentTypeModels)
                    //{
                    //    //they could add to the ones already in the database
                    //    //if it's in the array but not in the database
                    //    var appointmentType = unitOfWork.TeamMemberAppointmentTypeRepository.GetAll().Where(i =>
                    //    i.AppointmentTypeId == item.AppointmentTypeId && i.PersonId == personModel.PersonId).SingleOrDefault();
                    //    if (appointmentType != null)
                    //    {
                    //        appointmentType.Price = item.TeammatePrice;
                    //        unitOfWork.TeamMemberAppointmentTypeRepository.Save();
                    //    }
                    //    else
                    //    {
                    //        var at = new TeamMemberAppointmentType
                    //        {
                    //            PersonId = person.PersonId,
                    //            AppointmentTypeId = item.AppointmentTypeId,
                    //            Price = item.TeammatePrice
                    //        };
                    //        person.TeamMemberAppointmentTypes.Add(at);
                    //    }
                    //}

                    //foreach (var item in addonModels)
                    //{
                    //    var addon = unitOfWork.TeammateAddonRepository.GetAll().Where(i =>
                    //    i.AddonId == item.AddonId && i.PersonId == personModel.PersonId).SingleOrDefault();
                    //    if (addon != null)
                    //    {
                    //        addon.Price = item.TeammatePrice;
                    //        unitOfWork.TeammateAddonRepository.Save();
                    //    }
                    //    else
                    //    {
                    //        var addOn = new TeammateAddon
                    //        {
                    //            PersonId = person.PersonId,
                    //            AddonId = item.AddonId,
                    //            Price = item.TeammatePrice
                    //        };
                    //        person.TeammateAddons.Add(addOn);
                    //    }
                    //}

                    ////update person
                    //unitOfWork.PersonRepository.Save();
                }


            }

            return false;
        }

        public bool ReinstateTeamMember(long companyId, long personId)
        {
            bool isReinstated = false;
            var person = _dbContext.Person.Include(pc => pc.PersonCompany).Where(p => p.PersonId == personId).FirstOrDefault();
            if (person != null)
            {
                if (person.PersonCompany.CompanyId == companyId)
                {
                    person.IsAccountBanned = false;
                    person.IsAccountEnabled = true;
                    person.IsArchived = false; ;
                    _dbContext.SaveChanges();
                    isReinstated = true;
                }
            }
            return isReinstated;
        }

        public bool ArchiveTeamMember(long companyId, long personId)
        {
            bool isDeleted = false;
            var person = _dbContext.Person.Include(pc => pc.PersonCompany).Where(p => p.PersonId == personId).FirstOrDefault();
            if (person != null)
            {
                if (person.PersonCompany.CompanyId == companyId)
                {
                    person.IsAccountBanned = true;
                    person.IsAccountEnabled = false;
                    person.IsArchived = true;
                    _dbContext.SaveChanges();
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        public List<PersonModel> GetTeammates(long companyId)
        {
            List<PersonModel> peopleModel = new List<PersonModel>();

            var people = _dbContext.Person.Where(p => (p.UserRole.UserRoleName == UserRoleNameConst.Employee || p.UserRole.UserRoleName == UserRoleNameConst.Admin) && p.PersonCompany.CompanyId == companyId);

            peopleModel = (from pm in people
                           select new PersonModel
                           {
                               PersonId = pm.PersonId,
                               CompanyId = companyId,
                               EmailAddress = pm.EmailAddress,
                               FirstName = pm.FirstName,
                               LastName = pm.LastName,
                               Phone = pm.Phone,
                               IsAccountEnabled = pm.IsAccountEnabled,
                               UserRoleName = pm.UserRole.UserRoleName
                           }).ToList();
            return peopleModel;
        }
    }
}
