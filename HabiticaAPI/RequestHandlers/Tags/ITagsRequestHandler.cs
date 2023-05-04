using FluentResults;
using HabiticaAPI.Models;

namespace HabiticaAPI.RequestHandlers.Tags;

public interface ITagsRequestHandler
{
    Task<Result<GetAllTagsResponse>> GetAllTags();
}