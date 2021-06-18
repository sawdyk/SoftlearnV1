using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseRefund
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid LearnerId { get; set; }
        public long CourseId { get; set; }
        public string ReasonForReturn { get; set; }
        public bool ReviewStatus { get; set; }
        public bool IsSettled { get; set; }
        public DateTime DateSettled { get; set; }
        public DateTime DateReturned { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }
    }
}
