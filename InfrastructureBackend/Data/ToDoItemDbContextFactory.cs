using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace InfrastructureBackend.Data
{
    public class ToDoItemDbContextFactory : IDesignTimeDbContextFactory<ToDoItemDbContext>
    {
        public ToDoItemDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<ToDoItemDbContext> optionsBuilder = new();

            optionsBuilder.UseNpgsql
            (
                "Host=localhost;Port=5433;Database=ToDoItems;Username=postgres;Password=pass",
                npgsqlOptions => npgsqlOptions.UseNodaTime()
            )
            .UseSnakeCaseNamingConvention();

            return new ToDoItemDbContext(optionsBuilder.Options);
        }
    }
}