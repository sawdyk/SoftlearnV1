using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class FacilitatorsEarningsPerCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public long CourseId { get; set; }
        public long Amount { get; set; }
        public decimal Percentage { get; set; }
        public decimal AmountEarned { get; set; }
        public DateTime DateEarned { get; set; }

        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }
    }
}
