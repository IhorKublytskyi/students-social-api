using backend.Application;
using backend.Core.Interfaces;
using backend.Core.Interfaces.Repositories;
using backend.Infrastructure;
using Persistence;
using Persistence.Repositories;

namespace backend.API.Extensions;

public static class Services
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        //БД
        services.AddDbContext<StudentsSocialDbContext>();

        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

        services.AddJwtAuthentication(configuration);
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<ITokenReader, TokenReader>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();
        services.AddScoped<IRegistrationService, RegistrationService>();
        services.AddScoped<JwtOptions>();
        services.AddScoped<IUsersRepository,UsersRepository>();
        services.AddScoped<IPostsRepository,PostsRepository>();
        services.AddScoped<ICommentsRepository,CommentsRepository>();
        services.AddScoped<IRefreshTokensRepository,RefreshTokensRepository>();
        services.AddScoped<ISubscriptionsRepository,SubscriptionsRepository>();

        return services;
    }
}