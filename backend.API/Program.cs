using backend.API.Config;
using backend.Application.Interfaces;
using backend.Application.Models.RequestModels;
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
    [FromBody]RegistrationRequest request,
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
    ITokenReader reader) =>
{
    var accessToken = context.Request.Cookies["accessToken"]!;
    var id = Guid.Parse(reader.ReadToken(accessToken, "Id"));

    var response = await userService.GetUser(id);

    return response.IsSuccess? Results.Ok(response.Value) : Results.BadRequest(response.Error);
}).RequireAuthorization();

app.Run();