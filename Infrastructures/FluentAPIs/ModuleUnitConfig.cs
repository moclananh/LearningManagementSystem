using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class ModuleUnitConfig : IEntityTypeConfiguration<ModuleUnit>
    {
        public void Configure(EntityTypeBuilder<ModuleUnit> builder)
        {
            //set PK
            builder.HasKey(k => new { k.ModuleId, k.UnitId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            // set realtion
            builder.HasOne<Module>(m => m.Module)
                .WithMany(mu => mu.ModuleUnits)
                .HasForeignKey(pk => pk.ModuleId);

            builder.HasOne<Unit>(mu => mu.Unit)
                .WithMany(u => u.ModuleUnits)
                .HasForeignKey(fk => fk.UnitId);

        }
    }
}
