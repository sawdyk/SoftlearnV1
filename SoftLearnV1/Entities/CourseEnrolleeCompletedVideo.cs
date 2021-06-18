using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseEnrolleeCompletedVideo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid LearnerId { get; set; }
        public long CourseEnrolleeId { get; set; }
        public long CourseId { get; set; }
        public long CourseTopicId { get; set; }
        public long CourseTopicVideoId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }

        [ForeignKey("CourseTopicId")]
        public virtual CourseTopics CourseTopics { get; set; }
    
        [ForeignKey("CourseEnrolleeId")]
        public virtual CourseEnrollees CourseEnrollees { get; set; }

        [ForeignKey("CourseTopicVideoId")]
        public virtual CourseTopicVideos CourseTopicVideos { get; set; }
    }
}
