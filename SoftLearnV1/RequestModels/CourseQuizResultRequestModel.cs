using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseQuizResultRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public long CourseQuizId { get; set; }
        [Required]
        public IList<CourseResponse> Data { get; set; }
    }
    public class CourseResponse
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}

