using APIs.Validations;
using Applications.ViewModels.ClassViewModels;
using FluentValidation;
using APIs.Services;
using Applications.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
        services.AddScoped<IValidator<ClassViewModel>, ClassValidation>();
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
