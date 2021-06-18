using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseObjectivesRepo
    {
        //------------------------------Course Objectives----------------------------------------------------
        Task<GenericResponseModel> createCourseObjectivesAsync(CourseObjectivesRequestModel obj);
        Task<GenericResponseModel> createMultipleCourseObjectivesAsync(MultipleCourseObjectivesRequestModel obj);
        Task<GenericResponseModel> getCourseObjectivesByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getCourseObjectiveByIdAsync(long courseObjectiveId);
        Task<GenericResponseModel> deleteCourseObjectiveAsync(long courseObjectiveId);

    }
}
