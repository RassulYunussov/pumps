using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace pumps.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pumps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Temperature = table.Column<float>(nullable: false),
                    Pressure = table.Column<float>(nullable: false),
                    Ampers = table.Column<float>(nullable: false),
                    Volume = table.Column<float>(nullable: false),
                    Vibration = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pumps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SensorLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PumpId = table.Column<int>(nullable: true),
                    Sensor = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorLogs_Pumps_PumpId",
                        column: x => x.PumpId,
                        principalTable: "Pumps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorLogs_PumpId",
                table: "SensorLogs",
                column: "PumpId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorLogs");

            migrationBuilder.DropTable(
                name: "Pumps");
        }
    }
}
