using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class FeeMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SchoolLogoUrl",
                table: "SchoolInformation",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FacilitatorTypeId",
                table: "Facilitators",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "FacilitatorType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeeCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    CategoryName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeCategory_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeCategory_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeTemplate",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TemplateName = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTemplate", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeTemplate_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplate_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplate_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplate_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplate_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceTotal",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceCode = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    InvoiceSubTotal = table.Column<long>(nullable: false),
                    DateGenerated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceTotal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceTotal_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolFeesPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceCode = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    InvoiceTotal = table.Column<long>(nullable: false),
                    AmountPaid = table.Column<long>(nullable: false),
                    Balance = table.Column<long>(nullable: false),
                    IsPaymentCompleted = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolFeesPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolFeesPayments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MethodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSubTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolTypeId = table.Column<long>(nullable: false),
                    SubTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSubTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSubTypes_SchoolType_SchoolTypeId",
                        column: x => x.SchoolTypeId,
                        principalTable: "SchoolType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolTemporaryPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceCode = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    PaymentMethodId = table.Column<long>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    DepositorsAccountName = table.Column<string>(nullable: true),
                    ReferenceCode = table.Column<string>(nullable: true),
                    CardType = table.Column<string>(nullable: true),
                    AmountPaid = table.Column<long>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolTemporaryPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolTemporaryPayments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeSubCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    FeeCategoryId = table.Column<long>(nullable: false),
                    SubCategoryName = table.Column<string>(nullable: true),
                    FeeCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    FeeAmount = table.Column<long>(nullable: false),
                    IsMandatory = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeSubCategory_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeSubCategory_FeeCategory_FeeCategoryId",
                        column: x => x.FeeCategoryId,
                        principalTable: "FeeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeSubCategory_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReportCardTemplates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    SchoolSubTypeId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardTemplates_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardTemplates_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardTemplates_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardTemplates_SchoolSubTypes_SchoolSubTypeId",
                        column: x => x.SchoolSubTypeId,
                        principalTable: "SchoolSubTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeTemplateList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    FeeCategoryId = table.Column<long>(nullable: false),
                    FeeSubCategoryId = table.Column<long>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeTemplateList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeTemplateList_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplateList_FeeCategory_FeeCategoryId",
                        column: x => x.FeeCategoryId,
                        principalTable: "FeeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplateList_FeeSubCategory_FeeSubCategoryId",
                        column: x => x.FeeSubCategoryId,
                        principalTable: "FeeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplateList_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FeeTemplateList_FeeTemplate_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "FeeTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceList",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceCode = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    FeeSubCategoryId = table.Column<long>(nullable: false),
                    DateGenerated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceList_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_FeeSubCategory_FeeSubCategoryId",
                        column: x => x.FeeSubCategoryId,
                        principalTable: "FeeSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceList_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FacilitatorType",
                columns: new[] { "Id", "FacilitatorTypeName" },
                values: new object[,]
                {
                    { 1L, "External" },
                    { 2L, "Internal" }
                });

            migrationBuilder.InsertData(
                table: "SchoolPaymentMethods",
                columns: new[] { "Id", "MethodName" },
                values: new object[,]
                {
                    { 1L, "Bank Deposit" },
                    { 2L, "Online Transfer" },
                    { 3L, "Card Payment" }
                });

            migrationBuilder.InsertData(
                table: "SchoolSubTypes",
                columns: new[] { "Id", "SchoolTypeId", "SubTypeName" },
                values: new object[,]
                {
                    { 1L, 3L, "Junior" },
                    { 2L, 3L, "Senior" },
                    { 3L, 2L, "Primary" },
                    { 4L, 1L, "Nursery" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Facilitators_FacilitatorTypeId",
                table: "Facilitators",
                column: "FacilitatorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCategory_CampusId",
                table: "FeeCategory",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeCategory_SchoolId",
                table: "FeeCategory",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSubCategory_CampusId",
                table: "FeeSubCategory",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSubCategory_FeeCategoryId",
                table: "FeeSubCategory",
                column: "FeeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeSubCategory_SchoolId",
                table: "FeeSubCategory",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_CampusId",
                table: "FeeTemplate",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_ClassId",
                table: "FeeTemplate",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_SchoolId",
                table: "FeeTemplate",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_SessionId",
                table: "FeeTemplate",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplate_TermId",
                table: "FeeTemplate",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplateList_CampusId",
                table: "FeeTemplateList",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplateList_FeeCategoryId",
                table: "FeeTemplateList",
                column: "FeeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplateList_FeeSubCategoryId",
                table: "FeeTemplateList",
                column: "FeeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplateList_SchoolId",
                table: "FeeTemplateList",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeTemplateList_TemplateId",
                table: "FeeTemplateList",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_CampusId",
                table: "InvoiceList",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_ClassGradeId",
                table: "InvoiceList",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_ClassId",
                table: "InvoiceList",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_FeeSubCategoryId",
                table: "InvoiceList",
                column: "FeeSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_ParentId",
                table: "InvoiceList",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_SchoolId",
                table: "InvoiceList",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_SessionId",
                table: "InvoiceList",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_StudentId",
                table: "InvoiceList",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceList_TermId",
                table: "InvoiceList",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_CampusId",
                table: "InvoiceTotal",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_ClassGradeId",
                table: "InvoiceTotal",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_ClassId",
                table: "InvoiceTotal",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_ParentId",
                table: "InvoiceTotal",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_SchoolId",
                table: "InvoiceTotal",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_SessionId",
                table: "InvoiceTotal",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_StudentId",
                table: "InvoiceTotal",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceTotal_TermId",
                table: "InvoiceTotal",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardTemplates_CampusId",
                table: "ReportCardTemplates",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardTemplates_ClassId",
                table: "ReportCardTemplates",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardTemplates_SchoolId",
                table: "ReportCardTemplates",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardTemplates_SchoolSubTypeId",
                table: "ReportCardTemplates",
                column: "SchoolSubTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_CampusId",
                table: "SchoolFeesPayments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_ClassGradeId",
                table: "SchoolFeesPayments",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_ClassId",
                table: "SchoolFeesPayments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_ParentId",
                table: "SchoolFeesPayments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_SchoolId",
                table: "SchoolFeesPayments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_SessionId",
                table: "SchoolFeesPayments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_StudentId",
                table: "SchoolFeesPayments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolFeesPayments_TermId",
                table: "SchoolFeesPayments",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubTypes_SchoolTypeId",
                table: "SchoolSubTypes",
                column: "SchoolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_CampusId",
                table: "SchoolTemporaryPayments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_ClassGradeId",
                table: "SchoolTemporaryPayments",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_ClassId",
                table: "SchoolTemporaryPayments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_ParentId",
                table: "SchoolTemporaryPayments",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_PaymentMethodId",
                table: "SchoolTemporaryPayments",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_SchoolId",
                table: "SchoolTemporaryPayments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_SessionId",
                table: "SchoolTemporaryPayments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_StudentId",
                table: "SchoolTemporaryPayments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolTemporaryPayments_TermId",
                table: "SchoolTemporaryPayments",
                column: "TermId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilitators_FacilitatorType_FacilitatorTypeId",
                table: "Facilitators",
                column: "FacilitatorTypeId",
                principalTable: "FacilitatorType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilitators_FacilitatorType_FacilitatorTypeId",
                table: "Facilitators");

            migrationBuilder.DropTable(
                name: "FacilitatorType");

            migrationBuilder.DropTable(
                name: "FeeTemplateList");

            migrationBuilder.DropTable(
                name: "InvoiceList");

            migrationBuilder.DropTable(
                name: "InvoiceTotal");

            migrationBuilder.DropTable(
                name: "ReportCardTemplates");

            migrationBuilder.DropTable(
                name: "SchoolFeesPayments");

            migrationBuilder.DropTable(
                name: "SchoolPaymentMethods");

            migrationBuilder.DropTable(
                name: "SchoolTemporaryPayments");

            migrationBuilder.DropTable(
                name: "FeeTemplate");

            migrationBuilder.DropTable(
                name: "FeeSubCategory");

            migrationBuilder.DropTable(
                name: "SchoolSubTypes");

            migrationBuilder.DropTable(
                name: "FeeCategory");

            migrationBuilder.DropIndex(
                name: "IX_Facilitators_FacilitatorTypeId",
                table: "Facilitators");

            migrationBuilder.DeleteData(
                table: "SystemUserRoles",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "SchoolLogoUrl",
                table: "SchoolInformation");

            migrationBuilder.DropColumn(
                name: "FacilitatorTypeId",
                table: "Facilitators");
        }
    }
}
