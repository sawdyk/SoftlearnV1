using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class LearnersPaymentDisbursements
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid LearnerId { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Reference { get; set; }
        public string Integration { get; set; }
        public string Domain { get; set; }
        public long Amount { get; set; }
        public string Currency { get; set; }
        public string Source { get; set; }
        public string Reason { get; set; }
        public long Recipient { get; set; }
        public string DataStatus { get; set; }
        public string TransferCode { get; set; }
        public long DataId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

    }
}
