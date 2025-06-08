using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PostEntityConfiguration : IEntityTypeConfiguration<PostEntity>
{
    public void Configure(EntityTypeBuilder<PostEntity> builder)
    {
        builder.ToTable("Posts");

        builder.HasKey(c => c.Id);

        builder
            .HasOne(p => p.User)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.UserId);

        builder.Property(p => p.CreatedAt).HasColumnType("timestamp").HasColumnName("CreatedAt");
        builder.Property(p => p.Title).HasMaxLength(50).HasColumnName("Title");
        builder.Property(p => p.Description).HasMaxLength(50).HasColumnName("Description");
        builder.Property(p => p.IsPrivate).HasColumnType("boolean").HasColumnName("IsPrivate");

        builder
            .HasMany(p => p.Images)
            .WithOne(i => i.Post)
            .HasForeignKey(i => i.PostId)
            .IsRequired();

        builder
            .HasMany(p => p.Videos)
            .WithOne(v => v.Post)
            .HasForeignKey(v => v.PostId)
            .IsRequired();

        builder
            .HasMany(p => p.Comments)
            .WithOne(c => c.Post)
            .HasForeignKey(c => c.PostId)
            .IsRequired();
    }
}