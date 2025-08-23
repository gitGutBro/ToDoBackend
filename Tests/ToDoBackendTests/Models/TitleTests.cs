using ToDoBackend.Models;
using Xunit;

namespace Tests.ToDoBackendTests.Models;

public class TitleTests
{
    [Fact]
    public void Constructor_ShouldSetValue_WhenArgumentTitleIsValid()
    {
        //Arrange
        const string ValidTitle = "Title";

        //Act
        Title title = new(ValidTitle);

        //Assert
        Assert.Equal(ValidTitle, title.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_ShouldThrowArgumentException_WhenArgumentTitleIsNullOrWhiteSpace(string? title) =>
        //Arrange - Act - Assert
        Assert.Throws<ArgumentException>(() => new Title(title!));

    [Fact]
    public void SetValue_ShouldChangeValue_WhenNewValueIsValid()
    {
        //Arrange
        const string ValidTitle = "Title";
        const string NewValidTitleToUpdate = "New Title";

        Title title = new(ValidTitle);

        //Act
        title.SetValue(NewValidTitleToUpdate);

        //Assert
        Assert.Equal(NewValidTitleToUpdate, title.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void SetValue_ShouldThrowArgumentException_WhenNewValueIsNullOrWhiteSpace(string? newTitle)
    {
        //Arrange
        Title title = new("Title");

        //Act - Assert
        Assert.Throws<ArgumentException>(() => title.SetValue(newTitle!));
    }

    [Fact]
    public void Operator_ImplicitConversionToString_ShouldReturnUnderlyingValue()
    {
        //Arrange
        const string ValidTitle = "Title";

        Title title = new(ValidTitle);

        //Act
        string titleAsString = title;

        //Assert
        Assert.Equal(ValidTitle, titleAsString);
    }

    [Fact]
    public void Operator_ImplicitConversionFromString_ShouldCreateTitleInstance()
    {
        //Arrange
        const string ValidTitle = "Title";

        //Act
        Title title = ValidTitle;

        //Assert
        Assert.Equal(ValidTitle, title.Value);
    }
}