using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class ComputerBasedTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Description { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SubjectId { get; set; }
        public Guid TeacherId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long TypeId { get; set; }
        public long CategoryId { get; set; }
        public long Duration { get; set; }
        public long PassMark { get; set; }
        public string TermsAndConditions { get; set; }
        public long StatusId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastDateUpdated { get; set; }


        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("SubjectId")]
        public virtual SchoolSubjects SchoolSubjects { get; set; }

        [ForeignKey("TeacherId")]
        public virtual SchoolUsers SchoolUsers { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("CategoryId")]
        public virtual CbtCategory CbtCategory { get; set; }

        [ForeignKey("TypeId")]
        public virtual CbtTypes CbtTypes { get; set; }

        [ForeignKey("StatusId")]
        public virtual ActiveInActiveStatus ActiveInActiveStatus { get; set; }

    }
}
