using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class SchoolBasicInfoRequestModel
    {

        [Required]
        public Guid SchoolAdministratorId { get; set; }
        [Required]
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        [Required]
        public long SchoolTypeId { get; set; }
        [Required]
        public string Address { get; set; }

    }
}
