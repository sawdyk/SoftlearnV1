using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class CourseTopicsResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public CourseTopicData Data { get; set; }
    }

    public class CourseTopicData
    {
        public object CourseTopic { get; set; }
        public IList<object> TopicMaterials { get; set; }
        public IList<object> TopicVideos { get; set; }
    }
}
