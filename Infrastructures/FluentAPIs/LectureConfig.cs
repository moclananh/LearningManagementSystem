using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class LectureConfig : IEntityTypeConfiguration<Lecture>
    {
        public void Configure(EntityTypeBuilder<Lecture> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Unit>(s => s.Unit)
                .WithMany(s => s.Lectures)
                .HasForeignKey(fk => fk.UnitId);

        }
    }
}
