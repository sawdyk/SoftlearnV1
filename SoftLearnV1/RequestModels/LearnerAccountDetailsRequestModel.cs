using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class LearnerAccountDetailsRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public string BankName { get; set; }
        [Required]
        public string BankCode { get; set; }
        [Required]
        public string AccountName { get; set; }
        [Required]
        public string AccountNumber { get; set; }
    }
}
