using Applications;
using Applications.Interfaces;
using Applications.IRepositories;
using Applications.Repositories;
using Applications.Services;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // local; DBName: LMSFSoftDB
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(config.GetConnectionString("AppDB")));
            // Add Object Services
            services.AddScoped<IClassServices, ClassServices>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IQuizzServices, QuizzServices>();
            services.AddScoped<IQuizzRepository, QuizzRepository>();

            return services;
        }
    }
}
