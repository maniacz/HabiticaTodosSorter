using FluentResults;
using HabiticaTodosSorter.Clients;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.RequestHandlers.Tags;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.RequestHandlers;

public class TagsRequestHandlerTests
{
    private readonly Mock<IHabiticaClient> _habiticaClientMock;
    private readonly Mock<ILogger<TagsRequestHandler>> _loggerMock;
    private readonly TagsRequestHandler _tagsRequestHandler;

    public TagsRequestHandlerTests()
    {
        _habiticaClientMock = new Mock<IHabiticaClient>();
        _loggerMock = new Mock<ILogger<TagsRequestHandler>>();
        _tagsRequestHandler = new TagsRequestHandler(_habiticaClientMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllTags_ValidRequest_ReturnsOkWithData()
    {
        // Arrange
        var tagsResponse = new GetAllTagsResponse
        {
            Success = true,
            Data = new GetAllTagsResponse.TagData[]
            {
                new GetAllTagsResponse.TagData { Id = "958a73fb-d341-4513-83c2-c90c318193b5", Name = "dom" },
                new GetAllTagsResponse.TagData { Id = "f7ca5e48-7471-4bc3-ae65-baeba2fafa4c", Name = "praca" },
                new GetAllTagsResponse.TagData { Id = "7f74cad0-bd54-45ba-9562-9e590032e421", Name = "zakupy" },
                new GetAllTagsResponse.TagData { Id = "43e8cc3a-5e0b-48e0-9055-f37ba95b3a17", Name = "piwowarstwo" },
                new GetAllTagsResponse.TagData { Id = "2995fb09-6228-4388-9082-7fc657fd7a85", Name = "pilne ważne" },
                new GetAllTagsResponse.TagData { Id = "a62cb803-194d-4f1a-8bc2-a4ca8ecefc54", Name = "niepilne ważne" },
                new GetAllTagsResponse.TagData { Id = "d6443523-2d24-42bb-9136-88ae0d025ef0", Name = "niepilne nieważne" },
                new GetAllTagsResponse.TagData { Id = "0cfe6cb2-003c-4eaa-8d12-177aee36ec3f", Name = "pilne nieważne" }
            }
        };
        _habiticaClientMock.Setup(x => x.GetAllTags()).ReturnsAsync(Result.Ok(tagsResponse));

        // Act
        var result = await _tagsRequestHandler.GetAllTags();

        // Assert
        Assert.True(result.IsSuccess);
        var response = Assert.IsType<GetAllTagsResponse>(result.Value);
        Assert.Equal(tagsResponse, response);
    }

    [Fact]
    public async Task GetAllTags_RequestFailed_ReturnsBadRequest()
    {
        // Arrange
        _habiticaClientMock.Setup(x => x.GetAllTags()).ReturnsAsync(Result.Fail(""));

        // Act
        var result = await _tagsRequestHandler.GetAllTags();

        // Assert
        Assert.True(result.IsFailed);
    }
}