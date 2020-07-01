using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class SmcModelAddPositionsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PositionsCount",
                table: "SmcModels",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionsCount",
                table: "SmcModels");
        }
    }
}
