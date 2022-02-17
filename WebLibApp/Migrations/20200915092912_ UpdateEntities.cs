using Microsoft.EntityFrameworkCore.Migrations;

namespace WebLibApp.Migrations
{
    public partial class UpdateEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorrowHistoryId",
                table: "Copies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowHistoryId",
                table: "Copies",
                type: "int",
                nullable: true);
        }
    }
}
