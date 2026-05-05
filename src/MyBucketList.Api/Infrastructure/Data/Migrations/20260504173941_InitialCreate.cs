using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBucketList.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BucketItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BucketItems", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BucketItems",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "Description", "Priority", "Title" },
                values: new object[] { 1, null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Experience the breathtaking views of the Grand Canyon.", 1, "Visit the Grand Canyon" });

            migrationBuilder.InsertData(
                table: "BucketItems",
                columns: new[] { "Id", "CompletedAt", "CreatedAt", "Description", "IsCompleted", "Priority", "Title" },
                values: new object[] { 2, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Take guitar lessons and learn to play your favorite songs.", true, 2, "Learn to play the guitar" });

            migrationBuilder.CreateIndex(
                name: "IX_BucketItems_IsCompleted",
                table: "BucketItems",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_BucketItems_Priority",
                table: "BucketItems",
                column: "Priority");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BucketItems");
        }
    }
}
