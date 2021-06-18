using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class BulkCourseQuizQuestionRequestModel
    {
        [Required]
        public long CourseQuizId { get; set; }
        public IList<CourseRequestModel> Questions { get; set; }
    }
    public class CourseRequestModel
    {
        [Required]
        public long QuestionTypeId { get; set; }
        [Required]
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
