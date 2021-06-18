using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class InvoiceGenerationRequestModel
    {
        [Required]
        public Guid ChildId { get; set; }
        [Required]
        public Guid ParentId { get; set; }
        [Required]
        public long SessionId { get; set; }
        [Required]
        public long TermId { get; set; }
        public IList<fees> feeList { get; set; }
    }
    public class fees
    {
        [Required]
        public long subCategoryId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public bool IsMandatory { get; set; }
    }
}
