using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class ReportCardClassConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportCardConfigurationForClasses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ConfigurationId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardConfigurationForClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationForClasses_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationForClasses_ReportCardConfiguration_Co~",
                        column: x => x.ConfigurationId,
                        principalTable: "ReportCardConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationForClasses_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardConfigurationForClasses_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationForClasses_ClassId",
                table: "ReportCardConfigurationForClasses",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationForClasses_ConfigurationId",
                table: "ReportCardConfigurationForClasses",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationForClasses_SchoolId",
                table: "ReportCardConfigurationForClasses",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardConfigurationForClasses_TermId",
                table: "ReportCardConfigurationForClasses",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCardConfigurationForClasses");
        }
    }
}
