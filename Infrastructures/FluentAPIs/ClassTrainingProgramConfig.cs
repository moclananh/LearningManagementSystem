using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class ClassTrainingProgramConfig : IEntityTypeConfiguration<ClassTrainingProgram>
    {
        public void Configure(EntityTypeBuilder<ClassTrainingProgram> builder)
        {
            // Set PK
            builder.HasKey(k => new { k.ClassId, k.TrainingProgramId });
            // Set Relation
            builder.HasOne<Class>(u => u.Class)
                .WithMany(cu => cu.ClassTrainingPrograms)
                .HasForeignKey(fk => fk.ClassId);

            builder.HasOne<TrainingProgram>(c => c.TrainingProgram)
                .WithMany(cu => cu.ClassTrainingPrograms)
                .HasForeignKey(fk => fk.TrainingProgramId);

        }
    }
}
