using FluentResults;
using HabiticaAPI.Models;

namespace HabiticaAPI.Clients;

public interface IHabiticaClient
{
    Task<Result<GetAllTodosResponse>> GetAllTodos();
    Task<Result<GetAllTagsResponse>> GetAllTags();
}