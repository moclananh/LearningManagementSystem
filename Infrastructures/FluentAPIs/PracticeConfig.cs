using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class PracticeConfig : IEntityTypeConfiguration<Practice>
    {
        public void Configure(EntityTypeBuilder<Practice> builder)
        {
            builder.HasOne<Unit>(s => s.Unit)
                 .WithMany(s => s.Practices)
                 .HasForeignKey(fk => fk.UnitId);

        }
    }
}
