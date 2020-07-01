using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IogServices.Migrations
{
    public partial class ticketId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "Tickets",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "CommandTickets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommandTickets_TicketId",
                table: "CommandTickets",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommandTickets_Tickets_TicketId",
                table: "CommandTickets",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommandTickets_Tickets_TicketId",
                table: "CommandTickets");

            migrationBuilder.DropIndex(
                name: "IX_CommandTickets_TicketId",
                table: "CommandTickets");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "CommandTickets");
        }
    }
}
