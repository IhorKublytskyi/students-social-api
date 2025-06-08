using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class ReplyEntityConfiguration : IEntityTypeConfiguration<ReplyEntity>
{
    public void Configure(EntityTypeBuilder<ReplyEntity> builder)
    {
        builder.ToTable("Replies");

        builder.HasKey(r => new
        {
            r.ReplyId,
            r.RepliedId
        });

        builder.Property(r => r.RepliedAt).HasColumnType("timestamp").HasColumnName("RepliedAt");

        builder
            .HasOne(r => r.Reply)
            .WithMany(c => c.Replies)
            .HasForeignKey(r => r.ReplyId)
            .IsRequired();
    }
}