using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseEnrolledPayments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid LearnerId { get; set; }
        public long CartId { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public string Amount { get; set; }
        public string GatewayResponse { get; set; }
        public string Channel { get; set; }
        public string Currency { get; set; }
        public DateTime Paid_At { get; set; }
        public DateTime Created_At { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
    }
}
