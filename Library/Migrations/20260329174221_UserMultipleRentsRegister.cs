using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class UserMultipleRentsRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookRents_BookId",
                table: "BookRents");

            migrationBuilder.AddColumn<Guid>(
                name: "IdCurrentRent",
                table: "Books",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_IdCurrentRent",
                table: "Books",
                column: "IdCurrentRent");

            migrationBuilder.CreateIndex(
                name: "IX_BookRents_BookId",
                table: "BookRents",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookRents_IdCurrentRent",
                table: "Books",
                column: "IdCurrentRent",
                principalTable: "BookRents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookRents_IdCurrentRent",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_IdCurrentRent",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_BookRents_BookId",
                table: "BookRents");

            migrationBuilder.AlterColumn<int>(
                name: "IdCurrentRent",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookRents_BookId",
                table: "BookRents",
                column: "BookId",
                unique: true);
        }
    }
}
