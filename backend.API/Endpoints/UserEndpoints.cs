using backend.Core.Entities;
using backend.Infrastructure.Interfaces;
using DataAccess.Postgres;
using DataAccess.Postgres.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users").WithTags("Users");
        
        //Users GET
        userGroup.MapGet("/", async (
            [FromQuery] string? username, 
            UsersRepository usersRepository) =>
        {
            if (username != null)
                return Results.Ok(await usersRepository.GetByUsername(username));
    
            return Results.Ok(await usersRepository.Get());
        }).RequireAuthorization();
        
        //Users subscribe POST
        userGroup.MapPost("/subscribe", async (
            [FromQuery] string subscribedUsername,
            UsersRepository usersRepository,
            ITokenReader tokenReader,
            HttpContext context,
            StudentsSocialDbContext db
            ) =>
        {
            var accessToken = context.Request.Cookies["accessToken"];
            var subscriberUsername = tokenReader.ReadToken(accessToken, "Username");
            
            if (subscriberUsername == null)
                return Results.BadRequest("User with this username does not exist");

            var subscriber = await usersRepository.GetByUsername(subscriberUsername);
            if (subscriber == null)
                return Results.BadRequest("User with this username does not exist");
            var subscribed = await usersRepository.GetByUsername(subscribedUsername);
            if (subscribed == null)
                return Results.BadRequest("The user you are trying to subscribe to does not exist");

            var subscription = new SubscriptionEntity()
            {
                SubscriberId = subscriber.Id,
                SubscribedToId = subscribed.Id,
                SubscribedAt = DateTime.UtcNow
            };

            await db.Subscriptions.AddAsync(subscription);
            await db.SaveChangesAsync();

            return Results.Ok($"You have successfully subscribed to {subscribedUsername}");
        }).RequireAuthorization();

        userGroup.MapDelete("/unsubscribe", async (
            [FromQuery] string subscribedUsername,
            UsersRepository usersRepository,
            ITokenReader tokenReader,
            HttpContext context,
            StudentsSocialDbContext db
            ) =>
        {
            var accessToken = context.Request.Cookies["accessToken"];
            var subscriberUsername = tokenReader.ReadToken(accessToken, "Username");

            if (subscriberUsername == null)
                return Results.BadRequest("User with this username does not exist");

            var subscriber = await usersRepository.GetByUsername(subscriberUsername);

            if (subscriber == null)
                return Results.BadRequest("User with this username does not exist");

            var subscribed = await usersRepository.GetByUsername(subscribedUsername);
            if (subscribed == null)
                return Results.BadRequest("The user you are trying to unsubscribe to does not exist");

            int result = await db.Subscriptions
                .Where(s => s.SubscriberId == subscriber.Id && s.SubscribedToId == subscribed.Id)
                .ExecuteDeleteAsync();

            if (result < 1)
                return Results.BadRequest($"The user is not subscribed to {subscribedUsername}");
            
            return Results.Ok($"You have successfully stopped following {subscribedUsername}");

        }).RequireAuthorization();
    }
}