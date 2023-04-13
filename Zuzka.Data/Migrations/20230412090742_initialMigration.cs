using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zuzka.Data.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    DataId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublishedYear = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    IsBestseller = table.Column<bool>(type: "bit", nullable: false),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.DataId);
                    table.ForeignKey(
                        name: "FK_Data_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "DocumentId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Data_DocumentId",
                table: "Data",
                column: "DocumentId",
                unique: true,
                filter: "[DocumentId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data");

            migrationBuilder.DropTable(
                name: "Documents");
        }
    }
}
