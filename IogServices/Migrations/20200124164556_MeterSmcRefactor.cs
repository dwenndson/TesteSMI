using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class MeterSmcRefactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SmcSerial",
                table: "Meters");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SmcSerial",
                table: "Meters",
                nullable: true);
        }
    }
}
