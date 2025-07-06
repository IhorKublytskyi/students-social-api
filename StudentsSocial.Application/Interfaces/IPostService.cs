using StudentsSocial.Application.Models.RequestModels;
using StudentsSocial.Application.Models.ResponseModels;
using StudentsSocial.Core.Entities;
using StudentsSocial.Core.Results;

namespace StudentsSocial.Application.Interfaces;

public interface IPostService
{
    Task<Result<PostsResponse>> Get(Guid userId);
    Task<Result<PostCommentsResponse>> GetComments(Guid id);
    Task<Result> Create(CreatePostRequest request);
}