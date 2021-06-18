using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ScoresMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClassId",
                table: "SubjectDepartment",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ScoreCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCategory", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ScoreCategory",
                columns: new[] { "Id", "CategoryName", "DateCreated" },
                values: new object[,]
                {
                    { 1L, "Exam", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2L, "CA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3L, "Behavioural", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, "Extra Curricular", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDepartment_ClassId",
                table: "SubjectDepartment",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectDepartment_Classes_ClassId",
                table: "SubjectDepartment",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectDepartment_Classes_ClassId",
                table: "SubjectDepartment");

            migrationBuilder.DropTable(
                name: "ScoreCategory");

            migrationBuilder.DropIndex(
                name: "IX_SubjectDepartment_ClassId",
                table: "SubjectDepartment");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "SubjectDepartment");
        }
    }
}
