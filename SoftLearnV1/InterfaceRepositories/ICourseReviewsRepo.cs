using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseReviewsRepo
    {
        Task<GenericResponseModel> reviewCourseAsync(CourseReviewRequestModel obj);
        Task<GenericResponseModel> getCourseReviewsByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getCourseReviewsByLearnerIdAsync(Guid learnerId);
        Task<GenericResponseModel> deleteCourseReviewsAsync(long courseReviewId);

        //---------------------------Reviews with Pagination ------------------------------------
        Task<GenericResponseModel> getCourseReviewsAsync(int pageNumber, int pageSize);

        Task<GenericResponseModel> getCourseReviewsAtRandomAsync(int noOfCourseReviews);
        Task<GenericResponseModel> getAllFacilitatorCoursesReviewsAsync(Guid facilitatorId);

    }
}
