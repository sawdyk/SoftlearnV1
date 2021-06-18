using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class Courses
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public string CourseSubTitle { get; set; }
        public long CourseTypeId { get; set; }
        public long LevelTypeId { get; set; }
        public long CourseCategoryId { get; set; }
        public long CourseSubCategoryId { get; set; }
        public string CourseImageUrl { get; set; }
        public string CourseVideoPreviewUrl { get; set; }
        public long CourseAmount { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public DateTime DateCreated { get; set; }


        [ForeignKey("FacilitatorId")]
        public virtual Facilitators Facilitators { get; set; }

        [ForeignKey("LevelTypeId")]
        public virtual CourseLevelTypes CourseLevelTypes { get; set; }

        [ForeignKey("CourseTypeId")]
        public virtual CourseType CourseType { get; set; }

        [ForeignKey("CourseCategoryId")]
        public virtual CourseCategory CourseCategory { get; set; }

        [ForeignKey("CourseSubCategoryId")]
        public virtual CourseSubCategory CourseSubCategory { get; set; }

        public ICollection<CourseRatings> CourseRatings { get; set; }
        public ICollection<CourseReviews> CourseReviews { get; set; }

    }
}
