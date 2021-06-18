using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class UsedCouponCodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public long CartId { get; set; }
        public Guid LearnerId { get; set; }
        public DateTime DateUsed { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
    }
}
