﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class UploadScorePerStudentRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public long SubCategoryId { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public decimal MarkObtained { get; set; }
    }

    //CA and Examination Scores Request Model
    //Inherit from the parent class (UploadScoreRequestModel)
    public class UploadScorePerSubjectAndStudentRequestModel : UploadScorePerStudentRequestModel
    {
        [Required]
        public long SubjectId { get; set; }
    }

}
