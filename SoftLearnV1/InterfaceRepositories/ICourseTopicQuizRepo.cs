using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseTopicQuizRepo
    {
        //----------------------------CourseTopicQuiz---------------------------------------------------------------
        Task<GenericResponseModel> createCourseTopicQuizAsync(CourseTopicQuizRequestModel obj);
        Task<GenericResponseModel> updateCourseTopicQuizAsync(long quizId, CourseTopicQuizRequestModel obj);
        Task<GenericResponseModel> deleteCourseTopicQuizAsync(long quizId);
        Task<GenericResponseModel> getCourseTopicQuizByTopicIdAsync(long topicId);
        Task<GenericResponseModel> getAllCourseTopicQuizByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getAllCourseTopicQuizByFacilitatorIdAsync(Guid facilitatorId);
        Task<GenericResponseModel> getCourseTopicQuizByIdAsync(long quizId);

        //----------------------------CourseTopicQuizQuestion-------------------------------------------------------
        Task<GenericResponseModel> createCourseTopicQuizQuestionAsync(CourseTopicQuizQuestionRequestModel obj);
        Task<GenericResponseModel> createBulkCourseTopicQuizQuestionAsync(BulkCourseTopicQuizQuestionRequestModel obj);
        Task<GenericResponseModel> createBulkCourseTopicQuizQuestionFromExcelAsync(BulkQuizQuestionRequestModel obj);
        Task<GenericResponseModel> updateCourseTopicQuizQuestionAsync(long questionId, CourseTopicQuizQuestionRequestModel obj);
        Task<GenericResponseModel> deleteCourseTopicQuizQuestionAsync(long questionId);
        Task<GenericResponseModel> getAllCourseTopicQuizQuestionAsync();
        Task<GenericResponseModel> getAllCourseTopicQuizQuestionByQuizIdAsync(long quizId);
        Task<GenericResponseModel> getCourseTopicQuizQuestionByIdAsync(long questionId);

        //----------------------------CourseTopicQuizAnswer-------------------------------------------------------
        Task<GenericResponseModel> createCourseTopicQuizResultAsync(CourseTopicQuizResultRequestModel obj);
        Task<GenericResponseModel> getAllCourseTopicQuizResultByLearnerIdAsync(Guid learnerId);
        Task<GenericResponseModel> getCourseTopicQuizResultByIdAsync(long resultId);
    }
}
