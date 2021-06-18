using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class UpdateSchoolDetailsRequestModel
    {
        [Required]
        public string SchoolName { get; set; }
        [Required]
        public string SchoolLogouRL { get; set; }
    }
}
