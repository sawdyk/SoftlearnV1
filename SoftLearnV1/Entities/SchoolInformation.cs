using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class SchoolInformation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolLogoUrl { get; set; }
        public long SchoolTypeId { get; set; }
        public bool IsVerified { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        [ForeignKey("SchoolTypeId")]
        public virtual SchoolType SchoolType { get; set; }

    }
}
