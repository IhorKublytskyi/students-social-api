using StudentsSocial.API.Config;
using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.RequestModels;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentsSocial.Application;
using StudentsSocial.Application.Validators;
using StudentsSocial.Core.Interfaces.Repositories;
using StudentsSocial.Infrastructure;
using StudentsSocial.Persistence.Repositories;
using StudentsSocial.Persistence;
using StudentsSocial.Core.Models.FilterModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextPool<StudentsSocialDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("StudentsSocial"));
});
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<ITokenReader, TokenReader>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IRegistrationService, RegistrationService>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<JwtOptions>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPostsRepository, PostsRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
builder.Services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
builder.Services.AddScoped<UpdateUserRequestValidator>();
builder.Services.AddScoped<LoginRequestValidator>();
builder.Services.AddScoped<CreatePostValidator>();
builder.Services.AddScoped<RegistrationRequestValidator>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserUpdateMerger, UserUpdateMerger>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();
var userGroup = app.MapGroup("/api/users").WithTags("Users");
var postGroup = app.MapGroup("/api/posts").WithTags("Posts");

app.UseCookiePolicy(new CookiePolicyOptions
    {
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict,
    Secure = CookieSecurePolicy.Always
});
app.UseAuthentication();
app.UseAuthorization();

//Users GET
userGroup.MapGet("/", async (
    [FromQuery] Guid? id,
    [FromQuery] int? year,
    [FromQuery] string? firstName,
    [FromQuery] string? lastName,
    IUserService service) =>
{
    if (id != null)
    {
        var user = await service.GetUser((Guid)id);

        return user.IsSuccess ? Results.Ok(user.Value) : Results.BadRequest(user.Error);
    }

    var filter = new UserFilter();
    if (year != null)
        filter.Year = year;
    if (firstName != null)
        filter.FirstName = firstName;
    if (lastName != null)
        filter.LastName = lastName;

    var result = await service.Get(filter);

    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(result.Error);

}).RequireAuthorization();

//Users PUT
userGroup.MapPut("/", async (
    [FromBody] UpdateUserRequest request,
    HttpContext context,
    IUserService service,
    UpdateUserRequestValidator validator,
    ITokenReader reader) =>
{
    var accessToken = context.Request.Cookies["accessToken"]!;
    
    var tokenReaderResult = reader.ReadToken(accessToken, "Id");
    if (!tokenReaderResult.IsSuccess)
        return Results.BadRequest("Invalid access token payload");
    
    var id = Guid.Parse(tokenReaderResult.Value);

    var result = await service.Update(id, request);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
});


//Users subscribe POST
userGroup.MapPost("/subscribe", async (
    [FromQuery] string subscribedUsername,
    [FromQuery] bool toCheck,
    ITokenReader tokenReader,
    HttpContext context,
    ISubscriptionService subscriptionService
) =>
{
    var accessToken = context.Request.Cookies["accessToken"];
    var tokenReaderResult = tokenReader.ReadToken(accessToken, "Username");
    if (!tokenReaderResult.IsSuccess)
        return Results.BadRequest("Invalid access token payload");
    var subscriberUsername = tokenReaderResult.Value;

    if (toCheck)
    {
        var isSubscribed =
            await subscriptionService.CheckSubscriptionAsync(subscriberUsername, subscribedUsername);

        return isSubscribed.IsSuccess ? Results.Ok(isSubscribed.Value) : Results.BadRequest(isSubscribed.Error);
    }

    var result = await subscriptionService.SubscribeAsync(subscriberUsername, subscribedUsername);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
}).RequireAuthorization();

userGroup.MapDelete("/unsubscribe", async (
    [FromQuery] string subscribedUsername,
    ITokenReader tokenReader,
    HttpContext context,
    ISubscriptionService subscriptionService
) =>
{
    var accessToken = context.Request.Cookies["accessToken"];
    var tokenReaderResult = tokenReader.ReadToken(accessToken, "Username");
    if (!tokenReaderResult.IsSuccess)
        return Results.BadRequest("Invalid access token payload");
    var subscriberUsername = tokenReaderResult.Value;

    var result = await subscriptionService.UnsubscribeAsync(subscriberUsername, subscribedUsername);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
}).RequireAuthorization();

//Post`s comments GET
postGroup.MapGet("/comments", async (
    [FromQuery] Guid postId,
    IPostService service) =>
{
    var response = await service.GetComments(postId);

    return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
}).RequireAuthorization();


//Posts GET
postGroup.MapGet("/", async (
    [FromQuery] Guid userId,
    IPostService service) =>
{
    var response = await service.Get(userId);

    return response.IsSuccess ? Results.Ok(response.Value) : Results.BadRequest(response.Error);
}).RequireAuthorization();

//Posts POST
postGroup.MapPost("/", async (
    HttpContext context,
    ITokenReader reader,
    IPostService service,
    CreatePostRequest request,
    CreatePostValidator validator) =>
{
    var accessToken = context.Request.Cookies["accessToken"]!;
    var readTokenResult = reader.ReadToken(accessToken, "Id");
    if (!readTokenResult.IsSuccess)
        return Results.BadRequest("Invalid access token payload");

    var id = Guid.Parse(readTokenResult.Value);
    request.UserId = id;

    var validationResult = validator.Validate(request);
    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.ToString());

    var response = await service.Create(request);

    return response.IsSuccess ? Results.Ok() : Results.BadRequest(response.Error);
}).RequireAuthorization();

app.MapPost("/api/register", async (
    [FromBody]RegistrationRequest request,
    IRegistrationService service,
    RegistrationRequestValidator validator) =>
{
    var validationResult = validator.Validate(request);
    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.ToString());

    var result = await service.RegisterAsync(request);

    return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
});

app.MapPost("/api/login", async (
    [FromBody] LoginRequest request,
    LoginRequestValidator validator,
    ILoginService service,
    HttpContext context) =>
{
    var validationResult = validator.Validate(request);
    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.ToString());

    var result = await service.LoginAsync(request.Email!, request.Password!);

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
    
    var tokenReaderResult = reader.ReadToken(accessToken, "Id");
    if (!tokenReaderResult.IsSuccess)
        return Results.BadRequest("Invalid access token payload");
    
    var id = Guid.Parse(tokenReaderResult.Value);

    var response = await userService.GetUser(id);

    return response.IsSuccess? Results.Ok(response.Value) : Results.BadRequest(response.Error);
}).RequireAuthorization();

app.Run();