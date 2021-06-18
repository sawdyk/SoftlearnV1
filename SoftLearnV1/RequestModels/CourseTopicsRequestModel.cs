using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{

    public class CourseTopicsRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public string Topic { get; set; }
        public string Duration { get; set; }
    }

    public class MultipleCourseTopicsRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
        [Required]
        public long CourseId { get; set; }
        public IList<TopicList> Topics { get; set; }
        
    }

    public class TopicList
    {
        [Required]
        public string Topic { get; set; }
        public string Duration { get; set; }
    }
}
