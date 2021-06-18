using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class Teachers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid SchoolUserId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public bool IsAssignedToClass { get; set; }
        public bool IsAssignedSubjects{ get; set; }
        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("SchoolUserId")]
        public virtual SchoolUsers SchoolUsers { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }
    }
}
