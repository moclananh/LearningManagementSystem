using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class TrainingProgramSyllabusConfig : IEntityTypeConfiguration<TrainingProgramSyllabus>
    {
        public void Configure(EntityTypeBuilder<TrainingProgramSyllabus> builder)
        {
            // Set PK
            builder.HasKey(k => new { k.SyllabusId, k.TrainingProgramId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            // Set Relation
            builder.HasOne<Syllabus>(u => u.Syllabus)
                .WithMany(cu => cu.TrainingProgramSyllabi)
                .HasForeignKey(fk => fk.SyllabusId);

            builder.HasOne<TrainingProgram>(c => c.TrainingProgram)
                .WithMany(cu => cu.TrainingProgramSyllabi)
                .HasForeignKey(fk => fk.TrainingProgramId);

        }
    }
}
