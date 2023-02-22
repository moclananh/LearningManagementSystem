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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        // DbSet<>
        DbSet<AbsentRequest> AbsentRequests { get; set; }
        DbSet<Assignment> Assignments { get; set; }
        DbSet<AssignmentQuestion> AssignmentQuestions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Attendance> Attendances { get; set; }
        DbSet<AuditPlan> AuditPlans { get; set; }
        DbSet<AuditQuestion> AuditQuestions { get; set; }
        DbSet<AuditResult> AuditResults { get; set; }
        DbSet<Class> Classes { get; set; }
        DbSet<Lecture> Lectures { get; set; }
        DbSet<Domain.Entities.Module> Modules { get; set; }
        DbSet<OutputStandard> OutputStandards { get; set; }
        DbSet<Practice> Practices { get; set; }
        DbSet<PracticeQuestion> PracticesQuestions { get; set; }
        DbSet<Quizz> Quizzs { get; set; }
        DbSet<QuizzQuestion> QuizzQuestions { get; set; }
        DbSet<Syllabus> Syllabi { get; set; }
        DbSet<TrainingProgram> TrainingPrograms { get; set; }
        DbSet<Unit> Units { get; set; }
        DbSet<ClassTrainingProgram> ClassTrainingProgram { get; set; }
        DbSet<ClassUser> ClassUser { get; set; }
        DbSet<ModuleUnit> ModuleUnit { get; set; }
        DbSet<SyllabusModule> SyllabusModule { get; set; }
        DbSet<SyllabusOutputStandard> SyllabusOutputStandard { get; set; }
        DbSet<TrainingProgramSyllabus> TrainingProgramSyllabi { get; set; }
        DbSet<UserAuditPlan> UserAuditPlan { get; set; }
    }
}
