using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class SessionCreateRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public string SessionName { get; set; }
    }
}
