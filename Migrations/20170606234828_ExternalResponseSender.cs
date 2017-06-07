using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticketer.Migrations
{
    public partial class ExternalResponseSender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalTicketResponse_ExternalClients_ExternalClientId",
                table: "ExternalTicketResponse");

            migrationBuilder.RenameColumn(
                name: "ExternalClientId",
                table: "ExternalTicketResponse",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalTicketResponse_ExternalClientId",
                table: "ExternalTicketResponse",
                newName: "IX_ExternalTicketResponse_SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalTicketResponse_ExternalClients_SenderId",
                table: "ExternalTicketResponse",
                column: "SenderId",
                principalTable: "ExternalClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExternalTicketResponse_ExternalClients_SenderId",
                table: "ExternalTicketResponse");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "ExternalTicketResponse",
                newName: "ExternalClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ExternalTicketResponse_SenderId",
                table: "ExternalTicketResponse",
                newName: "IX_ExternalTicketResponse_ExternalClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExternalTicketResponse_ExternalClients_ExternalClientId",
                table: "ExternalTicketResponse",
                column: "ExternalClientId",
                principalTable: "ExternalClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
