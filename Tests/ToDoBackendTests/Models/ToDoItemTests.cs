using ToDoBackend.Models;
using Xunit;

namespace Tests.ToDoBackendTests.Models;

public class ToDoItemTests
{
    [Fact]
    public void Constructor_ShouldAssignNonEmptyId_WhenArgumentsAreValid()
    {
        //Arrange
        const string ValidTitle = "Title";

        //Act
        ToDoItem item = new(ValidTitle);

        //Assert
        Assert.True(item.Id != Guid.Empty);
    }

    [Fact]
    public void UpdateTitle_ShouldChangeTitle_WhenNewTitleIsValid()
    {
        //Arrange
        const string ValidTitle = "Title";
        const string NewValidTitleToUpdate = "New Title";

        ToDoItem item = new(ValidTitle);

        //Act
        item.UpdateTitle(NewValidTitleToUpdate);

        //Assert
        Assert.Equal(NewValidTitleToUpdate, item.Title);
    }
}