using backend.Infrastructure;
using backend.Infrastructure.Interfaces;
using DataAccess.Postgres;
using DataAccess.Postgres.Repositories;

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
        services.AddScoped<JwtOptions>();
        services.AddScoped<UsersRepository>();
        services.AddScoped<PostsRepository>();
        services.AddScoped<CommentsRepository>();
        services.AddScoped<RefreshTokensRepository>();

        return services;
    }
}