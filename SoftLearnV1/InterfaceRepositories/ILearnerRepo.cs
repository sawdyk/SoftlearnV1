using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ILearnerRepo
    {
        Task<GenericResponseModel> learnerSignUpAsync(LearnerRegRequestModel obj);
        Task<GenericResponseModel> learnerLoginAsync(LoginRequestModel obj);
        Task<GenericResponseModel> getAllLearnersAsync();
        Task<GenericResponseModel> getLearnerByIdAsync(Guid learnerId);
        Task<GenericResponseModel> activateAccountAsync(AccountActivationRequestModel obj);
        Task<GenericResponseModel> resendActivationCodeAsync(string email);
        Task<GenericResponseModel> forgotPasswordAsync(string email);
        Task<GenericResponseModel> changePasswordAsync(ChangePasswordRequestModel obj);
        Task<GenericResponseModel> updateProfileDetailsAsync(UpdateLearnerProfileRequestModel obj);
        Task<GenericResponseModel> updateProfilePictureAsync(Guid learnerId, string profilePictureUrl);

        //-------------------------Bank Account Details----------------------------------------

        Task<GenericResponseModel> createAccountDetailsAsync(LearnerAccountDetailsRequestModel obj);
        Task<GenericResponseModel> getAccountDetailsByIdAsync(Guid learnerId, long accountId);
        Task<GenericResponseModel> getAllAccountDetailsByLearnerIdAsync(Guid learnerId);
        Task<GenericResponseModel> updateAccountDetailsAsync(LearnerAccountDetailsRequestModel obj, long accountId);
        Task<GenericResponseModel> deleteAccountDetailsAsync(long accountId);

    }
}
