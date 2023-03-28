using Applications.Interfaces;
using Applications.Services;
using Applications;
using Applications.IRepositories;
using Applications.Repositories;
using Infrastructures.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Repositories;
using Infrastructure.Repositories;
using Application.Interfaces;
using Application.Services;
using Infrastructures.Mappers.UserMapperResovlers;

namespace Infrastructures
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentTime, CurrentTime>();
            // local; DBName: LMSFSoftDB
            services.AddDbContext<AppDBContext>(options => options.UseSqlServer(config.GetConnectionString("AppDB")));
            // Add Object Services
            services.AddScoped<IClassService, ClassServices>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IQuizzService, QuizzService>();
            services.AddScoped<IQuizzRepository, QuizzRepository>();
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<IOutputStandardRepository, OutputStandardRepository>();
            services.AddScoped<IOutputStandardService, OutputStandardService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IClassUserRepository, ClassUserRepository>();
            services.AddScoped<IClassUserServices, ClassUserService>();
            services.AddScoped<IAuditPlanService, AuditPlanService>();
            services.AddScoped<IAuditPlanRepository, AuditPlanRepository>();
            services.AddScoped<ILectureService, LectureServices>();
            services.AddScoped<ILectureRepository, LectureRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUnitServices, UnitServices>();
            services.AddScoped<IModuleService, ModuleService>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<ITrainingProgramRepository, TrainingProgramRepository>();
            services.AddScoped<ITrainingProgramService, TrainingProgramService>();
            services.AddScoped<ISyllabusServices, SyllabusServices>();
            services.AddScoped<ISyllabusRepository, SyllabusRepository>();
            services.AddScoped<IAssignmentQuestionRepository, AssignmentQuestionRepository>();
            services.AddScoped<IAssignmentQuestionService, AssignmentQuestionService>();
            services.AddScoped<IClassTrainingProgramRepository, ClassTrainingProgramRepository>();
            services.AddScoped<ISyllabusOutputStandardRepository, SyllabusOutputStandardRepository>();
            services.AddScoped<ISyllabusOutputStandardService, SyllabusOutputStandardService>();
            services.AddScoped<IPracticeRepository, PracticeRepository>();
            services.AddScoped<IPracticeService, PracticeService>();
            services.AddScoped<IAuditResultRepository, AuditResultRepository>();
            services.AddScoped<IAuditResultServices, AuditResultServices>();
            services.AddScoped<IPracticeQuestionRepository, PracticeQuestionRepository>();
            services.AddScoped<IPracticeQuestionService, PracticeQuestionService>();
            services.AddScoped<ITrainingProgramSyllabiRepository, TrainingProgramSyllabiRepository>();
            services.AddScoped<ISyllabusTrainingProgramService, SyllabusTrainingProgramService>();
            services.AddScoped<IModuleUnitRepository, ModuleUnitRepository>();
            services.AddScoped<IModuleUnitService, ModuleUnitService>();
            services.AddScoped<IUserAuditPlanRepository, UserAuditPlanRepository>();
            services.AddScoped<IQuizzQuestionRepository, QuizzQuestionRepository>();
            services.AddScoped<IQuizzQuestionService, QuizzQuestionService>();
            services.AddScoped<IClassTrainingProgramRepository, ClassTrainingProgramRepository>();
            services.AddScoped<IClassTrainingProgramService, ClassTrainingProgramService>();
            services.AddScoped<ISyllabusModuleRepository, SyllabusModuleRepository>();
            services.AddScoped<ISyllabusModuleService, SyllabusModuleService>();
            services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IAbsentRequestServices, AbsentRequestService>();
            services.AddScoped<IAbsentRequestRepository, AbsentRequestRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}
