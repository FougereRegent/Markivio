using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Markivio.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeFormat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "username",
                table: "user",
                newName: "identity_Username");

            migrationBuilder.RenameColumn(
                name: "lastName",
                table: "user",
                newName: "identity_LastName");

            migrationBuilder.RenameColumn(
                name: "firstName",
                table: "user",
                newName: "identity_FirstName");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "user",
                newName: "email_Email");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "tag",
                newName: "tagValue_Name");

            migrationBuilder.RenameColumn(
                name: "color",
                table: "tag",
                newName: "tagValue_Color");

            migrationBuilder.AlterColumn<string>(
                name: "email_Email",
                table: "user",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "identity_Username",
                table: "user",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "identity_LastName",
                table: "user",
                newName: "lastName");

            migrationBuilder.RenameColumn(
                name: "identity_FirstName",
                table: "user",
                newName: "firstName");

            migrationBuilder.RenameColumn(
                name: "email_Email",
                table: "user",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "tagValue_Name",
                table: "tag",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "tagValue_Color",
                table: "tag",
                newName: "color");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "user",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
