using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class InitPropertiesAndRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "User",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "User",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Tag",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tag",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Tag",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Folder",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Folder",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Article",
                type: "text",
                maxLength: -1,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "FolderId",
                table: "Article",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Article",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Article",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_UserId",
                table: "Tag",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Folder_UserId",
                table: "Folder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Article_FolderId",
                table: "Article",
                column: "FolderId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_User_Article_Id",
                table: "User",
                column: "Id",
                principalTable: "Article",
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
                name: "FK_Folder_User_UserId",
                table: "Folder");

            migrationBuilder.DropForeignKey(
                name: "FK_Tag_User_UserId",
                table: "Tag");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Article_Id",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_Tag_UserId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Folder_UserId",
                table: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Article_FolderId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Folder");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Folder");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Article");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Article");
        }
    }
}
