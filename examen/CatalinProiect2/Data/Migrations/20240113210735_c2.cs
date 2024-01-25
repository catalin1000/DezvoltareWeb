using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CatalinProiect2.Data.Migrations
{
    public partial class c2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_AspNetUsers_UserId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Categories_CategoryId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UserId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "DrinkBookmarks");

            migrationBuilder.DropTable(
                name: "Bookmarks");

            migrationBuilder.DropIndex(
                name: "IX_Drinks_UserId",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Drinks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Reviews",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                newName: "IX_Reviews_ApplicationUserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Reviews",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drinks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Drinks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Drinks",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Drinks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Drinks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCart = table.Column<bool>(type: "bit", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DrinkOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkOrders", x => new { x.Id, x.DrinkId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_DrinkOrders_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drinks_ApplicationUserId",
                table: "Drinks",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkOrders_DrinkId",
                table: "DrinkOrders",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkOrders_OrderId",
                table: "DrinkOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_AspNetUsers_ApplicationUserId",
                table: "Drinks",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Drinks_Categories_CategoryId",
                table: "Drinks",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_AspNetUsers_ApplicationUserId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Drinks_Categories_CategoryId",
                table: "Drinks");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.DropTable(
                name: "DrinkOrders");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Drinks_ApplicationUserId",
                table: "Drinks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Drinks");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "Reviews",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ApplicationUserId",
                table: "Reviews",
                newName: "IX_Reviews_UserId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Drinks",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Drinks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Drinks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Drinks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Drinks",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Bookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookmarks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookmarks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DrinkBookmarks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DrinkId = table.Column<int>(type: "int", nullable: false),
                    BookmarkId = table.Column<int>(type: "int", nullable: false),
                    BookmarkDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkBookmarks", x => new { x.Id, x.DrinkId, x.BookmarkId });
                    table.ForeignKey(
                        name: "FK_DrinkBookmarks_Bookmarks_BookmarkId",
                        column: x => x.BookmarkId,
                        principalTable: "Bookmarks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrinkBookmarks_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drinks_UserId",
                table: "Drinks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookmarks_UserId",
                table: "Bookmarks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkBookmarks_BookmarkId",
                table: "DrinkBookmarks",
                column: "BookmarkId");

            migrationBuilder.CreateIndex(
                name: "IX_DrinkBookmarks_DrinkId",
                table: "DrinkBookmarks",
                column: "DrinkId");

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
        }
    }
}
