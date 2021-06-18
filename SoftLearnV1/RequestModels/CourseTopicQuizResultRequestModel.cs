using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseTopicQuizResultRequestModel
    {
        [Required]
        public Guid LearnerId { get; set; }
        [Required]
        public long CourseTopicQuizId { get; set; }
        [Required]
        public IList<Response> Data { get; set; }
    }
    public class Response
    {
        public int QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
