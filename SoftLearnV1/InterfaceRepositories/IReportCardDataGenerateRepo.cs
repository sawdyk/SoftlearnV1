using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IReportCardDataGenerateRepo
    {
        Task<ReportCardDataResponseModel> getReportCardDataByStudentIdAsync(ReportCardDataRequestModel obj);
    }
}
