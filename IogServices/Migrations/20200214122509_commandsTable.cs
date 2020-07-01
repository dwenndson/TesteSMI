using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class commandsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Mk",
                table: "MeterKeys",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ek",
                table: "MeterKeys",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Ak",
                table: "MeterKeys",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "CommandTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CommandId = table.Column<Guid>(nullable: false),
                    InitialDate = table.Column<DateTime>(nullable: false),
                    FinishDate = table.Column<DateTime>(nullable: false),
                    Attempt = table.Column<int>(nullable: false),
                    Pdu = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    StatusCommand = table.Column<int>(nullable: false),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommandTickets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    CommandType = table.Column<int>(nullable: false),
                    User = table.Column<string>(nullable: true),
                    InitialDate = table.Column<DateTime>(nullable: false),
                    FinishDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    StatusCommand = table.Column<int>(nullable: false),
                    Interval = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommandTickets");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "Mk",
                table: "MeterKeys",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "Ek",
                table: "MeterKeys",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "Ak",
                table: "MeterKeys",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 32);
        }
    }
}
