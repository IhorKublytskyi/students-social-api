using backend.API.RequestModels;
using backend.API.ResponseModels;
using backend.Core.Entities;
using backend.Infrastructure.Interfaces;
using DataAccess.Postgres.Repositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using backend.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.MapEndpoints();
app.UseCookiePolicy(new CookiePolicyOptions()
{
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/register", async (
    RegisterRequest registerData, 
    IPasswordHasher passwordHasher, 
    UsersRepository usersRepository) =>
{
    var user = await usersRepository.GetByEmail(registerData.Email);

    if (await usersRepository.ExistsByUsername(registerData.Username))
    {
        return Results.BadRequest("This username is already taken");
    }
    if (user != null)
    {
        return Results.BadRequest("This e-mail is already taken");
    }
    
    user = new UserEntity()
    {
        Id = Guid.NewGuid(),
        Email = registerData.Email,
        PasswordHash = passwordHasher.HashPassword(registerData.Password),
        Username = registerData.Username,
        FirstName = registerData.FirstName,
        LastName = registerData.LastName,
        BirthDate = DateTime.Parse(registerData.BirthDate),
        CreatedAt = DateTime.UtcNow
    };
    await usersRepository.Add(user);
    
    return Results.Created("/api/register", "You have successfully registered");
});

app.MapPost("/api/login", async (
    [FromBody] LoginRequest loginData, 
    UsersRepository usersRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    HttpContext context,
    RefreshTokensRepository refreshTokensRepository) =>
{
    var user = await usersRepository.GetByEmail(loginData.Email);
    if (user == null)
    {
        return Results.BadRequest("The user with this email address does not exist");
    }

    if (!passwordHasher.VerifyHashedPassword(loginData.Password, user.PasswordHash))
    {
        return Results.BadRequest("Invalid email or password");
    }

    var accessToken = tokenProvider.Generate(user);
    var refreshToken = new RefreshTokenEntity()
    {
        Id = Guid.NewGuid(),
        Token = tokenProvider.GenerateRefreshToken(),
        ExpireIn = DateTime.UtcNow.AddHours(builder.Configuration.GetValue<int>("JwtOptions:RefreshTokenValidityHours")),
        UserId = user.Id
    };
    await refreshTokensRepository.Add(refreshToken);
    
    context.Response.Cookies.Append("accessToken", accessToken);
    context.Response.Cookies.Append("refreshToken", refreshToken.Token);

    return Results.Ok(refreshToken);
});

app.MapGet("/api/logout", async (HttpContext context) =>
{
    context.Response.Cookies.Delete("accessToken");
    context.Response.Cookies.Delete("refreshToken");
}).RequireAuthorization();

app.MapPost("/api/refresh-tokens", async (
    [FromBody] RefreshTokensRequest refreshTokensRequest, 
    RefreshTokensRepository refreshTokensRepository,
    ITokenProvider tokenProvider,
    HttpContext context) =>
{
    var refreshToken = await refreshTokensRepository.Get(refreshTokensRequest.RefreshToken);
    
    if (refreshToken == null || refreshToken.ExpireIn < DateTime.UtcNow)
        return Results.Problem(detail: "The refresh token has expired", statusCode:401);

    var user = refreshToken.User;
    await refreshTokensRepository.Delete(refreshToken.Id);

    refreshToken = new RefreshTokenEntity()
    {
        Id = Guid.NewGuid(),
        ExpireIn = DateTime.UtcNow.AddHours(builder.Configuration.GetValue<int>("JwtOptions:RefreshTokenValidityHours")),
        Token = tokenProvider.GenerateRefreshToken(),
        UserId = user.Id
    };
    await refreshTokensRepository.Add(refreshToken);

    var accessToken = tokenProvider.Generate(user);

    context.Response.Cookies.Append("accessToken", accessToken);
    context.Response.Cookies.Append("refreshToken", refreshToken.Token);

    return Results.Ok();
});

//Me GET
app.MapGet("/api/me", async (
    HttpContext context, 
    UsersRepository usersRepository,
    ITokenReader tokenReader) =>
{
    string? id = tokenReader.ReadToken(context.Request.Cookies["accessToken"], "Id");

    if (id == null)
        return Results.BadRequest("Invalid token data");

    var user = await usersRepository.GetById(Guid.Parse(id));

    var userInfo = new UserInfoResponse()
    {
        Email = user.Email,
        Username = user.Username,
        FirstName = user.FirstName,
        LastName = user.LastName,
        ProfilePicture = user.ProfilePicture,
        Status = user.Status,
        BirthDate = user.BirthDate,
        Biography = user.Biography,
        CreatedAt = user.CreatedAt,
        IsOnline = user.IsOnline
    };
    
    return Results.Ok(userInfo);
}).RequireAuthorization();

app.Run();
