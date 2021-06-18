using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class ScoreGradeCreateRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long LowestRange { get; set; }
        [Required]
        public long HighestRange { get; set; }
        [Required]
        public string Grade { get; set; }
        [Required]
        public string Remark { get; set; }
    }
}
