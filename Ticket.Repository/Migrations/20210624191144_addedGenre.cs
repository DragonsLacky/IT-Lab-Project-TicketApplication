using Microsoft.EntityFrameworkCore.Migrations;

namespace Ticket.Repository.Migrations
{
    public partial class addedGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Genre",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Tickets");
        }
    }
}
