using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inscribe.EntityFrameworkCore
{
    public class EntryConfiguration<TEntry> : IEntityTypeConfiguration<TEntry> where TEntry : Entry
    {
        public void Configure(EntityTypeBuilder<TEntry> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).HasMaxLength(255);
            builder.Property(e => e.Level).HasConversion<string>().HasMaxLength(32);
        }
    }
}
