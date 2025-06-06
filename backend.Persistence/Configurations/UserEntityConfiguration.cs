using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).HasMaxLength(50).HasColumnName("Email");
        builder.Property(u => u.PasswordHash).HasMaxLength(255).HasColumnName("Password");
        builder.Property(u => u.Username).HasMaxLength(20).HasColumnName("Username");
        builder.Property(u => u.FirstName).HasMaxLength(20).HasColumnName("Firstname");
        builder.Property(u => u.LastName).HasMaxLength(20).HasColumnName("Lastname");
        builder.Property(u => u.ProfilePicture).HasMaxLength(50).HasColumnName("ProfilePicture");
        builder.Property(u => u.Status).HasMaxLength(50).HasColumnName("Status");
        builder.Property(u => u.BirthDate).HasColumnType("timestamp").HasColumnName("BirthDate");
        builder.Property(u => u.Biography).HasMaxLength(255).HasColumnName("Biography");
        builder.Property(u => u.IsOnline).HasColumnType("boolean").HasColumnName("IsOnline");

        builder
            .HasMany(u => u.Posts)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .IsRequired();

        builder
            .HasMany(u => u.Comments)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .IsRequired();
    }
}