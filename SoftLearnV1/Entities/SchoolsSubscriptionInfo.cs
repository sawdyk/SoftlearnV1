using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class SchoolsSubscriptionInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long SubscriptionPlanId { get; set; }
        public long PaymentMethodId { get; set; }
        public long AmountPaid { get; set; }
        public long Duration { get; set; }
        public bool IsVerified { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DatePaid { get; set; }
        public DateTime DueDate { get; set; }


        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlans SubscriptionPlans { get; set; }

        [ForeignKey("PaymentMethodId")]
        public virtual PaymentMethods PaymentMethods { get; set; }

    }
}
