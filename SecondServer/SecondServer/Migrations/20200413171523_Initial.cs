using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SecondServer.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ToDoItems",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    IsCompleted = table.Column<bool>(nullable: false),
                    RecentUpdate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ToDoHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Status = table.Column<bool>(nullable: false),
                    TimeChange = table.Column<DateTime>(nullable: false),
                    ToDoItemID = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ToDoHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ToDoHistory_ToDoItems_ToDoItemID",
                        column: x => x.ToDoItemID,
                        principalTable: "ToDoItems",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ToDoHistory_ToDoItemID",
                table: "ToDoHistory",
                column: "ToDoItemID");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_IsCompleted",
                table: "ToDoItems",
                column: "IsCompleted");

            migrationBuilder.CreateIndex(
                name: "IX_ToDoItems_RecentUpdate",
                table: "ToDoItems",
                column: "RecentUpdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ToDoHistory");

            migrationBuilder.DropTable(
                name: "ToDoItems");
        }
    }
}
