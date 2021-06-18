using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseEnrolleeCompletedVideoRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public long CourseEnrolleeId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long CourseTopicId { get; set; }
        [Required]
        public long CourseTopicVideoId { get; set; }
    }
}
