using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISchoolRepo
    {
        //-------------------------SignUp, SignIn and Account Activation----------------------------------------
        Task<SchoolSignUpResponseModel> schoolSignUpAsync(SchoolSignUpRequestModel obj);
        Task<GenericResponseModel> activateAccountAsync(AccountActivationRequestModel obj);
        Task<GenericResponseModel> resendActivationCodeAsync(string email);
        Task<GenericResponseModel> updateSchoolDetailsAsync(long schoolId, UpdateSchoolDetailsRequestModel obj);
        Task<GenericResponseModel> updateCampusDetailsAsync(long campusId, SchoolCampusCreateRequestModel obj);




        //------------------------SchoolCampus------------------------------------------------------------------
        Task<GenericResponseModel> createSchoolCampusAsync(SchoolCampusCreateRequestModel obj);
        Task<GenericResponseModel> getAllSchoolCampusAsync(long schoolId);
        Task<GenericResponseModel> getSchoolCampusByIdAsync(long campusId);

        //------------------------SchoolUsers------------------------------------------------------------------
        Task<SchoolUsersCreateResponseModel> createSchoolUsersAsync(SchoolUsersCreateRequestModel obj);
        Task<SchoolUsersLoginResponseModel> schoolUserLoginAsync(LoginRequestModel obj);
        Task<GenericResponseModel> getSchoolUsersByRoleIdAsync(long schoolId, long campusId, long roleId);
        Task<GenericResponseModel> updateSchoolUserDetailsAsync(Guid schoolUserId, UpdateSchoolUsersDetailsRequestModel obj);
        Task<GenericResponseModel> deleteSchoolUsersAsync(Guid schoolUserId, long schoolId, long campusId);
        Task<GenericResponseModel> resetSchoolUserPasswordAsync(Guid userId, long sessionId, string newPassword, string userType);
        Task<GenericResponseModel> resendPasswordResetLinkAsync(string email);


        //------------------------SchoolRoles------------------------------------------------------------------
        Task<GenericResponseModel> getAllSchoolRolesAsync();
        Task<GenericResponseModel> getSchoolRolesForSchoolUserCreationAsync();
        Task<GenericResponseModel> getSchoolRolesByRoleIdAsync(long schoolRoleId);
        Task<GenericResponseModel> assignRolesToSchoolUsersAsync(AssignRolesToSchoolUsersRequestModel obj);
        Task<GenericResponseModel> deleteRolesAssignedToSchoolUsersAsync(DeleteRolesAssignedToSchoolUsersRequestModel obj);


        ////------------------------ Academic Session -------------------------------------------------------------
        //Task<GenericResponseModel> createSchoolAcademicSessionAsync(SchoolCampusCreateRequestModel obj);
        //Task<GenericResponseModel> getAllSchoolCampusAsync(long schoolId);
        //Task<GenericResponseModel> getSchoolCampusByIdAsync(long campusId);
    }
}
