using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApptHeroAPI.Repositories.Abstraction.Contracts;
using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Repositories.Implementation.Concrete;
using ApptHeroAPI.Services.Abstraction.Contracts;
using Microsoft.AspNetCore.Authentication;
using ApptHeroAPI.Services.Implementation.Concrete;
using ApptHeroAPI.Repositories.Context.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using ApptHeroAPI.Services.Implementation;
using ApptHeroAPI.Services.Concrete.Implementation;
using Microsoft.AspNetCore.Mvc.Versioning;
using ApptHeroAPI.Services.Abstraction.Models;
using ApptHeroAPI.Common.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ApptHeroAPI.Repositories.Abstraction.Abstracts;
using Microsoft.AspNetCore.Mvc.Filters;
using ApptHeroAPI.Middleware;
using ApptHeroAPI.Services.Implementation.Factory;

namespace ApptHeroAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            //var builder = new ConfigurationBuilder()
            //    .SetBasePath(env.ContentRootPath)
            //    .AddJsonFile(Path.Combine(env.ContentRootPath, "ApptHeroAPI", "appsettings.json"), optional: false, reloadOnChange: true)
            //    .AddJsonFile(Path.Combine(env.ContentRootPath, "ApptHeroAPI", $"appsettings.{env.EnvironmentName}.json"), optional: true)
            //    .AddEnvironmentVariables();

            //Configuration = builder.Build();
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });


            services.AddHttpContextAccessor();
            
            services.AddVersionedApiExplorer(opt => opt.GroupNameFormat = "'v'VVV");
            services.AddAuthentication(
                s =>
                {
                    s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
                ).AddJwtBearer(s =>
                {
                    //Configuration.GetSection("TokenSecretKey").Value
                    s.SaveToken = true;
                    s.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is a test key for api authentication")),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.Configure<AppSettings>(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApptHero API", Version = "1.0" });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "ApptHero API", Version = "2.0" });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                            },
                 },
                        new string[] {}
                } });
                c.ResolveConflictingActions(apiDescription => apiDescription.First());
            });
            services.AddApiVersioning(s =>
            {
                s.DefaultApiVersion = new ApiVersion(1, 0);
                s.AssumeDefaultVersionWhenUnspecified = false;
                s.ReportApiVersions = true;
                s.ApiVersionReader = new HeaderApiVersionReader("version");
            });

            //https://stackoverflow.com/questions/57626878/the-json-value-could-not-be-converted-to-system-int32
            services.AddControllers().AddNewtonsoftJson();

            //************************CONTEXT***************************//
            services.AddDbContextFactory<SqlDbContext>(s => s.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);
            services.AddDbContext<SqlDbContext>(s => s.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Scoped);

            //***********************FACTORY*************************//
            services.AddScoped<IMessageLogModelFactory, MessageLogModelFactory>();
            services.AddScoped<IAppointmentViewModelFactory, AppointmentViewModelFactory>();
            services.AddScoped<IStateProvinceModelFactory, StateProvinceModelFactory>();
            services.AddScoped<IAddressModelFactory, AddressModelFactory>();
            services.AddScoped<IPersonModelFactory, PersonModelFactory>();
            services.AddScoped<IPersonPackageModelFactory, PersonPackageModelFactory>();

            //***********************REPOSITORY*************************//
            services.AddScoped(typeof(IRepository<PersonPackage>), typeof(Repository<PersonPackage>));
            services.AddScoped<IRepository<MessageLog>, MessageLogRepository>();
            services.AddScoped<IAuthenticationRepository<Person>, AuthenticationRepository>();
            services.AddScoped<ServiceRepository, AppointmentTypeRepository>();
            services.AddScoped<IRepository<AppointmentTypeCategory>, AppointmentTypeCategoryRepository>();
            services.AddScoped<IRepository<Product>, ProductRepository>();
            services.AddScoped<IRepository<Addon>, AddOnRepository>();
            services.AddScoped<IRepository<Person>, PersonRepository>();
            services.AddScoped<ClientRepository, PersonRepository>();
            services.AddScoped<IAppointmentCategoryRepository, AppointmentCategoryRepository>();
            services.AddTransient<IExceptionLogger<ApiErrorLog>, ApiExceptionLogRepository>();
            services.AddScoped<IRepository<Appointment>, AppointmentRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IRepository<Calendar>, CalendarRepository>();
            services.AddScoped<IRepository<AppointmentSeries>, AppointmentSeriesRepository>();
            services.AddScoped<IRepository<Company>, CompanyRepository>();
            services.AddScoped<IRepository<CompanyEmailSetting>, CompanyEmailSettingRepository>();
            services.AddScoped<IRepository<CompanySetting>, CompanySettingRepository>();
            services.AddScoped<IRepository<GeneralForm>, GeneralFormRepository>();
            services.AddScoped<IRepository<IntakeFormClientSubmission>, IntakeFormClientRepository>();
            services.AddScoped<IRepository<IntakeFormTemplateAppointmentTypes>, IntakeFormTemplateApptTypeRepository>();
            services.AddScoped<IRepository<ApptHeroAPI.Repositories.Context.Entities.TimeZone>, TimeZoneRepository>();
            services.AddScoped<IRepository<BlockedOffTime>, BlockedOffTimeRepository>();
            services.AddScoped<IRepository<BlockedOffTimeSeries>, BlockedOffTimeSeriesRepository>();
            services.AddScoped<IRepository<AppointmentTypeAddon>, AppointmentTypeAddonRepository>();
            services.AddScoped<IRepository<ClientTag>, ClientTagRepository>();
            services.AddScoped<IRepository<TagType>, TagTypeRepository>();
            services.AddScoped<IRepository<UserRole>, UserRoleRepository>();
            services.AddScoped<IRepository<TeammateAddons>, TeammateAddOnRepository>();
            services.AddScoped<IRepository<OverrideAvailability>, OverrideAvailabilityRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            var claimNames = Configuration.GetSection("ClaimNames").Get<List<string>>();
            services.AddScoped(provider => new ReusableActionFilter(claimNames));

            services.AddControllers(options =>
            {
                options.Filters.Add<ReusableActionFilter>();
            });

            //************************SERVIES***************************//
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAppointmentTypeService, AppointmentTypeService>();
            services.AddScoped<IClientService<PersonModel>, ClientService>();
            services.AddSingleton<EMailHelper>();
            services.AddScoped<ITimeZoneService, TimeZoneService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentEmailService, AppointmentEmailService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IFormService, FormsService>();
            services.AddScoped<ITeammateService, TeammateService>();
            services.AddScoped<ICustomizeEmailService, CustomizeEmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllowSpecificOrigin");

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async s =>
              {
                  try
                  {
                      var scope = app.ApplicationServices.CreateScope();//.GetService<IAuthService>();
                      var errorRepo = scope.ServiceProvider.GetRequiredService<IExceptionLogger<ApiErrorLog>>();
                      var contextFeature = s.Features.Get<IExceptionHandlerFeature>();

                      string refrenceNumber = await errorRepo.LogAsync(contextFeature.Error);
                      s.Response.StatusCode = 500;
                      await s.Response.WriteAsync($"Something went wrong. Please check with App admin.\rReferenceId: {refrenceNumber}");
                  }
                  catch (Exception)
                  {
                      s.Response.StatusCode = 500;
                      await s.Response.WriteAsync($"Something went wrong. Please check with App admin.");
                  }
              });
            });
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "V1");
                s.SwaggerEndpoint("/swagger/v2/swagger.json", "V2");
            });
          

            app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();
            var claimNames = this.Configuration.GetSection("ClaimNames").Get<List<string>>();
            app.UseMiddleware<ReusableJwtMiddleware>(this.Configuration, claimNames);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
