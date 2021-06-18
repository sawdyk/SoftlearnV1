using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class QuestionTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuestionTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    QuestionTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "QuestionTypeName" },
                values: new object[] { 1L, "Multiple Choice" });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "QuestionTypeName" },
                values: new object[] { 2L, "Fill in the Gap" });

            migrationBuilder.InsertData(
                table: "QuestionTypes",
                columns: new[] { "Id", "QuestionTypeName" },
                values: new object[] { 3L, "True or False" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionTypes");
        }
    }
}
