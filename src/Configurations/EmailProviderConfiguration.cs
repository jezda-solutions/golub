using Golub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golub.Configurations
{
    internal class EmailProviderConfiguration : IEntityTypeConfiguration<EmailProvider>
    {
        public void Configure(EntityTypeBuilder<EmailProvider> builder)
        {
            builder.ToTable("email_provider");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(x => x.Name)
                .IsUnique(true);

            builder.Property(e => e.IsActive)
                .IsRequired();

            builder.Property(e => e.Configuration)
                .HasMaxLength(1000);

            builder.Property(e => e.LastUsedOn)
                .IsRequired();

            builder.Property(e => e.FreePlanQty)
                .IsRequired(false);

            builder.Property(e => e.RemainingQty)
                .IsRequired(false);

            builder.Property(e => e.Period)
                .IsRequired(false);

            builder.Property(e => e.CreatedOnUtc)
                .IsRequired();

            builder.Property(e => e.ModifiedOnUtc)
                .IsRequired(false);
        }
    }
}