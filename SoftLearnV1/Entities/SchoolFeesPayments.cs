using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class SchoolFeesPayments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string InvoiceCode { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public Guid StudentId { get; set; }
        public Guid ParentId { get; set; }
        public long ClassId { get; set; }
        public long ClassGradeId { get; set; }
        public long SessionId { get; set; }
        public long TermId { get; set; }
        public long InvoiceTotal { get; set; }
        public long AmountPaid { get; set; }
        public long Balance { get; set; }
        public bool IsPaymentCompleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("ParentId")]
        public virtual Parents Parents { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ClassGradeId")]
        public virtual ClassGrades ClassGrades { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SessionId")]
        public virtual Sessions Sessions { get; set; }

    }
}
