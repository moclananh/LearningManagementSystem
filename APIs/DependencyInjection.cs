using Applications.ViewModels.ClassViewModels;
using FluentValidation;
using APIs.Services;
using Applications.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Applications.ViewModels.AuditPlanViewModel;
using Applications.ViewModels.ModuleViewModels;
using APIs.Validations.ClassValidations;
using APIs.Validations.SyllabusValidations;
using Applications.ViewModels.SyllabusViewModels;
using APIs.Validations.LectureValidations;
using Applications.ViewModels.LectureViewModels;
using Applications.Interfaces.EmailServicesInterface;
using Applications.Services.EmailServices;
using APIs.Validations.AuditPlanValidations;
using Application.ViewModels.QuizzViewModels;
using APIs.Validations.QuizzValidations;
using Application.ViewModels.UnitViewModels;
using APIs.Validations.UnitValidations;
using Applications.ViewModels.AssignmentViewModels;
using APIs.Validations.AssignmentValidations;
using APIs.Validations.ModulesValidations;
using APIs.Validations.TrainingProgramValidations;
using Application.ViewModels.TrainingProgramModels;
using Applications.ViewModels.TrainingProgramModels;
using Applications.ViewModels.PracticeViewModels;
using APIs.Validations.PracticeValidations;
using Applications.ViewModels.OutputStandardViewModels;
using APIs.Validations.OutputStandardValidations;
using Applications.ViewModels.AuditResultViewModels;
using APIs.Validations.AuditResultValidations;
using System.Text.Json.Serialization;
using Quartz;
using Application.Cronjob;
using APIs.Validations.AttendanceValidations;
using Applications.ViewModels.AttendanceViewModels;

namespace APIs;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClaimService, ClaimsService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddControllers().AddJsonOptions(opt =>
            opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddHealthChecks();
        services.AddHttpContextAccessor();
        //services.Configure<MailSetting>(configuration.GetSection(nameof(MailSetting)));  // tranfer data to an instance of MailSettings at runtime
        services.AddTransient<IMailService, MailService>();
        services.AddScoped<IValidator<UpdateClassViewModel>, UpdateClassValidation>();
        services.AddScoped<IValidator<CreateClassViewModel>, CreateClassValidation>();
        services.AddScoped<IValidator<AuditPlanViewModel>, AuditPlanValidation>();
        services.AddScoped<IValidator<UpdateAuditPlanViewModel>, UpdateAuditPlanValidation>();
        services.AddScoped<IValidator<CreateAuditPlanViewModel>, CreateAuditPlanValidation>();
        services.AddScoped<IValidator<CreateModuleViewModel>, CreateModuleValidation>();
        services.AddScoped<IValidator<UpdateModuleViewModel>, UpdateModuleValidation>();
        services.AddScoped<IValidator<UpdateSyllabusViewModel>, UpdateSyllabusValidation>();
        services.AddScoped<IValidator<CreateSyllabusViewModel>, CreateSyllabusValidation>();
        services.AddScoped<IValidator<CreateLectureViewModel>, CreateLectureValidation>();
        services.AddScoped<IValidator<UpdateLectureViewModel>, UpdateLectureValidation>();
        services.AddScoped<IValidator<CreateQuizzViewModel>, CreateQuizzValidation>();
        services.AddScoped<IValidator<UpdateQuizzViewModel>, UpdateQuizzValidation>();
        services.AddScoped<IValidator<CreateUnitViewModel>, UnitValidation>();
        services.AddScoped<IValidator<CreateAssignmentViewModel>, CreateAssignmentValidation>();
        services.AddScoped<IValidator<UpdateAssignmentViewModel>, UpdateAssignmentValidation>();
        services.AddScoped<IValidator<CreateTrainingProgramViewModel>, CreateTrainingProgramValidation>();
        services.AddScoped<IValidator<UpdateTrainingProgramViewModel>, UpdateTrainingProgramValidation>();
        services.AddScoped<IValidator<UpdatePracticeViewModel>, UpdatePracticeValidation>();
        services.AddScoped<IValidator<UpdateOutputStandardViewModel>, UpdateOutputStandardValidation>();
        services.AddScoped<IValidator<UpdateAuditResultViewModel>, UpdateAuditResultValidation>();
        services.AddScoped<IValidator<CreatePracticeViewModel>, CreatePracticeValidation>();
        services.AddScoped<IValidator<CreateOutputStandardViewModel>, CreateOutputStandardValidation>();
        services.AddScoped<IValidator<ClassFiltersViewModel>, ClassFilterValidation>();
        services.AddScoped<IValidator<AttendanceFilterViewModel>, AttendanceFilterValidation>();
        //---------------------------------------------------------------------------------------
        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration.GetSection("Jwt:SecretKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        services.AddScoped<MailService>();
        services.AddTransient<SendAttendanceMailJob>();
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.ScheduleJob<SendAttendanceMailJob>(job => job
                .WithIdentity("AttendanceMailJob")
                .WithDescription("Sends attendance report emails at 11:30 am every day.")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(11, 30))
            );
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
        services.AddAuthorization(opt =>
        {
            // set Policy
            opt.AddPolicy("AuthUser", policy => policy.RequireAuthenticatedUser()); // only user already login
            opt.AddPolicy("All", policy => policy.RequireRole("SuperAdmin","ClassAdmin","Trainer","Student","Mentor","Auditor"));
            opt.AddPolicy("OnlySupperAdmin",policy => policy.RequireRole("SuperAdmin"));
            opt.AddPolicy("Admins", policy => policy.RequireRole("SuperAdmin", "ClassAdmin"));
            opt.AddPolicy("Audits", policy => policy.RequireRole("SuperAdmin", "Auditor"));
            opt.AddPolicy("AuditResults", policy => policy.RequireRole("SuperAdmin", "Auditor", "Mentor", "Trainer"));
        });
        //-------------------------------------------------------------------------------------------
        return services;
    }
}
