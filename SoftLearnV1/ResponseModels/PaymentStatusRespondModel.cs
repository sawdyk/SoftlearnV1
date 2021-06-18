using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class PaymentStatusRespondModel
    {
        public long PaymentId { get; set; }
        public Guid StudentId { get; set; }
        public string AdmissionNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string InvoiceCode { get; set; }
        public long InvoiceAmount { get; set; }
        public long AmountPaid { get; set; }
        public long Balance { get; set; }
        public bool IsPaymentCompleted { get; set; }
        public bool IsInvoiceGenerated { get; set; }
    }
}
