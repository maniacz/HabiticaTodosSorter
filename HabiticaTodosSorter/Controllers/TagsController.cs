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

    [HttpPost("addtag")]
    public async Task<IActionResult> AddTagToTodo([FromQuery] string todoId, [FromQuery] string tagId)
    {
        var assignResult = await _tagsRequestHandler.AssignTagToTodo(todoId, tagId);
        if (assignResult.IsSuccess)
        {
            return Ok(assignResult.Value);
        }
        else
        {
            return BadRequest(assignResult?.Errors.FirstOrDefault()?.Message);
        }
    }

    [HttpDelete("removetagfromtodo/{taskId}/{tagId}")]
    public async Task<IActionResult> DeleteTagFromTodo([FromRoute] string taskId, [FromRoute] string tagId)
    {
        var result = await _tagsRequestHandler.DeleteTagFromTodo(taskId, tagId);
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }
        else
        {
            return BadRequest(result?.Errors.FirstOrDefault()?.Message);
        }
    }
}