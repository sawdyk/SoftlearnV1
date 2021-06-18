using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseTopicQuizQuestionsRequestModel
    {
        public long CourseTopicQuizId { get; set; }
        public long QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Answer { get; set; }
    }
}
