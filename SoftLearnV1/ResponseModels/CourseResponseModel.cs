using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class CourseResponseModel
    {
        public long Id { get; set; }
        public long LevelTypeId { get; set; }
        public Guid FacilitatorId { get; set; }
        public string FacilitatorFirstName { get; set; }
        public string FacilitatorLastName { get; set; }
        public string CourseName { get; set; }
        public string CourseSubTitle { get; set; }
        public string CourseDescription { get; set; }
        public string CourseImageUrl { get; set; }
        public string CourseVideoPreviewUrl { get; set; }
        public long CourseCategoryId { get; set; }
        public string CourseCategoryName { get; set; }
        public string CategoryImageUrl { get; set; }
        public string CategoryDescription { get; set; }
        public long CourseSubCategoryId { get; set; }
        public string CourseSubCategoryName { get; set; }
        public string CourseSubCategoryImageUrl { get; set; }
        public string CourseSubCategoryDescription { get; set; }
        public string LevelTypeName { get; set; }
        public string CourseTypeName { get; set; }
        public long CourseTypeId { get; set; }
        public long CourseAmount { get; set; }
        public bool IsApproved { get; set; }
        public bool IsVerified { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
