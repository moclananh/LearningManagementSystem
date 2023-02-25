using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class AuditResultConfig : IEntityTypeConfiguration<AuditResult>
    {
        public void Configure(EntityTypeBuilder<AuditResult> builder)
        {
            builder.HasOne<AuditPlan>(s => s.AuditPlan)
                .WithMany(s => s.AuditResults)
                .HasForeignKey(fk => fk.AuditPlanId);
        }
    }
}
