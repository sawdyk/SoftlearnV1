using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICouponCodeRepo
    {
        Task<GenericResponseModel> createCouponCodesAsync(CouponCreateRequestModel obj);
        Task<GenericResponseModel> getAllCouponCodesAsync();
        Task<GenericResponseModel> getCouponCodesByIdAsync(long couponCodeId);
        Task<GenericResponseModel> getCouponCodesByCouponCodeAsync(string couponCode);
        Task<GenericResponseModel> applyCouponCodesAsync(ApplyCouponCodeRequestModel obj);

    }
}
