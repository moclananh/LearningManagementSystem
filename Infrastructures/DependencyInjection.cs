using Applications.Interfaces;
using Applications.Services;
using Applications;
using Applications.IRepositories;
using Applications.Repositories;
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
            services.AddScoped<ICurrentTime,CurrentTime>();
            // local; DBName: LMSFSoftDB
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(config.GetConnectionString("AppDB")));
            // Add Object Services
            services.AddScoped<IClassServices, ClassServices>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IQuizzServices, QuizzServices>();
            services.AddScoped<IQuizzRepository, QuizzRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILectureServices, LectureServies>();
            services.AddScoped<ILectureRepository, LectureRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUnitServices, UnitServices>();
            services.AddScoped<IModuleServices, ModuleService>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            return services;
        }
    }
}
