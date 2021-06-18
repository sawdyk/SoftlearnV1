using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IBankRepo
    {
        Task<GenericResponseModel> createBankAsync(BankRequestModel obj);
        Task<GenericResponseModel> updateBankAsync(long bankId, BankRequestModel obj);
        Task<GenericResponseModel> getBankByIdAsync(long bankId);
        Task<GenericResponseModel> getAllBankAsync();
        Task<GenericResponseModel> deleteBankAsync(long bankId);
    }
}
