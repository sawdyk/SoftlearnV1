using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseEnrollees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid LearnerId { get; set; }
        public long CourseId { get; set; }
        public bool IsInProgress { get; set; }
        public decimal PercentageCompletion { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateCompleted { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateEnrolled { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }

    }
}
