using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalinProiect2.Data.Migrations
{
    public partial class cata4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Drinks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Drinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
