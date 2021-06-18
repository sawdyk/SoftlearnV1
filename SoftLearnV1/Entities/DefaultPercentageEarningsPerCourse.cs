using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class DefaultPercentageEarningsPerCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long Percentage { get; set; }
        public DateTime LastDateUpdated { get; set; }
       
    }
}
