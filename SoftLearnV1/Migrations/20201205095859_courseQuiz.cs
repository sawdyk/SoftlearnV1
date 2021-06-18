using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class courseQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvalidQuestions",
                table: "CourseQuizResults",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "CourseQuizQuestions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CourseQuizQuestions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CourseQuiz",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CourseQuiz",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvalidQuestions",
                table: "CourseQuizResults");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "CourseQuizQuestions");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CourseQuizQuestions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CourseQuiz");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CourseQuiz");
        }
    }
}
