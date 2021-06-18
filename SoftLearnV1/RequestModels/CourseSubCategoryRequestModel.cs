using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseSubCategoryRequestModel
    {
        [Required]
        public long CourseCategoryId { get; set; }
        [Required]
        public string CourseSubCategoryName { get; set; } //Design, Marketing etc
        public string CourseSubCategoryImageUrl { get; set; } //Course SubCategory Image
        public string CourseSubCategoryDescription { get; set; }

    }
}
