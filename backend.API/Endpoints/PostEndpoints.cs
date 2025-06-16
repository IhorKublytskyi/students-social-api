using backend.Application.Models.RequestModels;
using backend.Application.Models.ResponseModels;
using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
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
            ICommentsRepository commentsRepository) =>
        {
            var comments = await commentsRepository.GetByPostId(postId);

            return Results.Ok(new CommentsResponse
            {
                Content = comments,
                TotalCount = comments.Count
            });
        }).RequireAuthorization();


        //Posts GET
        postGroup.MapGet("/", async (
            [FromQuery] Guid? userId,
            IPostsRepository postsRepository) =>
        {
            if (userId != null)
            {
                var posts = await postsRepository.GetByUserId(userId);

                return Results.Ok(new PostsResponse
                {
                    Content = posts,
                    TotalCount = posts.Count
                });
            }
            else
            {
                var posts = await postsRepository.Get();

                return Results.Ok(new PostsResponse
                {
                    Content = posts,
                    TotalCount = posts.Count
                });
            }
        }).RequireAuthorization();

        //Posts POST
        postGroup.MapPost("/", async (
            IUsersRepository usersRepository,
            CreatePostRequest createPostData,
            IPostsRepository postsRepository) =>
        {
            var user = await usersRepository.GetByEmail(createPostData.UserEmail);

            if (user != null)
            {
                var post = new PostEntity
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
    }
}