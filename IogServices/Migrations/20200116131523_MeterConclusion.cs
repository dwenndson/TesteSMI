using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class MeterConclusion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterModels_MeterModels_MeterModelId",
                table: "MeterModels");

            migrationBuilder.DropIndex(
                name: "IX_MeterModels_MeterModelId",
                table: "MeterModels");

            migrationBuilder.DropColumn(
                name: "MeterModelId",
                table: "MeterModels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeterModelId",
                table: "MeterModels",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MeterModels_MeterModelId",
                table: "MeterModels",
                column: "MeterModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterModels_MeterModels_MeterModelId",
                table: "MeterModels",
                column: "MeterModelId",
                principalTable: "MeterModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
