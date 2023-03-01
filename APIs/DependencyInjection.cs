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
using Applications.Utils;
using static Org.BouncyCastle.Math.EC.ECCurve;
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

namespace APIs;

public static class DependencyInjection
{
    public static IServiceCollection AddWebAPIService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClaimService, ClaimsService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddControllers();
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
                    ValidateAudience = false
                };
            });
        services.AddAuthorization(opt =>
        {
            // set Policy
            //opt.AddPolicy("require", policy => policy.RequireRole("User"));

        });
        //-------------------------------------------------------------------------------------------
        return services;
    }
}
