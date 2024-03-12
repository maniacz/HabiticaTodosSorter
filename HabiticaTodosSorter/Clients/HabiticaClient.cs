using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using FluentResults;
using HabiticaTodosSorter.Models.Errors;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Todos;
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
    
    public async Task<Result<GetAllTodosResponse?>> GetAllTodos()
    {
        var url = "/api/v3/tasks/user?type=todos";
        
        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(url);
            
            LogRemainingRequestsCountAndTimeLeftToEndPeriod(response);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<GetAllTodosResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }
            
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                _logger.LogWarning("Habitica API returned - 429 Too Many Requests");
                await WaitForNextPeriod(response);
                return Result.Ok();
            }

            return Result.Fail(new NoDataError("Unable to get all todos."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while trying to get all todos.");
            return Result.Fail(new Error("Unexpected error occurred while trying to get all todos."));
        }
    }

    public async Task<Result<GetTodoResponse?>> GetTodo(string taskId)
    {
        var url = $"/api/v3/tasks/{taskId}";

        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<GetTodoResponse>(responseContent);

                return Result.Ok(parsedResponse);
            }

            return Result.Fail(new NoDataError());
        }
        catch (Exception)
        {
            return Result.Fail(new NoDataError());
        }
    }

    public async Task<Result<GetAllTagsResponse?>> GetAllTags()
    {
        var url = "/api/v3/tags";
        
        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.GetAsync(url);

            LogRemainingRequestsCountAndTimeLeftToEndPeriod(response);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<GetAllTagsResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }
            
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                _logger.LogWarning("Habitica API returned - 429 Too Many Requests");
                await WaitForNextPeriod(response);
                return Result.Ok();
            }

            return Result.Fail(new NoDataError("Unable to get all tags."));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while trying to get all tags.");
            return Result.Fail(new Error("Unexpected error occurred while trying to get all tags."));
        }
    }

    public async Task<Result<MoveTodoToNewPositionResponse?>> MoveTodoToNewPosition(Todo todo, int todoFinalPosition)
    {
        var taskId = todo.TaskId.ToString();
        var url = $"/api/v3/tasks/{taskId}/move/to/{todoFinalPosition}";

        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.PostAsync(url, null);
            
            LogRemainingRequestsCountAndTimeLeftToEndPeriod(response);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<MoveTodoToNewPositionResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Not find todo {taskName} with id {taskId}", todo.TaskName, taskId);
                return Result.Fail(new NoDataError("Todo to move not found."));
            }

            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                _logger.LogWarning("Habitica API returned - 429 Too Many Requests");
                await WaitForNextPeriod(response);
                return Result.Ok();
            }

            _logger.LogError("Failed to move todo {todoName} to final position {todoPos}. Due to {reason}", todo.TaskName, todoFinalPosition, response.ReasonPhrase);
            return Result.Fail("Failed to move todo to a new position.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while moving todo {taskName} to position {pos}.", todo.TaskName, todoFinalPosition);
            return Result.Fail(new Error("Unexpected error occurred while while moving todo."));
        }
    }

    public async Task<Result<AddTagToTaskResponse?>> AssignTag(string todoId, string tagId)
    {
        var url = $"api/v3/tasks/{todoId}/tags/{tagId}";

        try
        {
            HttpClient httpClient = CreateHttpClient();
            var response = await httpClient.PostAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<AddTagToTaskResponse>(responseContent);

                return Result.Ok(parsedResponse);
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

                if (parsedResponse != null && parsedResponse.Message.Contains("To zadanie jest ju¿ oznaczone danym tagiem."))
                {
                    _logger.LogError("Todo with id {todoId} already has assigned tag with id {tagId}", todoId, tagId);
                    return Result.Fail(new Error("This tag has already been assigned to this todo."));
                }

                var errorsMessages = parsedResponse?.Errors.Select(e => e.Message).ToList();
                if (errorsMessages != null && errorsMessages.Contains("\"tagId\" powinien byæ prawid³owym UUID odpowiadaj¹cym tagowi nale¿¹cemu do u¿ytkownika."))
                {
                    _logger.LogError("User has no tag defined with tag id: {tagId} No tag has been assigned.", tagId);
                    return Result.Fail(new Error("No tag has been assigned. User doesn't have tag defined with such id."));
                }
            }

            _logger.LogError("Tag has not been assigned");
            return Result.Fail(new Error("Tag not assigned"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while trying to assign tag to todo.");
            return Result.Fail(new Error("Unexpected error occurred while trying to assign tag to todo."));
        }
    }

    private HttpClient CreateHttpClient()
    {
        var client = _httpClientFactory.CreateClient(nameof(HabiticaClient));
        var clientConfig = _configuration.GetSection("HabiticaClient").Get<HabiticaClientHeaders>();
        
        client.DefaultRequestHeaders.Add("x-client", clientConfig?.XClient);
        client.DefaultRequestHeaders.Add("x-api-user", clientConfig?.XApiUser);
        client.DefaultRequestHeaders.Add("x-api-key", clientConfig?.XApiKey);

        return client;
    }

    private async Task WaitForNextPeriod(HttpResponseMessage response)
    {
        var remainingSecondsResponse = response.Headers.GetValues("Retry-After").First();
        _logger.LogWarning("Required to wait for {seconds} seconds", remainingSecondsResponse);
        var millisecondsToWait = (int)(Single.Parse(remainingSecondsResponse, CultureInfo.InvariantCulture) * 1000);
        await Task.Delay(millisecondsToWait);
    }

    private void LogRemainingRequestsCountAndTimeLeftToEndPeriod(HttpResponseMessage response)
    {
        var remainingRequestCount = response.Headers.GetValues("X-RateLimit-Remaining").First();
        var periodEndHeader = response.Headers.GetValues("X-RateLimit-Reset").First();
        var periodEndUtcValue = periodEndHeader.Substring(0, periodEndHeader.IndexOf("GMT") - 1);
        var dateTimeFormat = "ddd MMM dd yyyy HH:mm:ss";
        var periodEndUtc = DateTime.ParseExact(periodEndUtcValue, dateTimeFormat, CultureInfo.InvariantCulture);
        var offsetToUtc = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
        var periodEnd = periodEndUtc + offsetToUtc;
        var secondsToEnd = (periodEnd - DateTime.Now).Seconds;
        _logger.LogInformation("There are {remainingRequestCount} requests allowed in current time period which ends at {periodEnd} which is within {secondsToEnd} seconds",
            remainingRequestCount, periodEnd.ToString("T"), secondsToEnd);
    }
}