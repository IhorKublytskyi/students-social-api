using backend.Application.Interfaces;
using backend.Core.Interfaces.Repositories;
using backend.Core.Models.FilterModels;
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
            [FromQuery] int? year,
            [FromQuery] string? firstName,
            [FromQuery] string? lastName,
            IUserService service) =>
        {
            if (username != null)
            {
                var user = await service.GetUser(username);

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
            var subscriberUsername = tokenReader.ReadToken(accessToken, "Username");

            var result = await subscriptionService.UnsubscribeAsync(subscriberUsername, subscribedUsername);

            return result.IsSuccess ? Results.Ok() : Results.BadRequest(result.Error);
        }).RequireAuthorization();
    }
}