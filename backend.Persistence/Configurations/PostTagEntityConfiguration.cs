using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class PostTagEntityConfiguration : IEntityTypeConfiguration<PostTagEntity>
{
    public void Configure(EntityTypeBuilder<PostTagEntity> builder)
    {
        builder.ToTable("PostsTags");

        builder.HasKey(pt => new
        {
            pt.UserId,
            pt.PostId
        });

        builder.Property(pt => pt.TaggedAt).HasColumnType("timestamp").HasColumnName("TaggedAt");

        builder
            .HasOne(pt => pt.User)
            .WithMany(u => u.Tags)
            .HasForeignKey(pt => pt.UserId)
            .IsRequired();
        
        builder
            .HasOne(pt => pt.Post)
            .WithMany(p => p.Tags)
            .HasForeignKey(pt => pt.PostId)
            .IsRequired();
    }
}