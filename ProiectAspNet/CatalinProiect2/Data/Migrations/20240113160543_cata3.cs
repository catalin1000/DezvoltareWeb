using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalinProiect2.Data.Migrations
{
    public partial class cata3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Drinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Drinks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Drinks");
        }
    }
}
