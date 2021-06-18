using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class ReportCardCommentListRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public Guid UploadedById { get; set; }
    }

    public class CommentList
    {
        [Required]
        public string Comment { get; set; }
    }

    //Inherits from the parent Class
    public class CommentListRequestModel : ReportCardCommentListRequestModel
    {
        [Required]
        public IList<CommentList> Comments { get; set; }
    }

    //inherits from the Parent Class
    public class UpdateCommentRequestModel : ReportCardCommentListRequestModel
    {
        [Required]
        public string Comment { get; set; }
    }
}
