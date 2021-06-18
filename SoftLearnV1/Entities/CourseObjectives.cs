using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseObjectives
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CourseId { get; set; }
        public string Objective { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }
    }
}
