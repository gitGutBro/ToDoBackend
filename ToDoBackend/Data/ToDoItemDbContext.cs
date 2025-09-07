using Microsoft.EntityFrameworkCore;
using ToDoBackend.Data.Configurations;
using ToDoBackend.Models.ToDoItem;

namespace ToDoBackend.Data;

internal class ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("public");
        builder.ApplyConfiguration(new ToDoItemConfiguration());
    }
}