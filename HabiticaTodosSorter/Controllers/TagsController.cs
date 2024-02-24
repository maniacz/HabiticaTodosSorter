using FluentResults;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.RequestHandlers.Tags;
using Microsoft.AspNetCore.Mvc;

namespace HabiticaTodosSorter.Controllers;

[ApiController]
public class TagsController : Controller
{
    private readonly ITagsRequestHandler _tagsRequestHandler;
    private readonly ILogger<TagsController> _logger;

    public TagsController(ITagsRequestHandler tagsRequestHandler, ILogger<TagsController> logger)
    {
        _tagsRequestHandler = tagsRequestHandler;
        _logger = logger;
    }

    [HttpGet("tags")]
    public async Task<IActionResult> GetTags([FromQuery] ICollection<string> tagIds)
    {
        Result<GetAllTagsResponse> tagsResult;
        if (tagIds.Count == 0)
        {
            tagsResult = await _tagsRequestHandler.GetAllTags();
        }
        else
        {
            tagsResult = await _tagsRequestHandler.GetTagsForGivenTagIds(tagIds);
        }

        if (tagsResult.IsSuccess)
        {
            return Ok(tagsResult.Value);
        }
        else
        {
            return BadRequest(tagsResult.Errors?.FirstOrDefault()?.Message);
        }
    }
}