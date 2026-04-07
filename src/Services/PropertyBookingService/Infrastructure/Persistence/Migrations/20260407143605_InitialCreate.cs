using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locataires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locataires", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    PrixParNuit = table.Column<double>(type: "float", nullable: false),
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    Adresse_CodePostal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_ComplementAdresse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresse_Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Long = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Pays = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Rue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adresse_Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logements_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogementId = table.Column<int>(type: "int", nullable: false),
                    LocataireId = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NbrJours = table.Column<int>(type: "int", nullable: false),
                    CoutTotal = table.Column<double>(type: "float", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Locataires_LocataireId",
                        column: x => x.LocataireId,
                        principalTable: "Locataires",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Logements_LogementId",
                        column: x => x.LogementId,
                        principalTable: "Logements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JourReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    Jour = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourReservations_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JourReservations_ReservationId",
                table: "JourReservations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Logements_OwnerId",
                table: "Logements",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LocataireId",
                table: "Reservations",
                column: "LocataireId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LogementId",
                table: "Reservations",
                column: "LogementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JourReservations");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Locataires");

            migrationBuilder.DropTable(
                name: "Logements");

            migrationBuilder.DropTable(
                name: "Owners");
        }
    }
}
