using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseTopics
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public long CourseId { get; set; }
        public string Topic { get; set; }
        public string Duration { get; set; }
        public string TopicDescription { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }

        public ICollection<CourseTopicMaterials> CourseTopicMaterials  { get; set; }
        public ICollection<CourseTopicVideos> CourseTopicVideos { get; set; }
        public ICollection<CourseTopicQuiz> CourseTopicQuiz { get; set; }

    }
}
