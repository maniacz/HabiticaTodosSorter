using FluentResults;
using HabiticaAPI.Models;
using HabiticaAPI.Models.Responses;

namespace HabiticaAPI.Clients;

public interface IHabiticaClient
{
    Task<Result<GetAllTodosResponse>> GetAllTodos();
    Task<Result<GetAllTagsResponse>> GetAllTags();
}