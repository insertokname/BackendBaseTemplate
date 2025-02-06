using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAnswersToQuestionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Answers_Answers",
                table: "Questions",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "Answers_CorrectAnswerIndex",
                table: "Questions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers_Answers",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Answers_CorrectAnswerIndex",
                table: "Questions");
        }
    }
}
