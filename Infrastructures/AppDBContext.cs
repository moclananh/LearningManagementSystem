using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructures
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }

        // DbSet<>
        public DbSet<AbsentRequest> AbsentRequests { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentQuestion> AssignmentQuestions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<AuditPlan> AuditPlans { get; set; }
        public DbSet<AuditQuestion> AuditQuestions { get; set; }
        public DbSet<AuditResult> AuditResults { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Domain.Entities.Module> Modules { get; set; }
        public DbSet<OutputStandard> OutputStandards { get; set; }
        public DbSet<Practice> Practices { get; set; }
        public DbSet<PracticeQuestion> PracticesQuestions { get; set; }
        public DbSet<Quizz> Quizzs { get; set; }
        public DbSet<QuizzQuestion> QuizzQuestions { get; set; }
        public DbSet<Syllabus> Syllabi { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<ClassTrainingProgram> ClassTrainingProgram { get; set; }
        public DbSet<ClassUser> ClassUser { get; set; }
        public DbSet<ModuleUnit> ModuleUnit { get; set; }
        public DbSet<SyllabusModule> SyllabusModule { get; set; }
        public DbSet<SyllabusOutputStandard> SyllabusOutputStandard { get; set; }
        public DbSet<TrainingProgramSyllabus> TrainingProgramSyllabi { get; set; }
        public DbSet<UserAuditPlan> UserAuditPlan { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
