using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CartReferenceId { get; set; }
        public bool IsCheckedOut { get; set; }
        public long SubTotal { get; set; }
        public long TotalCourse { get; set; }
        public string CouponCode { get; set; }
        public long TotalAmountPayable { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }
    }
}
