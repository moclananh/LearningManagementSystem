using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class AuditQuestionConfig : IEntityTypeConfiguration<AuditQuestion>
    {
        public void Configure(EntityTypeBuilder<AuditQuestion> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<AuditPlan>(x => x.AuditPlan)
                .WithMany(x => x.AuditQuestions)
                .HasForeignKey(x => x.AuditPlanId);
        }
    }
}
