using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructures.FluentAPIs
{
    public class OutputStandardConfig : IEntityTypeConfiguration<OutputStandard>
    {
        public void Configure(EntityTypeBuilder<OutputStandard> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
