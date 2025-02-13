using Golub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golub.Configurations
{
    internal class ApiKeyConfiguration : IEntityTypeConfiguration<api_key>
    {
        public void Configure(EntityTypeBuilder<api_key> builder)
        {
            builder.ToTable("api_key");

            builder.HasKey(e => e.id);

            builder.Property(a => a.api_key_value)
                .IsRequired();

            builder.Property(a => a.expiration_date)
                .IsRequired(false);

            builder.Property(a => a.application_name)
                .IsRequired(false);

            builder.Property(a => a.created_on_utc)
                .IsRequired();

            builder.Property(a => a.modified_on_utc)
                .IsRequired(false);
        }
    }
}