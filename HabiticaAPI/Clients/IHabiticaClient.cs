using FluentResults;
using HabiticaAPI.Models.Responses;
using HabiticaAPI.Models.Todos;

namespace HabiticaAPI.Clients;

public interface IHabiticaClient
{
    Task<Result<GetAllTodosResponse>> GetAllTodos();
    Task<Result<GetAllTagsResponse>> GetAllTags();
    Task<Result<MoveTodoToNewPositionResponse>> MoveTodoToNewPosition(Todo todo, int todoFinalPosition);
}