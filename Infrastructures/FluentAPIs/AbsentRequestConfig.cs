using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class AbsentRequestConfig : IEntityTypeConfiguration<AbsentRequest>
    {
        public void Configure(EntityTypeBuilder<AbsentRequest> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne<User>(s => s.User)
                 .WithMany(s => s.AbsentRequests)
                 .HasForeignKey(fk => fk.UserId);

            builder.HasOne<Class>(s => s.Class)
                 .WithMany(s => s.AbsentRequests)
                 .HasForeignKey(fk => fk.ClassId);

        }
    }
}
