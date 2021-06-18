using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseRatingRepo                
    {
        Task<GenericResponseModel> rateCourseAsync(CourseRatingRequestModel obj);
        Task<GenericResponseModel> getCourseRatingByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getCourseRatingByLearnerIdAsync(Guid learnerId);
        Task<GenericResponseModel> getCourseRatingByRatingValueAsync(long courseId, long ratingValue);
        Task<GenericResponseModel> deleteCourseRatingAsync(long courseRatingId);
        Task<GenericResponseModel> courseAverageRatingAsync(long courseId);

    }
}
