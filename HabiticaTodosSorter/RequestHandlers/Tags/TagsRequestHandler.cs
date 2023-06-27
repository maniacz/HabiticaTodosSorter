using FluentResults;
using HabiticaTodosSorter.Models;
using HabiticaTodosSorter.Clients;
using HabiticaTodosSorter.Models.Responses;

namespace HabiticaTodosSorter.RequestHandlers.Tags;

public class TagsRequestHandler : ITagsRequestHandler
{
    private readonly IHabiticaClient _habiticaClient;
    private readonly ILogger<TagsRequestHandler> _logger;

    public TagsRequestHandler(IHabiticaClient habiticaClient, ILogger<TagsRequestHandler> logger)
    {
        _habiticaClient = habiticaClient;
        _logger = logger;
    }
    
    public async Task<Result<GetAllTagsResponse>> GetAllTags()
    {
        var result = await _habiticaClient.GetAllTags();

        if (result.IsFailed)
        {
            return result.ToResult();
        }

        return Result.Ok(result.Value);
    }
}