using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class FeeTemplateList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long TemplateId { get; set; }
        public long FeeSubCategoryId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("TemplateId")]
        public virtual FeeTemplate FeeTemplate { get; set; }

        [ForeignKey("FeeSubCategoryId")]
        public virtual FeeSubCategory FeeSubCategory { get; set; }
    }
}
