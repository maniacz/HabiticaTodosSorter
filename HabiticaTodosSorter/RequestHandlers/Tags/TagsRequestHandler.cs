using FluentResults;
using HabiticaTodosSorter.Models;
using HabiticaTodosSorter.Clients;
using HabiticaTodosSorter.Models.Responses;
using AutoMapper.Internal;
using Microsoft.AspNetCore.Mvc;

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

    public async Task<Result<GetAllTagsResponse>> GetTagsForGivenTagIds(ICollection<string> tagIds)
    {
        var allTagsResult = await _habiticaClient.GetAllTags();
        if (allTagsResult.IsFailed)
        {
            return allTagsResult.ToResult();
        }

        var requestedTags = allTagsResult?.Value?.Data.Where(t => tagIds.Contains(t.Id)).ToArray();
        if (requestedTags == null || requestedTags.Length == 0)
        {
            // todo: To siê da przerobiæ jakoœ ¿eby zwracaæ zamiast Fail - NotFound
            return Result.Fail("There are no tags for given id(s)");
        }

        var result = new GetAllTagsResponse
        {
            Success = true,
            Data = requestedTags
        };

        // todo: komentarz linijka ni¿ej
        //return result; <- to te¿ dzia³a, wtf?
        return Result.Ok(result);
    }

    public async Task<Result<AddTagToTaskResponse>> AssignTagToTodo(string todoId, string tagId)
    {
        return await _habiticaClient.AssignTag(todoId, tagId);
    }

    public async Task<Result<bool>> DeleteTagFromTodo(string todoId, string tagId)
    {
        return await _habiticaClient.DeleteTagFromTodo(todoId, tagId);
    }
}