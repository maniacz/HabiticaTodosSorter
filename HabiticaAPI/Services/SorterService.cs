using FluentResults;
using HabiticaAPI.Clients;
using HabiticaAPI.Models.Responses;
using HabiticaAPI.Models.Tags;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public class SorterService : ISorterService
{
    private readonly ILogger<SorterService> _logger;
    private readonly IHabiticaClient _habiticaClient;
    private int _swapCounter = 0;

    public SorterService(ILogger<SorterService> logger, IHabiticaClient habiticaClient)
    {
        _logger = logger;
        _habiticaClient = habiticaClient;
    }
    public ICollection<Todo> GetTodosInFinalOrder(ICollection<Todo> todos, SortedList<int, Tag> tagsOrder)
    {
        var todosList = todos.ToList();
        todosList.Sort(new TodoComparer(tagsOrder, _logger));

        return todosList;
    }

    public async Task<Result> SortTodos(ICollection<Todo> todosToSort, ICollection<Todo> todosInFinalOrder)
    {
        var todosBeginOrder = todosToSort.ToList();
        var todosFinalOrder = todosInFinalOrder.ToList();
        var sortResult = await CyclicSort(todosBeginOrder, todosFinalOrder);
        
        if (sortResult.IsFailed)
            return Result.Fail(new Error(string.Join(", ", sortResult.Value.Notifications)));

        return Result.Ok();
    }

    private async Task<Result<MoveTodoToNewPositionResponse>> CyclicSort(List<Todo> todosToSort, List<Todo> todosInFinalOrder)
    {
        int i = 0;
        while (i < todosToSort.Count)
        {
            var actualIndex = i;
            var todo = todosToSort.ElementAt(actualIndex);
            var desiredIndex = todosInFinalOrder.IndexOf(todo);

            if (actualIndex != desiredIndex)
            {
                var swapResult = await SwapTodos(todosToSort, desiredIndex, actualIndex);
                if (swapResult.IsFailed)
                    return swapResult;
            }
            else
            {
                i++;
            }
        }
        
        _logger.LogInformation("Final swap counter value: {swapCounter} Total count of operations: {operationCount}", _swapCounter, _swapCounter * 2);

        return Result.Ok();
    }

    private async Task<Result<MoveTodoToNewPositionResponse>> SwapTodos(List<Todo> todosToSort, int firstTodoFinalPosition, int secondTodoFinalPosition)
    {
        var firstTodoToSwap = todosToSort.ElementAt(secondTodoFinalPosition);
        var secondTodoToSwap = todosToSort.ElementAt(firstTodoFinalPosition);
        
        todosToSort.RemoveAt(firstTodoFinalPosition);
        todosToSort.Insert(firstTodoFinalPosition, firstTodoToSwap);
        todosToSort.RemoveAt(secondTodoFinalPosition);
        todosToSort.Insert(secondTodoFinalPosition, secondTodoToSwap);

        // We will send 2 requests. The limit allows 30 requests every 60 seconds, so we need to wait for 2 sec.
        await Task.Delay(2000);

        var result = await _habiticaClient.MoveTodoToNewPosition(firstTodoToSwap, firstTodoFinalPosition);
        if (result.IsFailed)
            return result;
        
        result = await _habiticaClient.MoveTodoToNewPosition(secondTodoToSwap, secondTodoFinalPosition);
        if (result.IsFailed)
            return result;

        _swapCounter++;
        
        _logger.LogDebug("Swapped todo: {first} from position: {firstTodoPos}\n and todo: {second} from position: {sedondTodoPos}", 
            firstTodoToSwap.TaskName, secondTodoFinalPosition, secondTodoToSwap.TaskName, firstTodoFinalPosition);

        return Result.Ok();
    }
}