using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class TeacherLessons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid SchoolUserId { get; set; }
        public long LessonName { get; set; }
        public long SubjectId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public long StartTime { get; set; }
        public long Duration { get; set; }
        public bool isActive { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("SchoolUserId")]
        public virtual SchoolUsers SchoolUsers { get; set; }

        [ForeignKey("SubjectId")]
        public virtual SchoolSubjects SchoolSubjects { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }
    }
}
