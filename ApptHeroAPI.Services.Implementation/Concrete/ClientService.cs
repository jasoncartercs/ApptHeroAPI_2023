using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Contracts;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Common.Enums;
using ApptHeroAPI.Repositories.Abstraction.Abstracts;
using ApptHeroAPI.Repositories.Context.Context;
using Microsoft.EntityFrameworkCore;
using TimeZone = ApptHeroAPI.Repositories.Context.Entities.TimeZone;
using System.Linq.Expressions;
using ApptHeroAPI.Services.Implementation.Factory;
using System.Threading.Tasks;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class ClientService : IClientService<PersonModel>
    {
        private readonly SqlDbContext _dbContext;
        //IRepository<Appointment> 
        private readonly ClientRepository _personRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IRepository<ClientTag> _clientTagRepository;
        private readonly IRepository<TimeZone> _timeZoneRepository;
        private readonly IRepository<MessageLog> _messageLogRepository;
        private readonly IRepository<PersonPackage> _packageRepository;
        private readonly IPersonModelFactory _personModelFactory;
        private readonly IMessageLogModelFactory _messageLogModelFactory;
        private readonly IPersonPackageModelFactory _personPackageModelFactory;


        public ClientService(IDbContextFactory<SqlDbContext> dbContextFactory,
            IRepository<TimeZone> timeZoneRepository,
            ClientRepository personRepository,
            IAppointmentRepository appointmentRepository,
            IRepository<ClientTag> clientTagRepository,
            IPersonModelFactory personModelFactory,
            IRepository<MessageLog> messageLogRepository,
            IMessageLogModelFactory messageLogModelFactory,
            IRepository<PersonPackage> packageRepository,
            IPersonPackageModelFactory personPackageModelFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();

            this._personRepository = personRepository;
            this._appointmentRepository = appointmentRepository;
            this._clientTagRepository = clientTagRepository;
            this._timeZoneRepository = timeZoneRepository;
            this._personModelFactory = personModelFactory;
            _messageLogRepository = messageLogRepository;
            _messageLogModelFactory = messageLogModelFactory;
            _packageRepository = packageRepository;
            _personPackageModelFactory = personPackageModelFactory;
        }


        public bool ChangeBanStatus(long id, bool status)
        {
            try
            {
                Person person = this._personRepository.Get(s => s.PersonId == id).ConfigureAwait(false).GetAwaiter().GetResult();
                if (person == null) return false;
                person.IsAccountBanned = status;
                return this._personRepository.Update(person).ConfigureAwait(false).GetAwaiter().GetResult();
            }
            catch (Exception)
            { throw; }

        }

        public bool CreateClient(PersonModel model)
        {
            try
            {
                Person person = new Person()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    EmailAddress = model.EmailAddress,
                    IsAccountBanned = false,
                    IsAccountEnabled = true,
                    Phone = model.Phone,
                    Password = HashHelper.HashString(model.Password),
                    PersonCompany = new PersonCompany()
                    {
                        CompanyId = model.CompanyId
                    },
                    BirthDate = model.DOB,
                    Notes = model.Notes,
                    IsArchived = false,
                    IsColorCodingByService = true,//need to discuss...
                    IsSubscribedToNewsLetter = false,//need to discuss...
                    ShowGoogleAppointmentsOnCalendar = false,//need to discuss...
                    UserRoleId = (int)UserRoles.ClientId,
                    ShowOnCalendar = true,//need to discuss...
                    ShowOnOnlineSchedule = true//need to discuss...
                };
                Person prsn = this._personRepository.Add(person).ConfigureAwait(false).GetAwaiter().GetResult();
                if (model.Tags != null && model.Tags.Count > 0)
                {
                    List<ClientTag> clietTags = new List<ClientTag>();
                    foreach (TagModel tag in model.Tags)
                    {
                        clietTags.Add(new ClientTag() { PersonId = prsn.PersonId, TagId = tag.Id });
                    }
                    this._clientTagRepository.AddList(clietTags).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                if (prsn != null) return true;
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<TagModel> GetTags(long companyId)
        {
            try
            {
                List<ClientTag> clientTags = this._clientTagRepository.GetAll(s => s.Person.PersonCompany.CompanyId == companyId && s.Person.UserRoleId == (int)UserRoles.ClientId && s.TagId == (int)TagTypeEnum.User).ConfigureAwait(false).GetAwaiter().GetResult();
                HashSet<TagModel> tags = new HashSet<TagModel>(new TagComparer());
                if (clientTags != null && clientTags.Count > 0)
                {
                    foreach (ClientTag clientTag in clientTags)
                    {
                        tags.Add(new TagModel() { Id = clientTag.TagId, Name = clientTag.Tag?.TagName });

                    }
                    return tags.ToList();
                }
                return new List<TagModel>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool DeleteClient(long id)
        {
            try
            {
                bool isDeleted = this._personRepository.Delete(id).ConfigureAwait(false).GetAwaiter().GetResult();
                return isDeleted;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientPaginatedModel GetClients(long companyId, string query, int pageNumber, int pageSize,
               string dateFilterFrom, string dateFilterTo, string teammateFilter, bool lastAppointmentChecked, int lastAppointmentDays,
               bool hasFutureAppointmentChecked, bool hadServiceChecked, string categoryServicesFilter,
               bool hadServiceCategoryChecked, string categoryFilter)
        {
            try
            {
                int count = 0;
                if (pageNumber != 0) pageNumber--;

                List<PersonModel> personModels = new List<PersonModel>();
                List<Person> persons = new List<Person>();
                Expression<Func<Person, bool>> filterExpression = s => s.UserRoleId == (int)UserRoles.ClientId
                    && s.PersonCompany.CompanyId == companyId
                    && s.IsAccountEnabled
                    && !s.IsArchived;

                Expression<Func<Person, bool>> CombineExpressions(Expression<Func<Person, bool>> expr1, Expression<Func<Person, bool>> expr2)
                {
                    var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());
                    return Expression.Lambda<Func<Person, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
                }

                if (!string.IsNullOrEmpty(query))
                {
                    filterExpression = CombineExpressions(filterExpression, s => s.FirstName.Contains(query)
                        || s.LastName.Contains(query)
                        || s.Phone.Contains(query)
                        || s.EmailAddress.Contains(query));
                }

                // Add date filter
                if (!string.IsNullOrEmpty(dateFilterFrom) && !string.IsNullOrEmpty(dateFilterTo))
                {
                    if (DateTime.TryParse(dateFilterFrom, out DateTime dateFrom) && DateTime.TryParse(dateFilterTo, out DateTime dateTo))
                    {
                        filterExpression = CombineExpressions(filterExpression, s => s.Appointments.Any(a => a.StartTime.Date >= dateFrom.Date &&
                        a.StartTime.Date <= dateTo.Date));
                    }
                }

                // Add teammate filter
                if (!string.IsNullOrEmpty(teammateFilter))
                {
                    long teammateId;
                    if (long.TryParse(teammateFilter, out teammateId))
                    {
                        filterExpression = CombineExpressions(filterExpression, s => s.Appointments.Any(a => a.CalendarId == teammateId));
                    }
                }

                // Add last appointment filter
                if (lastAppointmentChecked)
                {
                    DateTime minDate = DateTime.Now.AddDays(-lastAppointmentDays).Date;
                    filterExpression = CombineExpressions(filterExpression, s => s.Appointments.Any(a => a.StartTime.Date >= minDate));
                }

                // Add future appointment filter
                if (hasFutureAppointmentChecked)
                {
                    DateTime now = DateTime.Now.Date;
                    filterExpression = CombineExpressions(filterExpression, s => s.Appointments.Any(a => a.StartTime.Date > now));
                }

                // Add service and service category filters
                if (hadServiceChecked)
                {
                    if (long.TryParse(categoryServicesFilter, out long appointmentTypeId))
                    {
                        filterExpression = CombineExpressions(filterExpression, s => s.Appointments.Any(a => a.AppointmentTypeId == appointmentTypeId));
                    }
                }

                if (hadServiceCategoryChecked)
                {
                    if (long.TryParse(categoryFilter, out long appointmentCategoryId))
                    {
                        filterExpression = CombineExpressions(filterExpression, s => s.Appointments.Any(a => a.AppointmentType.AppointmentTypeCategoryId == appointmentCategoryId));
                    }
                }

                persons = this._personRepository.GetByPagination(filterExpression, pageNumber, pageSize, out count);

                if (persons != null && persons.Count > 0)
                {
                    personModels = (from person in persons
                                    select new PersonModel
                                    {
                                        EmailAddress = person.EmailAddress,
                                        PersonId = person.PersonId,
                                        Phone = PhoneNumberUtility.FormatPhoneNumber(person.Phone),
                                        FirstName = person.FirstName,
                                        IsBanned = person.IsAccountBanned,
                                        LastName = person.LastName,
                                        CompanyId = person.PersonCompany.CompanyId,
                                        DOB = person.BirthDate,
                                        Notes = person.Notes
                                    }).OrderBy(s => s.FirstName).ToList();
                }

                return new ClientPaginatedModel()
                {
                    Clients = personModels,
                    PageNumber = pageNumber + 1,
                    RecordsReturned = personModels.Count,
                    TotalCount = count
                };
            }
            catch (Exception) { throw; }
        }

        public List<PersonModel> GetClients(long companyId)
        {
            List<PersonModel> personModels = new List<PersonModel>();
            List<Person> persons = new List<Person>();
            persons = _personRepository.GetAll(s => s.UserRoleId == (int)UserRoles.ClientId & s.PersonCompany.CompanyId == companyId && s.IsAccountEnabled && !s.IsArchived).ConfigureAwait(false).GetAwaiter().GetResult();
            personModels = (from person in persons
                            select new PersonModel
                            {
                                EmailAddress = person.EmailAddress,
                                PersonId = person.PersonId,
                                Phone = PhoneNumberUtility.FormatPhoneNumber(person.Phone),
                                FirstName = person.FirstName,
                                LastName = person.LastName,
                                CompanyId = person.PersonCompany.CompanyId,
                                DOB = person.BirthDate,
                                Notes = person.Notes,
                                AddressModel = person.Address != null ? new AddressModel
                                {
                                    AddressId = person.Address.AddressId,
                                    AddressLine1 = person.Address.AddressLine1,
                                    AddressLine2 = person.Address.AddressLine2,
                                    City = person.Address.City,
                                    ZipCode = person.Address.ZipCode,
                                    StateCode = person.Address.StateProvince?.StateCode,
                                    StateName = person.Address.StateProvince?.StateName,
                                    LocationName = person.Address.LocationName,
                                    StateProvinceId = person.Address.StateProvinceId
                                } : null
                            }).OrderBy(s => s.FirstName).ToList();
            return personModels;
        }

        public List<PersonModel> GetClients(long companyId, long tagId)
        {
            List<PersonModel> personModels = new List<PersonModel>();
            List<ClientTag> persons = this._clientTagRepository.GetAll(s => s.TagId == tagId && s.Person.PersonCompany.CompanyId == companyId).ConfigureAwait(false).GetAwaiter().GetResult();
            foreach (ClientTag clientTag in persons)
            {
                personModels.Add(new PersonModel()
                {
                    PersonId = clientTag.PersonId,
                    EmailAddress = clientTag.Person.EmailAddress,
                    //FullName = $"{clientTag.Person.,",
                    Phone = clientTag.Person.Phone,
                    IsBanned = clientTag.Person.IsAccountBanned,
                    CompanyId = clientTag.Person.PersonCompany.CompanyId,
                    DOB = clientTag.Person.BirthDate,
                    FirstName = clientTag.Person.FirstName,
                    LastName = clientTag.Person.LastName,
                    Notes = clientTag.Person.Notes,
                });
            }
            return personModels;
        }

        public PersonModel GetClient(long id)
        {
            try
            {
                Person person = this._personRepository.Get(s => s.PersonId == id && s.IsAccountEnabled && !s.IsArchived).ConfigureAwait(false).GetAwaiter().GetResult();
                if (person != null)
                {
                    List<ClientTag> tags = this._clientTagRepository.GetAll(s => s.PersonId == person.PersonId).ConfigureAwait(false).GetAwaiter().GetResult();


                    PersonModel personModel = new PersonModel();
                    personModel.PersonId = person.PersonId;
                    personModel.EmailAddress = person.EmailAddress;
                    // personModel.FullName = $"{person.FirstName} {person.LastName}";
                    personModel.Phone = person.Phone;
                    personModel.IsBanned = person.IsAccountBanned;
                    personModel.CompanyId = person.PersonCompany.CompanyId;
                    personModel.DOB = person.BirthDate;
                    personModel.FirstName = person.FirstName;
                    personModel.LastName = person.LastName;
                    personModel.Notes = person.Notes;

                    if (tags != null && tags.Count > 0)
                    {
                        personModel.Tags = new List<TagModel>();
                        foreach (ClientTag tag in tags)
                        {
                            personModel.Tags.Add(new TagModel() { Id = tag.TagId, Name = tag.Tag.TagName });
                        }
                    }
                    return personModel;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ClientModel GetClientAppointments(long Id)
        {
            try
            {
                ClientModel model = new ClientModel();
                var clientAppointments = (from a in this._dbContext.Appointment.Include(a => a.AppointmentType).Include(a => a.AppointmentType.Product).Include(a => a.Calendar).Include(a => a.Calendar.TimeZone)
                                          where a.PersonId == Id && a.AppointmentStatus == 1
                                          select new ClientAppointmentModel()
                                          {
                                              AppointmentTime = TimeZoneHelper.ConvertTimeToUsersTimeZone(a.StartTime, a.Calendar.TimeZone.SystemTimeZoneId),
                                              Duration = a.AppointmentType.Duration,
                                              Price = a.AppointmentType.Product.Price,
                                              ProviderName = a.Calendar.Name,
                                              Status = a.AppointmentStatus,
                                              StatusName = GetDynamicStatusName(a.IsNoShow, a.IsCancelled, TimeZoneHelper.ConvertTimeToUsersTimeZone(a.StartTime, a.Calendar.TimeZone.SystemTimeZoneId))
                                          }).ToList();
                model.Appointments = new List<ClientAppointmentModel>();
                model.Appointments = clientAppointments;
                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetDynamicStatusName(bool isNoShow, bool isCancelled, DateTime startTime)
        {
            if (isNoShow)
            {
                return "No Show";
            }

            if (isCancelled)
            {
                return "Cancelled";
            }

            if (startTime >= DateTime.Now)
            {
                return "Upcoming";
            }

            if (startTime < DateTime.Now)
            {
                return "Completed";
            }
            return "";
        }

        public ClientPaginatedModel GetClients(long companyId, int pageNumber, int pageSize, string searchString = null)
        {
            try
            {
                int count = 0;
                if (pageNumber != 0) pageNumber--;
                List<PersonModel> personModels = new List<PersonModel>();
                List<Person> persons = new List<Person>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    persons = this._personRepository.GetByPagination((s => s.UserRoleId == (int)UserRoles.ClientId &
                    s.PersonCompany.CompanyId == companyId &&
                    s.IsAccountEnabled &&
                    !s.IsArchived && (s.FirstName.Contains(searchString) || s.LastName.Contains(searchString))), pageNumber, pageSize, out count);
                }
                else
                {
                    persons = this._personRepository.GetByPagination((s => s.UserRoleId == (int)UserRoles.ClientId &
                    s.PersonCompany.CompanyId == companyId && s.IsAccountEnabled && !s.IsArchived), pageNumber, pageSize, out count);
                }

                // Step 1: Extract IDs of fetched persons.
                List<long> personIds = persons.Select(p => p.PersonId).ToList();
                // Assume GetAppointmentsByPersonIds is a method that fetches appointments by person IDs.
                List<Appointment> appointments = _appointmentRepository.GetAppointmentsByPersonIds(personIds);

                if (persons != null && persons.Count > 0)
                {
                    DateTime currentTime = DateTime.UtcNow;  // Get the current time
                    personModels = persons.Select(person => _personModelFactory.Create(person, appointments)).ToList();
                }
                return new ClientPaginatedModel()
                {
                    Clients = personModels,
                    PageNumber = pageNumber + 1,
                    RecordsReturned = personModels.Count,
                    TotalCount = count
                };
            }
            catch (Exception) { throw; }
        }

        public bool UpdateClient(PersonModel entity)
        {
            try
            {
                Person person = this._personRepository.Get(s => s.PersonId == entity.PersonId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (person == null) return false;
                person.BirthDate = entity.DOB.GetValueOrDefault().Date;
                person.EmailAddress = entity.EmailAddress;
                person.FirstName = entity.FirstName;
                person.LastName = entity.LastName;
                person.Phone = entity.Phone;
                person.Notes = entity.Notes;
                bool isUpdated = this._personRepository.Update(person).ConfigureAwait(false).GetAwaiter().GetResult();

                List<ClientTag> tags = this._clientTagRepository.GetAll(s => s.PersonId == entity.PersonId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (tags != null && tags.Count > 0)
                    this._clientTagRepository.Delete(entity.PersonId).ConfigureAwait(false).GetAwaiter().GetResult();
                if (entity.Tags != null && entity.Tags.Count > 0)
                {
                    tags = new List<ClientTag>();
                    this._clientTagRepository.AddList(entity.Tags.Select(s => new ClientTag() { PersonId = entity.PersonId, TagId = s.Id }).ToList()).ConfigureAwait(false).GetAwaiter().GetResult();
                }
                return isUpdated;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<AllFormsModel> GetClientForms(long companyId, long personId)
        {
            try
            {
                List<AllFormsModel> model = new List<AllFormsModel>();
                var calendar = (from s in this._dbContext.Calendar.Include(x => x.TimeZone)
                                join cc in this._dbContext.CompanyCalendar
                                on s.CalendarId equals cc.CalendarId
                                where cc.CompanyId == companyId
                                select s).FirstOrDefault();

                if (calendar != null)
                {
                    var covidList = GetClientCovidScreeningForms(calendar, companyId, personId);
                    model.AddRange(covidList);

                    var generalForms = GetClientGeneralForms(calendar, companyId, personId);
                    model.AddRange(generalForms);

                    var intakeForms = GetClientIntakeForms(calendar, companyId, personId);
                    model.AddRange(intakeForms);
                }

                return model.OrderByDescending(d => d.SubmissionDate).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<AllFormsModel> GetClientCovidScreeningForms(Calendar calendar, long companyId, long personId)
        {
            List<AllFormsModel> covidScreeningFormModels = new List<AllFormsModel>();

            covidScreeningFormModels = (from c in _dbContext.CovidScreeningForm
                                        where c.CompanyId == companyId && c.PersonId == personId && !c.IsArchived
                                        select new AllFormsModel
                                        {
                                            FormId = c.CovidFormId,
                                            FormType = "Covid Form",
                                            SubmissionDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(c.SubmissionDate, calendar.TimeZone.SystemTimeZoneId),
                                            Name = "",
                                            UniqueId = "0",
                                            ProviderId = 0
                                        }).ToList();

            return covidScreeningFormModels;
        }

        public List<MessageLogModel> GetClientMessageHistory(long companyId, long personId, int pageNumber, int pageSize)
        {
            var messageLogs = _messageLogRepository.GetAll(m => m.CompanyId == companyId && m.PersonId == personId).ConfigureAwait(false).GetAwaiter().GetResult();

            // Skip the records before the current page and take only the records of the current page
            messageLogs = messageLogs.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            // Create MessageLogModel list using factory and LINQ
            List<MessageLogModel> messageLogModels = messageLogs.Select(entity =>
                _messageLogModelFactory.Create(entity.MessageLogId, entity.DateTimeMessageSent, entity.CompanyId,
                                               entity.PersonId, entity.IsEmail, entity.Subject, entity.Body,
                                               entity.IsAdminEmail, entity.Person.FirstName + " " + entity.Person.LastName))
            .ToList();

            return messageLogModels;
        }

        public async Task<List<PersonPackageModel>> GetClientPackages(long personId, int pageNumber, int pageSize)
        {
            var personPackages = await _packageRepository.GetAllIncluding(
                                    pp => pp.PersonPurchasedForId == personId,
                                    pp => pp.Package,  
                                    pp => pp.Person,    
                                    pp => pp.Package.Product
                                   );


            personPackages = personPackages.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return _personPackageModelFactory.CreateList(personPackages);
        }


        public List<AllFormsModel> GetClientGeneralForms(Calendar calendar, long companyId, long personId)
        {
            List<AllFormsModel> generalFormModelSubmissionModels = new List<AllFormsModel>();

            generalFormModelSubmissionModels = (from c in _dbContext.GeneralFormClientSubmission
                                                join gf in _dbContext.GeneralForm
                                                on c.GeneralFormId equals gf.GeneralFormId
                                                where c.CompanyId == companyId && c.PersonId == personId && !c.IsArchived
                                                select new AllFormsModel
                                                {
                                                    FormId = c.GeneralFormClientSubmisionId,
                                                    FormType = "General Form",
                                                    SubmissionDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(c.SubmissionDate, calendar.TimeZone.SystemTimeZoneId),
                                                    Name = gf.Name,
                                                    UniqueId = "0",
                                                    ProviderId = 0
                                                }).ToList();

            return generalFormModelSubmissionModels;
        }


        public List<AllFormsModel> GetClientIntakeForms(Calendar calendar, long companyId, long personId)
        {
            List<AllFormsModel> intakeFormsModels = new List<AllFormsModel>();

            intakeFormsModels = (from c in _dbContext.IntakeFormClientSubmission
                                 join it in _dbContext.IntakeFormTemplate
                                 on c.IntakeFormId equals it.IntakeFormId
                                 where c.CompanyId == companyId && c.PersonId == personId && !c.IsArchived
                                 select new AllFormsModel
                                 {
                                     FormId = c.IntakeFormSubmissionId,
                                     FormType = "Intake Form	",
                                     SubmissionDate = TimeZoneHelper.ConvertTimeToUsersTimeZone(c.SubmissionDate, calendar.TimeZone.SystemTimeZoneId),
                                     Name = it.Name,
                                     UniqueId = "0",
                                     ProviderId = 0
                                 }).ToList();

            return intakeFormsModels;
        }
    }
}
