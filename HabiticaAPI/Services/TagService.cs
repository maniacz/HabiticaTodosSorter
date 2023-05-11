using AutoMapper;
using FluentResults;
using HabiticaAPI.Clients;
using HabiticaAPI.Constants;
using HabiticaAPI.Models.Tags;
using Microsoft.AspNetCore.Mvc;

namespace HabiticaAPI.Services;

public class TagService : ITagService
{
    private readonly IHabiticaClient _habiticaClient;
    private readonly IMapper _mapper;
    private readonly ILogger<TagService> _logger;

    public TagService(IHabiticaClient habiticaClient, IMapper mapper, ILogger<TagService> logger)
    {
        _habiticaClient = habiticaClient;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IEnumerable<Tag>>> GetAllTags()
    {
        var tagsResult = await _habiticaClient.GetAllTags();
        if (tagsResult.IsFailed)
        {
            return tagsResult.ToResult();
        }

        var tags = _mapper.Map<IEnumerable<Tag>>(tagsResult.Value.Data);
        return Result.Ok(tags);
    }

    public async Task<Result<IEnumerable<Tag>>> GetTagsForSorting()
    {
        var tagsResult = await _habiticaClient.GetAllTags();
        if (tagsResult.IsFailed)
        {
            return tagsResult.ToResult();
        }

        var tagsData = _mapper.Map<ICollection<Tag>>(tagsResult.Value.Data);

        // var tagNamesUsedForSorting = new string[]
        //     { "pilne ważne", "pilne nieważne", "niepilne ważne", "niepilne nieważne" };
        var sortingTags = tagsData.Where(t => AppConstants.SortingTagNames.Contains(t.Name));
        return Result.Ok(sortingTags);
    }

    public SortedList<int, Tag> SetTagSortingOrder(IEnumerable<Tag> tagsForSorting)
    {
        var sortedTags = new SortedList<int, Tag>();
        for (int i = 0; i < 4; i++)
        {
            var predefinedOrderedTagName = AppConstants.SortingTagNames[i];
            var matchingTag = tagsForSorting.FirstOrDefault(t => t.Name.ToLower().Equals(predefinedOrderedTagName));
            sortedTags.Add(i, matchingTag);
        }

        return sortedTags;
    }
}