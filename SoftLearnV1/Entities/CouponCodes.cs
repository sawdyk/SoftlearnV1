using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CouponCodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CouponCode { get; set; }
        public long CouponPercentage { get; set; }
        public Guid CreatedById { get; set; }
        public bool IsUsed { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("CreatedById")]
        public virtual SystemUsers SystemUsers { get; set; }
    }
}
