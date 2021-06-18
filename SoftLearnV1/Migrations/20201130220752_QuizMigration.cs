using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class QuizMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PassMark",
                table: "CourseQuiz",
                newName: "PercentagePassMark");

            migrationBuilder.CreateTable(
                name: "CourseQuizQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseQuizId = table.Column<long>(nullable: false),
                    QuestionTypeId = table.Column<long>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Option1 = table.Column<string>(nullable: true),
                    Option2 = table.Column<string>(nullable: true),
                    Option3 = table.Column<string>(nullable: true),
                    Option4 = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuizQuestions_CourseQuiz_CourseQuizId",
                        column: x => x.CourseQuizId,
                        principalTable: "CourseQuiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuizQuestions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseQuizResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseQuizId = table.Column<long>(nullable: false),
                    LearnerId = table.Column<Guid>(nullable: false),
                    NoOfQuestions = table.Column<long>(nullable: false),
                    RightAnswers = table.Column<long>(nullable: false),
                    WrongAnswers = table.Column<long>(nullable: false),
                    Score = table.Column<long>(nullable: false),
                    PercentageScore = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    DateTaken = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseQuizResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseQuizResults_CourseQuiz_CourseQuizId",
                        column: x => x.CourseQuizId,
                        principalTable: "CourseQuiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseQuizResults_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicQuiz",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<long>(nullable: false),
                    CourseTopicId = table.Column<long>(nullable: false),
                    Duration = table.Column<long>(nullable: false),
                    PercentagePassMark = table.Column<long>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicQuiz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicQuiz_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicQuiz_CourseTopics_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicQuizQuestions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseTopicQuizId = table.Column<long>(nullable: false),
                    QuestionTypeId = table.Column<long>(nullable: false),
                    Question = table.Column<string>(nullable: true),
                    Option1 = table.Column<string>(nullable: true),
                    Option2 = table.Column<string>(nullable: true),
                    Option3 = table.Column<string>(nullable: true),
                    Option4 = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicQuizQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicQuizQuestions_CourseTopicQuiz_CourseTopicQuizId",
                        column: x => x.CourseTopicQuizId,
                        principalTable: "CourseTopicQuiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicQuizQuestions_QuestionTypes_QuestionTypeId",
                        column: x => x.QuestionTypeId,
                        principalTable: "QuestionTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicQuizResults",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseTopicQuizId = table.Column<long>(nullable: false),
                    LearnerId = table.Column<Guid>(nullable: false),
                    NoOfQuestions = table.Column<long>(nullable: false),
                    RightAnswers = table.Column<long>(nullable: false),
                    WrongAnswers = table.Column<long>(nullable: false),
                    Score = table.Column<long>(nullable: false),
                    PercentageScore = table.Column<decimal>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    DateTaken = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicQuizResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicQuizResults_CourseTopicQuiz_CourseTopicQuizId",
                        column: x => x.CourseTopicQuizId,
                        principalTable: "CourseTopicQuiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicQuizResults_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuizQuestions_CourseQuizId",
                table: "CourseQuizQuestions",
                column: "CourseQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuizQuestions_QuestionTypeId",
                table: "CourseQuizQuestions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuizResults_CourseQuizId",
                table: "CourseQuizResults",
                column: "CourseQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseQuizResults_LearnerId",
                table: "CourseQuizResults",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicQuiz_CourseId",
                table: "CourseTopicQuiz",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicQuiz_CourseTopicId",
                table: "CourseTopicQuiz",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicQuizQuestions_CourseTopicQuizId",
                table: "CourseTopicQuizQuestions",
                column: "CourseTopicQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicQuizQuestions_QuestionTypeId",
                table: "CourseTopicQuizQuestions",
                column: "QuestionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicQuizResults_CourseTopicQuizId",
                table: "CourseTopicQuizResults",
                column: "CourseTopicQuizId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicQuizResults_LearnerId",
                table: "CourseTopicQuizResults",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseQuizQuestions");

            migrationBuilder.DropTable(
                name: "CourseQuizResults");

            migrationBuilder.DropTable(
                name: "CourseTopicQuizQuestions");

            migrationBuilder.DropTable(
                name: "CourseTopicQuizResults");

            migrationBuilder.DropTable(
                name: "CourseTopicQuiz");

            migrationBuilder.RenameColumn(
                name: "PercentagePassMark",
                table: "CourseQuiz",
                newName: "PassMark");
        }
    }
}
