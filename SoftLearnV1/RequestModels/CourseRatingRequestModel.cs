using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseRatingRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long RatingValue { get; set; } //One, Two, Three, Four Star etc
    }

    public class CourseReviewRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public string ReviewNote { get; set; }
    }
}
