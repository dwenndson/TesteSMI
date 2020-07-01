using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class RemoveIdentifiers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_SmcModels_Identifier",
                table: "SmcModels");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Manufacturers_Identifier",
                table: "Manufacturers");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "SmcModels");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Manufacturers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Identifier",
                table: "SmcModels",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Identifier",
                table: "Manufacturers",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_SmcModels_Identifier",
                table: "SmcModels",
                column: "Identifier");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Manufacturers_Identifier",
                table: "Manufacturers",
                column: "Identifier");
        }
    }
}
