using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalinProiect2.Data.Migrations
{
    public partial class Bar3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleBookmarks_Articles_DrinkId",
                table: "ArticleBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_ArticleBookmarks_Bookmarks_BookmarkId",
                table: "ArticleBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Categories_CategoryId",
                table: "Articles");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Articles_DrinkId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Articles",
                table: "Articles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ArticleBookmarks",
                table: "ArticleBookmarks");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "Reviews");

            migrationBuilder.RenameTable(
                name: "Articles",
                newName: "Drinks");

            migrationBuilder.RenameTable(
                name: "ArticleBookmarks",
                newName: "DrinkBookmarks");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_UserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_DrinkId",
                table: "Reviews",
                newName: "IX_Reviews_DrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_UserId",
                table: "Drinks",
                newName: "IX_Drinks_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Articles_CategoryId",
                table: "Drinks",
                newName: "IX_Drinks_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleBookmarks_DrinkId",
                table: "DrinkBookmarks",
                newName: "IX_DrinkBookmarks_DrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_ArticleBookmarks_BookmarkId",
                table: "DrinkBookmarks",
                newName: "IX_DrinkBookmarks_BookmarkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Drinks",
                table: "Drinks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DrinkBookmarks",
                table: "DrinkBookmarks",
                columns: new[] { "Id", "DrinkId", "BookmarkId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkBookmarks_Bookmarks_BookmarkId",
                table: "DrinkBookmarks",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrinkBookmarks_Drinks_DrinkId",
                table: "DrinkBookmarks",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_AspNetUsers_UserId",
                table: "Drinks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Categories_CategoryId",
                table: "Drinks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Drinks_DrinkId",
                table: "Reviews",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrinkBookmarks_Bookmarks_BookmarkId",
                table: "DrinkBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_DrinkBookmarks_Drinks_DrinkId",
                table: "DrinkBookmarks");

            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_AspNetUsers_UserId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Categories_CategoryId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Drinks_DrinkId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Drinks",
                table: "Drinks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DrinkBookmarks",
                table: "DrinkBookmarks");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Comments");

            migrationBuilder.RenameTable(
                name: "Drinks",
                newName: "Articles");

            migrationBuilder.RenameTable(
                name: "DrinkBookmarks",
                newName: "ArticleBookmarks");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Comments",
                newName: "IX_Comments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_DrinkId",
                table: "Comments",
                newName: "IX_Comments_DrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_Drinks_UserId",
                table: "Articles",
                newName: "IX_Articles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Drinks_CategoryId",
                table: "Articles",
                newName: "IX_Articles_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkBookmarks_DrinkId",
                table: "ArticleBookmarks",
                newName: "IX_ArticleBookmarks_DrinkId");

            migrationBuilder.RenameIndex(
                name: "IX_DrinkBookmarks_BookmarkId",
                table: "ArticleBookmarks",
                newName: "IX_ArticleBookmarks_BookmarkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Articles",
                table: "Articles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ArticleBookmarks",
                table: "ArticleBookmarks",
                columns: new[] { "Id", "DrinkId", "BookmarkId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleBookmarks_Articles_DrinkId",
                table: "ArticleBookmarks",
                column: "DrinkId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleBookmarks_Bookmarks_BookmarkId",
                table: "ArticleBookmarks",
                column: "BookmarkId",
                principalTable: "Bookmarks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_UserId",
                table: "Articles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Categories_CategoryId",
                table: "Articles",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Articles_DrinkId",
                table: "Comments",
                column: "DrinkId",
                principalTable: "Articles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_UserId",
                table: "Comments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
