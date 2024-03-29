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
            // todo: To si� da przerobi� jako� �eby zwraca� zamiast Fail - NotFound
            return Result.Fail("There are no tags for given id(s)");
        }

        var result = new GetAllTagsResponse
        {
            Success = true,
            Data = requestedTags
        };

        // todo: komentarz linijka ni�ej
        //return result; <- to te� dzia�a, wtf?
        return Result.Ok(result);
    }
}