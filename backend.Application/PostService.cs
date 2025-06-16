using backend.Application.Interfaces;
using backend.Application.Models.RequestModels;
using backend.Application.Models.ResponseModels;
using backend.Core.Entities;
using backend.Core.Interfaces.Repositories;
using backend.Core.Results;
using System.Threading.Tasks;

namespace backend.Application
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

            await _repository.Add(post);

            return Result.Success();
        }

        public async Task<Result<PostsResponse>> Get(Guid userId)
        {
            var posts = await _repository.GetByUserId(userId);
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
            var post = await _repository.GetById(id);
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
