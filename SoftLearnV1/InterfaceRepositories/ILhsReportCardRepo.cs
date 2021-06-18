using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ILhsReportCardRepo
    {
        Task<LhsReportCardResponseModel> getReportCardDatAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);
    }
}
