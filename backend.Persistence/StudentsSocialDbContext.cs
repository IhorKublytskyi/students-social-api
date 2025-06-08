using backend.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence.Configurations;

namespace Persistence;

public class StudentsSocialDbContext : DbContext
{
    private readonly IConfiguration _config;
                                                
    public StudentsSocialDbContext(IConfiguration config)
    {
        _config = config;
    }
                                                
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ImageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new VideoEntityConfiguration());
        modelBuilder.ApplyConfiguration(new SubscriptionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ReplyEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PostTagEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LikeCommentEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LikePostEntityConfiguration());
        modelBuilder.ApplyConfiguration(new FavouritePostEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CloseFriendEntityConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_config.GetConnectionString("StudentsSocial"));
    }
}