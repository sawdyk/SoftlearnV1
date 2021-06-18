using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class ReportCardConfigurationLegend
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string LegendName { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long StatusId { get; set; }
        public long TermId { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public Guid LastUpdatedBy { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("CreatedBy")]
        public virtual SchoolUsers SchoolUsersCreatedBy { get; set; }

        [ForeignKey("LastUpdatedBy")]
        public virtual SchoolUsers SchoolUsersLastUpdatedBy { get; set; }

        [ForeignKey("StatusId")]
        public virtual ActiveInActiveStatus ActiveInActiveStatus { get; set; }

        public ICollection<ReportCardConfigurationLegendList> ReportCardConfigurationLegendList { get; set; }

    }
}
