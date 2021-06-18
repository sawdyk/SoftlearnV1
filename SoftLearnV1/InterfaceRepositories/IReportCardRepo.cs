using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SoftLearnV1.InterfaceRepositories
{
    public interface IReportCardRepo
    {
        //---------------SAMPLE ALGORITHM TO CALCULATE STUDENTS POSITION BASED ON FINAL SCORE ACCUMULATED --------------------------------------
        Task<GenericResponseModel> getScorePositionAsync();

        //---------------COMPUTE STUDENT SCORE (EXAM AND CA) TO GET STUDENT POSITION IN CLASS--------------------------------------
        Task<ComputeResultResponseModel> computeResultAndSubjectPositionAsync(ComputeResultPositionRequestModel obj);
        Task<ComputeResultResponseModel> getAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<ComputeResultResponseModel> getComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> deleteComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> deleteAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);

    }
}
