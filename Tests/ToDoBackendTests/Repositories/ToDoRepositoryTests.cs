using ToDoBackend.Models;
using ToDoBackend.Repositories;
using Xunit;

namespace Tests.ToDoBackendTests.Repositories;

public class ToDoRepositoryTests
{
    [Fact]
    public async Task Constructor_ShouldDataBeInited_WhenRepositoryCreated()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        //Act
        IEnumerable<ToDoItem> allItems = await repository.GetAllAsync();

        //Assert
        Assert.NotNull(allItems);
    }

    [Fact]
    public async Task CreateAsync_ShouldAddItemToRepository_WhenItemIsValid()
    {
        //Arrange
        const string ValidTitle = "Title";

        ToDoItem item = new(ValidTitle);
        IToDoRepository repository = new ToDoRepository();

        //Act
        await repository.CreateAsync(item);
        IEnumerable<ToDoItem> allItems = await repository.GetAllAsync();

        //Assert
        Assert.Contains(item, allItems);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllItemsInRepository_WhenReposotoryContainsItems()
    {
        //Arrange
        const string FirstValidTitle = "First Title";
        const string SecondValidTitle = "Second Title";

        ToDoItem firstItem = new(FirstValidTitle);
        ToDoItem secondItem = new(SecondValidTitle);

        IToDoRepository repository = new ToDoRepository();

        //Act
        await repository.CreateAsync(firstItem);
        await repository.CreateAsync(secondItem);

        IEnumerable<ToDoItem> allItems = await repository.GetAllAsync();

        //Assert
        Assert.Contains(firstItem, allItems);
        Assert.Contains(secondItem, allItems);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnNull_WhenRepositoryEmpty()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();

        //Act
        IEnumerable<ToDoItem> allItems = await repository.GetAllAsync();

        //Assert
        Assert.Empty(allItems);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnItem_WhenItemWithGivenIdExistsInRepository()
    {
        //Arrange
        const string ValidTitle = "Title";

        ToDoItem item = new(ValidTitle);
        IToDoRepository repository = new ToDoRepository();

        //Act
        await repository.CreateAsync(item);
        ToDoItem? retrievedItem = await repository.GetByIdAsync(item.Id);

        //Assert
        Assert.NotNull(retrievedItem);
        Assert.Equal(item, retrievedItem);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_ItemWithGivenIdDoesNotExistInRepository()
    {
        //Arrange
        IToDoRepository repository = new ToDoRepository();
        Guid nonExistentId = Guid.NewGuid();

        //Act
        ToDoItem? retrievedItem = await repository.GetByIdAsync(nonExistentId);

        //Assert
        Assert.Null(retrievedItem);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveItemFromRepository_WhenItemWithGivenIdExists()
    {
        //Arrange
        const string ValidTitle = "Title";

        ToDoItem item = new(ValidTitle);
        IToDoRepository repository = new ToDoRepository();

        await repository.CreateAsync(item);

        //Act
        await repository.DeleteAsync(item.Id);
        IEnumerable<ToDoItem> allItems = await repository.GetAllAsync();

        //Assert
        Assert.DoesNotContain(item, allItems);
    }
    
    [Fact]
    public async Task UpdateAsync_ShouldUpdateToDoItem_WhenToDoItemWithGivenIdExists()
    {
        //Arrange
        const string ValidTitle = "Title";
        const string NewValidTitleToUpdate = "New Title";

        ToDoItem item = new(ValidTitle);
        IToDoRepository repository = new ToDoRepository();

        await repository.CreateAsync(item);
        item.UpdateTitle(NewValidTitleToUpdate);

        //Act
        await repository.UpdateAsync(item);
        ToDoItem? updatedItem = await repository.GetByIdAsync(item.Id);

        //Assert
        Assert.NotNull(updatedItem);
        Assert.Equal(NewValidTitleToUpdate, updatedItem!.Title.Value);
    }

    [Fact]
    public async Task Constructor_ShouldItemsBeIsolatedBetweenInstances_WhenMultipleRepositoryInstancesCreated()
    {
        //Arrange
        const string FirstValidTitle = "First Title";
        const string SecondValidTitle = "Second Title";

        ToDoItem firstItem = new(FirstValidTitle);
        ToDoItem secondItem = new(SecondValidTitle);

        IToDoRepository firstRepository = new ToDoRepository();
        IToDoRepository secondRepository = new ToDoRepository();

        //Act
        await firstRepository.CreateAsync(firstItem);
        await secondRepository.CreateAsync(secondItem);

        IEnumerable<ToDoItem> firstRepoItems = await firstRepository.GetAllAsync();
        IEnumerable<ToDoItem> secondRepoItems = await secondRepository.GetAllAsync();

        //Assert
        Assert.Contains(firstItem, firstRepoItems);
        Assert.DoesNotContain(secondItem, firstRepoItems);
        Assert.Contains(secondItem, secondRepoItems);
        Assert.DoesNotContain(firstItem, secondRepoItems);
    }
}