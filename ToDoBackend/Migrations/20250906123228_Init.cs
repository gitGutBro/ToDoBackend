using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;

#nullable disable

namespace ToDoBackend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "todo_items",
                schema: "public",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<Instant>(type: "timestamptz", nullable: false),
                    updated_at = table.Column<Instant>(type: "timestamptz", nullable: true),
                    due_date = table.Column<LocalDate>(type: "date", nullable: true),
                    due_time = table.Column<LocalTime>(type: "time", nullable: true),
                    time_zone_id = table.Column<string>(type: "text", nullable: true),
                    scheduled_at = table.Column<Instant>(type: "timestamptz", nullable: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    first_completed_at = table.Column<Instant>(type: "timestamptz", nullable: true),
                    last_completed_at = table.Column<Instant>(type: "timestamptz", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_todo_items", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_todo_items_scheduled_at",
                schema: "public",
                table: "todo_items",
                column: "scheduled_at",
                filter: "\"scheduled_at\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_todo_items_title",
                schema: "public",
                table: "todo_items",
                column: "title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "todo_items",
                schema: "public");
        }
    }
}
