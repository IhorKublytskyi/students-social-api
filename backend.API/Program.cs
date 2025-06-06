using backend.API.RequestModels;
using backend.API.ResponseModels;
using backend.Core.Entities;
using backend.Core.Interfaces;
using Persistence.Repositories;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using backend.API.Extensions;
using backend.Core.Interfaces.Repositories;

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
    RegisterRequest request, 
    IRegistrationService service) =>
{
    var result = await service.Register(request.FirstName, request.LastName, request.Username, request.Email, DateTime.Parse(request.BirthDate), 
        request.Password);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
});

app.MapPost("/api/login", async (
    [FromBody] LoginRequest loginData, 
    IUsersRepository usersRepository,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    HttpContext context,
    IRefreshTokensRepository refreshTokensRepository) =>
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

    return Results.Ok(refreshToken.Token);
});

app.MapGet("/api/logout", async (HttpContext context) =>
{
    context.Response.Cookies.Delete("accessToken");
    context.Response.Cookies.Delete("refreshToken");
}).RequireAuthorization();

app.MapPost("/api/refresh-tokens", async (
    IRefreshTokensRepository refreshTokensRepository,
    ITokenProvider tokenProvider,
    HttpContext context) =>
{
    var refreshToken = await refreshTokensRepository.Get(context.Request.Cookies["refreshToken"]);
    
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
    IUsersRepository usersRepository,
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
