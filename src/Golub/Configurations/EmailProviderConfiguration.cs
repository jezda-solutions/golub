using Golub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golub.Configurations
{
    internal class EmailProviderConfiguration : IEntityTypeConfiguration<email_provider>
    {
        public void Configure(EntityTypeBuilder<email_provider> builder)
        {
            builder.ToTable("email_provider");

            builder.HasKey(e => e.id);

            builder.Property(e => e.name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.name)
                .IsUnique(true);

            builder.Property(e => e.is_active)
                .IsRequired();

            builder.Property(e => e.configuration)
                .HasMaxLength(1000);

            builder.Property(e => e.last_used_on)
                .IsRequired();

            builder.Property(e => e.free_plan_qty)
                .IsRequired(false);

            builder.Property(e => e.remaining_qty)
                .IsRequired(false);

            builder.Property(e => e.period)
                .IsRequired(false);

            builder.Property(e => e.created_on_utc)
                .IsRequired();

            builder.Property(e => e.modified_on_utc)
                .IsRequired(false);
        }
    }
}