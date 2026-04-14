using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class ClientToUserRefact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Clients_ClientId",
                table: "BookRents");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BookRents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRents_UserId",
                table: "BookRents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Clients_UserId",
                table: "BookRents",
                column: "UserId",
                principalTable: "Clients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Clients_UserId",
                table: "BookRents");

            migrationBuilder.DropIndex(
                name: "IX_BookRents_UserId",
                table: "BookRents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookRents");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Clients_ClientId",
                table: "BookRents",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
