 using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class SchoolSubjects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ClassId { get; set; }
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public long MaximumScore { get; set; }
        public long ReportCardOrder { get; set; }
        public long? DepartmentId { get; set; }
        public bool IsAssignedToClass { get; set; }
        public bool IsAssignedToTeacher { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("CampusId")]
        public virtual SchoolCampuses SchoolCampuses { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual SubjectDepartment SubjectDepartment { get; set; }

    }
}
