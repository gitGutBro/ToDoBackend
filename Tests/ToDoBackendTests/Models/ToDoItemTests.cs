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

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_ShouldThrowArgumentException_WhenTitleIsNullOrWhiteSpace(string? invalidTitle) =>
        //Arrange - Act - Assert
        Assert.Throws<ArgumentException>(() => new ToDoItem(invalidTitle!));

    [Fact]
    public void UpdateTitle_ShouldChangeTitleValue_WhenNewTitleIsValid()
    {
        //Arrange
        const string ValidTitle = "Title";
        const string NewValidTitleToUpdate = "New Title";

        ToDoItem item = new(ValidTitle);

        //Act
        item.UpdateTitle(NewValidTitleToUpdate);

        //Assert
        Assert.Equal(NewValidTitleToUpdate, item.Title.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void UpdateTitle_ShouldThrowArgumentException_WhenNewTitleIsNullOrWhiteSpace(string? invalidTitle) 
    {
        //Arrange
        const string ValidTitle = "Title";

        ToDoItem item = new(ValidTitle);

        //Act - Assert
        Assert.Throws<ArgumentException>(() => item.UpdateTitle(invalidTitle!));
    }
}