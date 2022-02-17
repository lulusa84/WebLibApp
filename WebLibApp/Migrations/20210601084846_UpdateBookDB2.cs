using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLibApp.Migrations
{
    public partial class UpdateBookDB2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Copies_BookReservations_BookReservationId",
                table: "Copies");

            migrationBuilder.DropIndex(
                name: "IX_Copies_BookReservationId",
                table: "Copies");

            migrationBuilder.DropColumn(
                name: "BookReservationId",
                table: "Copies");

            migrationBuilder.AddColumn<int>(
                name: "CopyId",
                table: "BookReservations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_CopyId",
                table: "BookReservations",
                column: "CopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReservations_Copies_CopyId",
                table: "BookReservations",
                column: "CopyId",
                principalTable: "Copies",
                principalColumn: "CopyId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReservations_Copies_CopyId",
                table: "BookReservations");

            migrationBuilder.DropIndex(
                name: "IX_BookReservations_CopyId",
                table: "BookReservations");

            migrationBuilder.DropColumn(
                name: "CopyId",
                table: "BookReservations");

            migrationBuilder.AddColumn<int>(
                name: "BookReservationId",
                table: "Copies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Copies_BookReservationId",
                table: "Copies",
                column: "BookReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Copies_BookReservations_BookReservationId",
                table: "Copies",
                column: "BookReservationId",
                principalTable: "BookReservations",
                principalColumn: "BookReservationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
