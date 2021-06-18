using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class FacilitatorAccountDetailsRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
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
