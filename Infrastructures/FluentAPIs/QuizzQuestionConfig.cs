using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class QuizzQuestionConfig : IEntityTypeConfiguration<QuizzQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizzQuestion> builder)
        {
            builder.HasOne<Quizz>(s => s.Quizz)
                 .WithMany(s => s.QuizzQuestions)
                 .HasForeignKey(fk => fk.QuizzId);

        }
    }
}
