using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class AuditPlanConfig : IEntityTypeConfiguration<AuditPlan>
    {
        public void Configure(EntityTypeBuilder<AuditPlan> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Class>(s => s.Class)
                .WithMany(s => s.AuditPlans)
                .HasForeignKey(fk => fk.ClassId);

        }
    }
}
