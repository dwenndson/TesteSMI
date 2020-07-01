using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class UniqueKeyRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Smcs_Id_Serial",
                table: "Smcs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmcModels_Id_Name",
                table: "SmcModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RateTypes_Id_Name",
                table: "RateTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Modems_Id_Eui",
                table: "Modems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Meters_Id_Serial",
                table: "Meters");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MeterModels_Id_Name",
                table: "MeterModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Manufacturers_Id_Name",
                table: "Manufacturers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Smcs_Serial",
                table: "Smcs",
                column: "Serial");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmcModels_Name",
                table: "SmcModels",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RateTypes_Name",
                table: "RateTypes",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Modems_Eui",
                table: "Modems",
                column: "Eui");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Meters_Serial",
                table: "Meters",
                column: "Serial");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_MeterModels_Name",
                table: "MeterModels",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Smcs_Serial",
                table: "Smcs");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmcModels_Name",
                table: "SmcModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_RateTypes_Name",
                table: "RateTypes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Modems_Eui",
                table: "Modems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Meters_Serial",
                table: "Meters");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_MeterModels_Name",
                table: "MeterModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Manufacturers_Name",
                table: "Manufacturers");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Smcs_Id_Serial",
                table: "Smcs",
                columns: new[] { "Id", "Serial" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmcModels_Id_Name",
                table: "SmcModels",
                columns: new[] { "Id", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_RateTypes_Id_Name",
                table: "RateTypes",
                columns: new[] { "Id", "Name" });

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Modems_Id_Eui",
                table: "Modems",
                columns: new[] { "Id", "Eui" });

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
        }
    }
}
