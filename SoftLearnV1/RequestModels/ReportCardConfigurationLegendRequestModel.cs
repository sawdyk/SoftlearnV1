using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace SoftLearnV1.RequestModels
{
    public class ReportCardConfigurationLegendRequestModel
    {
        [Required]
        public string LegendName { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long StatusId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public Guid CreatedOrUpdatedBy { get; set; }
        public IList<LegendList> LegendList { get; set; }
    }

    public class LegendList
    {
        public string ReferenceRange { get; set; }
        public string ReferenceValue { get; set; }
    }

    public class UpdateLegendRequestModel
    {
        [Required]
        public string LegendName { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long StatusId { get; set; }
        [Required]
        public long TermId { get; set; }
        [Required]
        public Guid CreatedOrUpdatedBy { get; set; }
    }
}
