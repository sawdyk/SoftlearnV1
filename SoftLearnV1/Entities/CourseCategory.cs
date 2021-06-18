using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftLearnV1.Entities
{
    public class CourseCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string CourseCategoryName { get; set; } //Design, Marketing etc
        public string CategoryImageUrl { get; set; } //Course Category Image
        public string CategoryDescription { get; set; }

        public ICollection<CourseSubCategory> CourseSubCategory { get; set; }
    }
}
