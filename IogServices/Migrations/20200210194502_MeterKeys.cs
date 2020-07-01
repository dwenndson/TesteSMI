using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class MeterKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeterKeysId",
                table: "Meters",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeterKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Ak = table.Column<string>(nullable: true),
                    Ek = table.Column<string>(nullable: true),
                    Mk = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterKeys", x => x.Id);
                    table.UniqueConstraint("AK_MeterKeys_Serial", x => x.Serial);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meters_MeterKeysId",
                table: "Meters",
                column: "MeterKeysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_MeterKeys_MeterKeysId",
                table: "Meters",
                column: "MeterKeysId",
                principalTable: "MeterKeys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meters_MeterKeys_MeterKeysId",
                table: "Meters");

            migrationBuilder.DropTable(
                name: "MeterKeys");

            migrationBuilder.DropIndex(
                name: "IX_Meters_MeterKeysId",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "MeterKeysId",
                table: "Meters");
        }
    }
}
