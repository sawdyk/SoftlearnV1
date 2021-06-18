using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftLearnV1.Entities
{
    public class ReportCardStudentPerformance
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string TotalScoreObtained { get; set; }
        public string TotalScoreObtainable { get; set; }
        public string NoOfSubjects { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public Guid StudentId { get; set; }
        public long TermId { get; set; }
        public long SessionId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public DateTime DateComputed { get; set; }


        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }
    }
}
