using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IReportDashboardRepo
    {
        //-------------------------Parent--------------------------------------------
        
        Task<GenericResponseModel> getNoOfChildrenAsync(Guid parentId);
        Task<GenericResponseModel> getNoOfClassesForChildrenAsync(Guid parentId);
        Task<GenericResponseModel> getTotalAmountPaidForCurrentTermAsync(Guid parentId);
        Task<GenericResponseModel> getNoOfCampusesForChildrenAsync(Guid parentId);
        //-------------------------Class Teacher--------------------------------------------
        Task<GenericResponseModel> getNoOfStudentsInTeacherClassAsync(Guid teacherId);
        Task<GenericResponseModel> getNoOfMaleStudentsInTeacherClassAsync(Guid teacherId);
        Task<GenericResponseModel> getNoOfFemaleStudentsInTeacherClassAsync(Guid teacherId);
        Task<GenericResponseModel> getNoOfSubjectsInTeacherClassAsync(Guid teacherId);
        //-------------------------School Admin--------------------------------------------
        Task<GenericResponseModel> getNoOfStudentsInSchoolAsync(Guid schoolAdminId);
        Task<GenericResponseModel> getNoOfTeachersInSchoolAsync(Guid schoolAdminId);
        Task<GenericResponseModel> getNoOfNonTeachingStaffsInSchoolAsync(Guid schoolAdminId);
        Task<GenericResponseModel> getNoOfSchoolCampusesAsync(Guid schoolAdminId);
    }
}
