using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IFeeCategoryRepo
    {
        //----------------------------FeeCategory---------------------------------------------------------------
        Task<GenericResponseModel> createFeeCategoryAsync(FeeCategoryRequestModel obj);
        Task<GenericResponseModel> updateFeeCategoryAsync(long categoryId, FeeCategoryRequestModel obj);
        Task<GenericResponseModel> deleteFeeCategoryAsync(long categoryId);
        Task<GenericResponseModel> getAllFeeCategoryByCampusIdAsync(long campusId);
        Task<GenericResponseModel> getAllFeeCategoryBySchoolIdAsync(long schoolId);
        Task<GenericResponseModel> getFeeCategoryByIdAsync(long categoryId);
        //----------------------------FeeSubCategory---------------------------------------------------------------
        Task<GenericResponseModel> createFeeSubCategoryAsync(FeeSubCategoryRequestModel obj);
        Task<GenericResponseModel> updateFeeSubCategoryAsync(long subCategoryId, FeeSubCategoryRequestModel obj);
        Task<GenericResponseModel> deleteFeeSubCategoryAsync(long subCategoryId);
        Task<GenericResponseModel> getAllFeeSubCategoryByCategoryIdAsync(long categoryId);
        Task<GenericResponseModel> getAllFeeSubCategoryByCampusIdAsync(long campusId);
        Task<GenericResponseModel> getAllFeeSubCategoryBySchoolIdAsync(long schoolId);
        Task<GenericResponseModel> getFeeSubCategoryByIdAsync(long subCategoryId);
    }
}
