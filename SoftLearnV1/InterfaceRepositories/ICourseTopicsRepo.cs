using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseTopicsRepo
    {
        //-------------------------COURSE TOPICS--------------------------------------------

        Task<CourseTopicsResponseModel> createCourseTopicsAsync(CourseTopicsRequestModel obj);
        Task<CourseTopicsResponseModel> createMultipleCourseTopicsAsync(MultipleCourseTopicsRequestModel obj);
        Task<GenericResponseModel> getAllCourseTopicsAsync();
        Task<GenericResponseModel> getCourseTopicsByIdAsync(long courseTopicId);
        Task<GenericResponseModel> getAllCourseTopicsByCourseIdAsync(long courseId);
        Task<GenericResponseModel> getAllCourseTopicsByCourseIdWithApprovedDataAsync(long courseId);
        Task<GenericResponseModel> getAllCourseTopicsByFacilitatorIdAsync(Guid facilitatorId);
        Task<GenericResponseModel> deleteCourseTopicsAsync(long courseTopicId);


        //-------------------------COURSE TOPICS MATERIALS/RESOURCES--------------------------------------------

        Task<GenericResponseModel> approveCourseTopicMaterialAsync(long courseTopicMateriaId);
        Task<GenericResponseModel> createCourseTopicMaterialsAsync(CourseTopicsMaterialsRequestModel obj);
        Task<GenericResponseModel> createMultipleCourseTopicMaterialsAsync(MultipleCourseTopicsMaterialsRequestModel obj);
        Task<GenericResponseModel> getCourseTopicMaterialsByIdAsync(long courseTopicMaterialId);
        Task<GenericResponseModel> getCourseTopicMaterialsByCourseTopicIdAsync(long courseTopiclId);
        Task<GenericResponseModel> getAllCourseTopicMaterialsByCourseIdAsync(long courseId);
        Task<GenericResponseModel> deleteCourseTopicMaterialAsync(long courseTopicMaterialId);



        //-------------------------COURSE TOPICS VIDEOS--------------------------------------------

        Task<GenericResponseModel> approveCourseTopicVideoAsync(long courseTopicVideoId);
        Task<GenericResponseModel> createCourseTopicVideosAsync(CourseTopicsMaterialsRequestModel obj);
        Task<GenericResponseModel> createMultipleCourseTopicVideosAsync(MultipleCourseTopicsMaterialsRequestModel obj);
        Task<GenericResponseModel> getCourseTopicVideosByIdAsync(long courseTopicVideoId);
        Task<GenericResponseModel> getCourseTopicVideosByCourseTopicIdAsync(long courseTopicId);
        Task<GenericResponseModel> getAllCourseTopicVideosByCourseIdAsync(long courseId);
        Task<GenericResponseModel> deleteCourseTopicVideosAsync(long courseTopicVideoId);

        //-------------------------COURSE TOPICS VIDEO MATERIALS--------------------------------------------
        Task<GenericResponseModel> approveCourseTopicVideoMaterialAsync(long courseTopicVideoMateriaId);
        Task<GenericResponseModel> createCourseTopicVideoMaterialsAsync(CourseTopicVideoMaterialsRequestModel obj);
        Task<GenericResponseModel> createMultipleCourseTopicVideoMaterialsAsync(MultipleCourseTopicVideoMaterialsRequestModel obj);
        Task<GenericResponseModel> getCourseTopicVideoMaterialsByIdAsync(long courseTopicVideoMaterialId);
        Task<GenericResponseModel> getAllCourseTopicVideoMaterialsByVideoIdAsync(long courseTopicVideoId, bool? isApproved);
        Task<GenericResponseModel> getAllCourseTopicVideoMaterialsByCourseTopicIdAsync(long courseTopicId, bool? isApproved);
        Task<GenericResponseModel> deleteCourseTopicVideoMaterialsAsync(long courseTopicVideoMaterialId);

        //------------------------------- Course Topic Completed Video-----------------------------------------------------------
        Task<GenericResponseModel> createCourseTopicCompletedVideoAsync(CourseEnrolleeCompletedVideoRequestModel obj);
        Task<GenericResponseModel> getCourseTopicCompletedVideoByCourseIdAsync(long courseId, long courseEnrolleeId, Guid learnerId);
        Task<GenericResponseModel> getCourseTopicCompletedVideoByCourseTopicIdAsync(long courseTopicId, long courseEnrolleeId, Guid learnerId);
        Task<GenericResponseModel> getCourseTopicCompletedVideoByVideoIdAsync(long videoId, long courseEnrolleeId, Guid learnerId);
    }
}
