using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class Certificates
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid LearnerId { get; set; }
        public long CourseId { get; set; }
        public string CertificateNumber { get; set; }
        public DateTime DateGenerated { get; set; }
      
        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

        [ForeignKey("CourseId")]
        public virtual Courses Courses { get; set; }
    }
}
