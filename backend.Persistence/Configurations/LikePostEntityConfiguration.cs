using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LikePostEntityConfiguration : IEntityTypeConfiguration<LikePostEntity>
{
    public void Configure(EntityTypeBuilder<LikePostEntity> builder)
    {
        builder.ToTable("LikesPosts");

        builder.HasKey(lp => new
        {
            lp.UserId,
            lp.PostId
        });

        builder.Property(lp => lp.LikedAt).HasColumnType("timestamp").HasColumnName("LikedAt");

        builder
            .HasOne(lp => lp.User)
            .WithMany(u => u.Likes)
            .HasForeignKey(lp => lp.UserId)
            .IsRequired();
        
        builder
            .HasOne(lp => lp.Post)
            .WithMany(p => p.Likes)
            .HasForeignKey(lp => lp.PostId)
            .IsRequired();
    }
}