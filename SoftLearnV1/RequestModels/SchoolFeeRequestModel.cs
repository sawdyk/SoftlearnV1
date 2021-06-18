using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class SchoolFeeRequestModel
    {
        public decimal UpdateAmount { get; set; }
        public bool UpdateIsMandatory { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long ClassId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        public IList<TemplateList> TemplateList { get; set; }
    }
    public class TemplateList
    {
        public decimal Amount { get; set; }
        public bool IsMandatory { get; set; }
        public long TemplateId { get; set; }
        public long FeeSubCategoryId { get; set; }
    }
}
