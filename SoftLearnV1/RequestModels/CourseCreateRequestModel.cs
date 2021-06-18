using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseCreateRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string CourseDescription { get; set; }
        [Required]
        public string CourseSubTitle { get; set; }
        public string CourseImageUrl { get; set; }
        [Required]
        public long CourseTypeId { get; set; }
        [Required]
        public long LevelTypeId { get; set; }
        [Required]
        public long CourseCategoryId { get; set; }
        [Required]
        public long CourseSubCategoryId { get; set; }
        [Required]
        public long CourseAmount { get; set; }
    }
}
