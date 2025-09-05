using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;
using ToDoBackend.Data;
using ToDoBackend.Mappers;
using ToDoBackend.Repositories;
using ToDoBackend.Services;
using ToDoBackend.Validators;

const string LogFormat = "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {SourceContext} :: {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: LogFormat)
    .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .MinimumLevel.Debug()
    .CreateLogger();

try
{
    Log.Information("Starting up the ToDoBackend service...");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
            options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb));

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "ToDo API", Version = "v1" });

        options.MapType<Instant>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date-time",
            Example = new OpenApiString(Instant.FromUtc(2025, 8, 31, 12, 34, 56).ToString())
        });

        options.MapType<LocalDate>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = new OpenApiString("2025-08-31")
        });

        options.MapType<LocalTime>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = new OpenApiString("09:00:00")
        });
    });

    string connectionString = builder.Configuration.GetConnectionString("Postgres")
        ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");

    builder.Services.AddDbContext<TodoDbContext>(options =>
        options
            .UseNpgsql(connectionString, npgsql => npgsql.UseNodaTime())
            .UseSnakeCaseNamingConvention());

    builder.Services.AddHealthChecks()
        .AddNpgSql(connectionString, name: "postgres", tags: ["db", "postgres"]);

    builder.Services.AddSingleton<IToDoRepository, ToDoRepositoryEf>();
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

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthorization();

    app.MapHealthChecks("/health");
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