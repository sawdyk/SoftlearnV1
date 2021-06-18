using Microsoft.EntityFrameworkCore;
using SoftLearnV1.DataSeed;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Helpers
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<SystemUsers> SystemUsers { get; set; }
        public DbSet<SystemUserRoles> SystemUserRoles { get; set; }
        public DbSet<SystemRoles> SystemRoles { get; set; }
        public DbSet<SchoolAdmin> SchoolAdmin { get; set; }
        public DbSet<SchoolUserRoles> SchoolUserRoles { get; set; }
        public DbSet<Facilitators> Facilitators { get; set; }
        public DbSet<FacilitatorType> FacilitatorType { get; set; }
        public DbSet<Learners> Learners { get; set; }
        public DbSet<SchoolRoles> SchoolRoles { get; set; }
        public DbSet<FacilitatorAccountDetails> FacilitatorAccountDetails { get; set; }
        public DbSet<Courses> Courses { get; set; }
        public DbSet<CourseType> CourseType { get; set; }
        public DbSet<CourseCategory> CourseCategory { get; set; }
        public DbSet<CourseSubCategory> CourseSubCategory { get; set; }
        public DbSet<CourseLevelTypes> CourseLevelTypes { get; set; }
        public DbSet<CourseTopics> CourseTopics { get; set; }
        public DbSet<CourseTopicMaterials> CourseTopicMaterials { get; set; }
        public DbSet<CourseTopicVideos> CourseTopicVideos { get; set; }
        public DbSet<CourseTopicVideoMaterial> CourseTopicVideoMaterials { get; set; }
        //public DbSet<MaterialCategory> MaterialCategory { get; set; }
        //public DbSet<MaterialTypes> MaterialTypes { get; set; }
        public DbSet<CourseEnrollees> CourseEnrollees { get; set; }
        public DbSet<CourseEnrolledPayments> CourseEnrolledPayments { get; set; }
        public DbSet<CourseRatings> CourseRatings { get; set; }
        public DbSet<CourseReviews> CourseReviews { get; set; }
        public DbSet<CourseRequirements> CourseRequirements { get; set; }
        public DbSet<CourseObjectives> CourseObjectives { get; set; }
        public DbSet<SchoolInformation> SchoolInformation { get; set; }
        public DbSet<SchoolType> SchoolType { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<ClassGrades> ClassGrades { get; set; }
        public DbSet<GradeTeachers> GradeTeachers { get; set; }
        public DbSet<GradeStudents> GradeStudents { get; set; }
        public DbSet<SchoolSubjects> SchoolSubjects { get; set; }
        public DbSet<SubjectTeachers> SubjectTeachers { get; set; }
        public DbSet<TeacherLessons> TeacherLessons { get; set; }
        public DbSet<LessonAssessment> LessonAssessments { get; set; }
        public DbSet<LessonAttendance> LessonAttendance { get; set; }
        public DbSet<SubscriptionPlans> SubscriptionPlans { get; set; }
        //public DbSet<SubscriptionPlanMaterialTypes> SubscriptionPlanMaterialTypes { get; set; }
        public DbSet<SubscriptionPlanFee> SubscriptionPlanFee { get; set; }
        public DbSet<SchoolsSubscriptionInfo> SchoolsSubscriptionInfo { get; set; }
        //public DbSet<LearnersOtherInfo> LearnersOtherInfo { get; set; }
        public DbSet<PaymentMethods> PaymentMethods { get; set; }
        public DbSet<EmailConfirmationCodes> EmailConfirmationCodes { get; set; }
        public DbSet<LessonAssignments> LessonAssignments { get; set; }
        public DbSet<LessonSubmittedAssignments> LessonSubmittedAssignments { get; set; }
        public DbSet<LessonMaterials> LessonMaterials { get; set; }
        public DbSet<LessonVideos> LessonVideos { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<Parents> Parents { get; set; }
        public DbSet<ParentsStudentsMap> ParentsStudentsMap { get; set; }
        public DbSet<SchoolUsers> SchoolUsers { get; set; }
        public DbSet<SchoolCampuses> SchoolCampuses { get; set; }
        public DbSet<CouponCodes> CouponCodes { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItems> CartItems { get; set; }
        public DbSet<UsedCouponCodes> UsedCouponCodes { get; set; }
        public DbSet<MostViewedCourses> MostViewedCourses { get; set; }
        public DbSet<ForgotPasswordCodes> ForgotPasswordCodes { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<Terms> Terms { get; set; }
        public DbSet<AcademicSessions> AcademicSessions { get; set; }
        public DbSet<CourseQuiz> CourseQuiz { get; set; }
        public DbSet<CourseQuizQuestions> CourseQuizQuestions { get; set; }
        public DbSet<CourseQuizResults> CourseQuizResults { get; set; }
        public DbSet<QuestionTypes> QuestionTypes { get; set; }
        public DbSet<CourseTopicQuiz> CourseTopicQuiz { get; set; }
        public DbSet<CourseTopicQuizQuestions> CourseTopicQuizQuestions { get; set; }
        public DbSet<CourseTopicQuizResults> CourseTopicQuizResults { get; set; }
        public DbSet<SubjectDepartment> SubjectDepartment { get; set; }
        public DbSet<ScoreGrading> ScoreGrading { get; set; }
        public DbSet<ScoreCategory> ScoreCategory { get; set; }
        public DbSet<ScoreSubCategoryConfig> ScoreSubCategoryConfig { get; set; }
        public DbSet<ContinousAssessmentScores> ContinousAssessmentScores { get; set; }
        public DbSet<ExaminationScores> ExaminationScores { get; set; }
        public DbSet<BehavioralScores> BehavioralScores { get; set; }
        public DbSet<ExtraCurricularScores> ExtraCurricularScores { get; set; }
        public DbSet<Alumni> Alumni { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<ClassAlumni> ClassAlumni { get; set; }
        public DbSet<FeeCategory> FeeCategory { get; set; }
        public DbSet<FeeSubCategory> FeeSubCategory { get; set; }
        public DbSet<FeeTemplate> FeeTemplate { get; set; }
        public DbSet<FeeTemplateList> FeeTemplateList { get; set; }
        public DbSet<InvoiceList> InvoiceList { get; set; }
        public DbSet<InvoiceTotal> InvoiceTotal { get; set; }
        public DbSet<SchoolFeesPayments> SchoolFeesPayments { get; set; }
        public DbSet<SchoolPaymentMethods> SchoolPaymentMethods { get; set; }
        public DbSet<SchoolTemporaryPayments> SchoolTemporaryPayments { get; set; }
        public DbSet<SchoolSubTypes> SchoolSubTypes { get; set; }
        public DbSet<ReportCardTemplates> ReportCardTemplates { get; set; }
        public DbSet<AttendancePeriod> AttendancePeriod { get; set; }
        public DbSet<StudentAttendance> StudentAttendance { get; set; }
        public DbSet<Assignments> Assignments { get; set; }
        public DbSet<AssignmentsSubmitted> AssignmentsSubmitted { get; set; }
        public DbSet<LessonNotes> LessonNotes { get; set; }
        public DbSet<SubjectNotes> SubjectNotes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<ScoreStatus> ScoreStatus { get; set; }
        public DbSet<SchoolFee> SchoolFees { get; set; }
        public DbSet<ScoreUploadSheetTemplates> ScoreUploadSheetTemplates { get; set; }
        public DbSet<ReportCardComments> ReportCardComments { get; set; }
        public DbSet<ReportCardCommentConfig> ReportCardCommentConfig { get; set; }
        public DbSet<ReportCardNextTermBegins> ReportCardNextTermBegins { get; set; }
        public DbSet<ReportCardCommentsList> ReportCardCommentsList { get; set; }
        public DbSet<ReportCardSignature> ReportCardSignature { get; set; }
        public DbSet<CbtCategory> CbtCategory { get; set; }
        public DbSet<CbtTypes> CbtTypes { get; set; }
        public DbSet<ComputerBasedTest> ComputerBasedTest { get; set; }
        public DbSet<CbtQuestions> CbtQuestions { get; set; }
        public DbSet<CbtResults> CbtResults { get; set; }
        public DbSet<ActiveInActiveStatus> ActiveInActiveStatus { get; set; }
        public DbSet<ReportCardData> ReportCardData { get; set; }
        public DbSet<ReportCardPosition> ReportCardPosition { get; set; }
        public DbSet<AppTypes> AppTypes { get; set; }
        public DbSet<FolderTypes> FolderTypes { get; set; }
        public DbSet<PercentageEarnedOnCourses> PercentageEarnedOnCourses { get; set; }
        public DbSet<FacilitatorsEarningsPerCourse> FacilitatorsEarningsPerCourse { get; set; }
        public DbSet<FacilitatorsTotalEarnings> FacilitatorsTotalEarnings { get; set; }
        public DbSet<DefaultPercentageEarningsPerCourse> DefaultPercentageEarningsPerCourse { get; set; }
        public DbSet<CourseRefund> CourseRefund { get; set; }
        public DbSet<ActivityLog> ActivityLog { get; set; }
        public DbSet<LearnerAccountDetails> LearnerAccountDetails { get; set; }
        public DbSet<LearnersPaymentDisbursements> LearnersPaymentDisbursements { get; set; }
        public DbSet<FacilitatorPaymentDisbursements> FacilitatorPaymentDisbursements { get; set; }
        public DbSet<StudentDuplicates> StudentDuplicates { get; set; }
        public DbSet<ReportCardConfigurationLegend> ReportCardConfigurationLegend { get; set; }
        public DbSet<ReportCardConfiguration> ReportCardConfiguration { get; set; }
        public DbSet<ReportCardStudentPerformance> ReportCardStudentPerformance { get; set; }
        public DbSet<ReportCardConfigurationLegendList> ReportCardConfigurationLegendList { get; set; }
        public DbSet<ScoreCategoryConfig> ScoreCategoryConfig { get; set; }
        public DbSet<Certificates> Certificates { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CourseEnrolleProgressVideo> CourseEnrolleProgressVideos { get; set; }
        public DbSet<ReportCardConfigurationForClass> ReportCardConfigurationForClasses { get; set; }
        public DbSet<CourseEnrolleeCompletedVideo> CourseEnrolleeCompletedVideos { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SeedSchoolRoles(); 
            builder.SeedSchoolTypes();
            builder.SeedCourseCategoryRoles();
            builder.SeedCourseLevelTypes();
            builder.seedCourseType();
            builder.SeedTerms();
            builder.SeedQuestionTypes();
            builder.seedScoreCategory();
            builder.SeedSystemRoles();
            //builder.SeedSuperAdmin();
            builder.SeedGenderTypes();
            builder.SeedClassOrAlumni();
            builder.SeedPaymentMethods();
            builder.SeedSchoolSubTypes();
            builder.seedFacilitatorType();
            builder.SeedAttendancePeriod();
            builder.SeedStatus();
            builder.SeedScoreStatus();
            builder.SeedReportCardConfig();
            builder.SeedCbtCategory();
            builder.SeedCbtTypes();
            builder.SeedActiveInActiveStatus();
            builder.SeedAppTypes();
            builder.SeedFolderTypes();
            builder.SeedDefaultPercentageEarnings();
        }
    }
}
