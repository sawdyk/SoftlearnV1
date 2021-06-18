using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class PaymentRequestModel
    {
        [Required]
        public string InvoiceCode { get; set; }
        [Required]
        public Guid ParentId { get; set; }
        [Required]
        public long PaymentMethodId { get; set; }
        [Required]
        public string BankName { get; set; }
        public string DepositorsAccountName { get; set; }
        [Required]
        public string ReferenceCode { get; set; }
        public string CardType { get; set; } //Visa, Verve, Master etc
        [Required]
        public long AmountPaid { get; set; }
        [Required]
        public DateTime PaymentDate { get; set; }
    }
}
