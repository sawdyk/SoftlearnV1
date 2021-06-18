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
using Microsoft.EntityFrameworkCore;

namespace SoftLearnV1.Repositories
{
    public class CourseReviewsRepo : ICourseReviewsRepo
    {
        private readonly AppDbContext _context;

        public CourseReviewsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseModel> reviewCourseAsync(CourseReviewRequestModel obj)
        {
            try
            {
                var checkReview = _context.CourseReviews.Where(x => x.LearnerId == obj.LearnerId && x.CourseId == obj.CourseId).FirstOrDefault();

                //if course Review doesnt exists, create new Course Reviews
                if (checkReview == null)
                {
                    var rev = new CourseReviews
                    {
                        LearnerId = obj.LearnerId,
                        CourseId = obj.CourseId,
                        ReviewNote = obj.ReviewNote,
                        IsActive = true,
                        DateReviewed = DateTime.Now,
                    };

                    await _context.CourseReviews.AddAsync(rev);
                    await _context.SaveChangesAsync();
                   
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Reviewed Successfully", Data = rev };

                }
                else //Update the Course Review
                {
                    checkReview.ReviewNote = obj.ReviewNote;
                    checkReview.DateReviewed = DateTime.Now;
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Reviewed Successfully", Data = checkReview };

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
      
        public async Task<GenericResponseModel> getCourseReviewsByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseReviews
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.ReviewNote,
                                     cr.IsActive,
                                     cr.DateReviewed
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

        public async Task<GenericResponseModel> getCourseReviewsByLearnerIdAsync(Guid learnerId)
        {
            try
            {
                var result = from cr in _context.CourseReviews
                             where cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.ReviewNote,
                                 cr.IsActive,
                                 cr.DateReviewed
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

        public async Task<GenericResponseModel> deleteCourseReviewsAsync(long courseReviewId)
        {
            try
            {
                var obj = _context.CourseReviews.Where(cr => cr.Id == courseReviewId).FirstOrDefault();

                if (obj != null)
                {
                    _context.CourseReviews.Remove(obj);
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Review Deleted Successfully" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Review with the specified ID" };
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

        public async Task<GenericResponseModel> getCourseReviewsAsync(int pageNumber, int pageSize)
        {
            try
            {
                var result = (from cr in _context.CourseReviews orderby cr.Id ascending
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.ReviewNote,
                                 cr.IsActive,
                                 cr.DateReviewed
                             }).Skip((pageNumber - 1) * pageSize).Take(pageSize);

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

        public async Task<GenericResponseModel> getCourseReviewsAtRandomAsync(int noOfCourseReviews)
        {
            
            try
            {
                //gets the learners review at random
                Random rand = new Random();
                //value to skip
                int toSkip = rand.Next(0, _context.CourseReviews.Count());

                var result = (from cr in _context.CourseReviews
                              select new
                              {
                                  cr.Id,
                                  cr.LearnerId,
                                  cr.Learners.FirstName,
                                  cr.Learners.LastName,
                                  cr.CourseId,
                                  cr.Courses.CourseName,
                                  cr.ReviewNote,
                                  cr.IsActive,
                                  cr.DateReviewed
                              }).Skip(toSkip).Take(noOfCourseReviews);

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

        public async Task<GenericResponseModel> getAllFacilitatorCoursesReviewsAsync(Guid facilitatorId)
        {
            try
            {
                //using LINQ TO SQL SubQuery, returns all facilitator courses reviews
                var result = from cr in _context.CourseReviews
                          where (from cs in _context.Courses where cs.FacilitatorId == facilitatorId select cs.Id).Contains(cr.CourseId)
                          select new
                          {
                              cr.Id,
                              cr.LearnerId,
                              cr.Learners.FirstName,
                              cr.Learners.LastName,
                              cr.CourseId,
                              cr.Courses.CourseName,
                              cr.ReviewNote,
                              cr.IsActive,
                              cr.DateReviewed
                          };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result};
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
    }
}
