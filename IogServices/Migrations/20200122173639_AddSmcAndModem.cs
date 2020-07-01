using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class AddSmcAndModem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SmcModels",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RateTypes",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "Meters",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ModemId",
                table: "Meters",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SmcId",
                table: "Meters",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MeterModels",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Manufacturers",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmcModels_Id_Name",
                table: "SmcModels",
                columns: new[] { "Id", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RateTypes_Id_Name",
                table: "RateTypes",
                columns: new[] { "Id", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Meters_Id_Serial",
                table: "Meters",
                columns: new[] { "Id", "Serial" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MeterModels_Id_Name",
                table: "MeterModels",
                columns: new[] { "Id", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Manufacturers_Id_Name",
                table: "Manufacturers",
                columns: new[] { "Id", "Name" });

            migrationBuilder.CreateTable(
                name: "Modems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Eui = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modems", x => x.Id);
                    table.UniqueConstraint("AK_Modems_Id_Eui", x => new { x.Id, x.Eui });
                });

            migrationBuilder.CreateTable(
                name: "Smcs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Serial = table.Column<string>(nullable: false),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    SmcModelId = table.Column<Guid>(nullable: true),
                    ModemId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smcs", x => x.Id);
                    table.UniqueConstraint("AK_Smcs_Id_Serial", x => new { x.Id, x.Serial });
                    table.ForeignKey(
                        name: "FK_Smcs_Modems_ModemId",
                        column: x => x.ModemId,
                        principalTable: "Modems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Smcs_SmcModels_SmcModelId",
                        column: x => x.SmcModelId,
                        principalTable: "SmcModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meters_ModemId",
                table: "Meters",
                column: "ModemId");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_SmcId",
                table: "Meters",
                column: "SmcId");

            migrationBuilder.CreateIndex(
                name: "IX_Smcs_ModemId",
                table: "Smcs",
                column: "ModemId");

            migrationBuilder.CreateIndex(
                name: "IX_Smcs_SmcModelId",
                table: "Smcs",
                column: "SmcModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_Modems_ModemId",
                table: "Meters",
                column: "ModemId",
                principalTable: "Modems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_Smcs_SmcId",
                table: "Meters",
                column: "SmcId",
                principalTable: "Smcs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meters_Modems_ModemId",
                table: "Meters");

            migrationBuilder.DropForeignKey(
                name: "FK_Meters_Smcs_SmcId",
                table: "Meters");

            migrationBuilder.DropTable(
                name: "Smcs");

            migrationBuilder.DropTable(
                name: "Modems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmcModels_Id_Name",
                table: "SmcModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RateTypes_Id_Name",
                table: "RateTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Meters_Id_Serial",
                table: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_Meters_ModemId",
                table: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_Meters_SmcId",
                table: "Meters");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MeterModels_Id_Name",
                table: "MeterModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Manufacturers_Id_Name",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "ModemId",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "SmcId",
                table: "Meters");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SmcModels",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RateTypes",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Serial",
                table: "Meters",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MeterModels",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Manufacturers",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
