using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class SubmitAssignmentRequestModel
    {
        [Required]
        public string FileUrl { get; set; }
        [Required]
        public long AssignmentId { get; set; }
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
    }
}
