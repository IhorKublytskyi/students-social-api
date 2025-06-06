using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class SubscriptionEntityConfiguration : IEntityTypeConfiguration<SubscriptionEntity>
{
    public void Configure(EntityTypeBuilder<SubscriptionEntity> builder)
    {
        builder.ToTable("Subscriptions");

        builder.HasKey(s => new
        {
            s.SubscriberId,
            s.SubscribedToId
        });

        builder.Property(s => s.SubscribedAt).HasColumnType("timestamp").HasColumnName("SubscribedAt");

        builder
            .HasOne(s => s.Subscriber)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.SubscriberId)
            .IsRequired();
    }
}