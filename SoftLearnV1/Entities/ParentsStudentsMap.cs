using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class ParentsStudentsMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid StudentId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("ParentId")]
        public virtual Parents Parents { get; set; }

        [ForeignKey("StudentId")]
        public virtual Students Students { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }
    }
}
