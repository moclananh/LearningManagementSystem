using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class AssignmentQuestionConfig : IEntityTypeConfiguration<AssignmentQuestion>
    {
        public void Configure(EntityTypeBuilder<AssignmentQuestion> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Assignment>(s => s.Assignment)
                 .WithMany(s => s.AssignmentQuestions)
                 .HasForeignKey(fk => fk.AssignmentId);

        }
    }
}
