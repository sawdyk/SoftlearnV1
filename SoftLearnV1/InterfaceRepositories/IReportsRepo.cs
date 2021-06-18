using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IReportsRepo
    {
        //-------------------------Facilitators, Learners and Courses----------------------------------------
        Task<GenericResponseModel> entityReportsAsync();
        //-------------------------Facilitators, Learners and Courses Per Month----------------------------------------
        Task<MonthReportResponseModel> numberOfFacilitatorPerYearMonthAsync(int year);
        Task<MonthReportResponseModel> numberOfLearnersPerYearMonthAsync(int year);

    }
}
