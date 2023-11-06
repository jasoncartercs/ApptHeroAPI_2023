using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Factory
{
    public interface IPersonModelFactory
    {
        PersonModel Create(Person person, List<Appointment> appointments);
    }

    public class PersonModelFactory : IPersonModelFactory
    {
        private readonly IAppointmentViewModelFactory _appointmentViewModelFactory;
        private readonly IAddressModelFactory _addressModelFactory;

        public PersonModelFactory(IAppointmentViewModelFactory appointmentViewModelFactory, IAddressModelFactory addressModelFactory)
        {
            _appointmentViewModelFactory = appointmentViewModelFactory;
            _addressModelFactory = addressModelFactory;
        }

        public PersonModel Create(Person person, List<Appointment> appointments)
        {
            DateTime currentTime = DateTime.Now;

            return new PersonModel
            {
                PersonId = person.PersonId,
                FirstName = person.FirstName,
                LastName = person.LastName,
                EmailAddress = person.EmailAddress,
                IsBanned = person.IsAccountBanned,
                DOB = person.BirthDate,
                Phone = PhoneNumberUtility.FormatPhoneNumber(person.Phone),
                AlternatePhoneNumber = PhoneNumberUtility.FormatPhoneNumber(person.AlternatePhoneNumber),
                CompanyId = person.PersonCompany?.CompanyId ?? 0,
                RoleId = person.UserRoleId,
                Notes = person.Notes,
                IsArchived = person.IsArchived,
                AddressModel = _addressModelFactory.Create(person.Address),
                //... (other fields)
                UpcomingAppointments = appointments
                    .Where(a => a.PersonId == person.PersonId && a.StartTime > currentTime)
                    .Select(a => _appointmentViewModelFactory.Create(a))
                    .ToList(),
                PastAppointments = appointments
                    .Where(a => a.PersonId == person.PersonId && a.StartTime <= currentTime)
                    .Select(a => _appointmentViewModelFactory.Create(a))
                    .ToList()
            };
        }
    }

}
