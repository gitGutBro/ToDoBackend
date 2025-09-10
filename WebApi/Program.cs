using ApplicationBackend.Dtos;
using ApplicationBackend.Mappers;
using ApplicationBackend.Repositories;
using ApplicationBackend.Services;
using ApplicationBackend.Validators;
using Domain.Entities.ToDoItem;
using Domain.Factories;
using Domain.Services;
using InfrastructureBackend.Data;
using InfrastructureBackend.Repositories;
using InfrastructureBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NodaTime;
using NodaTime.Serialization.SystemTextJson;
using Serilog;

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
        {
            options.JsonSerializerOptions.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        });

    builder.Host.UseSerilog();
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

    string connectionString = builder.Configuration.GetConnectionString("Postgres")
        ?? throw new InvalidOperationException("Connection string 'Postgres' not found.");

    builder.Services.AddDbContext<ToDoItemDbContext>(options =>
        options
            .UseNpgsql(connectionString, npgsql => npgsql.UseNodaTime())
            .UseSnakeCaseNamingConvention());

    builder.Services.AddHealthChecks()
        .AddNpgSql(connectionString, name: "postgres", tags: ["db", "postgres"]);

    builder.Services.AddScoped<IToDoRepository, ToDoRepository>();
    builder.Services.AddScoped<IToDoService, ToDoService>();
    builder.Services.AddScoped<IToDoItemFactory, ToDoItemFactory>();
    builder.Services.AddScoped<IScheduleService, ScheduleService>();
    builder.Services.AddScoped<ICompletionService, CompletionService>();
    builder.Services.AddScoped<IToDoItemFactory, ToDoItemFactory>();
    builder.Services.AddTransient<ToDoItemValidator>();
    builder.Services.AddTransient<IMapper<ToDoItem, CreateToDoItemDto>, CreateToDoMapper>();
    builder.Services.AddTransient<IMapper<ToDoItem, ToDoItemDto>, ToDoItemMapper>();
    builder.Services.AddTransient<IUpdateMapper<ToDoItem, UpdateToDoItemDto>, UpdateToDoMapper>();

    WebApplication app = builder.Build();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider services = scope.ServiceProvider;

        try
        {
            ToDoItemDbContext dbContext = services.GetRequiredService<ToDoItemDbContext>();
            dbContext.Database.Migrate();

            if (app.Environment.IsDevelopment())
            {
                using IServiceScope seedScope = app.Services.CreateScope();
                IServiceProvider seedServices = seedScope.ServiceProvider;
                ToDoItemDbContext database = seedServices.GetRequiredService<ToDoItemDbContext>();

                if (await database.ToDoItems.AnyAsync() == false)
                {
                    database.ToDoItems.Add(new ToDoItem("Seed task 1", "Created by seed"));
                    database.ToDoItems.Add(new ToDoItem("Seed task 2", "Created by seed"));
                    await database.SaveChangesAsync();
                    Log.Information("Seeded database with initial ToDo items.");
                }
            }

            Log.Information("Database migration completed successfully.");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "An error occurred during database migration");
            throw;
        }
    }

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