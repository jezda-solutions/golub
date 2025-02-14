using Golub.Contexts;
using Golub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golub.Configurations
{
    internal class SentEmailConfiguration : IEntityTypeConfiguration<SentEmail>
    {
        public void Configure(EntityTypeBuilder<SentEmail> builder)
        {
            builder.ToTable("sent_email");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.From)
                .IsRequired();

            builder.Property(e => e.To)
                .IsRequired();

            builder.Property(e => e.Subject)
                .IsRequired();

            builder.Property(e => e.IsSuccessful)
                .IsRequired();

            builder.Property(e => e.Remark)
                .IsRequired();

            builder.Property(e => e.CreatedOnUtc)
                .IsRequired();

            builder.Property(e => e.ModifiedOnUtc)
                .IsRequired(false);

            builder.HasOne(e => e.EmailProvider)
                .WithMany()
                .HasForeignKey(e => e.EmailProviderId);
        }
    }
}