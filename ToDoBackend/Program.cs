using ToDoBackend.Repositories;
using ToDoBackend.Services;
using ToDoBackend.Utils;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<IToDoRepository, ToDoRepository>();
    builder.Services.AddSingleton<IToDoService, ToDoService>();
    builder.Services.AddSingleton<CreateToDoMapper>();
    builder.Services.AddSingleton<UpdateToDoMapper>();

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseRouting();
    app.UseAuthorization();
    app.UseHttpsRedirection();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
    Environment.Exit(1);
}
finally
{
    Console.WriteLine("Application has stopped.");
}