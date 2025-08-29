using Serilog;
using ToDoBackend.Mappers;
using ToDoBackend.Repositories;
using ToDoBackend.Services;
using ToDoBackend.Validators;

const string LogFormat = "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} :: {Message:lj}{NewLine}{Exception}";

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

    // Add services to the container.

    builder.Host.UseSerilog();
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddSingleton<IToDoRepository, ToDoRepository>();
    builder.Services.AddSingleton<IToDoService, ToDoService>();
    builder.Services.AddSingleton<ToDoItemValidator>();
    builder.Services.AddSingleton<CreateToDoMapper>();
    builder.Services.AddSingleton<UpdateToDoMapper>();

    WebApplication app = builder.Build();

    app.UseSerilogRequestLogging();

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
    Log.Fatal(ex, "Application start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program;