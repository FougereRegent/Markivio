using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoriteRemoveAndReadingColumnOnArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isFavorite",
                table: "articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isRemoved",
                table: "articles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "reading",
                table: "articles",
                type: "text",
                nullable: false,
                defaultValue: "");

			migrationBuilder.Sql("""
					UPDATE public.articles SET reading = 'New'
					""");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isFavorite",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "isRemoved",
                table: "articles");

            migrationBuilder.DropColumn(
                name: "reading",
                table: "articles");
        }
    }
}
