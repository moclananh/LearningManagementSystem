using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class AttendanceConfig : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<Class>(s => s.Class)
                .WithMany(s => s.Attendences)
                .HasForeignKey(fk => fk.ClassId);

            builder.HasOne<User>(s => s.User)
                .WithMany(s => s.Attendences)
                .HasForeignKey(fk => fk.UserId);

        }
    }
}
