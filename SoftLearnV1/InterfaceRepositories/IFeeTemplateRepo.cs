using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IFeeTemplateRepo
    {
        //----------------------------FeeTemplate---------------------------------------------------------------
        Task<GenericResponseModel> createFeeTemplateAsync(FeeTemplateRequestModel obj);
        Task<GenericResponseModel> updateFeeTemplateAsync(long templateId, FeeTemplateRequestModel obj);
        Task<GenericResponseModel> deleteFeeTemplateAsync(long templateId);
        Task<GenericResponseModel> getAllFeeTemplateByCampusIdAsync(long campusId);
        Task<GenericResponseModel> getAllFeeTemplateBySchoolIdAsync(long schoolId);
        Task<GenericResponseModel> getFeeTemplateByIdAsync(long templateId);
        //----------------------------FeeTemplateList---------------------------------------------------------------
        Task<GenericResponseModel> createFeeTemplateListAsync(FeeTemplateListRequestModel obj);
        Task<GenericResponseModel> updateFeeTemplateListAsync(long templateListId, FeeTemplateListRequestModel obj);
        Task<GenericResponseModel> deleteFeeTemplateListAsync(long templateId);
        Task<GenericResponseModel> deleteFeeInTemplateListAsync(long id);
        Task<GenericResponseModel> getFeeTemplateListByCampusIdAsync(long campusId, long templateId);
        Task<GenericResponseModel> getFeeTemplateListBySchoolIdAsync(long schoolId, long templateId);
        Task<GenericResponseModel> getAllFeeTemplateListByCampusIdAsync(long campusId);
        Task<GenericResponseModel> getAllFeeTemplateListBySchoolIdAsync(long schoolId);
        Task<GenericResponseModel> getFeeTemplateListByIdAsync(long templateListId);
    }
}
