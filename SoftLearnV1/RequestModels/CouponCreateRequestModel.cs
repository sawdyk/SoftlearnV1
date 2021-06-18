using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CouponCreateRequestModel
    {
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public long CouponPercentage { get; set; }
        [Required]
        public Guid CreatedById { get; set; }
    }
}
