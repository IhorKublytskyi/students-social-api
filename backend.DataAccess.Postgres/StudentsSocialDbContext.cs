using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using backend.Core.Entities;
using DataAccess.Postgres.Configurations;

namespace DataAccess.Postgres;

public class StudentsSocialDbContext : DbContext
{
    private readonly IConfiguration _config;
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<PostEntity> Posts { get; set; }
    public DbSet<CommentEntity> Comments { get; set; }
    public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
    public DbSet<SubscriptionEntity> Subscriptions { get; set; }
    
    public StudentsSocialDbContext(IConfiguration config)
    {
        _config = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration<UserEntity>(new UserEntityConfiguration());
        modelBuilder.ApplyConfiguration<PostEntity>(new PostEntityConfiguration());
        modelBuilder.ApplyConfiguration<CommentEntity>(new CommentEntityConfiguration());
        modelBuilder.ApplyConfiguration<ImageEntity>(new ImageEntityConfiguration());
        modelBuilder.ApplyConfiguration<VideoEntity>(new VideoEntityConfiguration());
        modelBuilder.ApplyConfiguration<SubscriptionEntity>(new SubscriptionEntityConfiguration());
        modelBuilder.ApplyConfiguration<ReplyEntity>(new ReplyEntityConfiguration());
        modelBuilder.ApplyConfiguration<PostTagEntity>(new PostTagEntityConfiguration());
        modelBuilder.ApplyConfiguration<LikeCommentEntity>(new LikeCommentEntityConfiguration());
        modelBuilder.ApplyConfiguration<LikePostEntity>(new LikePostEntityConfiguration());
        modelBuilder.ApplyConfiguration<FavouritePostEntity>(new FavouritePostEntityConfiguration());
        modelBuilder.ApplyConfiguration<CloseFriendEntity>(new CloseFriendEntityConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_config.GetConnectionString("StudentsSocial"));
    }
}