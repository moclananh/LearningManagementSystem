using APIs.Validations;
using Applications.ViewModels.ClassViewModels;
using FluentValidation;

namespace APIs
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            services.AddScoped<IValidator<ClassViewModel>, ClassValidation>();

            return services;
        }
    }
}
