using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class PracticeQuestionConfig : IEntityTypeConfiguration<PracticeQuestion>
    {
        public void Configure(EntityTypeBuilder<PracticeQuestion> builder)
        {
            builder.HasOne<Practice>(s => s.Practice)
                 .WithMany(s => s.PracticeQuestions)
                 .HasForeignKey(fk => fk.PracticeId);

        }
    }
}
