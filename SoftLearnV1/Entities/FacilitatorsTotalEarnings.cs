using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class FacilitatorsTotalEarnings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public decimal TotalAmountEarned { get; set; }
        public bool IsSettled { get; set; }
        public DateTime DateEarned { get; set; }
        public DateTime LastDateUpdated { get; set; }
        public DateTime DateSettled { get; set; }

        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }
       
    }
}
