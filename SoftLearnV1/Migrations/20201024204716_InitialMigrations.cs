using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoftLearnV1.Migrations
{
    public partial class InitialMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseCategoryName = table.Column<string>(nullable: true),
                    CategoryImageUrl = table.Column<string>(nullable: true),
                    CategoryDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseLevelTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LevelTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseLevelTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfirmationCodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    DateGenerated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfirmationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ErrorMessage = table.Column<string>(nullable: true),
                    ErrorSource = table.Column<string>(nullable: true),
                    ErrorStackTrace = table.Column<string>(nullable: true),
                    ErrorDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilitators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilitators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MethodName = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlans",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlanName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SuperAdministrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperAdministrator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseSubCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseCategoryId = table.Column<long>(nullable: false),
                    CourseSubCategoryName = table.Column<string>(nullable: true),
                    CourseSubCategoryImageUrl = table.Column<string>(nullable: true),
                    CourseSubCategoryDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseSubCategory_CourseCategory_CourseCategoryId",
                        column: x => x.CourseCategoryId,
                        principalTable: "CourseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Learners",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    LevelTypeId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Learners_CourseLevelTypes_LevelTypeId",
                        column: x => x.LevelTypeId,
                        principalTable: "CourseLevelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilitatorAccountDetails",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    BankName = table.Column<string>(nullable: true),
                    AccountName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorAccountDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilitatorAccountDetails_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FacilitatorOtherInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    InstitutionAttended = table.Column<string>(nullable: true),
                    CourseOfStudy = table.Column<string>(nullable: true),
                    CertificateObtained = table.Column<string>(nullable: true),
                    Profession = table.Column<string>(nullable: true),
                    Bio = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilitatorOtherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FacilitatorOtherInfo_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolInformation",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolCode = table.Column<string>(nullable: true),
                    SchoolTypeId = table.Column<long>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolInformation_SchoolType_SchoolTypeId",
                        column: x => x.SchoolTypeId,
                        principalTable: "SchoolType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionPlanFee",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SubscriptionPlanId = table.Column<long>(nullable: false),
                    Fee = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionPlanFee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionPlanFee_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponCodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CouponCode = table.Column<string>(nullable: true),
                    CouponPercentage = table.Column<long>(nullable: false),
                    SuperAdminId = table.Column<Guid>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponCodes_SuperAdministrator_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "SuperAdministrator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseName = table.Column<string>(nullable: true),
                    CourseDescription = table.Column<string>(nullable: true),
                    CourseTypeId = table.Column<long>(nullable: false),
                    LevelTypeId = table.Column<long>(nullable: false),
                    CourseCategoryId = table.Column<long>(nullable: false),
                    CourseSubCategoryId = table.Column<long>(nullable: false),
                    CourseImageUrl = table.Column<string>(nullable: true),
                    CourseAmount = table.Column<long>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_CourseCategory_CourseCategoryId",
                        column: x => x.CourseCategoryId,
                        principalTable: "CourseCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_CourseSubCategory_CourseSubCategoryId",
                        column: x => x.CourseSubCategoryId,
                        principalTable: "CourseSubCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_CourseType_CourseTypeId",
                        column: x => x.CourseTypeId,
                        principalTable: "CourseType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_CourseLevelTypes_LevelTypeId",
                        column: x => x.LevelTypeId,
                        principalTable: "CourseLevelTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CartReferenceId = table.Column<string>(nullable: true),
                    IsCheckedOut = table.Column<bool>(nullable: false),
                    SubTotal = table.Column<long>(nullable: false),
                    TotalCourse = table.Column<long>(nullable: false),
                    CouponCode = table.Column<string>(nullable: true),
                    TotalAmountPayable = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LearnerId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cart_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LearnersOtherInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    Bio = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnersOtherInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearnersOtherInfo_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolCampuses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusName = table.Column<string>(nullable: true),
                    CampusAddress = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolCampuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolCampuses_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolsSubscriptionInfo",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    SubscriptionPlanId = table.Column<long>(nullable: false),
                    PaymentMethodId = table.Column<long>(nullable: false),
                    AmountPaid = table.Column<long>(nullable: false),
                    Duration = table.Column<long>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DatePaid = table.Column<DateTime>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolsSubscriptionInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolsSubscriptionInfo_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolsSubscriptionInfo_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolsSubscriptionInfo_SubscriptionPlans_SubscriptionPlanId",
                        column: x => x.SubscriptionPlanId,
                        principalTable: "SubscriptionPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSuperAdmin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSuperAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSuperAdmin_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollees",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    IsInProgress = table.Column<bool>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    DateEnrolled = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrollees_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollees_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseObjectives",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<long>(nullable: false),
                    Objective = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseObjectives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseObjectives_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseRatings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    RatingValue = table.Column<long>(nullable: false),
                    DateRated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseRatings_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseRatings_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseRequirements",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CourseId = table.Column<long>(nullable: false),
                    Requirement = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseRequirements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseRequirements_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseReviews",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    ReviewNote = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateReviewed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseReviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseReviews_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseReviews_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopics",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    Topic = table.Column<string>(nullable: true),
                    Duration = table.Column<string>(nullable: true),
                    TopicDescription = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopics_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopics_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CartId = table.Column<long>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrolledPayments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LearnerId = table.Column<Guid>(nullable: false),
                    CartId = table.Column<long>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    Amount = table.Column<string>(nullable: true),
                    GatewayResponse = table.Column<string>(nullable: true),
                    Channel = table.Column<string>(nullable: true),
                    Currency = table.Column<string>(nullable: true),
                    Paid_At = table.Column<DateTime>(nullable: false),
                    Created_At = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrolledPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrolledPayments_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrolledPayments_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsedCouponCodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CouponCode = table.Column<string>(nullable: true),
                    CartId = table.Column<long>(nullable: false),
                    LearnerId = table.Column<Guid>(nullable: false),
                    DateUsed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsedCouponCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsedCouponCodes_Cart_CartId",
                        column: x => x.CartId,
                        principalTable: "Cart",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsedCouponCodes_Learners_LearnerId",
                        column: x => x.LearnerId,
                        principalTable: "Learners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassName = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Classes_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    hasChild = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parents_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolAdmin",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolAdmin_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolAdmin_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolUsers_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolUsers_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsAssignedToClass = table.Column<bool>(nullable: false),
                    hasParent = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicMaterials",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    CourseTopicId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicMaterials_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicMaterials_CourseTopics_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicMaterials_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseTopicVideos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FacilitatorId = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<long>(nullable: false),
                    CourseTopicId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTopicVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideos_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideos_CourseTopics_CourseTopicId",
                        column: x => x.CourseTopicId,
                        principalTable: "CourseTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTopicVideos_Facilitators_FacilitatorId",
                        column: x => x.FacilitatorId,
                        principalTable: "Facilitators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClassGrades",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    GradeName = table.Column<string>(nullable: true),
                    isAssignedToTeacher = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassGrades_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassGrades_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassGrades_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SubjectName = table.Column<string>(nullable: true),
                    IsAssignedToClass = table.Column<bool>(nullable: false),
                    IsAssignedToTeacher = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolUserRoles_SchoolRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SchoolRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolUserRoles_SchoolAdmin_UserId",
                        column: x => x.UserId,
                        principalTable: "SchoolAdmin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsAssignedToClass = table.Column<bool>(nullable: false),
                    IsAssignedSubjects = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentsStudentsMap",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentsStudentsMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeStudents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeStudents_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeTeachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherLessons",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    LessonName = table.Column<long>(nullable: false),
                    SubjectId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    StartTime = table.Column<long>(nullable: false),
                    Duration = table.Column<long>(nullable: false),
                    isActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherLessons_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssessments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssessmentName = table.Column<string>(nullable: true),
                    LessonId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    Duration = table.Column<long>(nullable: false),
                    PassMark = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssessments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonAssessments_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAssessments_TeacherLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TeacherLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAssessments_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PassMark = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonAssignments_TeacherLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TeacherLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAssignments_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonAttendance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsPresent = table.Column<bool>(nullable: false),
                    IsAbsent = table.Column<bool>(nullable: false),
                    AttendanceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonAttendance_SchoolCampuses_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAttendance_TeacherLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TeacherLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAttendance_SchoolInformation_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "SchoolInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonAttendance_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonMaterials",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeacherId = table.Column<Guid>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonMaterials_TeacherLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TeacherLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonMaterials_SchoolAdmin_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolAdmin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonSubmittedAssignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    DateSubmitted = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonSubmittedAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonSubmittedAssignments_TeacherLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TeacherLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonSubmittedAssignments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonVideos",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TeacherId = table.Column<Guid>(nullable: false),
                    LessonId = table.Column<long>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    FileType = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsAvailable = table.Column<bool>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonVideos_TeacherLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "TeacherLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonVideos_SchoolAdmin_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolAdmin",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CourseCategory",
                columns: new[] { "Id", "CategoryDescription", "CategoryImageUrl", "CourseCategoryName" },
                values: new object[,]
                {
                    { 1L, null, "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/angularjs.jpg.jpg", "Design" },
                    { 2L, null, "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/reactjs.jpg.jpg", "Networking" },
                    { 3L, null, "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/wordpress.jpg.jpg", "Painting" },
                    { 4L, null, "http://res.cloudinary.com/mywebsite/image/upload/v1601055911/Softlearn/course_category_Images/photoshop.jpg.jpg", "Advertising" },
                    { 5L, null, "https://res.cloudinary.com/mywebsite/image/upload/v1603445237/softlearn/course_category_images/marketing_ekmzwf.jpg", "Marketing" }
                });

            migrationBuilder.InsertData(
                table: "CourseLevelTypes",
                columns: new[] { "Id", "LevelTypeName" },
                values: new object[,]
                {
                    { 3L, "University/Adult" },
                    { 4L, "All" },
                    { 1L, "Primary/Elementary" },
                    { 2L, "Secondary/Higher" }
                });

            migrationBuilder.InsertData(
                table: "CourseType",
                columns: new[] { "Id", "CourseTypeName" },
                values: new object[,]
                {
                    { 1L, "Paid" },
                    { 2L, "Free" }
                });

            migrationBuilder.InsertData(
                table: "SchoolRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 1L, "Administrator" },
                    { 2L, "Class Teacher" },
                    { 3L, "Subject Teacher" }
                });

            migrationBuilder.InsertData(
                table: "SchoolType",
                columns: new[] { "Id", "SchoolTypeName" },
                values: new object[,]
                {
                    { 2L, "Primary" },
                    { 1L, "Nursery" },
                    { 3L, "Secondary" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_LearnerId",
                table: "Cart",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CourseId",
                table: "CartItems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CampusId",
                table: "Classes",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrades_CampusId",
                table: "ClassGrades",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrades_ClassId",
                table: "ClassGrades",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrades_SchoolId",
                table: "ClassGrades",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponCodes_CreatedById",
                table: "CouponCodes",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolledPayments_CartId",
                table: "CourseEnrolledPayments",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrolledPayments_LearnerId",
                table: "CourseEnrolledPayments",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollees_CourseId",
                table: "CourseEnrollees",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollees_LearnerId",
                table: "CourseEnrollees",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseObjectives_CourseId",
                table: "CourseObjectives",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRatings_CourseId",
                table: "CourseRatings",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRatings_LearnerId",
                table: "CourseRatings",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseRequirements_CourseId",
                table: "CourseRequirements",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseReviews_CourseId",
                table: "CourseReviews",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseReviews_LearnerId",
                table: "CourseReviews",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseCategoryId",
                table: "Courses",
                column: "CourseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseSubCategoryId",
                table: "Courses",
                column: "CourseSubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseTypeId",
                table: "Courses",
                column: "CourseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FacilitatorId",
                table: "Courses",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LevelTypeId",
                table: "Courses",
                column: "LevelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseSubCategory_CourseCategoryId",
                table: "CourseSubCategory",
                column: "CourseCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicMaterials_CourseId",
                table: "CourseTopicMaterials",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicMaterials_CourseTopicId",
                table: "CourseTopicMaterials",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicMaterials_FacilitatorId",
                table: "CourseTopicMaterials",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopics_CourseId",
                table: "CourseTopics",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopics_FacilitatorId",
                table: "CourseTopics",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideos_CourseId",
                table: "CourseTopicVideos",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideos_CourseTopicId",
                table: "CourseTopicVideos",
                column: "CourseTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTopicVideos_FacilitatorId",
                table: "CourseTopicVideos",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorAccountDetails_FacilitatorId",
                table: "FacilitatorAccountDetails",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FacilitatorOtherInfo_FacilitatorId",
                table: "FacilitatorOtherInfo",
                column: "FacilitatorId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_CampusId",
                table: "GradeStudents",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_ClassGradeId",
                table: "GradeStudents",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_ClassId",
                table: "GradeStudents",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_SchoolId",
                table: "GradeStudents",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_StudentId",
                table: "GradeStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_CampusId",
                table: "GradeTeachers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_ClassGradeId",
                table: "GradeTeachers",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_ClassId",
                table: "GradeTeachers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_SchoolId",
                table: "GradeTeachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_SchoolUserId",
                table: "GradeTeachers",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Learners_LevelTypeId",
                table: "Learners",
                column: "LevelTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LearnersOtherInfo_LearnerId",
                table: "LearnersOtherInfo",
                column: "LearnerId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssessments_CampusId",
                table: "LessonAssessments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssessments_LessonId",
                table: "LessonAssessments",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssessments_SchoolId",
                table: "LessonAssessments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssignments_LessonId",
                table: "LessonAssignments",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAssignments_SchoolUserId",
                table: "LessonAssignments",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAttendance_CampusId",
                table: "LessonAttendance",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAttendance_LessonId",
                table: "LessonAttendance",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAttendance_SchoolId",
                table: "LessonAttendance",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonAttendance_StudentId",
                table: "LessonAttendance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterials_LessonId",
                table: "LessonMaterials",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonMaterials_TeacherId",
                table: "LessonMaterials",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonSubmittedAssignments_LessonId",
                table: "LessonSubmittedAssignments",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonSubmittedAssignments_StudentId",
                table: "LessonSubmittedAssignments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonVideos_LessonId",
                table: "LessonVideos",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonVideos_TeacherId",
                table: "LessonVideos",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_CampusId",
                table: "Parents",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_SchoolId",
                table: "Parents",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_CampusId",
                table: "ParentsStudentsMap",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_ParentId",
                table: "ParentsStudentsMap",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_SchoolId",
                table: "ParentsStudentsMap",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_StudentId",
                table: "ParentsStudentsMap",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAdmin_CampusId",
                table: "SchoolAdmin",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolAdmin_SchoolId",
                table: "SchoolAdmin",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolCampuses_SchoolId",
                table: "SchoolCampuses",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolInformation_SchoolTypeId",
                table: "SchoolInformation",
                column: "SchoolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolsSubscriptionInfo_PaymentMethodId",
                table: "SchoolsSubscriptionInfo",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolsSubscriptionInfo_SchoolId",
                table: "SchoolsSubscriptionInfo",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolsSubscriptionInfo_SubscriptionPlanId",
                table: "SchoolsSubscriptionInfo",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_CampusId",
                table: "SchoolSubjects",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_ClassId",
                table: "SchoolSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_SchoolId",
                table: "SchoolSubjects",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSuperAdmin_SchoolId",
                table: "SchoolSuperAdmin",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUserRoles_RoleId",
                table: "SchoolUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUserRoles_UserId",
                table: "SchoolUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUsers_CampusId",
                table: "SchoolUsers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUsers_SchoolId",
                table: "SchoolUsers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CampusId",
                table: "Students",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_CampusId",
                table: "SubjectTeachers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_ClassId",
                table: "SubjectTeachers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SchoolId",
                table: "SubjectTeachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SchoolUserId",
                table: "SubjectTeachers",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SubjectId",
                table: "SubjectTeachers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlanFee_SubscriptionPlanId",
                table: "SubscriptionPlanFee",
                column: "SubscriptionPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_CampusId",
                table: "TeacherLessons",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_ClassGradeId",
                table: "TeacherLessons",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_ClassId",
                table: "TeacherLessons",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_SchoolId",
                table: "TeacherLessons",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_SchoolUserId",
                table: "TeacherLessons",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherLessons_SubjectId",
                table: "TeacherLessons",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CampusId",
                table: "Teachers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolUserId",
                table: "Teachers",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedCouponCodes_CartId",
                table: "UsedCouponCodes",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_UsedCouponCodes_LearnerId",
                table: "UsedCouponCodes",
                column: "LearnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CouponCodes");

            migrationBuilder.DropTable(
                name: "CourseEnrolledPayments");

            migrationBuilder.DropTable(
                name: "CourseEnrollees");

            migrationBuilder.DropTable(
                name: "CourseObjectives");

            migrationBuilder.DropTable(
                name: "CourseRatings");

            migrationBuilder.DropTable(
                name: "CourseRequirements");

            migrationBuilder.DropTable(
                name: "CourseReviews");

            migrationBuilder.DropTable(
                name: "CourseTopicMaterials");

            migrationBuilder.DropTable(
                name: "CourseTopicVideos");

            migrationBuilder.DropTable(
                name: "EmailConfirmationCodes");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "FacilitatorAccountDetails");

            migrationBuilder.DropTable(
                name: "FacilitatorOtherInfo");

            migrationBuilder.DropTable(
                name: "GradeStudents");

            migrationBuilder.DropTable(
                name: "GradeTeachers");

            migrationBuilder.DropTable(
                name: "LearnersOtherInfo");

            migrationBuilder.DropTable(
                name: "LessonAssessments");

            migrationBuilder.DropTable(
                name: "LessonAssignments");

            migrationBuilder.DropTable(
                name: "LessonAttendance");

            migrationBuilder.DropTable(
                name: "LessonMaterials");

            migrationBuilder.DropTable(
                name: "LessonSubmittedAssignments");

            migrationBuilder.DropTable(
                name: "LessonVideos");

            migrationBuilder.DropTable(
                name: "ParentsStudentsMap");

            migrationBuilder.DropTable(
                name: "SchoolsSubscriptionInfo");

            migrationBuilder.DropTable(
                name: "SchoolSuperAdmin");

            migrationBuilder.DropTable(
                name: "SchoolUserRoles");

            migrationBuilder.DropTable(
                name: "SubjectTeachers");

            migrationBuilder.DropTable(
                name: "SubscriptionPlanFee");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "UsedCouponCodes");

            migrationBuilder.DropTable(
                name: "SuperAdministrator");

            migrationBuilder.DropTable(
                name: "CourseTopics");

            migrationBuilder.DropTable(
                name: "TeacherLessons");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "SchoolRoles");

            migrationBuilder.DropTable(
                name: "SchoolAdmin");

            migrationBuilder.DropTable(
                name: "SubscriptionPlans");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "ClassGrades");

            migrationBuilder.DropTable(
                name: "SchoolUsers");

            migrationBuilder.DropTable(
                name: "SchoolSubjects");

            migrationBuilder.DropTable(
                name: "Learners");

            migrationBuilder.DropTable(
                name: "CourseSubCategory");

            migrationBuilder.DropTable(
                name: "CourseType");

            migrationBuilder.DropTable(
                name: "Facilitators");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "CourseLevelTypes");

            migrationBuilder.DropTable(
                name: "CourseCategory");

            migrationBuilder.DropTable(
                name: "SchoolCampuses");

            migrationBuilder.DropTable(
                name: "SchoolInformation");

            migrationBuilder.DropTable(
                name: "SchoolType");
        }
    }
}
