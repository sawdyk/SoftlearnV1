using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseSubCategoryRepo
    {
        //-------------------------------- Course SubCategory -----------------------------------
        Task<GenericResponseModel> createCourseSubCategoryAsync(CourseSubCategoryRequestModel obj);
        Task<GenericResponseModel> getAllCourseSubCategoryAsync();
        Task<GenericResponseModel> getCourseSubCategoryByIdAsync(long courseSubCategoryId);
        Task<GenericResponseModel> updateCourseSubCategoryAsync(long courseSubCategoryId, CourseSubCategoryRequestModel obj);
        Task<GenericResponseModel> deleteCourseSubCategoryAsync(long courseSubCategoryId);

        Task<GenericResponseModel> getAllCourseSubCategoryByCourseCategoryIdAsync(long courseCategoryId);

        //----------------------- All Course SubCategory with pagination-------------------------
        Task<GenericResponseModel> getAllCourseSubCategoryByCourseCategoryIdAsync(int pageNumber, int pageSize, long courseCategoryId);
    }
}
