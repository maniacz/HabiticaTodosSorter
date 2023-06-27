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
    public async Task<IActionResult> GetAllTags()
    {
        var tagsResult = await _tagsRequestHandler.GetAllTags();

        if (tagsResult.IsSuccess)
        {
            return Ok(tagsResult);
        }
        else
        {
            return BadRequest();
        }
    }
}