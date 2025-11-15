using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedGoogleUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "Secret");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Email");

            migrationBuilder.AddColumn<int>(
                name: "UserLoginType",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserLoginType",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Secret",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "Password");
        }
    }
}
