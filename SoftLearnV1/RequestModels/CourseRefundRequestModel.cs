using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseRefundRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public string ReasonForReturn { get; set; }
    }
}
