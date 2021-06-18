using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class LoginRequestModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }

    public class StudentLoginRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
