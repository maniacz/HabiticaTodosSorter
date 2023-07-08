using FluentResults;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Todos;

namespace HabiticaTodosSorter.Clients;

public interface IHabiticaClient
{
    Task<Result<GetAllTodosResponse?>> GetAllTodos();
    Task<Result<GetAllTagsResponse?>> GetAllTags();
    Task<Result<MoveTodoToNewPositionResponse?>> MoveTodoToNewPosition(Todo todo, int todoFinalPosition);
}