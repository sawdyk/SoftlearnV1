using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CourseRatingRepo : ICourseRatingRepo
    {
        private readonly AppDbContext _context;

        public CourseRatingRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseModel> rateCourseAsync(CourseRatingRequestModel obj)
        {
            try
            {
                var checkRating = _context.CourseRatings.Where(x => x.LearnerId == obj.LearnerId && x.CourseId == obj.CourseId).FirstOrDefault();

                //if course rating doesnt exists, create new Course Ratings
                if (checkRating == null)
                {
                    var rate = new CourseRatings
                    {
                        LearnerId = obj.LearnerId,
                        CourseId = obj.CourseId,
                        RatingValue = obj.RatingValue,
                        DateRated = DateTime.Now,
                    };

                    await _context.CourseRatings.AddAsync(rate);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Rated Successfully", Data = rate };

                }
                else //Update the Course Ratings
                {
                    checkRating.RatingValue = obj.RatingValue;
                    checkRating.DateRated = DateTime.Now;
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Rated Successfully", Data = checkRating };

                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getCourseRatingByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseRatings
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                    cr.Id,
                                    cr.LearnerId,
                                    cr.Learners.FirstName,
                                    cr.Learners.LastName,
                                    cr.CourseId,
                                    cr.Courses.CourseName,
                                    cr.RatingValue,
                                    cr.DateRated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getCourseRatingByLearnerIdAsync(Guid learnerId)
        {
            try
            {
                var result = from cr in _context.CourseRatings
                             where cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.RatingValue,
                                 cr.DateRated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };


            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getCourseRatingByRatingValueAsync(long courseId, long ratingValue)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseRatings
                                 where cr.CourseId == courseId && cr.RatingValue == ratingValue
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.RatingValue,
                                     cr.DateRated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> deleteCourseRatingAsync(long courseRatingId)
        {
            try
            {
                var obj = _context.CourseRatings.Where(cr => cr.Id == courseRatingId).FirstOrDefault();

                if (obj != null)
                {
                    _context.CourseRatings.Remove(obj);
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Rating Deleted Successfully" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Rating with the specified ID" };
                }
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> courseAverageRatingAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var average = (from cr in _context.CourseRatings
                                   where cr.CourseId == courseId
                                   select Convert.ToDecimal(cr.RatingValue)).Average();

                   // //gets the average rating value of a Course
                   // var result = _context.CourseRatings
                   //.Where(r => r.CourseId == courseId);
                   // .GroupBy(g => g.RatingValue)
                   // .Select(g => new
                   // {
                   //     Average_Rating = Convert.ToDecimal(g.Average())
                   // });

                   // if (result.Count() > 0)
                   // {
                   //     return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                   // }

                   // return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = average};

                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
    }
}
