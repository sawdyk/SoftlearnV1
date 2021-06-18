using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CashPaymentRequestModel
    {
        [Required]
        public string InvoiceCode { get; set; }
        [Required]
        public Guid ParentId { get; set; }
        [Required]
        public Guid FinanceUserId { get; set; }
        [Required]
        public long AmountPaid { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
