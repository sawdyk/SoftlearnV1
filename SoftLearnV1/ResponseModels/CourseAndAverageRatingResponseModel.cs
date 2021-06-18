using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class CourseAndAverageRatingResponseModel
    {
        public object CourseData { get; set; }
        public decimal AverageRating { get; set; }
        public long Duration { get; set; }
    }

    public class CourseTopicAndDurationResponseModel
    {
        public object CourseTopic { get; set; }
        public long Duration { get; set; }
    }
}
