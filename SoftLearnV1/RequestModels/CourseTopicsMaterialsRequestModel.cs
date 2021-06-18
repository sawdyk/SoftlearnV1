using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseTopicsMaterialsRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long CourseTopicId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileUrl { get; set; }
        [Required]
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public long Duration { get; set; }
    }

    public class MultipleCourseTopicsMaterialsRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
        [Required]
        public long CourseId { get; set; }
        [Required]
        public long CourseTopicId { get; set; }
        public IList<MaterialList> Materials { get; set; }
    }

    public class MaterialList
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public string FileUrl { get; set; }
        [Required]
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public long Duration { get; set; }
    }
}
