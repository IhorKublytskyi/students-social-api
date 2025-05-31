using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Postgres.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Token).HasMaxLength(200).HasColumnName("Token");
        builder.Property(r => r.ExpireIn).HasColumnType("timestamp").HasColumnName("ExpireIn");

        builder
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.Id);
    }
}