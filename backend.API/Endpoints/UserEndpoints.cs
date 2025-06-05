using backend.Core.Entities;
using backend.Core.Interfaces;
using backend.Infrastructure.Exceptions;
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
            ITokenReader tokenReader,
            HttpContext context,
            ISubscriptionService subscriptionService
            ) =>
        {
            var accessToken = context.Request.Cookies["accessToken"];
            var subscriberUsername = tokenReader.ReadToken(accessToken, "Username");

            try
            {
                string message = await subscriptionService.SubscribeAsync(subscriberUsername, subscribedUsername);

                return Results.Ok(message);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }

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

            try
            {
                string message = await subscriptionService.UnsubscribeAsync(subscriberUsername, subscribedUsername);

                return Results.Ok(message);
            }
            catch (Exception e)
            {
                return Results.BadRequest(e.Message);
            }

        }).RequireAuthorization();
    }
}