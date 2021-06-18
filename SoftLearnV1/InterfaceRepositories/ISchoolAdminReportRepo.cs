using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISchoolAdminReportRepo
    {
        //----------------------------Trend---------------------------------------------------------------
        Task<GenericResponseModel> getTrendReportByClassAsync(int sessionId, int termId, long schoolId, long campusId);
        Task<GenericResponseModel> getTrendReportBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId);
        //----------------------------Top Student---------------------------------------------------------------
        Task<GenericResponseModel> getTopStudentsByClassAsync(int topNumber, int sessionId, int termId, long schoolId, long campusId, long classId);
        Task<GenericResponseModel> getTopStudentsBySubjectAsync(int topNumber, int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId);
        //----------------------------Top Student---------------------------------------------------------------
        Task<GenericResponseModel> getLowStudentsByClassAsync(int lowNumber, int sessionId, int termId, long schoolId, long campusId, long classId);
        Task<GenericResponseModel> getLowStudentsBySubjectAsync(int lowNumber, int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId);
    }
}
