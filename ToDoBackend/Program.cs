using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using ToDoBackend.Mappers;
using ToDoBackend.Services;
using ToDoBackend.Validators;
using ToDoBackend.Repositories;

const string LogFormat = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {SourceContext} :: {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: LogFormat)
    .WriteTo.File($"logs/logs-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

try
{
    Log.Information("Starting up the ToDoBackend service...");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        });

    builder.Host.UseSerilog();
    builder.Services.AddControllers();
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

    app.UseSerilogRequestLogging();

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
    Log.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;