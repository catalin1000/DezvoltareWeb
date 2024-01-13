using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalinProiect2.Data.Migrations
{
    public partial class cata1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Drinks",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Drinks",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Drinks");
        }
    }
}
