using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IAssignmentRepo
    {
        //------------------------------------------------ASSIGNMENTS---------------------------------------------------------------
        Task<GenericResponseModel> createAssignmentAsync(AssignmentCreationRequestModel obj);
        Task<GenericResponseModel> getAssignmentByIdAsync(long assignmentId, long schoolId, long campusId);
        Task<GenericResponseModel> getAssignmentBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> updateAssignmentAsync(long assignmentId, AssignmentCreationRequestModel obj);
        Task<GenericResponseModel> deleteAssignmentAsync(long assignmentId, long schoolId, long campusId);



        //-----------------------------------------------SUBMIT AND GRADE ASSIGNMENTS-------------------------------------------------
        Task<GenericResponseModel> submitAssignmentAsync(SubmitAssignmentRequestModel obj);
        Task<GenericResponseModel> getSubmittedAssignmentByIdAsync(long assignmentSubmittedId, long schoolId, long campusId);
        Task<GenericResponseModel> getAllSubmittedAssignmentsByAssignmentIdAsync(long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getAllSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getAllUnSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericResponseModel> getSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long assignmentId, long schoolId, long campusId);
        Task<GenericResponseModel> updateSubmittedAssignmentsAsync(long assignmentSubmittedId, SubmitAssignmentRequestModel obj);
        Task<GenericResponseModel> deleteSubmittedAssignmentsAsync(long assignmentSubmittedId, long schoolId, long campusId);
        Task<GenericResponseModel> gradeSubmittedAssignmentsAsync(GradeAssignmentsRequestModel obj); 

    }
}
