using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class ModuleConfig : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            builder.HasOne<AuditPlan>(s => s.AuditPlan)
                .WithOne(s => s.Module)
                .HasForeignKey<AuditPlan>(fk => fk.ModuleId);

        }
    }
}
