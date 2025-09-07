using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoBackend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTitleIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_todo_items_title",
                schema: "public",
                table: "todo_items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_todo_items_title",
                schema: "public",
                table: "todo_items",
                column: "title",
                unique: true);
        }
    }
}
