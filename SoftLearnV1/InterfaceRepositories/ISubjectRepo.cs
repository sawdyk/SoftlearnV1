using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISubjectRepo
    {
        Task<GenericResponseModel> createSubjectAsync(SubjectCreationRequestModel obj);
        Task<GenericResponseModel> getSubjectByIdAsync(long subjectId);
        Task<GenericResponseModel> getAllSchoolSubjectsAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllClassSubjectsAsync(long classId, long schoolId, long campusId);
        Task<GenericResponseModel> assignSubjectToTeacherAsync(AssignSubjectToTeacherRequestModel obj);
        Task<GenericResponseModel> getAllAssignedSubjectsAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllUnAssignedSubjectsAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllSubjectsAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId);
        Task<GenericResponseModel> createSubjectDepartmentAsync(SubjectDepartmentCreateRequestModel obj);
        Task<GenericResponseModel> getAllSubjectDepartmentAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getSubjectDepartmentByIdAsync(long subjectDepartmentId);
        Task<GenericResponseModel> assignSubjectToDepartmentAsync(AssignSubjectToDepartmentRequestModel obj);
        Task<GenericResponseModel> getAllSubjectsAssignedToDepartmentAsync(long subjectDepartmentId);
        Task<GenericResponseModel> orderOfSubjectsAsync(long classId, IEnumerable<OrderOfSubjectsRequestModel> obj);
        Task<GenericResponseModel> deleteSubjectAsync(long subjectId);
        Task<GenericResponseModel> updateSubjectAsync(long subjectId, SubjectCreationRequestModel obj);
        Task<GenericResponseModel> deleteAssignedSubjectsAsync(long subjectAssignedId);
        Task<GenericResponseModel> deleteSubjectDepartmentAsync(long subjectDepartmentId);


    }
}
