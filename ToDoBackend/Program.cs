using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using ToDoBackend.Mappers;
using ToDoBackend.Repositories;
using ToDoBackend.Services;
using ToDoBackend.Validators;

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        });

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(converter =>
    {
        converter.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo API", Version = "v1" });

        converter.MapType<Instant>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date-time",
            Example = new OpenApiString(Instant.FromUtc(2025, 8, 31, 12, 34, 56).ToString())
        });

        converter.MapType<LocalDate>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = new OpenApiString("2025-08-31")
        });

        converter.MapType<LocalTime>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = new OpenApiString("09:00:00")
        });
    });

    builder.Services.AddSingleton<IToDoRepository, ToDoRepository>();
    builder.Services.AddSingleton<IToDoService, ToDoService>();
    builder.Services.AddSingleton<ToDoItemValidator>();
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

public partial class Program;