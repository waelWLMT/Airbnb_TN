using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLocataireAndOwnerTableNamesByConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logements_Owners_OwnerId",
                table: "Logements");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Locataires_LocataireId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Owners",
                table: "Owners");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Locataires",
                table: "Locataires");

            migrationBuilder.RenameTable(
                name: "Owners",
                newName: "Proprietaires");

            migrationBuilder.RenameTable(
                name: "Locataires",
                newName: "Voyageurs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proprietaires",
                table: "Proprietaires",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Voyageurs",
                table: "Voyageurs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Logements_Proprietaires_OwnerId",
                table: "Logements",
                column: "OwnerId",
                principalTable: "Proprietaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Voyageurs_LocataireId",
                table: "Reservations",
                column: "LocataireId",
                principalTable: "Voyageurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logements_Proprietaires_OwnerId",
                table: "Logements");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Voyageurs_LocataireId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Voyageurs",
                table: "Voyageurs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proprietaires",
                table: "Proprietaires");

            migrationBuilder.RenameTable(
                name: "Voyageurs",
                newName: "Locataires");

            migrationBuilder.RenameTable(
                name: "Proprietaires",
                newName: "Owners");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Locataires",
                table: "Locataires",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Owners",
                table: "Owners",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Logements_Owners_OwnerId",
                table: "Logements",
                column: "OwnerId",
                principalTable: "Owners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Locataires_LocataireId",
                table: "Reservations",
                column: "LocataireId",
                principalTable: "Locataires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
