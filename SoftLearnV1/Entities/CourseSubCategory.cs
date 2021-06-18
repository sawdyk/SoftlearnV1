using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseSubCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CourseCategoryId { get; set; }
        public string CourseSubCategoryName { get; set; } //Design, Marketing etc
        public string CourseSubCategoryImageUrl { get; set; } //Course SubCategory Image
        public string CourseSubCategoryDescription { get; set; }

        [ForeignKey("CourseCategoryId")]
        public virtual CourseCategory CourseCategory { get; set; }
    }
}
