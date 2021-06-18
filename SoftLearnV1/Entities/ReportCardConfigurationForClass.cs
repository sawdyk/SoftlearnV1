using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class ReportCardConfigurationForClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ConfigurationId { get; set; } 
        public long SchoolId { get; set; }
        public long TermId { get; set; }
        public long ClassId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("TermId")]
        public virtual Terms Terms { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

        [ForeignKey("ClassId")]
        public virtual Classes Classes { get; set; }

        [ForeignKey("ConfigurationId")]
        public virtual ReportCardConfiguration ReportCardConfiguration { get; set; }
    }
}
