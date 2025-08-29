using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models;
using Xunit;

namespace Tests.ToDoBackendTests.MappersTests;

public class CreateToDoMapperTests
{
    [Fact]
    public void MapToModel_ShouldMapCorrectly_WhenMappingCalled()
    {
        //Arrange
        CreateToDoMapper mapper = new();

        const string ValidTitle = nameof(ValidTitle);
        CreateToDoItemDto createDto = new(ValidTitle, false);

        //Act
        ToDoItem model = mapper.MapToModel(createDto);

        //Assert
        Assert.Equal(createDto.Title, model.Title);
        Assert.Equal(createDto.IsCompleted, model.IsCompleted);
    }

    [Fact]
    public void MapToDto_ShouldMapCorrectly_WhenMappingCalled()
    {
        //Arrange
        CreateToDoMapper mapper = new();

        const string ValidTitle = nameof(ValidTitle);
        ToDoItem model = new(ValidTitle);

        //Act
        CreateToDoItemDto createDto = mapper.MapToDto(model);

        //Assert
        Assert.Equal(model.Title, createDto.Title);
        Assert.Equal(model.IsCompleted, createDto.IsCompleted);
    }
}