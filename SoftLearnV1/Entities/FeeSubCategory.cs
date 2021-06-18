using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class FeeSubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long FeeCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string FeeCode { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("FeeCategoryId")]
        public virtual FeeCategory FeeCategory { get; set; }

    }
}
