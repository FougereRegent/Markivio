using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlurialsOnTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_article_folder_folderId",
                table: "article");

            migrationBuilder.DropForeignKey(
                name: "fK_article_user_userId",
                table: "article");

            migrationBuilder.DropForeignKey(
                name: "fK_folder_user_userId",
                table: "folder");

            migrationBuilder.DropForeignKey(
                name: "fK_tag_user_userId",
                table: "tag");

            migrationBuilder.DropPrimaryKey(
                name: "pK_user",
                table: "user");

            migrationBuilder.DropPrimaryKey(
                name: "pK_tag",
                table: "tag");

            migrationBuilder.DropPrimaryKey(
                name: "pK_folder",
                table: "folder");

            migrationBuilder.DropPrimaryKey(
                name: "pK_article",
                table: "article");

            migrationBuilder.RenameTable(
                name: "user",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "tag",
                newName: "tags");

            migrationBuilder.RenameTable(
                name: "folder",
                newName: "folders");

            migrationBuilder.RenameTable(
                name: "article",
                newName: "articles");

            migrationBuilder.RenameIndex(
                name: "iX_user_authId",
                table: "users",
                newName: "iX_users_authId");

            migrationBuilder.RenameIndex(
                name: "iX_tag_userId",
                table: "tags",
                newName: "iX_tags_userId");

            migrationBuilder.RenameIndex(
                name: "iX_folder_userId",
                table: "folders",
                newName: "iX_folders_userId");

            migrationBuilder.RenameIndex(
                name: "iX_article_userId",
                table: "articles",
                newName: "iX_articles_userId");

            migrationBuilder.RenameIndex(
                name: "iX_article_folderId",
                table: "articles",
                newName: "iX_articles_folderId");

            migrationBuilder.AddPrimaryKey(
                name: "pK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_tags",
                table: "tags",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_folders",
                table: "folders",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_articles",
                table: "articles",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_articles_folder_folderId",
                table: "articles",
                column: "folderId",
                principalTable: "folders",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_articles_user_userId",
                table: "articles",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_folders_user_userId",
                table: "folders",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_tags_users_userId",
                table: "tags",
                column: "userId",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fK_articles_folder_folderId",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "fK_articles_user_userId",
                table: "articles");

            migrationBuilder.DropForeignKey(
                name: "fK_folders_user_userId",
                table: "folders");

            migrationBuilder.DropForeignKey(
                name: "fK_tags_users_userId",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "pK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pK_tags",
                table: "tags");

            migrationBuilder.DropPrimaryKey(
                name: "pK_folders",
                table: "folders");

            migrationBuilder.DropPrimaryKey(
                name: "pK_articles",
                table: "articles");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "user");

            migrationBuilder.RenameTable(
                name: "tags",
                newName: "tag");

            migrationBuilder.RenameTable(
                name: "folders",
                newName: "folder");

            migrationBuilder.RenameTable(
                name: "articles",
                newName: "article");

            migrationBuilder.RenameIndex(
                name: "iX_users_authId",
                table: "user",
                newName: "iX_user_authId");

            migrationBuilder.RenameIndex(
                name: "iX_tags_userId",
                table: "tag",
                newName: "iX_tag_userId");

            migrationBuilder.RenameIndex(
                name: "iX_folders_userId",
                table: "folder",
                newName: "iX_folder_userId");

            migrationBuilder.RenameIndex(
                name: "iX_articles_userId",
                table: "article",
                newName: "iX_article_userId");

            migrationBuilder.RenameIndex(
                name: "iX_articles_folderId",
                table: "article",
                newName: "iX_article_folderId");

            migrationBuilder.AddPrimaryKey(
                name: "pK_user",
                table: "user",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_tag",
                table: "tag",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_folder",
                table: "folder",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pK_article",
                table: "article",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_article_folder_folderId",
                table: "article",
                column: "folderId",
                principalTable: "folder",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fK_article_user_userId",
                table: "article",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_folder_user_userId",
                table: "folder",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fK_tag_user_userId",
                table: "tag",
                column: "userId",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
