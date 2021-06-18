using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class FacilitatorsEarnings2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DefaultPercentageEarningsPerCourse",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Percentage = table.Column<long>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefaultPercentageEarningsPerCourse", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "DefaultPercentageEarningsPerCourse",
                columns: new[] { "Id", "LastDateUpdated", "Percentage" },
                values: new object[] { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 10L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefaultPercentageEarningsPerCourse");
        }
    }
}
