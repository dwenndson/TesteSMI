using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class commandInCommandTicket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Command",
                table: "CommandTickets",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Command",
                table: "CommandTickets");
        }
    }
}
