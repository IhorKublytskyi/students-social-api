using backend.API.Extensions;
using backend.Application.Interfaces;
using backend.Application.RequestModels;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

app.MapEndpoints();
app.UseCookiePolicy(new CookiePolicyOptions
{
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/api/register", async (
    RegistrationRequest request,
    IRegistrationService service,
    IRegistrationDataValidation validator) =>
{
    var validationResult = validator.Validate(request);
    if (!validationResult.IsSuccess)
        return Results.BadRequest(validationResult.Error);

    var result = await service.RegisterAsync(request);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
});

app.MapPost("/api/login", async (
    [FromBody] LoginRequest request,
    ILoginService service,
    HttpContext context) =>
{
    if (string.IsNullOrWhiteSpace(request.Email))
        return Results.BadRequest("Email is required");
    if (string.IsNullOrWhiteSpace(request.Password))
        return Results.BadRequest("Password is required");

    var result = await service.LoginAsync(request.Email, request.Password);

    if (!result.IsSuccess)
        return Results.BadRequest(result.Error);

    context.Response.Cookies.Append("accessToken", result.Value.AccessToken);
    context.Response.Cookies.Append("refreshToken", result.Value.RefreshToken);

    return Results.Ok();
});

app.MapGet("/api/logout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("accessToken");
    context.Response.Cookies.Delete("refreshToken");
}).RequireAuthorization();

app.MapPost("/api/refresh-tokens", async (
    IRefreshTokenService service,
    HttpContext context) =>
{
    var refreshTokenValue = context.Request.Cookies["refreshToken"];
    if (string.IsNullOrWhiteSpace(refreshTokenValue))
        return Results.BadRequest("Refresh token is required");

    var result = await service.RefreshAsync(refreshTokenValue);

    if (!result.IsSuccess)
        return Results.BadRequest(result.Error);

    context.Response.Cookies.Append("accessToken", result.Value.AccessToken);
    context.Response.Cookies.Append("refreshToken", result.Value.RefreshToken);

    return Results.Ok();
});

//Me GET
app.MapGet("/api/me", async (
    HttpContext context,
    IUserService userService,
    ITokenReader tokenReader) =>
{
    string id = tokenReader.ReadToken(context.Request.Cookies["accessToken"], "Id");
    if (string.IsNullOrWhiteSpace(id))
        return Results.BadRequest("Invalid token data");

    var user = await userService.GetUserInfo(Guid.Parse(id));

    return user == null ? Results.BadRequest(user.Error) : Results.Ok(user.Value);
}).RequireAuthorization();

app.Run();