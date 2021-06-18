using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseRequirementRequestModel
    {
        [Required]
        public long CourseId { get; set; }
        [Required]
        public string Requirement { get; set; }
    }

    public class MultipleCourseRequirementRequestModel
    {
        [Required]
        public long CourseId { get; set; }
        [Required]
        public IList<string> Requirement { get; set; }
    }
}
