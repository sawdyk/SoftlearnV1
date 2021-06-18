using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class SubscriptionPlanFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SubscriptionPlanId { get; set; }
        public long Fee { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


        [ForeignKey("SubscriptionPlanId")]
        public virtual SubscriptionPlans SubscriptionPlans { get; set; }
    }
}
