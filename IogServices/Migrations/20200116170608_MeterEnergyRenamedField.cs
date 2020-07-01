using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class MeterEnergyRenamedField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DirectReverse",
                table: "MeterEnergies",
                newName: "ReverseEnergy");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReverseEnergy",
                table: "MeterEnergies",
                newName: "DirectReverse");
        }
    }
}
