using FluentResults;
using HabiticaTodosSorter.Models.Tags;

namespace HabiticaTodosSorter.Services;

public interface ITagService
{
    Task<Result<IEnumerable<Tag>>> GetAllTags();
    Task<Result<IEnumerable<Tag>>>  GetTagsForSorting();
    SortedList<int, Tag> SetTagSortingOrder(IEnumerable<Tag> tagsForSorting);
}