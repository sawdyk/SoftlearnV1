using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
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
    public class CourseRequirementRepo : ICourseRequirementRepo
    {
        private readonly AppDbContext _context;

        public CourseRequirementRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseModel> createCourseRequirementAsync(CourseRequirementRequestModel obj)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(obj.CourseId);
                //check if a course objective to be created already exists
                var checkResult = _context.CourseRequirements.Where(x => x.CourseId == obj.CourseId && x.Requirement == obj.Requirement).FirstOrDefault();

                if (checkCourse != true)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "A Course With the specified ID doesnt exist!", };
                }

                //if the course requirement doesnt exist, Create the course requirement
                if (checkResult == null)
                {
                    var courseReq = new CourseRequirements
                    {
                        CourseId = obj.CourseId,
                        Requirement = obj.Requirement,
                        IsActive = true,
                        DateCreated = DateTime.Now,
                    };

                    await _context.CourseRequirements.AddAsync(courseReq);
                    await _context.SaveChangesAsync();

                    //get all the Course Requirements of the Course
                    var crsResult = (from cr in _context.CourseRequirements
                                     where cr.CourseId == obj.CourseId
                                     select new
                                     {
                                         cr.Id,
                                         cr.CourseId,
                                         cr.Courses.CourseName,
                                         cr.Requirement,
                                         cr.IsActive,
                                         cr.DateCreated,
                                     }).OrderByDescending(c => c.Id);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Requirements Added Successfully!", Data = crsResult.ToList() };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Course Requirement Already Exists!", };

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> createMultipleCourseRequirementAsync(MultipleCourseRequirementRequestModel obj)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(obj.CourseId);
              
                if (checkCourse != true)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "A Course With the specified ID doesnt exist!", };
                }

                foreach (var requirements in obj.Requirement)
                {
                    //check if a course objective to be created already exists
                    var checkResult = _context.CourseRequirements.Where(x => x.CourseId == obj.CourseId && x.Requirement == requirements).FirstOrDefault();

                    //if the course requirement doesnt exist, Create the course requirement
                    if (checkResult == null)
                    {
                        var courseReq = new CourseRequirements
                        {
                            CourseId = obj.CourseId,
                            Requirement = requirements,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                        };

                        await _context.CourseRequirements.AddAsync(courseReq);
                        await _context.SaveChangesAsync();
                    }                    
                }

                //get all the Course Requirements of the Course
                var crsResult = (from cr in _context.CourseRequirements
                                 where cr.CourseId == obj.CourseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Requirement,
                                     cr.IsActive,
                                     cr.DateCreated,
                                 }).OrderByDescending(c => c.Id);

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Requirements Added Successfully!", Data = crsResult.ToList() };

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> getCourseRequirementByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseRequirements
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Requirement,
                                     cr.IsActive,
                                     cr.DateCreated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID!" };

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCourseRequirementByIdAsync(long courseRequirementId)
        {
            try
            {
                //check if the courseRequirementID is valid
                var checkResult = new CheckerValidation(_context).checkCourseRequirementById(courseRequirementId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseRequirements
                                 where cr.Id == courseRequirementId
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Requirement,
                                     cr.IsActive,
                                     cr.DateCreated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Requirement with the specified ID!" };

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> deleteCourseRequirementAsync(long courseRequirementId)
        {
            try
            {
                //check if the courseRequirementID is valid
                var checkResult = new CheckerValidation(_context).checkCourseRequirementById(courseRequirementId);
                if (checkResult == true)
                {
                    var crs = _context.CourseRequirements.Where(c => c.Id == courseRequirementId).FirstOrDefault();

                     _context.CourseRequirements.Remove(crs);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Requirement Deleted Successfully"};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Requirement with the specified ID!" };

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

    }
}
