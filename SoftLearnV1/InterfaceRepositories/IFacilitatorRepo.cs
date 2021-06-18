using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IFacilitatorRepo
    {
        //-------------------------------SignUp, SignIn, Password ---------------------------------------------

        Task<GenericResponseModel> facilitatorSignUpAsync(FacilitatorRegRequestModel newFac);
        Task<GenericResponseModel> facilitatorSignUpInternalAsync(InternalFacilitatorRegRequestModel newFac);
        Task<GenericResponseModel> resetPasswordAsync(Guid userId, long sessionId, string newPassword, string userType);
        Task<GenericResponseModel> resendPasswordResetLinkAsync(string email);
        Task<GenericResponseModel> facilitatorLoginAsync(LoginRequestModel newFac);
        Task<GenericResponseModel> getAllFacilitatorAsync(long? facilitatorTypeId);
        Task<GenericResponseModel> getFacilitatorByIdAsync(Guid facilitatorId);
        Task<GenericResponseModel> activateAccountAsync(AccountActivationRequestModel obj);
        Task<GenericResponseModel> resendActivationCodeAsync(string email);
        Task<GenericResponseModel> updateProfileDetailsAsync(UpdateFacilitatorProfileRequestModel obj);
        Task<GenericResponseModel> forgotPasswordAsync(string email);
        Task<GenericResponseModel> changePasswordAsync(ChangePasswordRequestModel obj);
        Task<GenericResponseModel> updateProfilePictureAsync(Guid facilitatorId, string profilePictureUrl);

        //-------------------------Bank Account Details----------------------------------------

        Task<GenericResponseModel> createAccountDetailsAsync(FacilitatorAccountDetailsRequestModel obj);
        Task<GenericResponseModel> getAccountDetailsByIdAsync(Guid facilitatorId, long accountId);
        Task<GenericResponseModel> getAllAccountDetailsByFacilitatorIdAsync(Guid facilitatorId);
        Task<GenericResponseModel> setAccountDetailsAsDefaultAsync(Guid facilitatorId, long accountId);
        Task<GenericResponseModel> updateAccountDetailsAsync(FacilitatorAccountDetailsRequestModel obj, long accountId);
        Task<GenericResponseModel> deleteAccountDetailsAsync(long accountId);

        //-------------------------Percentage Earned on Courses----------------------------------------

        Task<GenericResponseModel> getAllPercentageEarnedOnCoursesAsync(Guid facilitatorId);
        Task<GenericResponseModel> getPercentageEarnedOnCoursesByCourseIdAsync(Guid facilitatorId, long courseId);
        Task<GenericResponseModel> getAllEarningsPerCourseAsync(Guid facilitatorId);
        Task<GenericResponseModel> getAllEarningsPerCourseByCourseIdAsync(Guid facilitatorId, long courseId);
        Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedAsync(Guid facilitatorId, DateTime date);
        Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedAndCourseIdAsync(Guid facilitatorId, long courseId, DateTime date);
        Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedRangeAsync(Guid facilitatorId, DateTime fromDate, DateTime toDate);
        Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedRangeAndCourseIdAsync(Guid facilitatorId, long courseId, DateTime fromDate, DateTime toDate);

        //------------------------------Total Earnings------------------------------------------------------------

        Task<GenericResponseModel> getTotalEarningsByDateEarnedAsync(Guid facilitatorId, DateTime date);
        Task<GenericResponseModel> getTotalEarningsByDateEarnedRangeAsync(Guid facilitatorId, DateTime fromDate, DateTime toDate);
        Task<GenericResponseModel> getTotalEarningsSettledAsync(Guid facilitatorId);
        Task<GenericResponseModel> getTotalEarningsUnSettledAsync(Guid facilitatorId);

        Task<GenericResponseModel> getFacilitatorsSettledEarningsAsync(DateTime fromDate, DateTime toDate);
        Task<GenericResponseModel> getFacilitatorsUnSettledEarningsAsync(DateTime fromDate, DateTime toDate);

    }
}
