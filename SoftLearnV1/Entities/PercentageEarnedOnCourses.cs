using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class PercentageEarnedOnCourses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public long CourseId { get; set; }
        public decimal Percentage { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }

        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }

    }
}
