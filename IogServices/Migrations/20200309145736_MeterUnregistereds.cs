using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class MeterUnregistereds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterUnregistered_SmcUnregistereds_SmcUnregisteredId",
                table: "MeterUnregistered");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeterUnregistered",
                table: "MeterUnregistered");

            migrationBuilder.RenameTable(
                name: "MeterUnregistered",
                newName: "MeterUnregistereds");

            migrationBuilder.RenameIndex(
                name: "IX_MeterUnregistered_SmcUnregisteredId",
                table: "MeterUnregistereds",
                newName: "IX_MeterUnregistereds_SmcUnregisteredId");

            migrationBuilder.AddColumn<string>(
                name: "DeviceEui",
                table: "MeterUnregistereds",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeterUnregistereds",
                table: "MeterUnregistereds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterUnregistereds_SmcUnregistereds_SmcUnregisteredId",
                table: "MeterUnregistereds",
                column: "SmcUnregisteredId",
                principalTable: "SmcUnregistereds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterUnregistereds_SmcUnregistereds_SmcUnregisteredId",
                table: "MeterUnregistereds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeterUnregistereds",
                table: "MeterUnregistereds");

            migrationBuilder.DropColumn(
                name: "DeviceEui",
                table: "MeterUnregistereds");

            migrationBuilder.RenameTable(
                name: "MeterUnregistereds",
                newName: "MeterUnregistered");

            migrationBuilder.RenameIndex(
                name: "IX_MeterUnregistereds_SmcUnregisteredId",
                table: "MeterUnregistered",
                newName: "IX_MeterUnregistered_SmcUnregisteredId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeterUnregistered",
                table: "MeterUnregistered",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterUnregistered_SmcUnregistereds_SmcUnregisteredId",
                table: "MeterUnregistered",
                column: "SmcUnregisteredId",
                principalTable: "SmcUnregistereds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
