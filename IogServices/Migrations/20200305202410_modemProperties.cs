using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class modemProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Modems_Eui",
                table: "Modems");

            migrationBuilder.RenameColumn(
                name: "Eui",
                table: "Modems",
                newName: "DeviceEui");

            migrationBuilder.AddColumn<int>(
                name: "FirmwareVersion",
                table: "Modems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Serial",
                table: "Modems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Modems_DeviceEui",
                table: "Modems",
                column: "DeviceEui");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Modems_DeviceEui",
                table: "Modems");

            migrationBuilder.DropColumn(
                name: "FirmwareVersion",
                table: "Modems");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Modems");

            migrationBuilder.RenameColumn(
                name: "DeviceEui",
                table: "Modems",
                newName: "Eui");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Modems_Eui",
                table: "Modems",
                column: "Eui");
        }
    }
}
