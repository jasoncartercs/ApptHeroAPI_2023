using ApptHeroAPI.Repositories.Context.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ApptHeroAPI.Repositories.Context.Context
{
    public class SqlDbContext : DbContext
    {
        public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
      
            builder.Entity<PersonPackage>()
                .HasOne(e => e.Person) // specify the navigation property on the dependent entity
                .WithMany(e => e.PersonPackages) // specify the collection navigation property on the principal entity
                .HasForeignKey(e => e.PersonPurchasedForId) // specify the foreign key property on the dependent entity
                .OnDelete(DeleteBehavior.Restrict); // disable cascade delete

            builder.Entity<PersonPackage>()
                .HasOne(e => e.Person1) // specify the navigation property on the dependent entity
                .WithMany(e => e.PersonPackages1) // specify the collection navigation property on the principal entity
                .HasForeignKey(e => e.PersonPurchasedId) // specify the foreign key property on the dependent entity
                .OnDelete(DeleteBehavior.Restrict); // disable cascade delete

            builder.Entity<CompanyCalendar>().HasKey(table => new {
                table.CompanyId,
                table.CalendarId
            });

            builder.Entity<TeamMemberPermission>().HasKey(table => new {
                table.PersonId,
                table.PermissionId
            });

            builder.Entity<PersonCompany>().HasKey(table => new {
                table.PersonId,
                table.CompanyId
            });

            builder.Entity<AppointmentAddon>().HasKey(table => new {
                table.AppointmentId,
                table.AddonId
            });

            builder.Entity<IntakeFormData>().HasKey(table => new {
                table.IntakeFormId,
                table.IntakeFormCategoryId
            });

            builder.Entity<IntakeFormTemplateAppointmentTypes>().HasKey(table => new {
                table.AppointmentTypeId,
                table.IntakeFormId
            });


            builder.Entity<TeamMemberAppointmentType>().HasKey(table => new {
                table.AppointmentTypeId,
                table.PersonId
            });

            builder.Entity<TeammateAddons>().HasKey(table => new {
                table.AddonId,
                table.PersonId
            });

            builder.Entity<IntakeFormData>(entity =>
            {
                entity.HasKey(e => new { e.IntakeFormId, e.IntakeFormCategoryId })
                    .HasName("PK_IntakeFormTemplate");

                entity.Property(e => e.FormData).HasColumnType("text");

                entity.HasOne(d => d.IntakeFormCategory)
                    .WithMany(p => p.IntakeFormDataIntakeFormCategories)
                    .HasForeignKey(d => d.IntakeFormCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IntakeFormData_IntakeFormCategory");

                entity.HasOne(d => d.IntakeForm)
                    .WithMany(p => p.IntakeFormData)
                    .HasForeignKey(d => d.IntakeFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IntakeFormData_IntakeFormTemplate");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.IntakeFormDataParents)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_IntakeFormData_IntakeFormCategory1");
            });


            builder.Entity<RefreshToken>()
           .HasOne(rt => rt.Person) // Assuming you have a Person navigation property in your RefreshToken model
           .WithMany(p => p.RefreshTokens) // Assuming you have a RefreshTokens navigation property in your Person model
           .HasForeignKey(rt => rt.PersonId) // Assuming the foreign key property is named 'PersonId' in your RefreshToken model
           .OnDelete(DeleteBehavior.Cascade); // or .OnDelete(DeleteBehavior.Restrict) based on your requirements

            //builder.Entity<GeneralFormAppointmentTypes>(entity =>
            //{
            //    entity.HasKey(e => new { e.GeneralFormId, e.AppointmentTypeId });

            //    entity.HasOne(d => d.AppointmentType)
            //        .WithMany(p => p.GeneralFormAppointmentTypes)
            //        .HasForeignKey(d => d.AppointmentTypeId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_GeneralFormAppointmentTypes_AppointmentType");

            //    entity.HasOne(d => d.GeneralForm)
            //        .WithMany(p => p.GeneralFormAppointmentTypes)
            //        .HasForeignKey(d => d.GeneralFormId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK_GeneralFormAppointmentTypes_GeneralForm");
            //});



            //builder.Entity<Calendar>()
            //    .HasMany(e => e.Companies)
            //    .WithMany(e => e.Calendars)
            //    .UsingEntity(join => join.ToTable("CompanyCalendar"));

        }


        public DbSet<Person> Person { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<PersonCompany> PersonCompany { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<AppointmentType> AppointmentType { get; set; }
        public DbSet<AppointmentTypeCategory> AppointmentTypeCategory { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Addon> Addon { get; set; }
        public DbSet<ApiErrorLog> ApiErrorLogs { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Calendar> Calendar { get; set; }

        public DbSet<Permission> Permission { get; set; }

        public DbSet<StateProvince> StateProvince { get; set; }
        public DbSet<AppointmentSeries> AppointmentSeries { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<TimeZone> TimeZone { get; set; }
        public DbSet<IntakeFormClientSubmission> IntakeFormClientSubmission { get; set; }

        public DbSet<GeneralFormClientSubmission> GeneralFormClientSubmission { get; set; }

        public DbSet<CompanySetting> CompanySetting { get; set; }
        public DbSet<GeneralForm> GeneralForm { get; set; }
        public DbSet<CovidScreeningForm> CovidScreeningForm { get; set; }
        public DbSet<IntakeFormTemplate> IntakeFormTemplate { get; set; }

        public DbSet<IntakeFormData> IntakeFormData { get; set; }
        public DbSet<IntakeFormTemplateAppointmentTypes> IntakeFormTemplateAppointmentTypes { get; set; }
        public DbSet<CompanyEmailSetting> CompanyEmailSettings { get; set; }

        public DbSet<CompanyEmailType> CompanyEmailTypes { get; set; }

        public DbSet<AppointmentAddon> AppointmentAddon { get; set; }

        public DbSet<BlockedOffTime> BlockedOffTime { get; set; }

        public DbSet<OverrideAvailability> OverrideAvailability { get; set; }
        public DbSet<BlockedOffTimeSeries> BlockedOffTimeSeries { get; set; }
        public DbSet<AppointmentTypeAddon> AppointmentTypeAddon { get; set; }

        public virtual DbSet<MessageLog> MessageLogs { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<TagType> TagType { get; set; }
        public DbSet<ClientTag> ClientTag { get; set; }

        public DbSet<CompanyCalendar> CompanyCalendar { get; set; }

        public DbSet<TeamMemberPermission> TeamMemberPermission { get; set; }

        public DbSet<PaymentOrder> PaymentOrder { get; set; }
        public DbSet<PaymentOrderMadeWith> PaymentOrderMadeWith { get; set; }

        public DbSet<PaymentOrderDetail> PaymentOrderDetail { get; set; }

        public DbSet<HoursOfOperation> HoursOfOperation { get; set; }

        public DbSet<TeammateAddons> TeammateAddons { get; set; }

        public DbSet<Package> Packages { get; set; }
        public DbSet<PersonPackage> PersonPackages { get; set; }
    }
}