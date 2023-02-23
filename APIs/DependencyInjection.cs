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
<<<<<<< HEAD
        services.AddScoped<IValidator<UpdateClassViewModel>, UpdateClassValidation>();
        services.AddScoped<IValidator<CreateClassViewModel>, CreateClassValidation>();
=======
        services.AddScoped<IValidator<ClassViewModel>, ClassValidation>();
        services.AddScoped<IValidator<AuditPlanViewModel>, AuditPlanValidation>();
        services.AddScoped<IValidator<UpdateAuditPlanViewModel>, UpdateAuditPlanValidation>();
>>>>>>> 32fd97d03e4718cc08c23206f91c1c4bd0ba2aec
        services.AddScoped<IValidator<ModuleViewModels>, ModuleValidation>();
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
