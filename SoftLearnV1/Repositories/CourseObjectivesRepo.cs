using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CourseObjectivesRepo : ICourseObjectivesRepo
    {

        private readonly AppDbContext _context;

        public CourseObjectivesRepo(AppDbContext context)
        {
            _context = context;
        }

        //----------------------Get Course Objectives By CourseID-----------------------------------------------
        public async Task<GenericResponseModel> getCourseObjectivesByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseObjectives
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Objective,
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

        //------------------------------------Create course Objectives-------------------------------------------------

        public async Task<GenericResponseModel> createCourseObjectivesAsync(CourseObjectivesRequestModel obj)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(obj.CourseId);
                //check if a course objective to be created already exists
                var checkResult = _context.CourseObjectives.Where(x => x.CourseId == obj.CourseId && x.Objective == obj.Objective).FirstOrDefault();

                if (checkCourse != true)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "A Course With a specified ID doesnt exist!", };
                }

                //if the course objective doesnt exist, Create the course objective
                if (checkResult == null)
                {
                    var courseObj = new CourseObjectives
                    {
                        CourseId = obj.CourseId,
                        Objective = obj.Objective,
                        IsActive = true,
                        DateCreated = DateTime.Now,
                    };

                    await _context.CourseObjectives.AddAsync(courseObj);
                    await _context.SaveChangesAsync();

                    //get all the Course Objectives of the Course
                    var crsResult = (from cr in _context.CourseObjectives
                                     where cr.CourseId == obj.CourseId
                                     select new
                                     {
                                         cr.Id,
                                         cr.CourseId,
                                         cr.Courses.CourseName,
                                         cr.Objective,
                                         cr.IsActive,
                                         cr.DateCreated,
                                     }).OrderByDescending(c => c.Id);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Objectives Added Successfully!", Data = crsResult.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Objectives Already Exists!"};

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


        public async Task<GenericResponseModel> createMultipleCourseObjectivesAsync(MultipleCourseObjectivesRequestModel obj)
        {
            try
            {
                //check if the course is valid
                var checkCourse = new CheckerValidation(_context).checkCourseById(obj.CourseId);

                if (checkCourse != true)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "A Course With a specified ID doesnt exist!", };
                }

                foreach (var objectives in obj.Objective)
                {
                    //check if a course objective to be created already exists
                    var checkResult = _context.CourseObjectives.Where(x => x.CourseId == obj.CourseId && x.Objective == objectives).FirstOrDefault();

                    //if the course objective doesnt exist, Create the course objective
                    if (checkResult == null)
                    {
                        var courseObj = new CourseObjectives
                        {
                            CourseId = obj.CourseId,
                            Objective = objectives,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                        };

                        await _context.CourseObjectives.AddAsync(courseObj);
                        await _context.SaveChangesAsync();
                    }
                }

                //get all the Course Objectives of the Course
                var crsResult = (from cr in _context.CourseObjectives
                                 where cr.CourseId == obj.CourseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Objective,
                                     cr.IsActive,
                                     cr.DateCreated,
                                 }).OrderByDescending(c => c.Id);

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Objectives Added Successfully!", Data = crsResult.ToList() };

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

        //----------------------Get Course Objective By ID -----------------------------------------------
        public async Task<GenericResponseModel> getCourseObjectiveByIdAsync(long courseObjectiveId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseObjectiveById(courseObjectiveId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseObjectives
                                 where cr.Id == courseObjectiveId
                                 select new
                                 {
                                     cr.Id,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Objective,
                                     cr.IsActive,
                                     cr.DateCreated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Objective with the specified ID!" };

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

        public async Task<GenericResponseModel> deleteCourseObjectiveAsync(long courseObjectiveId)
        {
            try
            {
                //check if the courseRequirementID is valid
                var checkResult = new CheckerValidation(_context).checkCourseObjectiveById(courseObjectiveId);
                if (checkResult == true)
                {
                    var crs = _context.CourseObjectives.Where(c => c.Id == courseObjectiveId).FirstOrDefault();

                    _context.CourseObjectives.Remove(crs);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Objective Deleted Successfully" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Objective with the specified ID!" };

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
