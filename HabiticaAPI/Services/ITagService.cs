using FluentResults;
using HabiticaAPI.Models.Tags;

namespace HabiticaAPI.Services;

public interface ITagService
{
    Task<Result<IEnumerable<Tag>>> GetAllTags();
    Task<Result<IEnumerable<Tag>>>  GetTagsForSorting();
    SortedList<int, Tag> SetTagSortingOrder(IEnumerable<Tag> tagsForSorting);
}