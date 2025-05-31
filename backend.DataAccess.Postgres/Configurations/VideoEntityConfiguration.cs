using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Postgres.Configurations;

public class VideoEntityConfiguration : IEntityTypeConfiguration<VideoEntity>
{
    public void Configure(EntityTypeBuilder<VideoEntity> builder)
    {
        builder.ToTable("Videos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Url).HasMaxLength(50).HasColumnName("Url");

        builder
            .HasOne(v => v.Post)
            .WithMany(p => p.Videos)
            .HasForeignKey(v => v.PostId)
            .IsRequired();
    }
}