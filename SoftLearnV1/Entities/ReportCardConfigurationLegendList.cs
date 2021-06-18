using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftLearnV1.Entities
{
    public class ReportCardConfigurationLegendList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long LegendId { get; set; }
        public string ReferenceRange { get; set; }
        public string ReferenceValue { get; set; }

        [ForeignKey("LegendId")]
        public virtual ReportCardConfigurationLegend ReportCardConfigurationLegend { get; set; }
    }
}
