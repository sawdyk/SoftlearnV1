using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class CourseTopicResponseModel
    {
        public long Id { get; set; }
        public Guid FacilitatorId { get; set; }
        public long CourseId { get; set; }
        public string CourseName { get; set; }
        public string Topic { get; set; }
        public DateTime DateCreated { get; set; }
        public object Material { get; set; }
        public object Video { get; set; }
        public object CourseTopicQuiz { get; set; }
    }
}
