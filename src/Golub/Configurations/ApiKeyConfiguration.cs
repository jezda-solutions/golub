using Golub.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golub.Configurations
{
    internal class ApiKeyConfiguration : IEntityTypeConfiguration<ApiKey>
    {
        public void Configure(EntityTypeBuilder<ApiKey> builder)
        {
            builder.ToTable("api_key");

            builder.HasKey(e => e.Id);

            builder.Property(a => a.ApiKeyValue)
                .IsRequired();

            builder.Property(a => a.ExpirationDate)
                .IsRequired(false);

            builder.Property(a => a.ApplicationName)
                .IsRequired(false);

            builder.Property(a => a.CreatedOnUtc)
                .IsRequired();

            builder.Property(a => a.ModifiedOnUtc)
                .IsRequired(false);
        }
    }
}