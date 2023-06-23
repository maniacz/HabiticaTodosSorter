using System.Net;
using HabiticaAPI.Clients;
using HabiticaAPI.Controllers;
using HabiticaAPI.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using PitneyBowes.NexShipService.UnitTests.Mocks;
using Xunit;

namespace Tests.Clients;

public class HabiticaClientTests
{
    private readonly Mock<ILogger<HabiticaClient>> _logerMock;
    private readonly IConfigurationRoot _configuration;

    public HabiticaClientTests()
    {
        _logerMock = new Mock<ILogger<HabiticaClient>>();
        _configuration = new ConfigurationBuilder().AddUserSecrets<TagsController>().Build();
    }

    [Fact]
    public async Task GetAllTags_NoErrors_ReturnsOkWithTags()
    {
        // Arrange
        var tagsResponse = new GetAllTagsResponse.TagData[]
        {
            new GetAllTagsResponse.TagData { Id = "958a73fb-d341-4513-83c2-c90c318193b5", Name = "dom" },
            new GetAllTagsResponse.TagData { Id = "f7ca5e48-7471-4bc3-ae65-baeba2fafa4c", Name = "praca" },
            new GetAllTagsResponse.TagData { Id = "7f74cad0-bd54-45ba-9562-9e590032e421", Name = "zakupy" },
            new GetAllTagsResponse.TagData { Id = "43e8cc3a-5e0b-48e0-9055-f37ba95b3a17", Name = "piwowarstwo" },
            new GetAllTagsResponse.TagData { Id = "2995fb09-6228-4388-9082-7fc657fd7a85", Name = "pilne ważne" },
            new GetAllTagsResponse.TagData { Id = "a62cb803-194d-4f1a-8bc2-a4ca8ecefc54", Name = "niepilne ważne" },
            new GetAllTagsResponse.TagData { Id = "d6443523-2d24-42bb-9136-88ae0d025ef0", Name = "niepilne nieważne" },
            new GetAllTagsResponse.TagData { Id = "0cfe6cb2-003c-4eaa-8d12-177aee36ec3f", Name = "pilne nieważne" }
        };
        
        var httpClientFactoryMock = HttpClientFactoryMockProvider.GetIHttpClientFactoryMock(
            new RouteResponse
            {
                Method = HttpMethod.Get,
                Route = $"/api/v3/tasks/user?type=todos",
                Response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(tagsResponse))
                }
            });

        var habiticaClient = new HabiticaClient(httpClientFactoryMock.Object, _configuration, _logerMock.Object);

        // Act
        var result = await habiticaClient.GetAllTags();

        // Assert
        Assert.True(result.IsSuccess);
        // todo: Assert result content, so far I don't know why it returns empty content
    }
}