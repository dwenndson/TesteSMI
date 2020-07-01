using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class AddMeter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MeterModelId",
                table: "MeterModels",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Meters",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Serial = table.Column<string>(nullable: true),
                    Installation = table.Column<string>(nullable: true),
                    Latitude = table.Column<string>(nullable: true),
                    Longitude = table.Column<string>(nullable: true),
                    Prefix = table.Column<string>(nullable: true),
                    SmcSerial = table.Column<string>(nullable: true),
                    BillingConstant = table.Column<decimal>(nullable: false),
                    TcRatio = table.Column<string>(nullable: true),
                    TpRatio = table.Column<string>(nullable: true),
                    Registrars = table.Column<string>(nullable: true),
                    Tli = table.Column<string>(nullable: true),
                    Identifier = table.Column<int>(nullable: false),
                    Phase = table.Column<int>(nullable: false),
                    ConnectionPhase = table.Column<int>(nullable: false),
                    AccountantStatus = table.Column<int>(nullable: false),
                    MeterModelId = table.Column<Guid>(nullable: true),
                    RateTypeId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meters_MeterModels_MeterModelId",
                        column: x => x.MeterModelId,
                        principalTable: "MeterModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meters_RateTypes_RateTypeId",
                        column: x => x.RateTypeId,
                        principalTable: "RateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeterModels_MeterModelId",
                table: "MeterModels",
                column: "MeterModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_MeterModelId",
                table: "Meters",
                column: "MeterModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Meters_RateTypeId",
                table: "Meters",
                column: "RateTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MeterModels_MeterModels_MeterModelId",
                table: "MeterModels",
                column: "MeterModelId",
                principalTable: "MeterModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeterModels_MeterModels_MeterModelId",
                table: "MeterModels");

            migrationBuilder.DropTable(
                name: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_MeterModels_MeterModelId",
                table: "MeterModels");

            migrationBuilder.DropColumn(
                name: "MeterModelId",
                table: "MeterModels");
        }
    }
}
