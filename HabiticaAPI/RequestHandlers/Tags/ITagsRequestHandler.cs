using FluentResults;
using HabiticaAPI.Models;
using HabiticaAPI.Models.Responses;

namespace HabiticaAPI.RequestHandlers.Tags;

public interface ITagsRequestHandler
{
    Task<Result<GetAllTagsResponse>> GetAllTags();
}