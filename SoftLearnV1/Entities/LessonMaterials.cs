using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class LessonMaterials
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid TeacherId { get; set; }
        public long LessonId { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime DateUploaded { get; set; }

        [ForeignKey("TeacherId")]
        public virtual SchoolAdmin SchoolUsers { get; set; }

        [ForeignKey("LessonId")]
        public virtual TeacherLessons TeacherLessons { get; set; }


    }
}
