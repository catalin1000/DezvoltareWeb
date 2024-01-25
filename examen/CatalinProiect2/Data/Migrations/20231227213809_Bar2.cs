using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalinProiect2.Data.Migrations
{
    public partial class Bar2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleBookmarks_Articles_ArticleId",
                table: "ArticleBookmarks");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "ArticleBookmarks",
                newName: "DrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleBookmarks_ArticleId",
                table: "ArticleBookmarks",
                newName: "IX_ArticleBookmarks_DrinkId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleBookmarks_Articles_DrinkId",
                table: "ArticleBookmarks",
                column: "DrinkId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleBookmarks_Articles_DrinkId",
                table: "ArticleBookmarks");

            migrationBuilder.RenameColumn(
                name: "DrinkId",
                table: "ArticleBookmarks",
                newName: "ArticleId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleBookmarks_DrinkId",
                table: "ArticleBookmarks",
                newName: "IX_ArticleBookmarks_ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleBookmarks_Articles_ArticleId",
                table: "ArticleBookmarks",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
