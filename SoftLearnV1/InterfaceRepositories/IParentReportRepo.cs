using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IParentReportRepo
    {
        //----------------------------TestPerformance---------------------------------------------------------------
        Task<GenericResponseModel> getTestPerformanceByTermAsync(Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId);
        //----------------------------ExamPerformance---------------------------------------------------------------
        Task<GenericResponseModel> getExamPerformanceByTermAsync(Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId);
        //----------------------------TopScore---------------------------------------------------------------
        Task<GenericResponseModel> getTopTestPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId);
        Task<GenericResponseModel> getTopExamPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId);
        Task<GenericResponseModel> getTopTotalPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId);
        //----------------------------Trend---------------------------------------------------------------
        Task<GenericResponseModel> getTrendReportbySubjectTestAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId);
        Task<GenericResponseModel> getTrendReportbySubjectExamAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId);
        Task<GenericResponseModel> getTrendReportbySubjectAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId);
    }
}
