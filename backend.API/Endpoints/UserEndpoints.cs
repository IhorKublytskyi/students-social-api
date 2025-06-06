using backend.Core.Interfaces;
using backend.Core.Interfaces.Repositories;
using Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var userGroup = app.MapGroup("/api/users").WithTags("Users");
        
        //Users GET
        userGroup.MapGet("/", async (
            [FromQuery] string? username, 
            IUsersRepository usersRepository) =>
        {
            if (username != null)
            {
                var user = await usersRepository.GetByUsername(username);
                if (user == null)
                    return Results.BadRequest("User does not exist");
                return Results.Ok(user);
            }

            var users = await usersRepository.Get();
            if (users == null || users.Count == 0)
                return Results.BadRequest("There are no users in the database");
            
            return Results.Ok(users);
            
        }).RequireAuthorization();
        
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
            var subscriberUsername = tokenReader.ReadToken(accessToken, "Username");

            if (toCheck)
            {
                var isSubscribed = await subscriptionService.CheckSubscriptionAsync(subscriberUsername, subscribedUsername);

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
            var subscriberUsername = tokenReader.ReadToken(accessToken, "Username");

            var result = await subscriptionService.UnsubscribeAsync(subscriberUsername, subscribedUsername);

            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).RequireAuthorization();
    }
}