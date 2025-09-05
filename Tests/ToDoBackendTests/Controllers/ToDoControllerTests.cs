using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ToDoBackend.Controllers;
using ToDoBackend.Models.ToDoItem;
using ToDoBackend.Repositories;
using Xunit;

namespace Tests.ToDoBackendTests.Controllers;

public class ToDoControllerTests
{
    [Fact]
    public async Task GetAll_ShouldReturnOk_WhenRepositoryContainsItems()
    {
        //Arrange
        WebApplicationFactory<Program> factory = new();
        HttpClient client = factory.CreateClient();
        IToDoRepository repository = factory.Services.GetRequiredService<IToDoRepository>();

        const string ValidTitle = "Title";
        string controllerName = nameof(ToDoController).Replace("Controller", "");

        //Act
        await repository.CreateAsync(new ToDoItem(ValidTitle), new CancellationToken());
        HttpResponseMessage responce = await client.GetAsync($"api/{controllerName}");

        //Assert
        responce.EnsureSuccessStatusCode();
    }
}