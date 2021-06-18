using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseTopicMaterials
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public long CourseId { get; set; }
        public long CourseTopicId { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public string NoOfPages { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateUploaded { get; set; }

        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }

        [ForeignKey("CourseTopicId")]
        public virtual CourseTopics CourseTopics { get; set; }

    }
}
