using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTagJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "articleContent",
                table: "articles");

            migrationBuilder.AddColumn<string>(
                name: "articleContent_Content",
                table: "articles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "articleContent_Description",
                table: "articles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "articleContent_Source",
                table: "articles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "articleTag",
                columns: table => new
                {
                    articleId = table.Column<Guid>(type: "uuid", nullable: false),
                    tagsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pK_articleTag", x => new { x.articleId, x.tagsId });
                    table.ForeignKey(
                        name: "fK_articleTag_articles_articleId",
                        column: x => x.articleId,
                        principalTable: "articles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fK_articleTag_tag_tagsId",
                        column: x => x.tagsId,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "iX_articleTag_tagsId",
                table: "articleTag",
                column: "tagsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articleTag");

            migrationBuilder.DropColumn(
                name: "articleContent_Content",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "articleContent_Description",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "articleContent_Source",
                table: "articles");

            migrationBuilder.AddColumn<string>(
                name: "articleContent",
                table: "articles",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }
    }
}
