using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class updateDBMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BehavioralScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "BehavioralScores");

            migrationBuilder.DropForeignKey(
                name: "FK_BehavioralScores_ScoreSubCategoryConfig_ScoreSubCategoryId",
                table: "BehavioralScores");

            migrationBuilder.DropForeignKey(
                name: "FK_BehavioralScores_SchoolUsers_UploadedById",
                table: "BehavioralScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreSubCategoryConfig_ScoreSubCat~",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ContinousAssessmentScores_SchoolUsers_UploadedById",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "ExaminationScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationScores_ScoreSubCategoryConfig_ScoreSubCategoryId",
                table: "ExaminationScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationScores_SchoolUsers_UploadedById",
                table: "ExaminationScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraCurricularScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "ExtraCurricularScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraCurricularScores_ScoreSubCategoryConfig_ScoreSubCategor~",
                table: "ExtraCurricularScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraCurricularScores_SchoolUsers_UploadedById",
                table: "ExtraCurricularScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_ScoreCategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropTable(
                name: "ScoreCategoryConfig");

            migrationBuilder.DropIndex(
                name: "IX_ScoreSubCategoryConfig_ScoreCategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DeleteData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SystemUsers",
                keyColumn: "Id",
                keyValue: new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"));

            migrationBuilder.DropColumn(
                name: "ScoreCategoryId",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.RenameColumn(
                name: "UploadedById",
                table: "ExtraCurricularScores",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "ScoreSubCategoryId",
                table: "ExtraCurricularScores",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "ScoreCategoryId",
                table: "ExtraCurricularScores",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraCurricularScores_UploadedById",
                table: "ExtraCurricularScores",
                newName: "IX_ExtraCurricularScores_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraCurricularScores_ScoreSubCategoryId",
                table: "ExtraCurricularScores",
                newName: "IX_ExtraCurricularScores_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraCurricularScores_ScoreCategoryId",
                table: "ExtraCurricularScores",
                newName: "IX_ExtraCurricularScores_CategoryId");

            migrationBuilder.RenameColumn(
                name: "UploadedById",
                table: "ExaminationScores",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "ScoreSubCategoryId",
                table: "ExaminationScores",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "ScoreCategoryId",
                table: "ExaminationScores",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationScores_UploadedById",
                table: "ExaminationScores",
                newName: "IX_ExaminationScores_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationScores_ScoreSubCategoryId",
                table: "ExaminationScores",
                newName: "IX_ExaminationScores_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationScores_ScoreCategoryId",
                table: "ExaminationScores",
                newName: "IX_ExaminationScores_CategoryId");

            migrationBuilder.RenameColumn(
                name: "UploadedById",
                table: "ContinousAssessmentScores",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "ScoreSubCategoryId",
                table: "ContinousAssessmentScores",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "ScoreCategoryId",
                table: "ContinousAssessmentScores",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ContinousAssessmentScores_UploadedById",
                table: "ContinousAssessmentScores",
                newName: "IX_ContinousAssessmentScores_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_ContinousAssessmentScores_ScoreSubCategoryId",
                table: "ContinousAssessmentScores",
                newName: "IX_ContinousAssessmentScores_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ContinousAssessmentScores_ScoreCategoryId",
                table: "ContinousAssessmentScores",
                newName: "IX_ContinousAssessmentScores_CategoryId");

            migrationBuilder.RenameColumn(
                name: "UploadedById",
                table: "BehavioralScores",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "ScoreSubCategoryId",
                table: "BehavioralScores",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "ScoreCategoryId",
                table: "BehavioralScores",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BehavioralScores_UploadedById",
                table: "BehavioralScores",
                newName: "IX_BehavioralScores_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_BehavioralScores_ScoreSubCategoryId",
                table: "BehavioralScores",
                newName: "IX_BehavioralScores_SubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BehavioralScores_ScoreCategoryId",
                table: "BehavioralScores",
                newName: "IX_BehavioralScores_CategoryId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ExtraCurricularScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ExaminationScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "ContinousAssessmentScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "BehavioralScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_BehavioralScores_ScoreCategory_CategoryId",
                table: "BehavioralScores",
                column: "CategoryId",
                principalTable: "ScoreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehavioralScores_ScoreSubCategoryConfig_SubCategoryId",
                table: "BehavioralScores",
                column: "SubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehavioralScores_SchoolUsers_TeacherId",
                table: "BehavioralScores",
                column: "TeacherId",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreCategory_CategoryId",
                table: "ContinousAssessmentScores",
                column: "CategoryId",
                principalTable: "ScoreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreSubCategoryConfig_SubCategory~",
                table: "ContinousAssessmentScores",
                column: "SubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContinousAssessmentScores_SchoolUsers_TeacherId",
                table: "ContinousAssessmentScores",
                column: "TeacherId",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationScores_ScoreCategory_CategoryId",
                table: "ExaminationScores",
                column: "CategoryId",
                principalTable: "ScoreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationScores_ScoreSubCategoryConfig_SubCategoryId",
                table: "ExaminationScores",
                column: "SubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationScores_SchoolUsers_TeacherId",
                table: "ExaminationScores",
                column: "TeacherId",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraCurricularScores_ScoreCategory_CategoryId",
                table: "ExtraCurricularScores",
                column: "CategoryId",
                principalTable: "ScoreCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraCurricularScores_ScoreSubCategoryConfig_SubCategoryId",
                table: "ExtraCurricularScores",
                column: "SubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraCurricularScores_SchoolUsers_TeacherId",
                table: "ExtraCurricularScores",
                column: "TeacherId",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BehavioralScores_ScoreCategory_CategoryId",
                table: "BehavioralScores");

            migrationBuilder.DropForeignKey(
                name: "FK_BehavioralScores_ScoreSubCategoryConfig_SubCategoryId",
                table: "BehavioralScores");

            migrationBuilder.DropForeignKey(
                name: "FK_BehavioralScores_SchoolUsers_TeacherId",
                table: "BehavioralScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreCategory_CategoryId",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreSubCategoryConfig_SubCategory~",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ContinousAssessmentScores_SchoolUsers_TeacherId",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationScores_ScoreCategory_CategoryId",
                table: "ExaminationScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationScores_ScoreSubCategoryConfig_SubCategoryId",
                table: "ExaminationScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationScores_SchoolUsers_TeacherId",
                table: "ExaminationScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraCurricularScores_ScoreCategory_CategoryId",
                table: "ExtraCurricularScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraCurricularScores_ScoreSubCategoryConfig_SubCategoryId",
                table: "ExtraCurricularScores");

            migrationBuilder.DropForeignKey(
                name: "FK_ExtraCurricularScores_SchoolUsers_TeacherId",
                table: "ExtraCurricularScores");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ExtraCurricularScores");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ExaminationScores");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "BehavioralScores");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ExtraCurricularScores",
                newName: "UploadedById");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "ExtraCurricularScores",
                newName: "ScoreSubCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ExtraCurricularScores",
                newName: "ScoreCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraCurricularScores_TeacherId",
                table: "ExtraCurricularScores",
                newName: "IX_ExtraCurricularScores_UploadedById");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraCurricularScores_SubCategoryId",
                table: "ExtraCurricularScores",
                newName: "IX_ExtraCurricularScores_ScoreSubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExtraCurricularScores_CategoryId",
                table: "ExtraCurricularScores",
                newName: "IX_ExtraCurricularScores_ScoreCategoryId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ExaminationScores",
                newName: "UploadedById");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "ExaminationScores",
                newName: "ScoreSubCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ExaminationScores",
                newName: "ScoreCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationScores_TeacherId",
                table: "ExaminationScores",
                newName: "IX_ExaminationScores_UploadedById");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationScores_SubCategoryId",
                table: "ExaminationScores",
                newName: "IX_ExaminationScores_ScoreSubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ExaminationScores_CategoryId",
                table: "ExaminationScores",
                newName: "IX_ExaminationScores_ScoreCategoryId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "ContinousAssessmentScores",
                newName: "UploadedById");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "ContinousAssessmentScores",
                newName: "ScoreSubCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "ContinousAssessmentScores",
                newName: "ScoreCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ContinousAssessmentScores_TeacherId",
                table: "ContinousAssessmentScores",
                newName: "IX_ContinousAssessmentScores_UploadedById");

            migrationBuilder.RenameIndex(
                name: "IX_ContinousAssessmentScores_SubCategoryId",
                table: "ContinousAssessmentScores",
                newName: "IX_ContinousAssessmentScores_ScoreSubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ContinousAssessmentScores_CategoryId",
                table: "ContinousAssessmentScores",
                newName: "IX_ContinousAssessmentScores_ScoreCategoryId");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                table: "BehavioralScores",
                newName: "UploadedById");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "BehavioralScores",
                newName: "ScoreSubCategoryId");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "BehavioralScores",
                newName: "ScoreCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BehavioralScores_TeacherId",
                table: "BehavioralScores",
                newName: "IX_BehavioralScores_UploadedById");

            migrationBuilder.RenameIndex(
                name: "IX_BehavioralScores_SubCategoryId",
                table: "BehavioralScores",
                newName: "IX_BehavioralScores_ScoreSubCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BehavioralScores_CategoryId",
                table: "BehavioralScores",
                newName: "IX_BehavioralScores_ScoreCategoryId");

            migrationBuilder.AddColumn<long>(
                name: "ScoreCategoryId",
                table: "ScoreSubCategoryConfig",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ScoreCategoryConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampusId = table.Column<long>(nullable: false),
                    CategoryId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCategoryConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_ScoreCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScoreCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SystemUsers",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "IsActive", "LastLoginDate", "LastName", "LastPasswordChangedDate", "LastUpdatedDate", "PasswordHash", "Salt", "UserName" },
                values: new object[] { new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575"), new DateTime(2020, 12, 15, 16, 25, 27, 538, DateTimeKind.Local).AddTicks(4405), "superadmin@superadmin.com", "Super", true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Admin", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 12, 15, 16, 25, 28, 48, DateTimeKind.Local).AddTicks(8719), "e54b729420b96a6362b57ee8cc9b7ab17d9df3a7b6c56fefd9231480d1e9e565", "e40ee543b2cc23e4be6b083fd55fb837", "superadmin@superadmin.com" });

            migrationBuilder.InsertData(
                table: "SystemUserRoles",
                columns: new[] { "Id", "DateCreated", "RoleId", "UserId" },
                values: new object[] { 1L, new DateTime(2020, 12, 15, 16, 25, 28, 48, DateTimeKind.Local).AddTicks(9229), 1L, new Guid("a18be9c0-aa65-4af8-bd17-00bd9344e575") });

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_ScoreCategoryId",
                table: "ScoreSubCategoryConfig",
                column: "ScoreCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_CampusId",
                table: "ScoreCategoryConfig",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_CategoryId",
                table: "ScoreCategoryConfig",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_ClassId",
                table: "ScoreCategoryConfig",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_SchoolId",
                table: "ScoreCategoryConfig",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_SessionId",
                table: "ScoreCategoryConfig",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_TermId",
                table: "ScoreCategoryConfig",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_BehavioralScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "BehavioralScores",
                column: "ScoreCategoryId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehavioralScores_ScoreSubCategoryConfig_ScoreSubCategoryId",
                table: "BehavioralScores",
                column: "ScoreSubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BehavioralScores_SchoolUsers_UploadedById",
                table: "BehavioralScores",
                column: "UploadedById",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "ContinousAssessmentScores",
                column: "ScoreCategoryId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContinousAssessmentScores_ScoreSubCategoryConfig_ScoreSubCat~",
                table: "ContinousAssessmentScores",
                column: "ScoreSubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContinousAssessmentScores_SchoolUsers_UploadedById",
                table: "ContinousAssessmentScores",
                column: "UploadedById",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "ExaminationScores",
                column: "ScoreCategoryId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationScores_ScoreSubCategoryConfig_ScoreSubCategoryId",
                table: "ExaminationScores",
                column: "ScoreSubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationScores_SchoolUsers_UploadedById",
                table: "ExaminationScores",
                column: "UploadedById",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraCurricularScores_ScoreCategoryConfig_ScoreCategoryId",
                table: "ExtraCurricularScores",
                column: "ScoreCategoryId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraCurricularScores_ScoreSubCategoryConfig_ScoreSubCategor~",
                table: "ExtraCurricularScores",
                column: "ScoreSubCategoryId",
                principalTable: "ScoreSubCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExtraCurricularScores_SchoolUsers_UploadedById",
                table: "ExtraCurricularScores",
                column: "UploadedById",
                principalTable: "SchoolUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ScoreSubCategoryConfig_ScoreCategoryConfig_ScoreCategoryId",
                table: "ScoreSubCategoryConfig",
                column: "ScoreCategoryId",
                principalTable: "ScoreCategoryConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
