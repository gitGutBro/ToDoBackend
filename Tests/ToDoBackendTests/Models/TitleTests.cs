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