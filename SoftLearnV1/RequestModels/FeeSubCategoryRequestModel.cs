using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class FeeSubCategoryRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long FeeCategoryId { get; set; }
        [Required]
        public string SubCategoryName { get; set; }
        [Required]
        public string FeeCode { get; set; }
        public string Description { get; set; }
    }
}
