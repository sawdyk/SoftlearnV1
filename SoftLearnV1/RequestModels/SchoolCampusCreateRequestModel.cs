using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class SchoolCampusCreateRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }
    }
}
