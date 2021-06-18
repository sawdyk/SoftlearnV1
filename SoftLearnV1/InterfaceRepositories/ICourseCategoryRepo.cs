using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseCategoryRepo
    {
        Task<GenericResponseModel> createCourseCategoryAsync(CourseCategoryCreateRequestModel obj);
        Task<GenericResponseModel> getAllCourseCategoryAsync();
        Task<GenericResponseModel> getCourseCategoryByIdAsync(long courseCategoryId);
        Task<GenericResponseModel> updateCourseCategoryAsync(long courseCategoryId, CourseCategoryCreateRequestModel obj);
        Task<GenericResponseModel> deleteCourseCategoryAsync(long courseCategoryId);

        //-------------------------------- All Course Category With Pagination ------------------
        Task<GenericResponseModel> getAllCourseCategoryAsync(int pageNumber, int pageSize);
        Task<GenericResponseModel> popularCourseCategoryAsync();
        Task<GenericResponseModel> topCoursesInCourseCategoryAsync(long categoryId);


    }
}
