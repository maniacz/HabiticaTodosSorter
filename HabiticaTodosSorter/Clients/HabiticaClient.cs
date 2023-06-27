using System.Net;
using FluentResults;
using HabiticaTodosSorter.Models.Errors;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Todos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HabiticaTodosSorter.Clients;

public class HabiticaClient : IHabiticaClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HabiticaClient> _logger;

    public HabiticaClient(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<HabiticaClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }
    
    public async Task<Result<GetAllTodosResponse>> GetAllTodos()
    {
        var url = "/api/v3/tasks/user?type=todos";
        
        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<GetAllTodosResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }
            else
            {
                return Result.Fail(new NoDataError("Unable to get all todos."));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while trying to get all todos.");
            return Result.Fail(new Error("Unexpected error occurred while trying to get all todos."));
        }
    }

    public async Task<Result<GetAllTagsResponse>> GetAllTags()
    {
        var url = "/api/v3/tags";
        
        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<GetAllTagsResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }
            else
            {
                return Result.Fail(new NoDataError("Unable to get all tags."));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while trying to get all tags.");
            return Result.Fail(new Error("Unexpected error occurred while trying to get all tags."));
        }
    }

    public async Task<Result<MoveTodoToNewPositionResponse>> MoveTodoToNewPosition(Todo todo, int todoFinalPosition)
    {
        var taskId = todo.TaskId.ToString();
        var url = $"/api/v3/tasks/{taskId}/move/to/{todoFinalPosition}";

        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<MoveTodoToNewPositionResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }
            else
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError("Not find todo: {taskName} with id: {taskId}", todo.TaskName, taskId);
                    return Result.Fail(new NoDataError("Todo to move not found."));
                }
                else
                {
                    return Result.Fail("Failed to move todo to a new position.");
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while moving todo: {taskName} to position: {pos}.", todo.TaskName, todoFinalPosition);
            return Result.Fail(new Error("Unexpected error occurred while while moving todo."));
        }
    }

    private HttpClient CreateHttpClient()
    {
        var client = _httpClientFactory.CreateClient(nameof(HabiticaClient));
        var clientConfig = _configuration.GetSection("HabiticaClient").Get<HabiticaClientHeaders>();
        
        client.DefaultRequestHeaders.Add("x-client", clientConfig.XClient);
        client.DefaultRequestHeaders.Add("x-api-user", clientConfig.XApiUser);
        client.DefaultRequestHeaders.Add("x-api-key", clientConfig.XApiKey);

        return client;
    }
}