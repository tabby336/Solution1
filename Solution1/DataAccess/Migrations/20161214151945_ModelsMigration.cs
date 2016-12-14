using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ModelsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Marks",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "HomeworkId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "Marks");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    CourseId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 1024, nullable: true),
                    HasHomework = table.Column<bool>(nullable: false),
                    HasTest = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(maxLength: 255, nullable: false),
                    UrlPdf = table.Column<string>(maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "Marks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Marks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Marks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Marks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Marks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marks",
                table: "Marks",
                columns: new[] { "ModuleId", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Marks",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Marks");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.AddColumn<Guid>(
                name: "HomeworkId",
                table: "Marks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "Marks",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "Marks",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Marks",
                table: "Marks",
                columns: new[] { "HomeworkId", "StudentId" });
        }
    }
}
