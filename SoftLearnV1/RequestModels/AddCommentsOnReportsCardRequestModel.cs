﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    //Parent Class
    public class AddCommentsOnReportsCardRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long ClassGradeId { get; set; }
        [Required]
        public long CommentConfigId { get; set; }
        [Required]
        public Guid UploadedById { get; set; }
       
    }

    public class CommentsAndRemarks
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public string Remark { get; set; }
    }

    //Inherits from the parent Class
    public class CommentsOnReportsCardForAllStudent : AddCommentsOnReportsCardRequestModel
    {
        [Required]
        public IList<CommentsAndRemarks> Comments { get; set; }
    }

    //Inherits from the parent Class
    public class CommentsOnReportsCardForSingleStudent : AddCommentsOnReportsCardRequestModel
    {
        [Required]
        public CommentsAndRemarks Comments { get; set; }
    }
}
