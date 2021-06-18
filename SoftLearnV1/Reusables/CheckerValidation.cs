using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftLearnV1.Utilities;

namespace SoftLearnV1.Reusables
{
    public class CheckerValidation
    {
        private readonly AppDbContext _context;

        public CheckerValidation(AppDbContext context)
        {
            _context = context;
        }

        //------------------------checks if a user Email Address Exist---------------------------------------

        public bool checkIfEmailExist(string email, long userCategory)
        {
            try
            {
                bool emailExist = false;

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Facilitator))
                {
                    Facilitators facilitator = _context.Facilitators.Where(u => u.Email == email).FirstOrDefault();
                    if (facilitator != null)
                    {
                        emailExist = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers))
                {
                    SchoolUsers SchoolUsers = _context.SchoolUsers.Where(u => u.Email == email).FirstOrDefault();
                    if (SchoolUsers != null)
                    {
                        emailExist = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Learners))
                {
                    Learners learners = _context.Learners.Where(u => u.Email == email).FirstOrDefault();
                    if (learners != null)
                    {
                        emailExist = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Parents))
                {
                    Parents parents = _context.Parents.Where(u => u.Email == email).FirstOrDefault();
                    if (parents != null)
                    {
                        emailExist = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Students))
                {
                    Students students = _context.Students.Where(u => u.Email == email).FirstOrDefault();
                    if (students != null)
                    {
                        emailExist = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.SystemUser))
                {
                    SystemUsers systemUsers = _context.SystemUsers.Where(u => u.Email == email).FirstOrDefault();
                    if (systemUsers != null)
                    {
                        emailExist = true;
                    }
                }

                return emailExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //---------------------Checks if an Account exists and not confirmed------------------------------------------

