using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class PaymentDisbursementRequestModel
    {
        [Required]
        public long TotalEarningsId { get; set; }
    }

    public class LearnerPaymentDisbursementRequestModel
    {
        [Required]
        public long CourseRefundId { get; set; }
    }
}
