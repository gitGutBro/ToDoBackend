using Domain.Entities.ToDoItem;
using InfrastructureBackend.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace InfrastructureBackend.Data;

public class ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("public");
        builder.ApplyConfiguration(new ToDoItemConfiguration());
    }
}