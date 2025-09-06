    using Microsoft.EntityFrameworkCore;
    using ToDoBackend.Models.ToDoItem;

    namespace ToDoBackend.Data;

    public class ToDoItemDbContext(DbContextOptions<ToDoItemDbContext> options) : DbContext(options)
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

            entity.Property(item => item.Title)
                  .HasColumnName("title")
                  .IsRequired()
                  .HasMaxLength(100);

            // Маппинг приватного поля _xmin в модели:
            // - Property<uint?>("_xmin") создаёт в модели EF property с типом uint?
            // - HasField("_xmin") связывает это свойство с приватным полем в классе
            // - UsePropertyAccessMode(Field) говорит EF читать/писать напрямую в поле
            // - ValueGeneratedOnAddOrUpdate + IsConcurrencyToken делают xmin токеном optimistic concurrency
            entity.Property<uint?>("_xmin")
                  .HasColumnName("xmin")
                  // .HasColumnType("xid") // опционально; если миграции начнут пытаться создать колонку, уберите этот вызов
                  .ValueGeneratedOnAddOrUpdate()
                  .IsConcurrencyToken()
                  .HasField("_xmin")
                  .UsePropertyAccessMode(PropertyAccessMode.Field);

            // Owned: AuditInfo
            entity.OwnsOne(item => item.AuditInfo, navigation =>
            {
                navigation.Property(property => property.CreatedAt)
                          .HasColumnName("created_at")
                          .HasColumnType("timestamptz");
                navigation.Property(property => property.UpdatedAt)
                          .HasColumnName("updated_at")
                          .HasColumnType("timestamptz");
            });

            // Owned: ScheduleInfo — определяем поля и индекс внутри OwnsOne
            // Это корректный способ создать индекс для колонки owned-типа (scheduled_at).
            entity.OwnsOne(item => item.ScheduleInfo, navigation =>
            {
                navigation.Property(property => property.DueDate)
                          .HasColumnName("due_date")
                          .HasColumnType("date");

                navigation.Property(property => property.DueTime)
                          .HasColumnName("due_time")
                          .HasColumnType("time");

                navigation.Property(property => property.TimeZoneId)
                          .HasColumnName("time_zone_id")
                          .HasColumnType("text");

                navigation.Property(property => property.ScheduledAt)
                          .HasColumnName("scheduled_at")
                          .HasColumnType("timestamptz");

                // Индекс на owned property — объявляем на уровне navigation builder
                navigation.HasIndex(property => property.ScheduledAt)
                          .HasDatabaseName("ix_todo_items_scheduled_at")
                          .HasFilter("\"scheduled_at\" IS NOT NULL");
            });

            // Owned: CompletionInfo
            entity.OwnsOne(item => item.CompletionInfo, navigation =>
            {
                navigation.Property(property => property.IsCompleted)
                          .HasColumnName("is_completed")
                          .HasColumnType("boolean")
                          .IsRequired();

                navigation.Property(property => property.FirstCompletedAt)
                          .HasColumnName("first_completed_at")
                          .HasColumnType("timestamptz");

                navigation.Property(property => property.LastCompletedAt)
                          .HasColumnName("last_completed_at")
                          .HasColumnType("timestamptz");
            });

            // UNIQUE индекс на title (Acceptance criteria)
            entity.HasIndex(item => item.Title)
                  .IsUnique()
                  .HasDatabaseName("ix_todo_items_title");
        });
    }
}