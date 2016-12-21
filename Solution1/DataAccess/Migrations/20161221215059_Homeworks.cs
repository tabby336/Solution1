using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Homeworks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Homeworks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                        .Annotation("Npgsql:ValueGeneratedOnAdd", true),
                    ModuleId = table.Column<Guid>(nullable: false),
                    Observations = table.Column<string>(maxLength: 2048, nullable: true),
                    OwesMeMoney = table.Column<bool>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Url = table.Column<string>(maxLength: 1024, nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Homeworks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Homeworks");
        }
    }
}
