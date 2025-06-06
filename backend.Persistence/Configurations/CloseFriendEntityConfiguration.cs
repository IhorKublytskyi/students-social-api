using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class CloseFriendEntityConfiguration : IEntityTypeConfiguration<CloseFriendEntity>
{
    public void Configure(EntityTypeBuilder<CloseFriendEntity> builder)
    {
        builder.ToTable("CloseFriends");
        
        builder.HasKey(cf => new
        {
            cf.OwnerUserId,
            cf.CloseFriendUserId
        });

        builder.Property(cfu => cfu.FollowedAt).HasColumnType("timestamp").HasColumnName("FollowedAt");
        
        builder
            .HasOne(ou => ou.OwnerUser)
            .WithMany(u => u.ClosedFriends)
            .HasForeignKey(ou =>  ou.OwnerUserId)
            .IsRequired();
    }
}