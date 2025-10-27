using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.DbUpdater.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthFieldAndIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthId",
                table: "User",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_User_AuthId",
                table: "User",
                column: "AuthId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_AuthId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "AuthId",
                table: "User");
        }
    }
}
