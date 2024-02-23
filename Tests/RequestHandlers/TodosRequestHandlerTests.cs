using System.Collections;
using AutoMapper;
using FluentResults;
using HabiticaTodosSorter.Clients;
using HabiticaTodosSorter.Mappings;
using HabiticaTodosSorter.Models.Dto;
using HabiticaTodosSorter.Models.Responses;
using HabiticaTodosSorter.Models.Tags;
using HabiticaTodosSorter.Models.Todos;
using HabiticaTodosSorter.RequestHandlers.Todos;
using HabiticaTodosSorter.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using Xunit;

namespace Tests.RequestHandlers;

public class TodosRequestHandlerTests
{
    private readonly Mock<IHabiticaClient> _habiticaClientMock;
    private readonly Mock<ITodoService> _todoServiceMock;
    private readonly Mock<ITagService> _tagServiceMock;
    private readonly Mock<ISorterService> _sorterServiceMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<TodosRequestHandler>> _loggerMock;
    private readonly TodosRequestHandler _todosRequestHandler;

    public TodosRequestHandlerTests()
    {
        _habiticaClientMock = new Mock<IHabiticaClient>();
        _todoServiceMock = new Mock<ITodoService>();
        _tagServiceMock = new Mock<ITagService>();
        _sorterServiceMock = new Mock<ISorterService>();
        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<TodoProfile>());
        _mapper = mapperConfig.CreateMapper();
        _loggerMock = new Mock<ILogger<TodosRequestHandler>>();
        _todosRequestHandler = new TodosRequestHandler(
            _habiticaClientMock.Object,
            _todoServiceMock.Object,
            _tagServiceMock.Object,
            _sorterServiceMock.Object,
            _mapper,
            _loggerMock.Object);
    }

    [Fact]
    public async Task GetAllTodos_ValidRequest_ReturnsOk()
    {
        // Arrange
        var todosResponse = new GetAllTodosResponse
        {
            Success = true,
            Data = new TaskDto[]
            {
                new TaskDto() { Id = "f472523e-9b46-4011-b6b4-9000408d3412", Text = ":inbox_tray: In-Basket" },
                new TaskDto() { Id = "e26536e9-876b-4c10-bf8f-2147874ad708", Text = ":bangbang::house: Przesiądź się na nowy Kontomierz" },
                new TaskDto() { Id = "3fb6acc1-e060-446f-a936-f9664fb21c65", Text = ":bangbang::moneybag: Przerób TV Pivot strategy, żeby działała dla n ostatnich barów" },
                new TaskDto() { Id = "16313b4a-3c7f-4bb1-816c-9eefb24b9731", Text = ":exclamation::moneybag: Obejrzyj filmiki z maila od Arkadiusz Wójcic z Exante" },
                new TaskDto() { Id = "ab6a21ea-0602-471a-9677-f3acffbb8372", Text = ":warning::car: Zareklamuj okulary" },
                new TaskDto() { Id = "7c971a37-b340-4aac-802a-651035222797", Text = ":warning::office: Ogarnij nową myszkę" },
                new TaskDto() { Id = "06df9500-2288-4f32-8bad-3b641d906c4b", Text = ":x::mag: Wypróbuj Bitwarden" }
            }
        };
        _habiticaClientMock.Setup(x => x.GetAllTodos()).ReturnsAsync(todosResponse);

        var tagsResponse = new List<Tag>()
        {
            new Tag { Id = new Guid("958a73fb-d341-4513-83c2-c90c318193b5"), Name = "dom" },
            new Tag { Id = new Guid("f7ca5e48-7471-4bc3-ae65-baeba2fafa4c"), Name = "praca" },
            new Tag { Id = new Guid("7f74cad0-bd54-45ba-9562-9e590032e421"), Name = "zakupy" },
            new Tag { Id = new Guid("43e8cc3a-5e0b-48e0-9055-f37ba95b3a17"), Name = "piwowarstwo" },
            new Tag { Id = new Guid("2995fb09-6228-4388-9082-7fc657fd7a85"), Name = "pilne ważne" },
            new Tag { Id = new Guid("a62cb803-194d-4f1a-8bc2-a4ca8ecefc54"), Name = "niepilne ważne" },
            new Tag { Id = new Guid("d6443523-2d24-42bb-9136-88ae0d025ef0"), Name = "niepilne nieważne" },
            new Tag { Id = new Guid("0cfe6cb2-003c-4eaa-8d12-177aee36ec3f"), Name = "pilne nieważne" }
        };
        _tagServiceMock.Setup(x => x.GetAllTags()).ReturnsAsync(tagsResponse);

        // Act
        var result = await _todosRequestHandler.GetAllTodos();

        // Assert
        Assert.True(result.IsSuccess);
    }
}