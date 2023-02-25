using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class UserAuditResultConfig : IEntityTypeConfiguration<UserAuditResult>
    {
        public void Configure(EntityTypeBuilder<UserAuditResult> builder)
        {
            // Set PK
            builder.HasKey(k => new { k.UserId, k.AuditResultId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            // Set Relation
            builder.HasOne<User>(u => u.User)
                .WithMany(cu => cu.UserAuditResults)
                .HasForeignKey(fk => fk.UserId);

            builder.HasOne<AuditResult>(u => u.AuditResult)
                .WithMany(cu => cu.UserAuditResults)
                .HasForeignKey(fk => fk.UserId);
        }
    }
}
