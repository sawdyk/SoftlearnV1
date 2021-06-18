using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class FacilitatorsEarnings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "FacilitatorAccountDetails",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CourseEnrollees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "CourseEnrollees",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "FacilitatorsEarningsPerCourse",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    Amount = table.Column<long>(nullable: false),
                    Percentage = table.Column<decimal>(nullable: false),
                    AmountEarned = table.Column<decimal>(nullable: false),
                    DateEarned = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorsEarningsPerCourse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilitatorsEarningsPerCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilitatorsEarningsPerCourse_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilitatorsTotalEarnings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    TotalAmountEarned = table.Column<decimal>(nullable: false),
                    IsSettled = table.Column<bool>(nullable: false),
                    DateEarned = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false),
                    DateSettled = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorsTotalEarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilitatorsTotalEarnings_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PercentageEarnedOnCourses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    Percentage = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentageEarnedOnCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PercentageEarnedOnCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PercentageEarnedOnCourses_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorsEarningsPerCourse_CourseId",
                table: "FacilitatorsEarningsPerCourse",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorsEarningsPerCourse_FacilitatorId",
                table: "FacilitatorsEarningsPerCourse",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorsTotalEarnings_FacilitatorId",
                table: "FacilitatorsTotalEarnings",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_PercentageEarnedOnCourses_CourseId",
                table: "PercentageEarnedOnCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_PercentageEarnedOnCourses_FacilitatorId",
                table: "PercentageEarnedOnCourses",
                column: "FacilitatorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FacilitatorsEarningsPerCourse");

            migrationBuilder.DropTable(
                name: "FacilitatorsTotalEarnings");

            migrationBuilder.DropTable(
                name: "PercentageEarnedOnCourses");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CourseEnrollees");

            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "CourseEnrollees");

            migrationBuilder.AlterColumn<long>(
                name: "AccountNumber",
                table: "FacilitatorAccountDetails",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
