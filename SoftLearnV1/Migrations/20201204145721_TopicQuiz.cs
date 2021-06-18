using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class TopicQuiz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "CourseTopicQuizQuestions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CourseTopicQuizQuestions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "CourseTopicQuiz",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "CourseTopicQuiz",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "CourseTopicQuizQuestions");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CourseTopicQuizQuestions");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "CourseTopicQuiz");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "CourseTopicQuiz");
        }
    }
}
