using Microsoft.AspNetCore.Http;
using SoftLearnV1.Entities;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IStudentRepo
    {
        Task<GenericResponseModel> createStudentAsync(StudentCreationRequestModel obj);
        Task<GenericResponseModel> addStudentToExistingParentAsync(StudentParentExistCreationRequestModel obj);
        Task<GenericResponseModel> resendPasswordResetLinkAsync(string email);
        Task<SchoolUsersLoginResponseModel> studentLoginAsync(StudentLoginRequestModel obj);
        Task<GenericResponseModel> getStudentByIdAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericResponseModel> assignStudentToClassAsync(AssignStudentToClassRequestModel obj);
        Task<GenericResponseModel> getStudentParentAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericResponseModel> getAllAssignedStudentAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllUnAssignedStudentAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllStudentInSchoolAsync(long schoolId);
        Task<GenericResponseModel> getAllStudentInCampusAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getStudentsBySessionIdAsync(long schoolId, long campusId, long sessionId);
        Task<GenericResponseModel> moveStudentToNewClassAndClassGradeAsync(MoveStudentRequestModel obj);
        Task<GenericResponseModel> updateStudentDetailsAsync(Guid studentId, UpdateStudentRequestModel obj);
        Task<GenericResponseModel> deleteStudentsAssignedToClassAsync(DeleteStudentAssignedRequestModel obj);
        Task<GenericResponseModel> deleteStudentAsync(Guid studentId, long schoolId, long campusId);

        //--------------------------BULK CREATION OF STUDENTS-------------------------------------------
        Task<StudentBulkCreationResponseModel> createStudentFromExcelAsync(BulkStudentRequestModel obj);

        //------------------------------DUPLICATE STUDENTS-----------------------------------------------
        Task<GenericResponseModel> getAllStudentDuplicatesAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getStudentDuplicateByStudentIdAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericResponseModel> updateStudentDuplicateAsync(StudentDuplicateRequestModel obj);
        Task<GenericResponseModel> deleteStudentDuplicateAsync(Guid studentId, long schoolId, long campusId);
    }
}
