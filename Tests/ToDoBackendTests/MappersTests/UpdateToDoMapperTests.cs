using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models;
using Xunit;

namespace Tests.ToDoBackendTests.MappersTests;

public class UpdateToDoMapperTests
{
    [Fact]
    public void MapToModel_ShouldMapCorrectly_WhenMappingCalled()
    {
        //Arrange
        UpdateToDoMapper mapper = new();

        const string ValidTitle = nameof(ValidTitle);
        UpdateToDoItemDto updateDto = new(ValidTitle, false);

        //Act
        ToDoItem model = mapper.MapToModel(updateDto);

        //Assert
        Assert.Equal(updateDto.Title, model.Title);
        Assert.Equal(updateDto.IsCompleted, model.IsCompleted);
    }

    [Fact]
    public void ModelToMap_ShouldMapCorrectly_WhenMappingCalled()
    {
        //Arrange
        UpdateToDoMapper mapper = new();

        const string ValidTitle = nameof(ValidTitle);
        ToDoItem model = new(ValidTitle);

        //Act
        UpdateToDoItemDto createDto = mapper.MapToDto(model);

        //Assert
        Assert.Equal(createDto.Title, model.Title);
        Assert.Equal(createDto.IsCompleted, model.IsCompleted);
    }
}