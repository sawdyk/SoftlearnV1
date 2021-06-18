using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class BankRequestModel
    {
        [Required]
        public string BankName { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
