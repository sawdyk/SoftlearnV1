using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseRequirementRepo
    {
        //------------------------------Course Requirements----------------------------------------------------
        Task<GenericResponseModel> createCourseRequirementAsync(CourseRequirementRequestModel obj);
        Task<GenericResponseModel> createMultipleCourseRequirementAsync(MultipleCourseRequirementRequestModel obj);

        Task<GenericResponseModel> getCourseRequirementByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getCourseRequirementByIdAsync(long courseRequirementId);
        Task<GenericResponseModel> deleteCourseRequirementAsync(long courseRequirementId);
    }
}
