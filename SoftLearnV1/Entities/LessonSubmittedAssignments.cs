using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class LessonSubmittedAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid StudentId { get; set; }
        public long LessonId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public DateTime DateSubmitted { get; set; }


        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("LessonId")]
        public virtual TeacherLessons TeacherLessons { get; set; }
    }
}
