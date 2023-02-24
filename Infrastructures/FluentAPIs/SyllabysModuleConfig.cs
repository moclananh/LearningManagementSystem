using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class SyllabysModuleConfig : IEntityTypeConfiguration<SyllabusModule>
    {
        public void Configure(EntityTypeBuilder<SyllabusModule> builder)
        {
            //set PK
            builder.HasKey(k => new { k.SyllabusId, k.ModuleId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            //set relation
            builder.HasOne<Syllabus>(s => s.Syllabus)
                .WithMany(s => s.SyllabusModules)
                .HasForeignKey(fk => fk.SyllabusId);

            builder.HasOne<Module>(s => s.Module)
                .WithMany(s => s.SyllabusModules)
                .HasForeignKey(fk => fk.ModuleId);

        }
    }
}
