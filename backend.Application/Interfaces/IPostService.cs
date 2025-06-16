using backend.Application.Models.RequestModels;
using backend.Application.Models.ResponseModels;
using backend.Core.Entities;
using backend.Core.Results;

namespace backend.Application.Interfaces;

public interface IPostService
{
    Task<Result<PostsResponse>> Get(Guid userId);
    Task<Result<PostCommentsResponse>> GetComments(Guid id);
    Task<Result> Create(CreatePostRequest request);
}