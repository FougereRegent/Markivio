using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class InitRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Folder_FolderId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_Folder_User_UserId",
                table: "Folder");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_User_UserId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_UserId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Folder_UserId",
                table: "Folder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Folder");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "User",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "User",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ArticleTag",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleTag", x => new { x.ArticleId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_ArticleTag_Article_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Article",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleTag_TagsId",
                table: "ArticleTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Folder_FolderId",
                table: "Article",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Folder_Id",
                table: "User",
                column: "Id",
                principalTable: "Folder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Tag_Id",
                table: "User",
                column: "Id",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Article_Folder_FolderId",
                table: "Article");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Folder_Id",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Tag_Id",
                table: "User");

            migrationBuilder.DropTable(
                name: "ArticleTag");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "User");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Folder",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                table: "Tag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_UserId",
                table: "Folder",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Article_Folder_FolderId",
                table: "Article",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Folder_User_UserId",
                table: "Folder",
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
    }
}
