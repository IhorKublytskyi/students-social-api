using StudentsSocial.Application.Interfaces;
using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Interfaces.Repositories;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application
{
    public class PostService : IPostService
    {
        private readonly IPostsRepository _repository;

        public PostService(IPostsRepository repository)
        {
            _repository = repository;
        }
        public async Task<Result> Create(CreatePostRequest request)
        {
            var post = new PostEntity() 
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                Title = request.Title,
                Description = request.Description,
                IsPrivate = request.IsPrivate
            };

            await _repository.AddAsync(post);

            return Result.Success();
        }

        public async Task<Result<PostsResponse>> Get(Guid userId)
        {
            var posts = await _repository.GetByUserIdAsync((Guid?)userId);
            if (posts.Count == 0)
                return Result<PostsResponse>.Failure("The user has no posts yet");

            var response = new PostsResponse()
            {
                Content = posts,
                TotalCount = posts.Count
            };

            return Result<PostsResponse>.Success(response);
        }

        public async Task<Result<PostCommentsResponse>> GetComments(Guid id)
        {
            var post = await _repository.GetByIdAsync(id);
            if (post == null)
                return Result<PostCommentsResponse>.Failure("Post not found");

            var response = new PostCommentsResponse()
            {
                PostId = id,
                Content = post.Comments,
                TotalCount = post.Comments.Count
            };

            return Result<PostCommentsResponse>.Success(response);
        }
    }
}
