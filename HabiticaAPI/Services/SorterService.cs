using FluentResults;
using HabiticaAPI.Models.Tags;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Services;

public class SorterService : ISorterService
{
    private readonly ILogger<SorterService> _logger;
    private int _swapCounter = 0;

    public SorterService(ILogger<SorterService> logger)
    {
        _logger = logger;
    }
    public ICollection<Todo> GetTodosInFinalOrder(ICollection<Todo> todos, SortedList<int, Tag> tagsOrder)
    {
        var todosList = todos.ToList();
        todosList.Sort(new TodoComparer(tagsOrder, _logger));

        return todosList;
    }

    public Result SortTodos(ICollection<Todo> todosToSort, ICollection<Todo> todosInFinalOrder)
    {
        var todosBeginOrder = todosToSort.ToList();
        var todosFinalOrder = todosInFinalOrder.ToList();
        CyclicSort(todosBeginOrder, todosFinalOrder);

        return null;
    }

    private void CyclicSort(List<Todo> todosToSort, List<Todo> todosInFinalOrder)
    {
        int i = 0;
        while (i < todosToSort.Count)
        {
            var actualIndex = i;
            var todo = todosToSort.ElementAt(actualIndex);
            var desiredIndex = todosInFinalOrder.IndexOf(todo);
            if (actualIndex != desiredIndex)
            {
                SwapTodos(todosToSort, actualIndex, desiredIndex);
            }
            else
            {
                i++;
            }
        }
        
        _logger.LogInformation("Final swap counter value: {swapCounter} Total count of operations: {operationCount}", _swapCounter, _swapCounter * 2);
    }

    private void SwapTodos(List<Todo> todosToSort, int actualIndex, int desiredIndex)
    {
        var firstTodoToSwap = todosToSort.ElementAt(actualIndex);
        var secondTodoToSwap = todosToSort.ElementAt(desiredIndex);
        
        todosToSort.RemoveAt(desiredIndex);
        todosToSort.Insert(desiredIndex, firstTodoToSwap);
        todosToSort.RemoveAt(actualIndex);
        todosToSort.Insert(actualIndex, secondTodoToSwap);
        
        _swapCounter++;
        
        _logger.LogDebug("Swapped todo: {first} from position: {firstTodoPos}\n and todo: {second} from position: {sedondTodoPos}", 
            firstTodoToSwap.TaskName, actualIndex, secondTodoToSwap.TaskName, desiredIndex);
    }
}