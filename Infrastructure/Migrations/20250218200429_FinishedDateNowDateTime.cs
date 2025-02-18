using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinishedDateNowDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "AnsweredQuestion");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedDate",
                table: "AnsweredQuestion",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedDate",
                table: "AnsweredQuestion");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "AnsweredQuestion",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
