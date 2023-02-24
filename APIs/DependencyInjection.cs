using APIs.Validations;
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
        services.AddScoped<IValidator<UpdateClassViewModel>, UpdateClassValidation>();
        services.AddScoped<IValidator<CreateClassViewModel>, CreateClassValidation>();
        services.AddScoped<IValidator<AuditPlanViewModel>, AuditPlanValidation>();
        services.AddScoped<IValidator<UpdateAuditPlanViewModel>, UpdateAuditPlanValidation>();
        services.AddScoped<IValidator<ModuleViewModels>, ModuleValidation>();
        services.AddScoped<IValidator<UpdateSyllabusViewModel>, UpdateSyllabusValidation>();
        services.AddScoped<IValidator<CreateSyllabusViewModel>, CreateSyllabusValidation>();
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
