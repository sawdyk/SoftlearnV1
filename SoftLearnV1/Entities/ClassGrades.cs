using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftLearnV1.Entities
{
    public class ClassGrades
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ClassId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public string GradeName { get; set; }
        public bool isAssignedToTeacher { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }
    }
}
