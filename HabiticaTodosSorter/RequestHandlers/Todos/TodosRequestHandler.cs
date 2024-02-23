using AutoMapper;
using FluentResults;
using HabiticaTodosSorter.Extensions;
using HabiticaTodosSorter.Models;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Clients;
using HabiticaTodosSorter.Models.Todos;
using HabiticaTodosSorter.Services;
using Microsoft.AspNetCore.Routing.Matching;
using HabiticaTodosSorter.Models.Requests;

namespace HabiticaTodosSorter.RequestHandlers.Todos;

public class TodosRequestHandler : ITodosRequestHandler
{
    private readonly IHabiticaClient _habiticaClient;
    private readonly ITodoService _todoService;
    private readonly ITagService _tagService;
    private readonly ISorterService _sorterService;
    private readonly IMapper _mapper;
    private readonly ILogger<TodosRequestHandler> _logger;

    public TodosRequestHandler(IHabiticaClient habiticaClient, ITodoService todoService, ITagService tagService, ISorterService sorterService, IMapper mapper, ILogger<TodosRequestHandler> logger)
    {
        _habiticaClient = habiticaClient;
        _todoService = todoService;
        _tagService = tagService;
        _sorterService = sorterService;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<Result<IList<Todo>>> GetAllTodos()
    {
        var result = await _habiticaClient.GetAllTodos();

        if (result.IsFailed)
        {
            return result.ToResult();
        }
        
        var allTagsResult = await _tagService.GetAllTags();
        if (allTagsResult.IsFailed)
        {
            // todo: zaloguj
            return result.ToResult();
        }
        
        var allTodos = _mapper.Map<List<Todo>>(result.Value.Data).SetTaskPosition().AssignTagNames(allTagsResult.Value);
        _logger.LogInformation("Initial todos order: {todosList}", allTodos.ListTodosNames());
        return Result.Ok(allTodos);
    }

    public async Task<Result<Todo>> GetTodo(GetTodoRequest request)
    {
        string taskId;

        try
        {
            taskId = await GetTaskIdForTodo(request);
        }
        catch (InvalidOperationException ex) when (ex.Message == "Sequence contains more than one matching element")
        {
            _logger.LogError(ex, "There are more todos containing searched name: {searchedName}", request.Name);
            return Result.Fail("There are more todos containing searched name");
        }
        catch (InvalidOperationException ex) when (ex.Message == "Sequence contains no matching element")
        {
            _logger.LogError(ex, "There is no todo containing searched name: {searchedName}", request.Name);
            return Result.Fail("There is no todo containing searched name");
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, "No search criteria provided");
            return Result.Fail("No search criteria provided");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            return Result.Fail("Unexpected error");
        }

        var result = await _habiticaClient.GetTodo(taskId);

        var todo = _mapper.Map<Todo>(result.Value.Data);

        return todo;
    }

    private async Task<string> GetTaskIdForTodo(GetTodoRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.Id))
            return request.Id;

        if (!string.IsNullOrEmpty(request.Name))
        {
            var allTodos = await _habiticaClient.GetAllTodos();
            var taskId = allTodos?.Value?.Data.Single(task => task.Text.ToLower().Contains(request.Name.ToLower())).Id;
            return taskId;
        }

        throw new ArgumentNullException(nameof(request));
    }

    public async Task<Result> SortTodos(ICollection<Todo> todosToSort)
    {
        var sortingTagsResult = await _tagService.GetTagsForSorting();
        if (sortingTagsResult.IsFailed)
        {
            return sortingTagsResult.ToResult();
        }

        var tagsOrdered = _tagService.SetTagSortingOrder(sortingTagsResult.Value);
        var sortedTodos = _sorterService.GetTodosInFinalOrder(todosToSort, tagsOrdered);

        var sortingResult = await _sorterService.SortTodos(todosToSort, sortedTodos);
        return sortingResult;
    }
}