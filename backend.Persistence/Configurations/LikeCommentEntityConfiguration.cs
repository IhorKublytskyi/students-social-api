using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class LikeCommentEntityConfiguration : IEntityTypeConfiguration<LikeCommentEntity>
{
    public void Configure(EntityTypeBuilder<LikeCommentEntity> builder)
    {
        builder.ToTable("LikesComments");

        builder.HasKey(lc => new
        {
            lc.UserId,
            lc.CommentId
        });

        builder.Property(lc => lc.LikedAt).HasColumnType("timestamp").HasColumnName("LikedAt");

        builder
            .HasOne(lc => lc.User)
            .WithMany(u => u.LikesComments)
            .HasForeignKey(lc => lc.UserId)
            .IsRequired();
        
        builder
            .HasOne(lc => lc.Comment)
            .WithMany(u => u.Likes)
            .HasForeignKey(lc => lc.CommentId)
            .IsRequired();
    }
}