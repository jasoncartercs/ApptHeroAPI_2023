using ApptHeroAPI.Services.Abstraction.Contracts;
using System;
using System.Collections.Generic;
using ApptHeroAPI.Common.Utilities;
using ApptHeroAPI.Repositories.Context.Context;
using ApptHeroAPI.Services.Abstraction.Enum;
using ApptHeroAPI.Services.Abstraction.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApptHeroAPI.Services.Implementation.Concrete
{
    public class FormsService : IFormService
    {
        private readonly SqlDbContext _dbContext;
        public FormsService(IDbContextFactory<SqlDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
        }
        public FormSettingsModel GetFormSettings(long companyId)
        {
            var formSettingsModel = new FormSettingsModel();

            var companySettings = _dbContext.CompanySetting.Where(cs => cs.CompanyId == companyId).FirstOrDefault();

            if (companySettings != null)
            {
                formSettingsModel.ShouldSendPreScreeningForm = companySettings.ShouldSendPreScreeningForm;
                formSettingsModel.ShouldSendCovid19Form = companySettings.ShouldSendCovid19Form;
                formSettingsModel.ShouldSendIntakeFormToNewClients = companySettings.ShouldSendIntakeFormToNewClients;
            }

            return formSettingsModel;
        }


        public List<AllFormsModel> GetForms(long companyId)
        {
            try
            {
                List<AllFormsModel> model = new List<AllFormsModel>();

                var generalForms = GetGeneralForms(companyId);
                model.AddRange(generalForms);

                var intakeForms = GetIntakeForms(companyId);
                model.AddRange(intakeForms);

                return model.OrderByDescending(d => d.SubmissionDate).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IntakeFormModel GetIntakeForm(long intakeFormId)
        {
            IntakeFormModel intakeFormModel = new IntakeFormModel();

            var intakeForm = _dbContext.IntakeFormTemplate
              //  .Include(a => a.IntakeFormTemplateAppointmentTypes)
             //   .ThenInclude(x => x.AppointmentType)
             //   .ThenInclude(y => y.Product)
                .FirstOrDefault(i => i.IntakeFormId == intakeFormId);
            if (intakeForm != null)
            {
                intakeFormModel = new IntakeFormModel()
                {
                    //CompanyId = intakeForm.CompanyId,
                    //ConsentPolicy = intakeForm.ConsentPolicy,
                    //CreatedDate = intakeForm.CreatedDate,
                    //Description = intakeForm.Description,
                    //IntakeFormId = intakeForm.IntakeFormId,
                    //IsArchived = intakeForm.IsArchived,
                    //IsTemplate = intakeForm.IsTemplate,
                    //LastModifiedDate = intakeForm.LastModifiedDate,
                    Name = intakeForm.Name,


                    //IntakeFormDataModels = (from intake in intakeForm.IntakeFormData
                    //                        select new IntakeFormDataModel
                    //                        {
                    //                            IntakeFormId = intake.IntakeFormId,
                    //                            ParentId = intake.ParentId,
                    //                            FormData = intake.FormData,
                    //                            IntakeFormCategoryId = intake.IntakeFormCategoryId,
                    //                            //SubCategoryName = intake.IntakeFormCategory.CategoryName,
                    //                            //CategoryName = intake.IntakeFormCategory1.CategoryName,

                    //                        }).ToList(),
                    //AppointmentTypeModels = (from at in intakeForm.IntakeFormTemplateAppointmentTypes
                    //                         select new AppointmentTypeModel
                    //                         {

                    //                             //AccessLevelName = at.AccessLevelName,
                    //                             // Color = at.Color,
                    //                             AppointmentTypeId = at.AppointmentTypeId,
                    //                             // BlockedOffMinutesAfterAppointment = at.BlockedOffMinutesAfterAppointment,
                    //                             // BlockedOffMinutesBeforeAppointment = at.BlockedOffMinutesBeforeAppointment,
                    //                             CompanyId = at.AppointmentType.Product.CompanyID,
                    //                             // ConfirmationMessage = at.ConfirmationMessage,
                    //                             // DurationInMinutes = at.Duration,
                    //                             AppointmentTypeName = at.AppointmentType.Product.Name,
                    //                             // SortOrder = at.SortOrder,
                    //                             ProductModel = new ProductModel
                    //                             {
                    //                                 CompanyID = at.AppointmentType.Product.CompanyID,
                    //                                 Description = at.AppointmentType.Product.Description,
                    //                                 ImageUrl = at.AppointmentType.Product.ImageUrl,
                    //                                 Price = at.AppointmentType.Product.Price,
                    //                             }
                    //                         }).ToList()
                };
            }
            return intakeFormModel;
        }

        public GeneralFormModel GetCustomForm(long customFormId)
        {
            GeneralFormModel generalFormModel = new GeneralFormModel();
            var generalForm = _dbContext.GeneralForm
                .Include(y => y.GeneralFormAppointmentTypes)
              //  .ThenInclude(x => x.AppointmentType)
             //   .ThenInclude(z => z.Product)
                .Where(g => g.GeneralFormId == customFormId).FirstOrDefault();
            if (generalForm != null)
            {
                generalFormModel = new GeneralFormModel
                {
                    CompanyId = generalForm.CompanyId,
                    ConsentPolicy = generalForm.ConsentPolicy,
                    CreatedDate = generalForm.CreatedDate,
                    Description = generalForm.Description,
                    FormData = generalForm.FormData,
                    GeneralFormId = generalForm.GeneralFormId,
                    IsArchived = generalForm.IsArchived,
                    LastModifiedDate = generalForm.LastModifiedDate,
                    Name = generalForm.Name,
                    HeaderText = generalForm.HeaderText,
                    //CompanyModel = new CompanyModel
                    //{
                    //    CompanyId = generalForm.Company.CompanyId,
                    //    Name = generalForm.Company.Name
                    //},
                    //AppointmentTypeModels = (from at in generalForm.GeneralFormAppointmentTypes
                    //                         select new AppointmentTypeModel
                    //                         {
                    //                            // AccessLevelId = at.AccessLevelId,
                    //                            // AccessLevelName = at.AccessLevel.Name,
                    //                            // Color = at.Color,
                    //                             AppointmentTypeId = at.AppointmentTypeId,
                    //                            // BlockedOffMinutesAfterAppointment = at.BlockedOffMinutesAfterAppointment,
                    //                            // BlockedOffMinutesBeforeAppointment = at.BlockedOffMinutesBeforeAppointment,
                    //                             //CompanyId = at.AppointmentType.Product.CompanyId,
                    //                             //ConfirmationMessage = at.ConfirmationMessage,
                    //                             DurationInMinutes = at.AppointmentType.Duration,
                    //                             AppointmentTypeName = at.AppointmentType.Product.Name,
                    //                             ProductModel = new ProductModel
                    //                             {
                    //                                 //CompanyId = at.Product.CompanyId,
                    //                                 Description = at.AppointmentType.Product.Description,
                    //                                 ImageUrl = at.AppointmentType.Product.ImageUrl,
                    //                                 Price = at.AppointmentType.Product.Price,
                    //                             }
                    //                         }).ToList()

                };
            }


            return generalFormModel;
        }






        private List<AllFormsModel> GetGeneralForms(long companyId)
        {
            List<AllFormsModel> generalFormModelSubmissionModels = new List<AllFormsModel>();

            generalFormModelSubmissionModels = (from c in _dbContext.GeneralFormClientSubmission
                                                join gf in _dbContext.GeneralForm
                                                on c.GeneralFormId equals gf.GeneralFormId
                                                where c.CompanyId == companyId && !c.IsArchived
                                                select new AllFormsModel
                                                {
                                                    FormId = c.GeneralFormClientSubmisionId,
                                                    FormType = "General Form",
                                                    SubmissionDate = gf.CreatedDate,
                                                    Name = gf.Name,
                                                    UniqueId = "0",
                                                    ProviderId = 0
                                                }).ToList();

            return generalFormModelSubmissionModels;
        }

        private List<AllFormsModel> GetIntakeForms(long companyId)
        {
            List<AllFormsModel> intakeFormsModels = new List<AllFormsModel>();
            var intakeForms = _dbContext.IntakeFormTemplate.Where(i => i.CompanyId == companyId).ToList();

            //intakeFormsModels = (from c in intakeForms

            //                     select new AllFormsModel
            //                     {
            //                         FormId = c.IntakeFormId,
            //                         FormType = "Intake Form",
            //                         SubmissionDate = c.LastModifiedDate,
            //                         Name = c.Name,
            //                         UniqueId = "0",
            //                         ProviderId = 0
            //                     }).ToList();

            return intakeFormsModels;
        }

        public bool SaveFormSettings(FormSettingsModel formSettingsModel)
        {
            bool didSave = false;
            var companySetting = _dbContext.CompanySetting.Where(cs => cs.CompanyId == formSettingsModel.CompanyId).FirstOrDefault();

            if (companySetting != null)
            {
                companySetting.ShouldSendCovid19Form = formSettingsModel.ShouldSendCovid19Form;
                companySetting.ShouldSendIntakeFormToNewClients = formSettingsModel.ShouldSendIntakeFormToNewClients;
                companySetting.ShouldSendPreScreeningForm = formSettingsModel.ShouldSendPreScreeningForm;
                _dbContext.SaveChanges();
                didSave = true;
            }

            return didSave;

        }

        public bool DeleteIntakeForm(long intakeFormId)
        {
            bool didSave = false;

            var intakeForm = _dbContext.IntakeFormTemplate.Find(intakeFormId);

            if (intakeForm != null)
            {
                intakeForm.IsArchived = true;
                _dbContext.SaveChanges();
                didSave = true;
            }

            return didSave;
        }

        public bool RestoreIntakeForm(long intakeFormId)
        {
            bool didSave = false;

            var intakeForm = _dbContext.IntakeFormTemplate.Find(intakeFormId);

            if (intakeForm != null)
            {
                intakeForm.IsArchived = false;
                _dbContext.SaveChanges();
                didSave = true;
            }

            return didSave;
        }

        public bool DeleteCustomForm(long customFormId)
        {
            bool didSave = false;

            var generalForm = _dbContext.GeneralForm.Find(customFormId);

            if (generalForm != null)
            {
                generalForm.IsArchived = true;
                _dbContext.SaveChanges();
                didSave = true;
            }

            return didSave;
        }

        public bool RestoreCustomForm(long customFormId)
        {
            bool didSave = false;

            var generalForm = _dbContext.GeneralForm.Find(customFormId);

            if (generalForm != null)
            {
                generalForm.IsArchived = false;
                _dbContext.SaveChanges();
                didSave = true;
            }

            return didSave;
        }
    }
}
