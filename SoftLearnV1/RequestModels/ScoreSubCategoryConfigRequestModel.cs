using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class ScoreSubCategoryConfigRequestModel
    {
        [Required]
        public long CategoryId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public string SubCategoryName { get; set; }
        [Required]
        public long ScoreObtainable { get; set; }
    }
}
