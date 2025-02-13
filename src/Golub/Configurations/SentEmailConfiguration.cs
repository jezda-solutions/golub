using Golub.Contexts;
using Golub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golub.Configurations
{
    internal class SentEmailConfiguration : IEntityTypeConfiguration<sent_email>
    {
        public void Configure(EntityTypeBuilder<sent_email> builder)
        {
            builder.ToTable("sent_email");

            builder.HasKey(e => e.id);

            builder.Property(e => e.from)
                .IsRequired();

            builder.Property(e => e.to)
                .IsRequired();

            builder.Property(e => e.subject)
                .IsRequired();

            builder.Property(e => e.is_successful)
                .IsRequired();

            builder.Property(e => e.remark)
                .IsRequired();

            builder.Property(e => e.created_on_utc)
                .IsRequired();

            builder.Property(e => e.modified_on_utc)
                .IsRequired(false);

            builder.HasOne(e => e.email_provider)
                .WithMany()
                .HasForeignKey(e => e.email_provider_id);
        }
    }
}