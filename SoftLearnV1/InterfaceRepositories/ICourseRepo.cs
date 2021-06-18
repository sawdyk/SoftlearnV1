using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseRepo
    {
        //----------------------------Courses---------------------------------------------------------------
        Task<GenericResponseModel> createCourseAsync(CourseCreateRequestModel obj);
        Task<GenericResponseModel> approveCourseCreationAsync(long courseId);
        Task<GenericResponseModel> getAllCoursesAsync(int pageNumber, int pageSize);
        Task<GenericResponseModel> getCourseByIdAsync(long courseId);
        Task<GenericResponseModel> getAllCourseByFacilitatorIdAsync(Guid facilitatorId);
        Task<GenericResponseModel> deleteCourseAsync(long courseId);
        Task<GenericResponseModel> deleteCourseAttachedToEnrolleeAsync(long courseId);
        Task<GenericResponseModel> updateCourseVideoPreviewAsync(long courseId, string courseVideoPreviewUrl);



        //With Pagination
        Task<GenericResponseModel> getAllCourseByFacilitatorIdAsync(Guid facilitatorId, int pageNumber, int pageSize);
        Task<GenericResponseModel> searchCourseByCourseNameAsync(string courseName);

        //----------------------------Course By Type, Category and Level with Pagination ------------------------------------
        Task<GenericResponseModel> getCoursesByTypeIdAsync(long typeId, int pageNumber, int pageSize);
        Task<GenericResponseModel> getCoursesByCategoryIdAsync(long categoryId, int pageNumber, int pageSize);
        Task<GenericResponseModel> getCoursesByLevelIdAsync(long levelId, int pageNumber, int pageSize);
        Task<GenericResponseModel> getCoursesBySubCategoryIdAsync(long subCategoryId, int pageNumber, int pageSize);
        Task<GenericResponseModel> getCoursesByStatusIdAsync(long statusId, int pageNumber, int pageSize);


        //----------------------------Course By Type, Category and Level without Pagination ------------------------------------

        Task<GenericResponseModel> getCoursesByTypeIdAsync(long typeId);
        Task<GenericResponseModel> getCoursesByCategoryIdAsync(long categoryId);
        Task<GenericResponseModel> getCoursesByLevelIdAsync(long levelId);
        Task<GenericResponseModel> getCoursesBySubCategoryIdAsync(long subCategoryId);
        Task<GenericResponseModel> getCoursesByStatusIdAsync(long statusId);
        Task<GenericResponseModel> getAllCoursesAsync(long typeId, long categoryId, long subCategoryId, long levelId, long statusId, Guid facilitatorId, int pageNumber, int pageSize);


        //----------------------------Course Enrolles -------------------------------------------------------
        Task<GenericResponseModel> courseEnrollAsync(CourseEnrollRequestModel obj);
        Task<GenericResponseModel> getCourseEnrollByIdAsync(long courseEnrollId);
        Task<GenericResponseModel> searchCoursesEnrolledForAsync(Guid learnerId, string courseName);
        Task<GenericResponseModel> getAllCourseEnrolledForByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getAllLearnersEnrolledForCourseAsync(long courseId);
        Task<GenericResponseModel> getAllCoursesLearnerEnrolledForAysnc(Guid learnerId);
        Task<GenericResponseModel> getAllCoursesLearnerEnrolledForAysnc(Guid learnerId, int pageNumber, int pageSize);
        Task<GenericResponseModel> getAllCoursesByFacilitatorLearnersEnrolledForAsync(Guid facilitatorId);
        Task<GenericResponseModel> deleteAllCoursesEnrolledForByCourseIdAsync(long courseId);
        Task<GenericResponseModel> deleteCourseEnrolledForAsync(Guid learnerId, long courseId);



        //----------------------------Course Enroll Payments -------------------------------------------------
        Task<GenericResponseModel> verifyPaymentAysnc(long cartId, Guid learnerId,  string reference);

        //----------------------------Popular,Most Viewed, Recommended -------------------------------------------------
        Task<GenericResponseModel> popularCoursesAsync();
        Task<GenericResponseModel> createMostViewedCoursesAsync(long courseId);
        Task<GenericResponseModel> mostViewedCoursesAsync();
        Task<GenericResponseModel> recommendedCoursesAsync(Guid learnerId);
        Task<GenericResponseModel> getAllCourseRatingAndReviewByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getAllCourseAndAverageRatingAsync();

        //--------------------Course Archiving, Activation and deactivaton by Learners--------------------------------

        Task<GenericResponseModel> archiveOrUnArchiveCourseEnrolledForAsync(Guid learnerId, long courseId);
        Task<GenericResponseModel> getAllArchivedCoursesEnrolledForAsync(Guid learnerId);
        Task<GenericResponseModel> getAllUnArchivedCoursesEnrolledForAsync(Guid learnerId);
        Task<GenericResponseModel> updateStatusForCourseEnrolledForAsync(Guid learnerId, long courseId, long statusId); //removing courses from learners gallery (Active/InActive)
        Task<GenericResponseModel>getCoursesEnrolledForByStatusIdAsync(Guid learnerId, long statusId); //removing courses from learners gallery (Active/InActive)

        //----------------------Earning Percentage on Courses (SuperAdmin/Admin)-------------------------------------------------------------
        Task<GenericResponseModel> getDefaultPercentageEarningsPerCourseAsync();
        Task<GenericResponseModel> updateDefaultPercentageEarningsPerCourseAsync(long Id, long percentage);

        Task<GenericResponseModel> getAllPercentageEarnedOnCoursesAsync();
        Task<GenericResponseModel> getPercentageEarnedOnCoursesByIdAsync(long Id);
        Task<GenericResponseModel> getPercentageEarnedOnCoursesByCourseIdAsync(long courseId);
        Task<GenericResponseModel> updatePercentageEarnedOnCoursesAsync(long courseId, long percentage);


        //------------------------------- Course Return -------------------------------------------------------------

        Task<GenericResponseModel> refundCourseAsync(CourseRefundRequestModel obj);
        Task<GenericResponseModel> deleteRefundCourseAsync(long refundCourseId);
        Task<GenericResponseModel> getAllRefundCoursesAsync();
        Task<GenericResponseModel> getRefundCoursesByIdAsync(long refundCourseId);
        Task<GenericResponseModel> getRefundCourseByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getRefundCourseByLearnerIdAsync(Guid learnerId);


        //------------------------------- Course Progress-------------------------------------------------------------
   
        Task<GenericResponseModel> courseProgressAsync(Guid learnerId, long courseId, long videoId); //taking the course progress
        Task<GenericResponseModel> getCourseProgressAsync(Guid learnerId, long courseId); //getting the course progress

        //------------------------------- Course Certigicate-----------------------------------------------------------
        Task<GenericResponseModel> getCourseCertificateAsync(Guid learnerId, long courseId); //getting the course progress
    }
}