        public bool checkIfAccountExistAndNotConfirmed(string email, long userCategory)
        {

            try
            {
                bool accountExistButNotConfirmed = false;

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Facilitator))
                {
                    Facilitators facilitator = _context.Facilitators.Where(u => u.Email == email  && u.EmailConfirmed == false).FirstOrDefault();
                    if (facilitator != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Learners))
                {
                    Learners learners = _context.Learners.Where(u => u.Email == email && u.EmailConfirmed == false).FirstOrDefault();
                    if (learners != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers)) //School Super Administrators
                {
                    SchoolUsers schoolUsers = _context.SchoolUsers.Where(u => u.Email == email && u.EmailConfirmed == false).FirstOrDefault();
                    if (schoolUsers != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.SystemUser))
                {
                    SystemUsers systemUsers = _context.SystemUsers.Where(u => u.Email == email).FirstOrDefault();
                    if (systemUsers != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }
                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Students)) //School Super Administrators
                {
                    Students students = _context.Students.Where(u => u.Email == email && u.EmailConfirmed == false).FirstOrDefault();
                    if (students != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }

                if (userCategory == Convert.ToInt64(EnumUtility.UserCategoty.Parents))
                {
                    Parents parents = _context.Parents.Where(u => u.Email == email && u.EmailConfirmed == false).FirstOrDefault();
                    if (parents != null)
                    {
                        accountExistButNotConfirmed = true;
                    }
                }

                return accountExistButNotConfirmed;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }

        }

        //-------------------------------Checks if a School Name exist------------------------------------------

        public bool checkIfSchoolNameExist(string schoolName)
        {
            try
            {
                bool schoolExist = false;

                SchoolInformation schInfo = _context.SchoolInformation.Where(s =>s.SchoolName  == schoolName).FirstOrDefault();
                if (schInfo != null)
                {
                    schoolExist = true;
                }

                return schoolExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }


        //-------------------------------Checks if a School Code exist------------------------------------------

        public bool checkIfSchoolCodeExist(string schoolCode)
        {
            try
            {
                bool schoolCodeExist = false;

                SchoolInformation schInfo = _context.SchoolInformation.Where(s => s.SchoolCode == schoolCode).FirstOrDefault();
                if (schInfo != null)
                {
                    schoolCodeExist = true;
                }

                return schoolCodeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //-------------------------------Checks if a Campus Name exist------------------------------------------

        public bool checkIfSchoolCampusNameExist(string campusName)
        {
            try
            {
                bool campusExist = false;

                SchoolCampuses camp = _context.SchoolCampuses.Where(s => s.CampusName == campusName).FirstOrDefault();
                if (camp != null)
                {
                    campusExist = true;
                }

                return campusExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a facilitator exists
        public bool checkFacilitatorById(Guid facilitatorId)
        {
            try
            {
                bool facilitatorExist = false;

                Facilitators fac = _context.Facilitators.Where(s => s.Id == facilitatorId).FirstOrDefault();
                if (fac != null)
                {
                    facilitatorExist = true;
                }

                return facilitatorExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a parent exists
        public bool checkParentById(Guid parentId)
        {
            try
            {
                bool itExists = false;

                var result = _context.Parents.Where(s => s.Id == parentId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a student exists
        public bool checkStudentById(Guid studentId)
        {
            try
            {
                bool itExists = false;

                var result = _context.Students.Where(s => s.Id == studentId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a student exists
        public bool checkLearnerById(Guid learnerId)
        {
            try
            {
                bool itExists = false;

                var result = _context.Learners.Where(s => s.Id == learnerId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a student exists with Admission Number
        public bool checkStudentByAdmissionNumber(string admissionNumber)
        {
            try
            {
                bool itExists = false;

                var result = _context.Students.Where(s => s.AdmissionNumber == admissionNumber).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a school exists
        public bool checkSchoolById(long schoolId)
        {
            try
            {
                bool itExists = false;

                var result = _context.SchoolInformation.Where(s => s.Id == schoolId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a school campus exists
        public bool checkSchoolCampusById(long schoolCampusId)
        {
            try
            {
                bool itExists = false;

                var result = _context.SchoolCampuses.Where(s => s.Id == schoolCampusId).FirstOrDefault();
                if (result != null)
                {
                    itExists = true;
                }

                return itExists;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course exists
        public bool checkCourseById(long courseId)
        {
            try
            {
                bool courseExist = false;

                Courses crs = _context.Courses.Where(s => s.Id == courseId).FirstOrDefault();
                if (crs != null)
                {
                    courseExist = true;
                }

                return courseExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Cart exists
        public bool checkCartById(long cartId)
        {
            try
            {
                bool cartExist = false;

                Cart crs = _context.Cart.Where(s => s.Id == cartId).FirstOrDefault();
                if (crs != null)
                {
                    cartExist = true;
                }

                return cartExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a CartItem exists
        public bool checkCartItemById(long cartItemId)
        {
            try
            {
                bool cartItemExist = false;

                CartItems crs = _context.CartItems.Where(s => s.Id == cartItemId).FirstOrDefault();
                if (crs != null)
                {
                    cartItemExist = true;
                }

                return cartItemExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Objective exists
        public bool checkCourseObjectiveById(long courseObjectiveId)
        {
            try
            {
                bool courseObjExist = false;

                CourseObjectives crs = _context.CourseObjectives.Where(s => s.Id == courseObjectiveId).FirstOrDefault();
                if (crs != null)
                {
                    courseObjExist = true;
                }

                return courseObjExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Type exists
        public bool checkCourseTypeById(long typeId)
        {
            try
            {
                bool courseTypeExist = false;

                CourseType crs = _context.CourseType.Where(s => s.Id == typeId).FirstOrDefault();
                if (crs != null)
                {
                    courseTypeExist = true;
                }

                return courseTypeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Category exists
        public bool checkCourseCategoryById(long categoryId)
        {
            try
            {
                bool courseCategoryExist = false;

                CourseCategory crs = _context.CourseCategory.Where(s => s.Id == categoryId).FirstOrDefault();
                if (crs != null)
                {
                    courseCategoryExist = true;
                }

                return courseCategoryExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Level exists
        public bool checkCourseLevelById(long levelId)
        {
            try
            {
                bool courseLevelExist = false;

                CourseLevelTypes crs = _context.CourseLevelTypes.Where(s => s.Id == levelId).FirstOrDefault();
                if (crs != null)
                {
                    courseLevelExist = true;
                }

                return courseLevelExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Level exists
        public bool checkCourseSubCategoryById(long subCategoryId)
        {
            try
            {
                bool courseSubCategoryExist = false;

                CourseSubCategory crs = _context.CourseSubCategory.Where(s => s.Id == subCategoryId).FirstOrDefault();
                if (crs != null)
                {
                    courseSubCategoryExist = true;
                }

                return courseSubCategoryExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Class exists
        public bool checkClassById(long classId)
        {
            try
            {
                bool classExist = false;

                Classes cls = _context.Classes.Where(s => s.Id == classId).FirstOrDefault();
                if (cls != null)
                {
                    classExist = true;
                }

                return classExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Class Grade exists
        public bool checkClassGradeById(long classGradeId)
        {
            try
            {
                bool classGradeExist = false;

                ClassGrades clsGrd = _context.ClassGrades.Where(s => s.Id == classGradeId).FirstOrDefault();
                if (clsGrd != null)
                {
                    classGradeExist = true;
                }

                return classGradeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Requirement exists
        public bool checkCourseRequirementById(long courseRequirementId)
        {
            try
            {
                bool courseReqExist = false;

                CourseRequirements crs = _context.CourseRequirements.Where(s => s.Id == courseRequirementId).FirstOrDefault();
                if (crs != null)
                {
                    courseReqExist = true;
                }

                return courseReqExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Course Topic Quiz exists
        public bool checkCourseTopicQuizById(long topicQuizId)
        {
            try
            {
                bool courseTopicQuizExist = false;

                CourseTopicQuiz crs = _context.CourseTopicQuiz.Where(s => s.Id == topicQuizId).FirstOrDefault();
                if (crs != null)
                {
                    courseTopicQuizExist = true;
                }

                return courseTopicQuizExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if a Course Quiz exists
        public bool checkCourseQuizById(long topicQuizId)
        {
            try
            {
                bool courseQuizExist = false;

                CourseQuiz crs = _context.CourseQuiz.Where(s => s.Id == topicQuizId).FirstOrDefault();
                if (crs != null)
                {
                    courseQuizExist = true;
                }

                return courseQuizExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if a Topic exists
        public bool checkCourseTopicById(long topicId)
        {
            try
            {
                bool courseTopicExist = false;

                CourseTopics crs = _context.CourseTopics.Where(s => s.Id == topicId).FirstOrDefault();
                if (crs != null)
                {
                    courseTopicExist = true;
                }

                return courseTopicExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if a Topic Quiz Result exists
        public bool checkCourseTopicQuizResultById(long resultId)
        {
            try
            {
                bool courseTopicQuizResultExist = false;

                CourseTopicQuizResults crs = _context.CourseTopicQuizResults.Where(s => s.Id == resultId).FirstOrDefault();
                if (crs != null)
                {
                    courseTopicQuizResultExist = true;
                }

                return courseTopicQuizResultExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if a Course Quiz Result exists
        public bool checkCourseQuizResultById(long resultId)
        {
            try
            {
                bool courseQuizResultExist = false;

                CourseQuizResults crs = _context.CourseQuizResults.Where(s => s.Id == resultId).FirstOrDefault();
                if (crs != null)
                {
                    courseQuizResultExist = true;
                }

                return courseQuizResultExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if Question Type exists
        public bool checkQuestionTypeById(long questionTypeId)
        {
            try
            {
                bool questionTypeExist = false;

                QuestionTypes crs = _context.QuestionTypes.Where(s => s.Id == questionTypeId).FirstOrDefault();
                if (crs != null)
                {
                    questionTypeExist = true;
                }

                return questionTypeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if Session exists
        public bool checkSessionById(long sessionId)
        {
            try
            {
                bool sessionExist = false;

                Sessions crs = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
                if (crs != null)
                {
                    sessionExist = true;
                }

                return sessionExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if term exists
        public bool checkTermById(long termId)
        {
            try
            {
                bool termExist = false;

                Terms crs = _context.Terms.Where(s => s.Id == termId).FirstOrDefault();
                if (crs != null)
                {
                    termExist = true;
                }

                return termExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }

        //check if feeTemplate exists
        public bool checkFeeTemplateById(long templateId)
        {
            try
            {
                bool templateExist = false;

                FeeTemplate crs = _context.FeeTemplate.Where(s => s.Id == templateId).FirstOrDefault();
                if (crs != null)
                {
                    templateExist = true;
                }

                return templateExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if feeTemplateList exists
        public bool checkFeeTemplateListById(long templateListId)
        {
            try
            {
                bool templateListExist = false;

                FeeTemplateList crs = _context.FeeTemplateList.Where(s => s.Id == templateListId).FirstOrDefault();
                if (crs != null)
                {
                    templateListExist = true;
                }

                return templateListExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if feeCategory exists
        public bool checkFeeCategoryById(long categoryId)
        {
            try
            {
                bool categoryExist = false;

                FeeCategory crs = _context.FeeCategory.Where(s => s.Id == categoryId).FirstOrDefault();
                if (crs != null)
                {
                    categoryExist = true;
                }

                return categoryExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if feeSubCategory exists
        public bool checkFeeSubCategoryById(long subCategoryId)
        {
            try
            {
                bool subCategoryExist = false;

                FeeSubCategory crs = _context.FeeSubCategory.Where(s => s.Id == subCategoryId).FirstOrDefault();
                if (crs != null)
                {
                    subCategoryExist = true;
                }

                return subCategoryExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if SchoolFee exists
        public bool checkSchoolFeeById(long schoolFeeId)
        {
            try
            {
                bool schoolFeeExist = false;

                SchoolFee crs = _context.SchoolFees.Where(s => s.Id == schoolFeeId).FirstOrDefault();
                if (crs != null)
                {
                    schoolFeeExist = true;
                }

                return schoolFeeExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if PaymentMethod exists
        public bool checkPaymentMethodById(long methodId)
        {
            try
            {
                bool paymentMethodExist = false;

                SchoolPaymentMethods crs = _context.SchoolPaymentMethods.Where(s => s.Id == methodId).FirstOrDefault();
                if (crs != null)
                {
                    paymentMethodExist = true;
                }

                return paymentMethodExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if Invoice exists
        public bool checkInvoiceById(long invoiceId)
        {
            try
            {
                bool invoiceExist = false;

                InvoiceTotal crs = _context.InvoiceTotal.Where(s => s.Id == invoiceId).FirstOrDefault();
                if (crs != null)
                {
                    invoiceExist = true;
                }

                return invoiceExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if Payment exists
        public bool checkPaymentById(long paymentId)
        {
            try
            {
                bool paymentExist = false;

                SchoolTemporaryPayments crs = _context.SchoolTemporaryPayments.Where(s => s.Id == paymentId).FirstOrDefault();
                if (crs != null)
                {
                    paymentExist = true;
                }

                return paymentExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
        //check if Subject exists
        public bool checkSubjectById(long subjectId)
        {
            try
            {
                bool subjectExist = false;

                SchoolSubjects crs = _context.SchoolSubjects.Where(s => s.Id == subjectId).FirstOrDefault();
                if (crs != null)
                {
                    subjectExist = true;
                }

                return subjectExist;
            }
            catch (Exception exMessage)
            {
                throw exMessage;
            }
        }
    }
}
