using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Utilities
{
    public class CourseRating
    {
        private readonly AppDbContext _context;
        private readonly Duration courseDuration;

        public CourseRating(AppDbContext context, Duration courseDuration)
        {
            _context = context;
            this.courseDuration = courseDuration;
        }
        IList<CourseAndAverageRatingResponseModel> respList = new List<CourseAndAverageRatingResponseModel>();
        public IList<CourseAndAverageRatingResponseModel> AverageRating(IEnumerable<CourseResponseModel> courses)
        {
            foreach (var rslt in courses)
            {
                //Response Obj
                CourseAndAverageRatingResponseModel resp = new CourseAndAverageRatingResponseModel();

                decimal averageRatings = 0;
                //Average Rating
                var courseRatings = (from cr in _context.CourseRatings
                                     where cr.CourseId == rslt.Id
                                     select Convert.ToDecimal(cr.RatingValue)).ToList();
                //Converts to array
                decimal[] ratings = courseRatings.ToArray();

                //Check if the array contains items
                if (ratings.Length > 0)
                {
                    averageRatings = ratings.Average();
                }
                //response object
                resp.CourseData = rslt;
                resp.AverageRating = averageRatings;
                resp.Duration = courseDuration.CourseDuration(rslt.Id);

                //response List
                respList.Add(resp);

            }

            return respList;
        }
    }
}
