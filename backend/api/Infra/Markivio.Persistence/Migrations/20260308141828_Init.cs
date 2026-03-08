using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    username = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    firstName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    lastName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    authId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "folder",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_folder", x => x.id);
                    table.ForeignKey(
                        name: "fK_folder_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    color = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_tag", x => x.id);
                    table.ForeignKey(
                        name: "fK_tag_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "article",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    userId = table.Column<Guid>(type: "uuid", nullable: false),
                    folderId = table.Column<Guid>(type: "uuid", nullable: true),
                    articleContent = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_article", x => x.id);
                    table.ForeignKey(
                        name: "fK_article_folder_folderId",
                        column: x => x.folderId,
                        principalTable: "folder",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fK_article_user_userId",
                        column: x => x.userId,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_article_folderId",
                table: "article",
                column: "folderId");

            migrationBuilder.CreateIndex(
                name: "iX_article_userId",
                table: "article",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_folder_userId",
                table: "folder",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_tag_name_userId",
                table: "tag",
                columns: new[] { "name", "userId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "iX_tag_userId",
                table: "tag",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "iX_user_authId",
                table: "user",
                column: "authId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "article");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "folder");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
