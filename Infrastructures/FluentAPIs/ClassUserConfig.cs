using Domain.Entities;
using Domain.EntityRelationship;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class ClassUserConfig : IEntityTypeConfiguration<ClassUser>
    {
        public void Configure(EntityTypeBuilder<ClassUser> builder)
        {
            // Set PK
            builder.HasKey(k => new { k.UserId, k.ClassId });
            // Ignore Id in BaseEntity
            builder.Ignore(i => i.Id);
            // Set Relation
            builder.HasOne<User>(u => u.User)
                .WithMany(cu => cu.ClassUsers)
                .HasForeignKey(fk => fk.UserId);

            builder.HasOne<Class>(c => c.Class)
                .WithMany(cu => cu.ClassUsers)
                .HasForeignKey(fk => fk.ClassId);

        }
    }
}
