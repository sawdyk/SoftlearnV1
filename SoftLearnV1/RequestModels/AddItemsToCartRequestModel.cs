using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class AddItemsToCartRequestModel
    {
        [Required]
        public long CartId { get; set; }
        [Required]
        public long CourseId { get; set; }
    }
}
