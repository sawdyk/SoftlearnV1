using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class AccountActivationRequestModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
