using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using ToDoBackend.Models;

namespace ToDoBackend.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<ToDoItem> ToDoItems => Set<ToDoItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("public");

        builder.Entity<ToDoItem>(entity =>
        {
            entity.ToTable("todo_items");

            entity.HasKey(item => item.Id);

            entity.Property(item => item.Id)
                  .HasColumnName("id")
                  .ValueGeneratedNever();

            ValueConverter<Title, string> titleConverter = new
            (
                value => value.Value,
                value => new Title(value)
            );

            entity.Property(item => item.Title)
                  .HasConversion(titleConverter)
                  .HasColumnName("title")
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property<uint?>(nameof(ToDoItem.Xmin))
                .HasColumnName("xmin")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();

            // AuditInfo — Owned
            entity.OwnsOne(item => item.AuditInfo, navigation =>
            {
                navigation.Property(property => property.CreatedAt).HasColumnName("created_at").HasColumnType("timestamptz");
                navigation.Property(property => property.UpdatedAt).HasColumnName("updated_at").HasColumnType("timestamptz");
            });

            entity.OwnsOne(item => item.ScheduleInfo, navigation =>
            {
                navigation.Property(property => property.DueDate).HasColumnName("due_date").HasColumnType("date");
                navigation.Property(property => property.DueTime).HasColumnName("due_time").HasColumnType("time");
                navigation.Property(property => property.TimeZoneId).HasColumnName("time_zone_id").HasColumnType("text");
                navigation.Property(property => property.ScheduledAt).HasColumnName("scheduled_at").HasColumnType("timestamptz");
            });

            entity.OwnsOne(item => item.CompletionInfo, navigation =>
            {
                navigation.Property(property => property.IsCompleted).HasColumnName("is_completed").HasColumnType("boolean").IsRequired();
                navigation.Property(property => property.FirstCompletedAt).HasColumnName("first_completed_at").HasColumnType("timestamptz");
                navigation.Property(property => property.LastCompletedAt).HasColumnName("last_completed_at").HasColumnType("timestamptz");
            });

            entity.HasIndex("scheduled_at")
                  .HasDatabaseName("ix_todo_items_scheduled_at")
                  .HasFilter("scheduled_at IS NOT NULL");
        });
    }
}
