using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class SyllabusOutputStandardConfig : IEntityTypeConfiguration<SyllabusOutputStandard>
    {
        public void Configure(EntityTypeBuilder<SyllabusOutputStandard> builder)
        {
            //set Pk
            builder.HasKey(k => new { k.SyllabusId, k.OutputStandardId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            //set realtion
            builder.HasOne<Syllabus>(s => s.Syllabus)
                .WithMany(s => s.SyllabusOutputStandards)
                .HasForeignKey(s => s.SyllabusId);

            builder.HasOne<OutputStandard>(o => o.OutputStandard)
                .WithMany(o => o.SyllabusOutputStandards)
                .HasForeignKey(fk => fk.OutputStandardId);

        }
    }
}
