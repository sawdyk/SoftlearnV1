using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IFinanceReportRepo
    {
        //----------------------------SchoolFeePaymentStatus---------------------------------------------------------------
        Task<GenericResponseModel> getFeePaymentStatusAsync(int sessionId, int termId, long schoolId, long classId, long gradeId);
        //----------------------------PaymentMethod---------------------------------------------------------------
        Task<GenericResponseModel> getAllFeePaymentByMethodAsync(int methodId, int sessionId, int termId, long schoolId, long classId, long gradeId);
        Task<GenericResponseModel> getAllFeePaymentTotalByMethodAsync(int sessionId, int termId, long schoolId, long classId, long gradeId);
    }
}