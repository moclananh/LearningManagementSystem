using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class UserAuditPlanConfig : IEntityTypeConfiguration<UserAuditPlan>
    {
        public void Configure(EntityTypeBuilder<UserAuditPlan> builder)
        {
            // Set PK
            builder.HasKey(k => new { k.UserId, k.AuditPlanId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            // Set Relation
            builder.HasOne<User>(u => u.User)
                .WithMany(cu => cu.UserAuditPlans)
                .HasForeignKey(fk => fk.UserId);

            builder.HasOne<AuditPlan>(u => u.AuditPlan)
                .WithMany(cu => cu.UserAuditPlans)
                .HasForeignKey(fk => fk.AuditPlanId);

        }
    }
}
