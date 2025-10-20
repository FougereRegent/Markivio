using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTenancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Article_ArticleId",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Tag_TagId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_ArticleId",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_TagId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "ArticleId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Article",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                table: "Tag",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Article_UserId",
                table: "Article",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Article_User_UserId",
                table: "Article",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_User_UserId",
                table: "Tag",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_User_UserId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_User_UserId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_UserId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Article_UserId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Article");

            migrationBuilder.AddColumn<Guid>(
                name: "ArticleId",
                table: "User",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TagId",
                table: "User",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_User_ArticleId",
                table: "User",
                column: "ArticleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_TagId",
                table: "User",
                column: "TagId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Article_ArticleId",
                table: "User",
                column: "ArticleId",
                principalTable: "Article",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Tag_TagId",
                table: "User",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
