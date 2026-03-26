using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Migrations
{
    /// <inheritdoc />
    public partial class CorrecaoTabelaBookRent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRent_Books_BookId",
                table: "BookRent");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRent_Clients_ClientId",
                table: "BookRent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRent",
                table: "BookRent");

            migrationBuilder.RenameTable(
                name: "BookRent",
                newName: "BookRents");

            migrationBuilder.RenameIndex(
                name: "IX_BookRent_ClientId",
                table: "BookRents",
                newName: "IX_BookRents_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRent_BookId",
                table: "BookRents",
                newName: "IX_BookRents_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRents",
                table: "BookRents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Books_BookId",
                table: "BookRents",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Clients_ClientId",
                table: "BookRents",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Books_BookId",
                table: "BookRents");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Clients_ClientId",
                table: "BookRents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRents",
                table: "BookRents");

            migrationBuilder.RenameTable(
                name: "BookRents",
                newName: "BookRent");

            migrationBuilder.RenameIndex(
                name: "IX_BookRents_ClientId",
                table: "BookRent",
                newName: "IX_BookRent_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRents_BookId",
                table: "BookRent",
                newName: "IX_BookRent_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRent",
                table: "BookRent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookRent_Books_BookId",
                table: "BookRent",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRent_Clients_ClientId",
                table: "BookRent",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
