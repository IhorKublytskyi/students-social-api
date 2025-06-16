using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class VideoEntityConfiguration : IEntityTypeConfiguration<VideoEntity>
{
    public void Configure(EntityTypeBuilder<VideoEntity> builder)
    {
        builder.ToTable("Videos");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Data).HasColumnType("bytea").HasColumnName("Data");

        builder
            .HasOne(v => v.Post)
            .WithMany(p => p.Videos)
            .HasForeignKey(v => v.PostId)
            .IsRequired();
    }
}