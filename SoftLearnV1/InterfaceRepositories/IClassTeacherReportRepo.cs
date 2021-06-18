using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IClassTeacherReportRepo
    {
        //----------------------------Performance---------------------------------------------------------------
        Task<GenericResponseModel> getTestPerformanceBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId);
        Task<GenericResponseModel> getExamPerformanceBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId);
    }
}
