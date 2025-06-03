using System.Text;
using backend.API.RequestModels;
using backend.API.ResponseModels;
using backend.Core.Entities;
using backend.Infrastructure;
using backend.Infrastructure.Interfaces;
using DataAccess.Postgres;
using DataAccess.Postgres.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TokenReader = backend.Infrastructure.TokenReader;

var builder = WebApplication.CreateBuilder(args);
var cfg = builder.Configuration;
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["JwtOptions:SecretKey"]))
        };

        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["accessToken"];
                
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();

//БД
builder.Services.AddDbContext<StudentsSocialDbContext>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<ITokenReader, TokenReader>();
builder.Services.AddScoped<JwtOptions>();
builder.Services.AddScoped<UsersRepository>();
builder.Services.AddScoped<PostsRepository>();
builder.Services.AddScoped<CommentsRepository>();
builder.Services.AddScoped<RefreshTokensRepository>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

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
        ExpireIn = DateTime.UtcNow.AddSeconds(cfg.GetValue<int>("JwtOptions:RefreshTokenValidityHours")),
        UserId = user.Id
    };
    await refreshTokensRepository.Add(refreshToken);
    
    context.Response.Cookies.Append("accessToken", accessToken);
    context.Response.Cookies.Append("refreshToken", refreshToken.Token);

    return Results.Ok();
});

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
        ExpireIn = DateTime.UtcNow.AddHours(cfg.GetValue<int>("JwtOptions:RefreshTokenValidityHours")),
        Token = tokenProvider.GenerateRefreshToken(),
        UserId = user.Id
    };
    await refreshTokensRepository.Add(refreshToken);

    var accessToken = tokenProvider.Generate(user);

    context.Response.Cookies.Append("accessToken", accessToken);
    context.Response.Cookies.Append("refreshToken", refreshToken.Token);

    return Results.Ok();
});

//Users GET
app.MapGet("/api/users", async (
    [FromQuery] string? email, 
    UsersRepository usersRepository) =>
{
    if (email != null)
        return Results.Ok(await usersRepository.GetByEmail(email));
    
    return Results.Ok(await usersRepository.Get());
}).RequireAuthorization();

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
    
    if(user == null)
        return Results.BadRequest("User with this id does not exist");

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
//Comments GET
app.MapGet("/api/posts/comments", async (
    [FromQuery] Guid postId,
    CommentsRepository commentsRepository) =>
{
    var comments = await commentsRepository.GetByPostId(postId);

    return Results.Ok(new CommentsResponse()
    {
        Content = comments,
        TotalCount = comments.Count
    });
}).RequireAuthorization();

//Posts GET
app.MapGet("/api/posts", async (
    [FromQuery] Guid? userId, 
    PostsRepository postsRepository) =>
{
    if (userId != null)
    {
        var posts = await postsRepository.GetByUserId(userId);

        return Results.Ok(new PostsResponse()
        {
            Content = posts,
            TotalCount = posts.Count
        });
    }
    else
    {
        var posts = await postsRepository.Get();

        return Results.Ok(new PostsResponse()
        {
            Content = posts,
            TotalCount = posts.Count
        });
    }
    
    
}).RequireAuthorization();

//Posts POST
app.MapPost("/api/posts", async (
    UsersRepository usersRepository, 
    CreatePostRequest createPostData, 
    PostsRepository postsRepository) =>
{
    var user = await usersRepository.GetByEmail(createPostData.UserEmail);

    if (user != null)
    {
        var post = new PostEntity()
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = DateTime.Now,
            Title = createPostData.Title,
            Description = createPostData.Description,
            IsPrivate = createPostData.IsPrivate
        };
        await postsRepository.Add(post);
        
        return Results.Ok("Post was successfully added");
    }

    return Results.BadRequest("User with this email was not found");
}).RequireAuthorization();


app.Run();
