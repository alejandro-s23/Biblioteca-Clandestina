using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class ChangeClientIdToUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Users_UserId",
                table: "BookRents");

            migrationBuilder.DropIndex(
                name: "IX_BookRents_ClientId",
                table: "BookRents");

            migrationBuilder.DropIndex(
                name: "IX_BookRents_UserId",
                table: "BookRents");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "BookRents");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BookRents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRents_UserId",
                table: "BookRents",
                column: "UserId",
                unique: true,
                filter: "[ReturnDate] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Users_UserId",
                table: "BookRents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Users_UserId",
                table: "BookRents");

            migrationBuilder.DropIndex(
                name: "IX_BookRents_UserId",
                table: "BookRents");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "BookRents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "BookRents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_BookRents_ClientId",
                table: "BookRents",
                column: "ClientId",
                unique: true,
                filter: "[ReturnDate] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BookRents_UserId",
                table: "BookRents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Users_UserId",
                table: "BookRents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
