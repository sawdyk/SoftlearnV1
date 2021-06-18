using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class UpdateLearnerProfileRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Bio { get; set; }
    }
}
