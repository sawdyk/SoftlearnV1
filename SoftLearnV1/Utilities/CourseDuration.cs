using SoftLearnV1.Helpers;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Utilities
{
    public class Duration
    {
        private readonly AppDbContext _context;
        IList<CourseTopicAndDurationResponseModel> respList = new List<CourseTopicAndDurationResponseModel>();
        public Duration(AppDbContext context)
        {
            _context = context;
        }

        public long CourseDuration(long courseId)
        {
            long total = 0;
            var courseVideos = _context.CourseTopicVideos.Where(x => x.CourseId == courseId && x.IsApproved == true).ToList();
            foreach (var courseVideo in courseVideos)
            {
                total += Convert.ToInt64(courseVideo.Duration);
            }
            return total;
        }

        public long CourseTopicDuration(long courseTopicId, bool? isVideoApproved = true)
        {
            long total = 0;

            if (isVideoApproved == false)
            {
                var courseVideos = _context.CourseTopicVideos.Where(x => x.CourseTopicId == courseTopicId).ToList();
                foreach (var courseVideo in courseVideos)
                {
                    total += Convert.ToInt64(courseVideo.Duration);
                }
            }
            else
            {
                var courseVideos = _context.CourseTopicVideos.Where(x => x.CourseTopicId == courseTopicId && x.IsApproved == true).ToList();
                foreach (var courseVideo in courseVideos)
                {
                    total += Convert.ToInt64(courseVideo.Duration);
                }
            }
            return total;
        }

        public IList<CourseTopicAndDurationResponseModel> CourseTopicAndDuration(IEnumerable<CourseTopicResponseModel> courseTopics, bool? isVideoApproved = true)
        {
            foreach (var rslt in courseTopics)
            {
                //Response Obj
                CourseTopicAndDurationResponseModel resp = new CourseTopicAndDurationResponseModel
                {
                    CourseTopic = rslt,
                    Duration = CourseTopicDuration(rslt.Id, isVideoApproved)
                };
                //response List
                respList.Add(resp);
            }
            return respList;
        }
    }
}
