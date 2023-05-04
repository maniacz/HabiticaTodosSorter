using System.Text.Json.Serialization;
using FluentResults;
using HabiticaAPI.Models;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HabiticaAPI.Clients;

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
        HttpClient httpClient = CreateHttpClient();
        
        try
        {
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonConvert.DeserializeObject<GetAllTodosResponse>(responseContent);
                
                return Result.Ok(parsedResponse);
            }
            else
            {
                return Result.Fail(new Error("Unable to get all todos."));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unexpected error occurred while trying to get all todos.");
            return Result.Fail(new Error("Unexpected error occurred while trying to get all todos."));
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