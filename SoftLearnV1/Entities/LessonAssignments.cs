using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class LessonAssignments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid SchoolUserId { get; set; }
        public long LessonId { get; set; }
        public string Description { get; set; }
        public long PassMark { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public bool IsActive { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime DateUploaded { get; set; }

        [ForeignKey("LessonId")]
        public virtual TeacherLessons TeacherLessons { get; set; }

        [ForeignKey("SchoolUserId")]
        public virtual SchoolUsers SchoolUsers { get; set; }
 
    }
}
