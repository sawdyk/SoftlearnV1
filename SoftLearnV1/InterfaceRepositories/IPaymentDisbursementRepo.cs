using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IPaymentDisbursementRepo
    {
        Task<PayStackBulkTransferResponseModel> facilitatorsTotalEarningsAsync(IList<PaymentDisbursementRequestModel> objList);
        Task<PayStackBulkTransferResponseModel>learnersCourseRefundAsync(IList<LearnerPaymentDisbursementRequestModel> objList);

    }
}
