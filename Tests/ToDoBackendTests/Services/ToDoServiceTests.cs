using ToDoBackend.Dtos;
using ToDoBackend.Mappers;
using ToDoBackend.Models;
using ToDoBackend.Repositories;
using ToDoBackend.Services;
using Xunit;

namespace Tests.ToDoBackendTests.Services;

public class ToDoServiceTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItemsInRepository_WhenRepositoryContainsItems()
    {
        //Arrange
        const string FirstValidTitle = "First Title";
        const string SecondValidTitle = "Second Title";

        CreateToDoItemDto firstDto = new(FirstValidTitle, false);
        CreateToDoItemDto secondDto = new(SecondValidTitle, false);

        IToDoRepository repository = new ToDoRepository();
        CreateToDoMapper createMapper = new();
        UpdateToDoMapper updateMapper = new();

        IToDoService service = new ToDoService(repository, createMapper, updateMapper);

        //Act
        ToDoItem firstItem = await service.CreateAsync(firstDto);
        ToDoItem secondItem =  await service.CreateAsync(secondDto);

        IEnumerable<ToDoItem> allItems = await service.GetAllAsync();

        //Assert
        Assert.Contains(firstItem, allItems);
        Assert.Contains(secondItem, allItems);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnValidItem_WhenItemExists()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        CreateToDoMapper createMapper = new();
        UpdateToDoMapper updateMapper = new();

        IToDoService service = new ToDoService(repository, createMapper, updateMapper);

        const string ValidTitle = "Title";
        CreateToDoItemDto dto = new(ValidTitle, false);

        ToDoItem createdItem = await service.CreateAsync(dto);

        //Act
        ToDoItem? retrievedItem = await service.GetByIdAsync(createdItem.Id);

        //Assert
        Assert.NotNull(retrievedItem);
        Assert.Equal(createdItem.Id, retrievedItem?.Id);
        Assert.Equal(createdItem.Title, retrievedItem?.Title);
        Assert.Equal(createdItem.IsCompleted, retrievedItem?.IsCompleted);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenItemDoesNotExist()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        CreateToDoMapper createMapper = new();
        UpdateToDoMapper updateMapper = new();

        IToDoService service = new ToDoService(repository, createMapper, updateMapper);
        Guid nonExistentId = Guid.NewGuid();

        //Act
        ToDoItem? retrievedItem = await service.GetByIdAsync(nonExistentId);

        //Assert
        Assert.Null(retrievedItem);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCorrectly_WhenCreateAsyncCalled()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        CreateToDoMapper createMapper = new();
        UpdateToDoMapper updateMapper = new();

        IToDoService service = new ToDoService(repository, createMapper, updateMapper);

        const string ValidTitle = nameof(ValidTitle);
        CreateToDoItemDto createDto = new(ValidTitle, false); 

        //Act
        ToDoItem itemToCreate = await service.CreateAsync(createDto);
        ToDoItem? createdItem = await service.GetByIdAsync(itemToCreate.Id);

        //Assert
        Assert.NotNull(itemToCreate);
        Assert.NotNull(createdItem);
        Assert.Equal(itemToCreate.Id, createdItem.Id);
        Assert.Equal(itemToCreate.Title, createdItem.Title);
        Assert.Equal(itemToCreate.IsCompleted, createdItem.IsCompleted);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateModelCorrectly_WhenUpdateAsyncCalled()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        CreateToDoMapper createMapper = new();
        UpdateToDoMapper updateMapper = new();

        IToDoService service = new ToDoService(repository, createMapper, updateMapper);

        const string FirstValidTitle = nameof(FirstValidTitle);
        CreateToDoItemDto createDto = new(FirstValidTitle, false);
        ToDoItem createdItem = await service.CreateAsync(createDto);

        const string SecondValidTitle = nameof(SecondValidTitle);
        UpdateToDoItemDto updateDto = new(SecondValidTitle, true);

        //Act
        await service.UpdateAsync(createdItem.Id, updateDto);
        ToDoItem? updatedItem = await service.GetByIdAsync(createdItem.Id);

        //Assert
        Assert.Equal(updatedItem!.Id, createdItem.Id);
        Assert.Equal(updatedItem.Title, updateDto.Title);
        Assert.Equal(updatedItem.IsCompleted, updateDto.IsCompleted);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteItemCorrectly_WhenDeleteAsyncCalledAndItemExist()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        CreateToDoMapper createMapper = new();
        UpdateToDoMapper updateMapper = new();

        IToDoService service = new ToDoService(repository, createMapper, updateMapper);

        const string ValidTitle = nameof(ValidTitle);
        CreateToDoItemDto createDto = new(ValidTitle, false);
        ToDoItem createdItem = await service.CreateAsync(createDto);

        //Act
        await service.DeleteAsync(createdItem.Id);
        ToDoItem? item = await service.GetByIdAsync(createdItem.Id);

        //Assert
        Assert.Null(item);
    }
}