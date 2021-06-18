using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IParentRepo
    {
        Task<GenericResponseModel> getParentDetailsByEmailAsync(string email, long schoolId, long campusId);
        Task<SchoolUsersLoginResponseModel> parentLoginAsync(LoginRequestModel obj);
        Task<GenericResponseModel> resendPasswordResetLinkAsync(string email);
        Task<GenericResponseModel> getParentDetailsByIdAsync(Guid parentId, long schoolId, long campusId);
        Task<GenericResponseModel> getAllParentAsync(long schoolId, long campusId);
        Task<ParentChildResponseModel> getAllParentChildAsync(Guid parentId, long schoolId, long campusId);
        //-------------------------------------ChildrenProfile-----------------------------------------------
        Task<GenericResponseModel> getChildrenProfileAsync(ChildrenProfileRequestModel obj);
        //-------------------------------------ChildrenAttendance-----------------------------------------------
        Task<GenericResponseModel> getChildrenAttendanceBySessionIdAsync(IList<Guid> childrenId, Guid parentId, long sessionId);
        Task<GenericResponseModel> getChildrenAttendanceByTermIdAsync(IList<Guid> childrenId, Guid parentId, long termId);
        Task<GenericResponseModel> getChildrenAttendanceByDateAsync(IList<Guid> childrenId, Guid parentId, DateTime startDate, DateTime endDate);
        //------------------------------------------ChildrenSubject--------------------------------------------------
        Task<GenericResponseModel> getChildrenSubjectAsync(IList<Guid> childrenId, Guid parentId);
        //-------------------------------------ChildAttendance-----------------------------------------------
        Task<GenericResponseModel> getChildAttendanceBySessionIdAsync(Guid childId, Guid parentId, long sessionId);
        Task<GenericResponseModel> getChildAttendanceByTermIdAsync(Guid childId, Guid parentId, long termId);
        Task<GenericResponseModel> getChildAttendanceByDateAsync(Guid childId, Guid parentId, DateTime startDate, DateTime endDate);
        //------------------------------------------ChildSubject--------------------------------------------------
        Task<GenericResponseModel> getChildSubjectAsync(Guid childId, Guid parentId);
    }
}
