using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class FavouritePostEntityConfiguration : IEntityTypeConfiguration<FavouritePostEntity>
{
    public void Configure(EntityTypeBuilder<FavouritePostEntity> builder)
    {
        builder.ToTable("FavouritePosts");

        builder.HasKey(fp => new
        {
            fp.UserId,
            fp.PostId
        });

        builder.Property(fp => fp.FavouredAt).HasColumnType("timestamp").HasColumnName("FavouredAt");

        builder
            .HasOne(fp => fp.User)
            .WithMany(u => u.FavouritePosts)
            .HasForeignKey(fp => fp.UserId)
            .IsRequired();

        builder
            .HasOne(fp => fp.Post)
            .WithMany(p => p.FavouredBy)
            .HasForeignKey(fp => fp.PostId)
            .IsRequired();

    }
}