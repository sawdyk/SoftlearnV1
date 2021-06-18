using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ITeacherRepo
    {
        Task<TeacherCreateResponseModel> createTeacherAsync(TeacherCreateRequestModel obj);
        Task<GenericResponseModel> getTeacherByIdAsync(Guid teacherId, long schoolId, long campusId);
        Task<GenericResponseModel> getTeacherRolesAsync();
        Task<GenericResponseModel> getAllTeachersAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllTeachersByRoleIdAsync(long roleId, long schoolId, long campusId);
        Task<GenericResponseModel> assignTeacherToClassGradeAsync(AssignTeacherToClassRequestModel obj);
        Task<GenericResponseModel> getAllClassGradeAssignedToTeacherAsync(long schoolId, long campusId, long classId, Guid teacherId);
        Task<GenericResponseModel> getAllClassAssignedToTeacherAsync(long schoolId, long campusId, Guid teacherId);
        Task<GenericResponseModel> getAssignedTeachersAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllRolesAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId);

        //-----------------------------------ATTENDANCE---------------------------------------------------------------
        Task<GenericResponseModel> takeClassAttendanceAsync(TakeAttendanceRequestModel obj);
        Task<GenericResponseModel> getClassAttendanceAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getClassGradeAttendanceAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getClassAttendanceByPeriodIdAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId);
        Task<GenericResponseModel> getClassGradeAttendanceByPeriodIdAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId);
        Task<GenericResponseModel> getStudentAttendanceAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getStudentAttendanceByPeriodIdAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId);

    }
}
