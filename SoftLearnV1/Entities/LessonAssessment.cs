using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class LessonAssessment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string AssessmentName { get; set; }
        public long LessonId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long Duration { get; set; }
        public long PassMark { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("LessonId")]
        public virtual TeacherLessons TeacherLessons { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }
    }
}
