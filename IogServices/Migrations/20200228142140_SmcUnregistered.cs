using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class SmcUnregistered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmcUnregistereds",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Serial = table.Column<string>(nullable: true),
                    DeviceEui = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmcUnregistereds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeterUnregistered",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Serial = table.Column<string>(nullable: true),
                    MeterPhase = table.Column<string>(nullable: true),
                    SmcUnregisteredId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterUnregistered", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterUnregistered_SmcUnregistereds_SmcUnregisteredId",
                        column: x => x.SmcUnregisteredId,
                        principalTable: "SmcUnregistereds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterUnregistered_SmcUnregisteredId",
                table: "MeterUnregistered",
                column: "SmcUnregisteredId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeterUnregistered");

            migrationBuilder.DropTable(
                name: "SmcUnregistereds");
        }
    }
}
