using FluentResults;
using HabiticaTodosSorter.Models;
using HabiticaTodosSorter.Models.Responses;

namespace HabiticaTodosSorter.RequestHandlers.Tags;

public interface ITagsRequestHandler
{
    Task<Result<GetAllTagsResponse>> GetAllTags();
    Task<Result<GetAllTagsResponse>> GetTagsForGivenTagIds(ICollection<string> tagIds);
    Task<Result<AddTagToTaskResponse>> AssignTagToTodo(string todoId, string tagId);
    Task<Result<bool>> DeleteTagFromTodo(string todoId, string tagId);
}