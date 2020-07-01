using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class ModemRefator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Modems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Modems",
                nullable: true);
        }
    }
}
