using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardConfig33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReferenceRange",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "ReferenceValue",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.CreateTable(
                name: "ReportCardConfigurationLegendList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LegendId = table.Column<long>(nullable: false),
                    ReferenceRange = table.Column<string>(nullable: true),
                    ReferenceValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardConfigurationLegendList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationLegendList_ReportCardConfigurationLeg~",
                        column: x => x.LegendId,
                        principalTable: "ReportCardConfigurationLegend",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationLegendList_LegendId",
                table: "ReportCardConfigurationLegendList",
                column: "LegendId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCardConfigurationLegendList");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceRange",
                table: "ReportCardConfigurationLegend",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceValue",
                table: "ReportCardConfigurationLegend",
                nullable: true);
        }
    }
}
