using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class FacilitatorAccountDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsVerified { get; set; }
        public string RecipientCode { get; set; } //RecipientCode from Paystack after creation of transfer receipt
        public string BankCode { get; set; } //BankCode from Paystack API
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }

    }
}
