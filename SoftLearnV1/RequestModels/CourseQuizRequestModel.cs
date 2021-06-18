using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseQuizRequestModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public long CourseId { get; set; }
        public long Duration { get; set; }
        [Required]
        public long PercentagePassMark { get; set; }
        public bool Status { get; set; }
    }
}
