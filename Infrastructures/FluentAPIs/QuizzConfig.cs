using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class QuizzConfig : IEntityTypeConfiguration<Quizz>
    {
        public void Configure(EntityTypeBuilder<Quizz> builder)
        {
            builder.HasOne<Unit>(s => s.Unit)
                .WithMany(s => s.Quizzs)
                .HasForeignKey(fk => fk.UnitId);

        }
    }
}
