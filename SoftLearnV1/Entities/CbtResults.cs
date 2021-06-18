using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CbtResults
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid StudentId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long CbtId { get; set; }
        public long TypeId { get; set; }
        public long CategoryId { get; set; }
        public long NoOfQuestion { get; set; }
        public long RightAnswers { get; set; }
        public long WrongAnswers { get; set; }
        public long TotalScore { get; set; }
        public decimal PercentageScore { get; set; }
        public long StatuId{ get; set; }
        public DateTime DateTaken { get; set; }


        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }
      
        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("CbtId")]
        public virtual ComputerBasedTest ComputerBasedTest { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CbtCategory CbtCategory { get; set; }

        [ForeignKey("TypeId")]
        public virtual CbtTypes CbtTypes { get; set; }

        [ForeignKey("StatuId")]
        public virtual ScoreStatus ScoreStatus { get; set; }
    }
}
