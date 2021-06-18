﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class ReportCardData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public decimal CAScore { get; set; }
        public decimal ExamScore { get; set; }
        public decimal TotalScore { get; set; }
        public long Position { get; set; }
        public Guid StudentId { get; set; }
        public string AdmissionNumber { get; set; }
        public long? DepartmentId { get; set; }
        public long SubjectId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long AverageScore { get; set; }
        public long AveragePosition { get; set; }
        public decimal CumulativeCA_Score { get; set; }
        public decimal FirstTermTotalScore { get; set; }
        public decimal SecondTermTotalScore { get; set; }
        public decimal AverageTotalScore { get; set; }
        public string Grade { get; set; }
        public string Remark { get; set; }
        public DateTime DateComputed { get; set; }


        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("SubjectId")]
        public virtual SchoolSubjects SchoolSubjects { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual SubjectDepartment SubjectDepartment { get; set; }

    }
}
