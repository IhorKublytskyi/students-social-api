using backend.Application.Interfaces;
using backend.Application.Models.RequestModels;
using Microsoft.AspNetCore.Mvc;

namespace backend.API.Endpoints;

public static class PostEndpoints
{
    public static void MapPostEndpoints(this WebApplication app)
    {
        var postGroup = app.MapGroup("/api/posts").WithTags("Posts");

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
            CreatePostRequest request) =>
        {
            var accessToken = context.Request.Cookies["accessToken"]!;
            var id = Guid.Parse(reader.ReadToken(accessToken, "Id"));

            request.UserId = id;

            var response = await service.Create(request);

            return response.IsSuccess ? Results.Ok() : Results.BadRequest(response.Error);
        }).RequireAuthorization();
    }
}