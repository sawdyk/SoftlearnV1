using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISystemUserRepo
    {
        //-------------------------SignUp, SignIn and Account Activation----------------------------------------
        Task<SystemUserLoginResponseModel> systemUserLoginAsync(LoginRequestModel obj);
        Task<GenericResponseModel> createSystemUserAsync(SystemUserRegRequestModel model);

        //-------------------------System User Roles----------------------------------------
        Task<GenericResponseModel> getAllSystemRolesAsync();
        Task<GenericResponseModel> getSystemRolesByIdAsync(long systemRoleId);


    }
}
