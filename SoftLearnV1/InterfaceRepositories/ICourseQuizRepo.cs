using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseQuizRepo
    {
        //----------------------------CourseQuiz---------------------------------------------------------------
        Task<GenericResponseModel> createCourseQuizAsync(CourseQuizRequestModel obj);
        Task<GenericResponseModel> updateCourseQuizAsync(long quizId, CourseQuizRequestModel obj);
        Task<GenericResponseModel> deleteCourseQuizAsync(long quizId);
        Task<GenericResponseModel> getCourseQuizByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getAllCourseQuizByFacilitatorIdAsync(Guid facilitatorId);
        Task<GenericResponseModel> getCourseQuizByIdAsync(long quizId);

        //----------------------------CourseQuizQuestion-------------------------------------------------------
        Task<GenericResponseModel> createCourseQuizQuestionAsync(CourseQuizQuestionRequestModel obj);
        Task<GenericResponseModel> createBulkCourseQuizQuestionAsync(BulkCourseQuizQuestionRequestModel obj);
        Task<GenericResponseModel> createBulkCourseQuizQuestionFromExcelAsync(BulkQuizQuestionRequestModel obj);
        Task<GenericResponseModel> updateCourseQuizQuestionAsync(long questionId, CourseQuizQuestionRequestModel obj);
        Task<GenericResponseModel> deleteCourseQuizQuestionAsync(long questionId);
        Task<GenericResponseModel> getAllCourseQuizQuestionAsync();
        Task<GenericResponseModel> getAllCourseQuizQuestionByQuizIdAsync(long quizId);
        Task<GenericResponseModel> getCourseQuizQuestionByIdAsync(long questionId);

        //----------------------------CourseQuizAnswer-------------------------------------------------------
        Task<GenericResponseModel> createCourseQuizResultAsync(CourseQuizResultRequestModel obj);
        Task<GenericResponseModel> getAllCourseQuizResultByLearnerIdAsync(Guid learnerId);
        Task<GenericResponseModel> getCourseQuizResultByIdAsync(long resultId);
    }
}
