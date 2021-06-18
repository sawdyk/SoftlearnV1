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
using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Reusables;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CourseRepo : ICourseRepo
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryRepo _cloudinary;
        public IConfiguration Configuration;
        private readonly CourseRating _courseRating;
        private readonly Duration courseDuration;

        public CourseRepo(AppDbContext context, ICloudinaryRepo cloudinary, IConfiguration configuration, CourseRating courseRating, Duration courseDuration)
        {
            _context = context;
            _cloudinary = cloudinary;
            Configuration = configuration;
            this._courseRating = courseRating;
            this.courseDuration = courseDuration;
        }

        //---------------------------Creates a new course -------------------------------------------
        public async Task<GenericResponseModel> createCourseAsync(CourseCreateRequestModel obj)
        {
            try
            {
                //check if a course to be created already exists
               var checkResult = _context.Courses.Where(x => x.CourseName == obj.CourseName && x.CourseCategoryId == obj.CourseCategoryId && x.CourseSubCategoryId == obj.CourseSubCategoryId && x.CourseTypeId == obj.CourseTypeId
               && x.LevelTypeId == obj.LevelTypeId).FirstOrDefault();

                //if the course doesnt exist, Create the course
                if (checkResult == null)
                {
                    var course = new Courses
                    {
                        FacilitatorId = obj.FacilitatorId,
                        CourseName = obj.CourseName,
                        CourseDescription = obj.CourseDescription,
                        CourseSubTitle = obj.CourseSubTitle,
                        CourseImageUrl = obj.CourseImageUrl, //Course image upload to cloudinary
                        CourseTypeId = obj.CourseTypeId,
                        LevelTypeId = obj.LevelTypeId,
                        CourseCategoryId = obj.CourseCategoryId,
                        CourseSubCategoryId = obj.CourseSubCategoryId,
                        CourseAmount = obj.CourseAmount,
                        IsApproved = false,
                        IsVerified = false,
                        DateCreated = DateTime.Now,
                    };

                    await _context.Courses.AddAsync(course);
                    await _context.SaveChangesAsync();

                    //get the default percentage earnings per courses created on the platform
                    var defaultPercentage = _context.DefaultPercentageEarningsPerCourse.FirstOrDefault();

                    //creates the  CourseSharingRatio (amount to be earned on each course created by percentage) 
                    //default percentage is 10% on every course created (can be dited by the superamdmin for each courses)

                    var percent = new PercentageEarnedOnCourses
                    {
                        FacilitatorId = obj.FacilitatorId,
                        CourseId = course.Id,
                        Percentage = defaultPercentage.Percentage, //default percentage to be earned by facilitator on each courses sold on the platform by default
                        DateCreated = DateTime.Now,
                    };

                    await _context.PercentageEarnedOnCourses.AddAsync(percent);
                    await _context.SaveChangesAsync();

                    //get the Course Created
                    var getCourse = from cr in _context.Courses
                                    where cr.Id == course.Id
                                    select new
                                    {
                                        cr.Id,
                                        cr.FacilitatorId,
                                        cr.Facilitators.FirstName,
                                        cr.Facilitators.LastName,
                                        cr.CourseName,
                                        cr.CourseSubTitle,
                                        cr.CourseDescription,
                                        cr.CourseImageUrl,
                                        cr.CourseVideoPreviewUrl,
                                        cr.CourseCategoryId,
                                        cr.CourseCategory.CourseCategoryName,
                                        cr.CourseCategory.CategoryImageUrl,
                                        cr.CourseCategory.CategoryDescription,
                                        cr.CourseSubCategoryId,
                                        cr.CourseSubCategory.CourseSubCategoryName,
                                        cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                        cr.CourseSubCategory.CourseSubCategoryDescription,
                                        cr.CourseLevelTypes.LevelTypeName,
                                        cr.CourseType.CourseTypeName,
                                        cr.CourseAmount,
                                        cr.IsApproved,
                                        cr.IsVerified,
                                        cr.DateCreated,
                                    };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Created Successfully", Data = getCourse.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Already Exists" };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> approveCourseCreationAsync(long courseId)
        {
            try
            {
                //check if the CourseId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    Courses course = _context.Courses.Where(x => x.Id == courseId).FirstOrDefault();
                    course.IsApproved = true;
                    course.IsVerified = true;

                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Approved and Verified Successfully" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //---------------------Get all courses by Facilitator ID ------------------------------------------------
        public async Task<GenericResponseModel> getAllCourseByFacilitatorIdAsync(Guid facilitatorId)
        {
            try
            {
                //check if the facilitatorId is valid
                var checkResult = new CheckerValidation(_context).checkFacilitatorById(facilitatorId);
                if (checkResult == true)
                {

                    var result = from cr in _context.Courses
                                 where cr.FacilitatorId == facilitatorId && cr.IsVerified == true && cr.IsApproved == true
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No facilitator with the specified ID"};

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> deleteCourseAsync(long courseId)
        {
            try
            {
                //check if the courseId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    //get the course
                    var course = _context.Courses.Where(cr => cr.Id == courseId).FirstOrDefault();

                    //check if a learner has enrolled for the course
                    var checkEnroll = _context.CourseEnrollees.Where(cr => cr.CourseId == courseId).FirstOrDefault();
                    if (checkEnroll == null)
                    {
                         _context.Courses.Remove(course);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Deleted Successfully" };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "This Course has been Enrolled for"};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> deleteCourseAttachedToEnrolleeAsync(long courseId)
        {
            try
            {
                //check if the courseId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    //get the course
                    var course = _context.Courses.Where(cr => cr.Id == courseId).FirstOrDefault();
                    
                        _context.Courses.Remove(course);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Deleted Successfully" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> updateCourseVideoPreviewAsync(long courseId, string courseVideoPreviewUrl)
        {
            try
            {
                //check if the CourseId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    Courses course = _context.Courses.Where(x => x.Id == courseId).FirstOrDefault();
                    course.CourseVideoPreviewUrl = courseVideoPreviewUrl;

                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Video Preview Updated Successfully" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //---------------------Get all courses by Facilitator ID With Pagination------------------------------------------------
        public async Task<GenericResponseModel> getAllCourseByFacilitatorIdAsync(Guid facilitatorId, int pageNumber, int pageSize)
        {
            try
            {
                //check if the facilitatorId is valid
                var checkResult = new CheckerValidation(_context).checkFacilitatorById(facilitatorId);
                if (checkResult == true)
                {

                    var result = (from cr in _context.Courses
                                 where cr.FacilitatorId == facilitatorId && cr.IsVerified == true && cr.IsApproved == true orderby cr.Id ascending
                                  select new CourseResponseModel
                                  {
                                      Id = cr.Id,
                                      LevelTypeId = cr.LevelTypeId,
                                      FacilitatorId = cr.FacilitatorId,
                                      FacilitatorFirstName = cr.Facilitators.FirstName,
                                      FacilitatorLastName = cr.Facilitators.LastName,
                                      CourseName = cr.CourseName,
                                      CourseSubTitle = cr.CourseSubTitle,
                                      CourseDescription = cr.CourseDescription,
                                      CourseImageUrl = cr.CourseImageUrl,
                                      CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                      CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                      CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                      CategoryDescription = cr.CourseCategory.CategoryDescription,
                                      CourseCategoryId = cr.CourseCategoryId,
                                      CourseSubCategoryId = cr.CourseSubCategoryId,
                                      CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                      CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                      CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                      LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                      CourseTypeName = cr.CourseType.CourseTypeName,
                                      CourseAmount = cr.CourseAmount,
                                      IsApproved = cr.IsApproved,
                                      IsVerified = cr.IsVerified,
                                      DateCreated = cr.DateCreated,
                                  }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No facilitator with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }




        //--------------Get all Courses Created------------------------------------------------------------------
        public async Task<GenericResponseModel> getAllCoursesAsync(int pageNumber, int pageSize)
        {
            try
            {
                //Included query for pagination
                var result = (from cr in _context.Courses where cr.IsVerified == true && cr.IsApproved == true orderby cr.Id ascending
                              select new CourseResponseModel
                              {
                                  Id = cr.Id,
                                  LevelTypeId = cr.LevelTypeId,
                                  FacilitatorId = cr.FacilitatorId,
                                  FacilitatorFirstName = cr.Facilitators.FirstName,
                                  FacilitatorLastName = cr.Facilitators.LastName,
                                  CourseName = cr.CourseName,
                                  CourseSubTitle = cr.CourseSubTitle,
                                  CourseDescription = cr.CourseDescription,
                                  CourseImageUrl = cr.CourseImageUrl,
                                  CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                  CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                  CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                  CategoryDescription = cr.CourseCategory.CategoryDescription,
                                  CourseCategoryId = cr.CourseCategoryId,
                                  CourseSubCategoryId = cr.CourseSubCategoryId,
                                  CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                  CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                  CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                  LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                  CourseTypeName = cr.CourseType.CourseTypeName,
                                  CourseAmount = cr.CourseAmount,
                                  IsApproved = cr.IsApproved,
                                  IsVerified = cr.IsVerified,
                                  DateCreated = cr.DateCreated,
                              }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)



                IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //-------------------------------Get Course By CourseID-------------------------------------------------
        public async Task<GenericResponseModel> getCourseByIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.Courses
                                 where cr.Id == courseId
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCoursesByTypeIdAsync(long typeId, int pageNumber, int pageSize)
        {
            try
            {
                //check if the typeId is valid
                var checkResult = new CheckerValidation(_context).checkCourseTypeById(typeId);
                if (checkResult == true)
                {
                    var result = (from cr in _context.Courses
                                 where cr.CourseTypeId == typeId && cr.IsVerified == true && cr.IsApproved == true orderby cr.Id ascending
                                  select new CourseResponseModel
                                  {
                                      Id = cr.Id,
                                      LevelTypeId = cr.LevelTypeId,
                                      FacilitatorId = cr.FacilitatorId,
                                      FacilitatorFirstName = cr.Facilitators.FirstName,
                                      FacilitatorLastName = cr.Facilitators.LastName,
                                      CourseName = cr.CourseName,
                                      CourseSubTitle = cr.CourseSubTitle,
                                      CourseDescription = cr.CourseDescription,
                                      CourseImageUrl = cr.CourseImageUrl,
                                      CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                      CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                      CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                      CategoryDescription = cr.CourseCategory.CategoryDescription,
                                      CourseCategoryId = cr.CourseCategoryId,
                                      CourseSubCategoryId = cr.CourseSubCategoryId,
                                      CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                      CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                      CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                      LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                      CourseTypeName = cr.CourseType.CourseTypeName,
                                      CourseAmount = cr.CourseAmount,
                                      IsApproved = cr.IsApproved,
                                      IsVerified = cr.IsVerified,
                                      DateCreated = cr.DateCreated,
                                  }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCoursesByCategoryIdAsync(long categoryId, int pageNumber, int pageSize)
        {
            try
            {
                //check if the typeId is valid
                var checkResult = new CheckerValidation(_context).checkCourseCategoryById(categoryId);
                if (checkResult == true)
                {
                    var result = (from cr in _context.Courses
                                 where cr.CourseCategoryId == categoryId && cr.IsVerified == true && cr.IsApproved == true
                                  orderby cr.Id ascending
                                  select new CourseResponseModel
                                  {
                                      Id = cr.Id,
                                      LevelTypeId = cr.LevelTypeId,
                                      FacilitatorId = cr.FacilitatorId,
                                      FacilitatorFirstName = cr.Facilitators.FirstName,
                                      FacilitatorLastName = cr.Facilitators.LastName,
                                      CourseName = cr.CourseName,
                                      CourseSubTitle = cr.CourseSubTitle,
                                      CourseDescription = cr.CourseDescription,
                                      CourseImageUrl = cr.CourseImageUrl,
                                      CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                      CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                      CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                      CategoryDescription = cr.CourseCategory.CategoryDescription,
                                      CourseCategoryId = cr.CourseCategoryId,
                                      CourseSubCategoryId = cr.CourseSubCategoryId,
                                      CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                      CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                      CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                      LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                      CourseTypeName = cr.CourseType.CourseTypeName,
                                      CourseAmount = cr.CourseAmount,
                                      IsApproved = cr.IsApproved,
                                      IsVerified = cr.IsVerified,
                                      DateCreated = cr.DateCreated
                                  }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)


                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCoursesByLevelIdAsync(long levelId, int pageNumber, int pageSize)
        {
            try
            {
                var result = (from cr in _context.Courses
                              where cr.LevelTypeId == levelId && cr.IsVerified == true && cr.IsApproved == true
                              select new CourseResponseModel
                              {
                                  Id = cr.Id,
                                  LevelTypeId = cr.LevelTypeId,
                                  FacilitatorId = cr.FacilitatorId,
                                  FacilitatorFirstName = cr.Facilitators.FirstName,
                                  FacilitatorLastName = cr.Facilitators.LastName,
                                  CourseName = cr.CourseName,
                                  CourseSubTitle = cr.CourseSubTitle,
                                  CourseDescription = cr.CourseDescription,
                                  CourseImageUrl = cr.CourseImageUrl,
                                  CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                  CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                  CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                  CategoryDescription = cr.CourseCategory.CategoryDescription,
                                  CourseCategoryId = cr.CourseCategoryId,
                                  CourseSubCategoryId = cr.CourseSubCategoryId,
                                  CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                  CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                  CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                  LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                  CourseTypeName = cr.CourseType.CourseTypeName,
                                  CourseAmount = cr.CourseAmount,
                                  IsApproved = cr.IsApproved,
                                  IsVerified = cr.IsVerified,
                                  DateCreated = cr.DateCreated,
                              }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                //Response Llist
                //IList<CourseAndAverageRatingResponseModel> respList = new List<CourseAndAverageRatingResponseModel>();

                //foreach (var rslt in result)
                //{
                //    //Response Obj
                //    CourseAndAverageRatingResponseModel resp = new CourseAndAverageRatingResponseModel();

                //    decimal averageRatings = 0;
                //    //Average Rating
                //    var courseRatings = (from cr in _context.CourseRatings
                //                         where cr.CourseId == rslt.Id
                //                         select Convert.ToDecimal(cr.RatingValue)).ToList();
                //    //Converts to array
                //    decimal[] ratings = courseRatings.ToArray();

                //    //Check if the array contains items
                //    if (ratings.Length > 0)
                //    {
                //        averageRatings = ratings.Average();
                //    }

                //    //response object
                //    resp.CourseData = rslt;
                //    resp.AverageRating = averageRatings;

                //    //response List
                //    respList.Add(resp);

                //}
                IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
        
        //------------------Enrollment for Course, Facilitators Earnings and Total Earning For each Courses Enrolled For -----------------------------------------

        public async Task<GenericResponseModel> courseEnrollAsync(CourseEnrollRequestModel obj)
        {
            try
            {
                var checkCartId = new CheckerValidation(_context).checkCartById(obj.CartId);
                if (checkCartId != true)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Cart with the specified ID" };
                }
                else
                {
                    //get the cart Items
                    var cartItems = from crt in _context.CartItems where crt.CartId == obj.CartId select crt;

                    //totalAmountEarned
                    decimal totalAmountEarned = 0;

                    foreach (var cartItem in cartItems)
                    {
                        //save all the courses learners paid for
                        var crsEnroll = new CourseEnrollees
                        {
                            LearnerId = obj.LearnerId,
                            CourseId = cartItem.CourseId, //courses in the cart
                            IsInProgress = true,
                            IsActive = true,
                            IsArchived = false,
                            IsCompleted = false,
                            DateEnrolled = DateTime.Now,
                        };
                        await _context.CourseEnrollees.AddAsync(crsEnroll);

                        //get the faciltator that has the courses enrolled for
                        Courses getCourse = _context.Courses.Where(x => x.Id == cartItem.CourseId).FirstOrDefault();

                        //get the percentage to be earned by facilitators on courses created
                        PercentageEarnedOnCourses getPercentage = _context.PercentageEarnedOnCourses.Where(x => x.CourseId == cartItem.CourseId).FirstOrDefault();

                        decimal percentage = getPercentage.Percentage;
                        long amount = getCourse.CourseAmount;
                        decimal amountEarned = (percentage * amount) / 100; //calculates the amount earned on each courses bought by the learner

                        //save facilitators earnings per courses enrolled for
                        var facEarns = new FacilitatorsEarningsPerCourse
                        {
                            FacilitatorId = getCourse.FacilitatorId, //facilitator that created the course
                            CourseId = cartItem.CourseId, //courses in the cart
                            Amount = amount,
                            Percentage = percentage,
                            AmountEarned = amountEarned,
                            DateEarned = DateTime.Today.Date
                        };
                        await _context.FacilitatorsEarningsPerCourse.AddAsync(facEarns);

                        
                        //get the total earning for the current date
                        FacilitatorsTotalEarnings facTotalEarnings = _context.FacilitatorsTotalEarnings.Where(x => x.FacilitatorId == getCourse.FacilitatorId && x.DateEarned == DateTime.Today.Date).FirstOrDefault();

                        if (facTotalEarnings == null)
                        {
                            ///adds the amount earned from
                            totalAmountEarned += facEarns.AmountEarned;
                         
                            //saves total total earnings of a facilitator per current day if none has been created for the current day
                            var facTotalEarns = new FacilitatorsTotalEarnings
                            {
                                FacilitatorId = getCourse.FacilitatorId, //facilitator that created the course
                                TotalAmountEarned = totalAmountEarned,
                                IsSettled = false,
                                DateEarned = DateTime.Today.Date
                            };

                            await _context.FacilitatorsTotalEarnings.AddAsync(facTotalEarns);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            //adds the totalAmountEarned from TotalEarnings with the Amountearnerd from the next course
                            totalAmountEarned = facEarns.AmountEarned + facTotalEarnings.TotalAmountEarned;
                          
                            //updates total earnings of a facilitator per current date if a record already exists
                            facTotalEarnings.FacilitatorId = getCourse.FacilitatorId; //facilitator that created the course
                            facTotalEarnings.TotalAmountEarned = totalAmountEarned; //the total amount earned for each courses purchased for that day
                            facTotalEarnings.LastDateUpdated = DateTime.Today.Date;
                           
                            await _context.SaveChangesAsync();
                        }

                    }
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Enrolled Successfully"};
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCourseEnrollByIdAsync(long courseEnrollId)
        {
            try
            {
                var result = from cr in _context.CourseEnrollees
                             where cr.Id == courseEnrollId
                             select new
                             {
                                cr.Id,
                                cr.LearnerId,
                                cr.CourseId,
                                cr.IsCompleted,
                                cr.IsInProgress,
                                cr.DateEnrolled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!"};
            }
        }


        public async Task<GenericResponseModel> searchCoursesEnrolledForAsync(Guid learnerId, string courseName)
        {
            try
            {
                //check if courseName is not Empty
                if (courseName != string.Empty)
                {
                    var result = (from cr in _context.Courses
                                  join crs in _context.CourseEnrollees on cr.Id equals crs.CourseId
                                  where cr.CourseName.Trim().ToLower().Contains(courseName.Trim().ToLower()) && crs.LearnerId == learnerId
                                  select new CourseResponseModel
                                  {
                                      Id = cr.Id,
                                      LevelTypeId = cr.LevelTypeId,
                                      FacilitatorId = cr.FacilitatorId,
                                      FacilitatorFirstName = cr.Facilitators.FirstName,
                                      FacilitatorLastName = cr.Facilitators.LastName,
                                      CourseName = cr.CourseName,
                                      CourseSubTitle = cr.CourseSubTitle,
                                      CourseDescription = cr.CourseDescription,
                                      CourseImageUrl = cr.CourseImageUrl,
                                      CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                      CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                      CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                      CategoryDescription = cr.CourseCategory.CategoryDescription,
                                      CourseCategoryId = cr.CourseCategoryId,
                                      CourseSubCategoryId = cr.CourseSubCategoryId,
                                      CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                      CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                      CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                      LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                      CourseTypeName = cr.CourseType.CourseTypeName,
                                      CourseAmount = cr.CourseAmount,
                                      IsApproved = cr.IsApproved,
                                      IsVerified = cr.IsVerified,
                                      DateCreated = cr.DateCreated,
                                  }).Distinct();

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "There is no Course with the Name Specified" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "CourseName paramter is Empty" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseEnrolledForByCourseIdAsync(long courseId)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseEnrollees
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllLearnersEnrolledForCourseAsync(long courseId)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseEnrollees
                                 where cr.CourseId == courseId
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCoursesLearnerEnrolledForAysnc(Guid learnerId)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseEnrollees
                                 where cr.LearnerId == learnerId && cr.IsArchived == false && cr.IsActive == true
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Learner With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCoursesLearnerEnrolledForAysnc(Guid learnerId, int pageNumber, int pageSize)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    var result = (from cr in _context.CourseEnrollees
                                 where cr.LearnerId == learnerId && cr.IsArchived == false && cr.IsActive == true orderby cr.Id ascending
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.DateEnrolled,
                                 }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Learner With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //Consume PayStack Endpoint for Payment Verification
        public async Task<GenericResponseModel> verifyPaymentAysnc(long cartId, Guid learnerId, string reference)
        {
            try
            {
                HttpClientConfig httpClientConfig = new HttpClientConfig();
                var apiResponse = await httpClientConfig.ApiGetRequest("https://api.paystack.co/transaction/verify/" + reference + "", Configuration["PayStack:SecretTestKey"]);
                var response = JsonConvert.DeserializeObject<PayStackVerificationResponse>(apiResponse);

                if (response.Data.Status == "success")
                {
                    var tran = new CourseEnrolledPayments
                    {
                        LearnerId = learnerId,
                        CartId = cartId,
                        TransactionId = response.Data.Id.ToString(),
                        Message = response.Data.Message,
                        Status = response.Data.Status,
                        Reference = response.Data.reference,
                        Amount = response.Data.Amount,
                        GatewayResponse = response.Data.Gateway_Response,
                        Channel = response.Data.Channel,
                        Currency = response.Data.Currency,
                        Paid_At = Convert.ToDateTime(response.Data.Paid_At),
                        Created_At = Convert.ToDateTime(response.Data.Created_At)
                    };
                    await _context.CourseEnrolledPayments.AddAsync(tran);
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Verification Successful"};
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> searchCourseByCourseNameAsync(string courseName)
        {
            try
            {
                //check if courseName is not Empty
                if (courseName != string.Empty)
                {
                    var result = from cr in _context.Courses
                                 where cr.CourseName.Trim().ToLower().Contains(courseName.Trim().ToLower()) 
                                 && cr.IsVerified == true && cr.IsApproved == true
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "There is no Course with the Name " + courseName + "" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "CourseName paramter is Empty" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        //All Courses by Type without pagination
        public async Task<GenericResponseModel> getCoursesByTypeIdAsync(long typeId)
        {
            try
            {
                //check if the typeId is valid
                var checkResult = new CheckerValidation(_context).checkCourseTypeById(typeId);
                if (checkResult == true)
                {
                    var result = from cr in _context.Courses
                                  where cr.CourseTypeId == typeId && cr.IsVerified == true && cr.IsApproved == true
                                 orderby cr.Id ascending
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //All Courses by Category without pagination
        public async Task<GenericResponseModel> getCoursesByCategoryIdAsync(long categoryId)
        {
            try
            {
                //check if the categoryId is valid
                var checkResult = new CheckerValidation(_context).checkCourseCategoryById(categoryId);
                if (checkResult == true)
                {
                    var result = from cr in _context.Courses
                                  where cr.CourseCategoryId == categoryId && cr.IsVerified == true && cr.IsApproved == true
                                 orderby cr.Id ascending
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //All Courses by Level without pagination
        public async Task<GenericResponseModel> getCoursesByLevelIdAsync(long levelId)
        {
            try
            {
                //check if the levelId is valid
                var checkResult = new CheckerValidation(_context).checkCourseLevelById(levelId);
                if (checkResult == true)
                {
                    var result = from cr in _context.Courses
                                  where cr.LevelTypeId == levelId && cr.IsVerified == true && cr.IsApproved == true
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    //Response Llist
                    //IList<CourseAndAverageRatingResponseModel> respList = new List<CourseAndAverageRatingResponseModel>();

                    //foreach (var rslt in result)
                    //{
                    //    //Response Obj
                    //    CourseAndAverageRatingResponseModel resp = new CourseAndAverageRatingResponseModel();

                    //    decimal averageRatings = 0;
                    //    //Average Rating
                    //    var courseRatings = (from cr in _context.CourseRatings
                    //                         where cr.CourseId == rslt.Id
                    //                         select Convert.ToDecimal(cr.RatingValue)).ToList();
                    //    //Converts to array
                    //    decimal[] ratings = courseRatings.ToArray();

                    //    //Check if the array contains items
                    //    if (ratings.Length > 0)
                    //    {
                    //        averageRatings = ratings.Average();
                    //    }

                    //    //response object
                    //    resp.CourseData = rslt;
                    //    resp.AverageRating = averageRatings;

                    //    //response List
                    //    respList.Add(resp);

                    //}
                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //All Courses by SubCategory with pagination
        public async Task<GenericResponseModel> getCoursesBySubCategoryIdAsync(long subCategoryId, int pageNumber, int pageSize)
        {
            try
            {
                //check if the SubcategoryId valid
                var checkResult = new CheckerValidation(_context).checkCourseSubCategoryById(subCategoryId);
                if (checkResult == true)
                {
                    var result = (from cr in _context.Courses
                                 where cr.CourseSubCategoryId == subCategoryId && cr.IsVerified == true && cr.IsApproved == true
                                  select new CourseResponseModel
                                  {
                                      Id = cr.Id,
                                      LevelTypeId = cr.LevelTypeId,
                                      FacilitatorId = cr.FacilitatorId,
                                      FacilitatorFirstName = cr.Facilitators.FirstName,
                                      FacilitatorLastName = cr.Facilitators.LastName,
                                      CourseName = cr.CourseName,
                                      CourseSubTitle = cr.CourseSubTitle,
                                      CourseDescription = cr.CourseDescription,
                                      CourseImageUrl = cr.CourseImageUrl,
                                      CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                      CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                      CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                      CategoryDescription = cr.CourseCategory.CategoryDescription,
                                      CourseCategoryId = cr.CourseCategoryId,
                                      CourseSubCategoryId = cr.CourseSubCategoryId,
                                      CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                      CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                      CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                      LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                      CourseTypeName = cr.CourseType.CourseTypeName,
                                      CourseAmount = cr.CourseAmount,
                                      IsApproved = cr.IsApproved,
                                      IsVerified = cr.IsVerified,
                                      DateCreated = cr.DateCreated,
                                  }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                    //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Category with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //All Courses by Status (Approved/Pending) with pagination
        public async Task<GenericResponseModel> getCoursesByStatusIdAsync(long statusId, int pageNumber, int pageSize)
        {
            try
            {
                bool status = false;

                if (statusId == (int)EnumUtility.Status.Approved)
                {
                    status = true;
                }
                else if (statusId == (int)EnumUtility.Status.Pending)
                {
                    status = false;
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Status With the Specified ID" };
                }
                var result = (from cr in _context.Courses
                              where cr.IsApproved == status && cr.IsVerified == status
                              select new CourseResponseModel
                              {
                                  Id = cr.Id,
                                  LevelTypeId = cr.LevelTypeId,
                                  FacilitatorId = cr.FacilitatorId,
                                  FacilitatorFirstName = cr.Facilitators.FirstName,
                                  FacilitatorLastName = cr.Facilitators.LastName,
                                  CourseName = cr.CourseName,
                                  CourseSubTitle = cr.CourseSubTitle,
                                  CourseDescription = cr.CourseDescription,
                                  CourseImageUrl = cr.CourseImageUrl,
                                  CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                  CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                  CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                  CategoryDescription = cr.CourseCategory.CategoryDescription,
                                  CourseCategoryId = cr.CourseCategoryId,
                                  CourseSubCategoryId = cr.CourseSubCategoryId,
                                  CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                  CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                  CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                  LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                  CourseTypeName = cr.CourseType.CourseTypeName,
                                  CourseAmount = cr.CourseAmount,
                                  IsApproved = cr.IsApproved,
                                  IsVerified = cr.IsVerified,
                                  DateCreated = cr.DateCreated,
                              }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //All Courses without pagination using all parameters
        public async Task<GenericResponseModel> getAllCoursesAsync(long typeId, long categoryId, long subCategoryId, long levelId, long statusId, Guid facilitatorId, int pageNumber, int pageSize)
        {
            try
            {
                //check if the facilitatorId valid
                var checkResult = new CheckerValidation(_context).checkFacilitatorById(facilitatorId);
                bool status = false;

                if (statusId == (int)EnumUtility.Status.Approved)
                {
                    status = true;
                }
                else if (statusId == (int)EnumUtility.Status.Pending)
                {
                    status = false;
                }
                var result = from cr in _context.Courses
                             select new CourseResponseModel
                             {
                                 Id = cr.Id,
                                 LevelTypeId = cr.LevelTypeId,
                                 FacilitatorId = cr.FacilitatorId,
                                 FacilitatorFirstName = cr.Facilitators.FirstName,
                                 FacilitatorLastName = cr.Facilitators.LastName,
                                 CourseName = cr.CourseName,
                                 CourseSubTitle = cr.CourseSubTitle,
                                 CourseDescription = cr.CourseDescription,
                                 CourseImageUrl = cr.CourseImageUrl,
                                 CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                 CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                 CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                 CategoryDescription = cr.CourseCategory.CategoryDescription,
                                 CourseCategoryId = cr.CourseCategoryId,
                                 CourseSubCategoryId = cr.CourseSubCategoryId,
                                 CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                 CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                 CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                 LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                 CourseTypeName = cr.CourseType.CourseTypeName,
                                 CourseTypeId = cr.CourseTypeId,
                                 CourseAmount = cr.CourseAmount,
                                 IsApproved = cr.IsApproved,
                                 IsVerified = cr.IsVerified,
                                 DateCreated = cr.DateCreated,
                             };
                if(checkResult == true)
                {
                    result = result.Where(x => x.FacilitatorId == facilitatorId);
                }
                if(typeId > 0)
                {
                    result = result.Where(x => x.CourseTypeId == typeId);
                }
                if (categoryId > 0)
                {
                    result = result.Where(x => x.CourseCategoryId == categoryId);
                }
                if (subCategoryId > 0)
                {
                    result = result.Where(x => x.CourseSubCategoryId == subCategoryId);
                }
                if (levelId > 0)
                {
                    if (levelId != (long)EnumUtility.CourseLevel.All)
                    {
                        result = result.Where(x => x.LevelTypeId == levelId);
                    }
                }
                if (statusId == 1 || statusId == 2)
                {
                    result = result.Where(x => x.IsApproved == status && x.IsVerified == status);
                }
                if (pageNumber > 0 && pageSize > 0)
                {
                    result = result.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                }

                IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result.OrderByDescending(x=>x.Id));
                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //All Courses by Status (Approved/Pending) without pagination
        public async Task<GenericResponseModel> getCoursesByStatusIdAsync(long statusId)
        {
            try
            {
                bool status = false;

                if (statusId == (int)EnumUtility.Status.Approved)
                {
                    status = true;
                }
                else if (statusId == (int)EnumUtility.Status.Pending)
                {
                    status = false;
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Status With the Specified ID" };
                }
                var result = from cr in _context.Courses
                              where cr.IsApproved == status && cr.IsVerified == status
                             select new CourseResponseModel
                             {
                                 Id = cr.Id,
                                 LevelTypeId = cr.LevelTypeId,
                                 FacilitatorId = cr.FacilitatorId,
                                 FacilitatorFirstName = cr.Facilitators.FirstName,
                                 FacilitatorLastName = cr.Facilitators.LastName,
                                 CourseName = cr.CourseName,
                                 CourseSubTitle = cr.CourseSubTitle,
                                 CourseDescription = cr.CourseDescription,
                                 CourseImageUrl = cr.CourseImageUrl,
                                 CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                 CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                 CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                 CategoryDescription = cr.CourseCategory.CategoryDescription,
                                 CourseCategoryId = cr.CourseCategoryId,
                                 CourseSubCategoryId = cr.CourseSubCategoryId,
                                 CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                 CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                 CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                 LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                 CourseTypeName = cr.CourseType.CourseTypeName,
                                 CourseAmount = cr.CourseAmount,
                                 IsApproved = cr.IsApproved,
                                 IsVerified = cr.IsVerified,
                                 DateCreated = cr.DateCreated,
                             };

                IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        //All Courses by SubCategory without pagination
        public async Task<GenericResponseModel> getCoursesBySubCategoryIdAsync(long subCategoryId)
        {
            try
            {
                //check if the SubcategoryId valid
                var checkResult = new CheckerValidation(_context).checkCourseSubCategoryById(subCategoryId);
                if (checkResult == true)
                {
                    var result = from cr in _context.Courses
                                 where cr.CourseSubCategoryId == subCategoryId && cr.IsVerified == true && cr.IsApproved == true
                                 select new CourseResponseModel
                                 {
                                     Id = cr.Id,
                                     LevelTypeId = cr.LevelTypeId,
                                     FacilitatorId = cr.FacilitatorId,
                                     FacilitatorFirstName = cr.Facilitators.FirstName,
                                     FacilitatorLastName = cr.Facilitators.LastName,
                                     CourseName = cr.CourseName,
                                     CourseSubTitle = cr.CourseSubTitle,
                                     CourseDescription = cr.CourseDescription,
                                     CourseImageUrl = cr.CourseImageUrl,
                                     CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                     CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                     CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                     CategoryDescription = cr.CourseCategory.CategoryDescription,
                                     CourseCategoryId = cr.CourseCategoryId,
                                     CourseSubCategoryId = cr.CourseSubCategoryId,
                                     CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                     CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                     CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                     LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                     CourseTypeName = cr.CourseType.CourseTypeName,
                                     CourseAmount = cr.CourseAmount,
                                     IsApproved = cr.IsApproved,
                                     IsVerified = cr.IsVerified,
                                     DateCreated = cr.DateCreated,
                                 };

                    IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(result);
                    if (respList.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                    }

                    return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course Category with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //----------------------------Popular,Most Viewed, Recommended Courses-------------------------------------------------

        public async Task<GenericResponseModel> popularCoursesAsync()
        {
            try
            {
                int top = 10;
                //IList<object> result = new List<object>();
                // top ten most frequent courses in courseEnrollee table
                var courseId = from cr in _context.CourseEnrollees
                             .GroupBy(c => c.CourseId)
                             .OrderByDescending(gp => gp.Count())
                             .Take(top) //take top ten
                             .Select(g => g.Key)
                             select cr;

                IList<CourseAndAverageRatingResponseModel> respList = new List<CourseAndAverageRatingResponseModel>();

                foreach (var courseIds in courseId)
                {
                    CourseAndAverageRatingResponseModel respObj = new CourseAndAverageRatingResponseModel();

                    var objCourse = (from cr in _context.Courses
                               where cr.Id == courseIds && cr.IsVerified == true && cr.IsApproved == true
                               select new
                               {
                                   cr.Id,
                                   cr.FacilitatorId,
                                   cr.Facilitators.FirstName,
                                   cr.Facilitators.LastName,
                                   cr.CourseName,
                                   cr.CourseSubTitle,
                                   cr.CourseDescription,
                                   cr.CourseImageUrl,
                                   cr.CourseCategory.CourseCategoryName,
                                   cr.CourseCategory.CategoryImageUrl,
                                   cr.CourseCategory.CategoryDescription,
                                   cr.CourseCategoryId,
                                   cr.CourseSubCategoryId,
                                   cr.CourseSubCategory.CourseSubCategoryName,
                                   cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                   cr.CourseSubCategory.CourseSubCategoryDescription,
                                   cr.CourseLevelTypes.LevelTypeName,
                                   cr.CourseType.CourseTypeName,
                                   cr.CourseAmount,
                                   cr.IsApproved,
                                   cr.IsVerified,
                                   cr.DateCreated,
                               }).FirstOrDefault();
                    
                    decimal averageRatings = 0;
                    //Average Rating
                    var courseRatings = (from cr in _context.CourseRatings
                                         where cr.CourseId == courseIds
                                         select Convert.ToDecimal(cr.RatingValue)).ToList();
                    //Converts to array
                    decimal[] ratings = courseRatings.ToArray();

                    //Check if the array contains items
                    if (ratings.Length > 0)
                    {
                        averageRatings = ratings.Average();
                    }

                    //response object
                    respObj.CourseData = objCourse;
                    respObj.AverageRating = averageRatings;
                    respObj.Duration = courseDuration.CourseDuration(courseIds);

                    //response List
                    respList.Add(respObj);
                }

                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> createMostViewedCoursesAsync(long courseId)
        {
            try
            {
                //saves the courses viewed by users on click of course Preview/View
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    var obj = new MostViewedCourses
                    {
                        CourseId = courseId,
                        DateViewed = DateTime.Now,
                    };

                    await _context.MostViewedCourses.AddAsync(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Created Successfully", Data = obj };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> mostViewedCoursesAsync()
        {
            try
            {
                int top = 10;
                IList<object> result = new List<object>();

                // top ten most Frequent Viewed courses in MostViewedCourses table (returns all the courseId)
                var courseId = from cr in _context.MostViewedCourses
                             .GroupBy(c => c.CourseId)
                             .OrderByDescending(gp => gp.Count())
                             .Take(top) //take top ten
                             .Select(g => g.Key)
                             select cr;

                IList<CourseAndAverageRatingResponseModel> respList = new List<CourseAndAverageRatingResponseModel>();

                //loops through each courseId to select the courses from the course table
                foreach (var courseIds in courseId)
                {
                    CourseAndAverageRatingResponseModel respObj = new CourseAndAverageRatingResponseModel();

                    var objCourse = (from cr in _context.Courses
                                 where cr.Id == courseIds && cr.IsVerified == true && cr.IsApproved == true
                              select new
                              {
                                  cr.Id,
                                  cr.FacilitatorId,
                                  cr.Facilitators.FirstName,
                                  cr.Facilitators.LastName,
                                  cr.CourseName,
                                  cr.CourseSubTitle,
                                  cr.CourseDescription,
                                  cr.CourseImageUrl,
                                  cr.CourseVideoPreviewUrl,
                                  cr.CourseCategory.CourseCategoryName,
                                  cr.CourseCategory.CategoryImageUrl,
                                  cr.CourseCategory.CategoryDescription,
                                  cr.CourseCategoryId,
                                  cr.CourseSubCategoryId,
                                  cr.CourseSubCategory.CourseSubCategoryName,
                                  cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                  cr.CourseSubCategory.CourseSubCategoryDescription,
                                  cr.CourseLevelTypes.LevelTypeName,
                                  cr.CourseType.CourseTypeName,
                                  cr.CourseAmount,
                                  cr.IsApproved,
                                  cr.IsVerified,
                                  cr.DateCreated,
                              }).FirstOrDefault();

                    decimal averageRatings = 0;
                    //Average Rating
                    var courseRatings = (from cr in _context.CourseRatings
                                         where cr.CourseId == courseIds
                                         select Convert.ToDecimal(cr.RatingValue)).ToList();
                    //Converts to array
                    decimal[] ratings = courseRatings.ToArray();

                    //Check if the array contains items
                    if (ratings.Length > 0)
                    {
                        averageRatings = ratings.Average();
                    }

                    //response object
                    respObj.CourseData = objCourse;
                    respObj.AverageRating = averageRatings;
                    respObj.Duration = courseDuration.CourseDuration(courseIds);

                    //response List
                    respList.Add(respObj);
                }

                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> recommendedCoursesAsync(Guid learnerId)
        {
            try
            {
                long levelId;
                //get the learners LevelTypeID
                var levelResult = _context.Learners.Where(l=>l.Id == learnerId).FirstOrDefault();
                levelId = levelResult.LevelTypeId;

                //get all  the courses Learners Enrolled for
                var courseId = (from cr in _context.CourseEnrollees.Where(c => c.LearnerId == learnerId) select cr.CourseId).ToList();

                //All courses in a levelType that are not enrolled by learner 
                var courses = from cr in _context.Courses
                             where !(from s in _context.CourseEnrollees where s.LearnerId == learnerId select s.CourseId).Contains(cr.Id) && cr.LevelTypeId == levelId
                             && cr.IsVerified == true && cr.IsApproved == true
                              select new CourseResponseModel
                              {
                                  Id = cr.Id,
                                  LevelTypeId = cr.LevelTypeId,
                                  FacilitatorId = cr.FacilitatorId,
                                  FacilitatorFirstName = cr.Facilitators.FirstName,
                                  FacilitatorLastName = cr.Facilitators.LastName,
                                  CourseName = cr.CourseName,
                                  CourseSubTitle = cr.CourseSubTitle,
                                  CourseDescription = cr.CourseDescription,
                                  CourseImageUrl = cr.CourseImageUrl,
                                  CourseVideoPreviewUrl = cr.CourseVideoPreviewUrl,
                                  CourseCategoryName = cr.CourseCategory.CourseCategoryName,
                                  CategoryImageUrl = cr.CourseCategory.CategoryImageUrl,
                                  CategoryDescription = cr.CourseCategory.CategoryDescription,
                                  CourseCategoryId = cr.CourseCategoryId,
                                  CourseSubCategoryId = cr.CourseSubCategoryId,
                                  CourseSubCategoryName = cr.CourseSubCategory.CourseSubCategoryName,
                                  CourseSubCategoryImageUrl = cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                  CourseSubCategoryDescription = cr.CourseSubCategory.CourseSubCategoryDescription,
                                  LevelTypeName = cr.CourseLevelTypes.LevelTypeName,
                                  CourseTypeName = cr.CourseType.CourseTypeName,
                                  CourseAmount = cr.CourseAmount,
                                  IsApproved = cr.IsApproved,
                                  IsVerified = cr.IsVerified,
                                  DateCreated = cr.DateCreated,
                              };

                IList<CourseAndAverageRatingResponseModel> respList = _courseRating.AverageRating(courses);
                if (respList.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCoursesByFacilitatorLearnersEnrolledForAsync(Guid facilitatorId)
        {
            try
            {
                //using LINQ TO SQL SubQuery, returns all learners that enrolled for all facilitator courses
                var result = from cr in _context.CourseEnrollees
                                where (from s in _context.Courses where s.FacilitatorId == facilitatorId select s.Id).Contains(cr.CourseId)
                                select new
                                {
                                    cr.Id,
                                    cr.LearnerId,
                                    cr.Learners.FirstName,
                                    cr.Learners.LastName,
                                    cr.Learners.UserName,
                                    cr.Learners.Email,
                                    cr.CourseId,
                                    cr.Courses.CourseName,
                                    cr.Courses.CourseSubTitle,
                                    cr.Courses.CourseImageUrl,
                                    cr.Courses.CourseVideoPreviewUrl,
                                    cr.Courses.CourseType.CourseTypeName,
                                    cr.Courses.CourseLevelTypes.LevelTypeName,
                                    cr.Courses.CourseCategory.CourseCategoryName,
                                    cr.Courses.CourseCategory.CategoryImageUrl,
                                    cr.Courses.CourseCategory.CategoryDescription,
                                    cr.Courses.CourseCategoryId,
                                    cr.Courses.CourseSubCategoryId,
                                    cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                    cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                    cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                    cr.Courses.CourseAmount,
                                    cr.IsCompleted,
                                    cr.IsInProgress,
                                    cr.DateEnrolled,
                                };
           
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record"};
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> deleteAllCoursesEnrolledForByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the CourseEnrollId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    //get the list of all courses enrolled for
                    IList<CourseEnrollees> enrolledCourses = (_context.CourseEnrollees.Where(c => c.CourseId == courseId)).ToList();

                    if (enrolledCourses.Count() > 0)
                    {
                        _context.CourseEnrollees.RemoveRange(enrolledCourses);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Courses Enrolled For Deleted Successfully"};
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Record Available to be Deleted"};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID"};

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> deleteCourseEnrolledForAsync(Guid learnerId, long courseId)
        {
            try
            {
                //check if the CourseEnrollId is valid
                var checkCourse = new CheckerValidation(_context).checkCourseById(courseId);
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);

                if (checkCourse == true && checkLearner == true)
                {
                    //get the list of all courses enrolled for
                    CourseEnrollees enrolledCourses = _context.CourseEnrollees.Where(c => c.CourseId == courseId && c.LearnerId == learnerId).FirstOrDefault();

                    if (enrolledCourses != null)
                    {
                        _context.CourseEnrollees.Remove(enrolledCourses);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Courses Enrolled For Deleted Successfully" };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "This Course was not Enrolled For" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course/Learner with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseRatingAndReviewByCourseIdAsync(long courseId)
        {
            try
            {
                //check if the courseID is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    //get all the learners
                    IList<Learners> learners = (from lr in _context.Learners select lr).ToList();
                    //a list 
                    IList<object> objLst = new List<object>();

                    //check if learners exist
                    if (learners.Count() > 0)
                    {
                        foreach (Learners lrn in learners)
                        {
                            //check if the learner has a courseRating
                            CourseRatings ratings = await  _context.CourseRatings.Where(c => c.LearnerId == lrn.Id && c.CourseId == courseId).FirstOrDefaultAsync();
                            //check if the learner has a courseReview
                            CourseReviews reviews = await _context.CourseReviews.Where(c => c.LearnerId == lrn.Id && c.CourseId == courseId).FirstOrDefaultAsync();

                            if (ratings == null && reviews != null)
                            {
                                //joins the rating and reviews table on the learnerId then group by the learnerId
                                var result = (from c in _context.Learners
                                              join crv in _context.CourseReviews on c.Id equals crv.LearnerId
                                              where crv.CourseId == courseId && c.Id == lrn.Id
                                              select new
                                              {
                                                  learnerId = c.Id,
                                                  //courseId = crv.CourseId,
                                                  learner_FirstName = crv.Learners.FirstName,
                                                  learner_LastName = crv.Learners.LastName,
                                                  Rating_Value = "",
                                                  Date_Rated = "",
                                                  Review_Note = crv.ReviewNote,
                                                  Review_Date = crv.DateReviewed
                                              }).GroupBy(l => l.learnerId).FirstOrDefault();

                                //adds the result to a list 
                                objLst.Add(result.FirstOrDefault());
                            }
                            else if (ratings != null && reviews == null)
                            {
                                //joins the rating and reviews table on the learnerId then group by the learnerId
                                var result = (from c in _context.Learners
                                              join crr in _context.CourseRatings on c.Id equals crr.LearnerId
                                              where crr.CourseId == courseId && c.Id == lrn.Id
                                              select new
                                              {
                                                  learnerId = c.Id,
                                                  //courseId = crr.CourseId,
                                                  learner_FirstName = crr.Learners.FirstName,
                                                  learner_LastName = crr.Learners.LastName,
                                                  Rating_Value = crr.RatingValue,
                                                  Date_Rated = crr.DateRated,
                                                  Review_Note = "",
                                                  Review_Date = ""
                                              }).GroupBy(l => l.learnerId).FirstOrDefault();

                                //adds the result to a list 
                                objLst.Add(result.FirstOrDefault());
                            }
                            else if(ratings != null && reviews != null)
                            {
                                //joins the rating and reviews table on the learnerId then group by the learnerId
                                var result = (from c in _context.Learners
                                              join crr in _context.CourseRatings on c.Id equals crr.LearnerId
                                              join crv in _context.CourseReviews on c.Id equals crv.LearnerId
                                              where crr.CourseId == courseId && crv.CourseId == courseId && c.Id == lrn.Id
                                              select new
                                              {
                                                  learnerId = c.Id,
                                                  //courseId = crv.CourseId,
                                                  learner_FirstName = crv.Learners.FirstName,
                                                  learner_LastName = crv.Learners.LastName,
                                                  Rating_Value = crr.RatingValue,
                                                  Date_Rated = crr.DateRated,
                                                  Review_Note = crv.ReviewNote,
                                                  Review_Date = crv.DateReviewed
                                              }).GroupBy(l => l.learnerId).FirstOrDefault();

                                //adds the result to a list 
                                objLst.Add(result.FirstOrDefault());
                            }
                            else
                            {
                                //joins the rating and reviews table on the learnerId then group by the learnerId
                                var result = (from c in _context.Learners
                                              where c.Id == lrn.Id
                                              select new
                                              {
                                                  learnerId = c.Id,
                                                  learner_FirstName = c.FirstName,
                                                  learner_LastName = c.LastName,
                                                  Rating_Value = "",
                                                  Date_Rated = "",
                                                  Review_Note = "",
                                                  Review_Date = ""
                                              }).GroupBy(l => l.learnerId).FirstOrDefault();

                                //adds the result to a list 
                                objLst.Add(result.FirstOrDefault());
                            }
                        }

                        if (objLst.Count() > 0)
                        {
                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = objLst.ToList() };
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Learners on the platform!" };
                    
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Course with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseAndAverageRatingAsync()
        {
            try
            {
                IList<CourseAndAverageRatingResponseModel> respList = new List<CourseAndAverageRatingResponseModel>();
                //get all the learners
                IList<Courses> courses = (from cr in _context.Courses select cr).ToList();
                //a list 
                IList<object> objLst = new List<object>();

                //check if Courses exist
                if (courses.Count() > 0)
                {
                    foreach (Courses crs in courses)
                    {
                        CourseAndAverageRatingResponseModel resp = new CourseAndAverageRatingResponseModel();

                        //Courses
                        var objCourse = (from cr in _context.Courses
                                   where cr.Id == crs.Id && cr.IsVerified == true && cr.IsApproved == true
                                   select new
                                   {
                                       cr.Id,
                                       cr.FacilitatorId,
                                       cr.Facilitators.FirstName,
                                       cr.Facilitators.LastName,
                                       cr.CourseName,
                                       cr.CourseSubTitle,
                                       cr.CourseDescription,
                                       cr.CourseImageUrl,
                                       cr.CourseCategory.CourseCategoryName,
                                       cr.CourseCategory.CategoryImageUrl,
                                       cr.CourseCategory.CategoryDescription,
                                       cr.CourseCategoryId,
                                       cr.CourseSubCategoryId,
                                       cr.CourseSubCategory.CourseSubCategoryName,
                                       cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                       cr.CourseSubCategory.CourseSubCategoryDescription,
                                       cr.CourseLevelTypes.LevelTypeName,
                                       cr.CourseType.CourseTypeName,
                                       cr.CourseAmount,
                                       cr.IsApproved,
                                       cr.IsVerified,
                                       cr.DateCreated,
                                   }).FirstOrDefault();

                        decimal averageRatings = 0;
                        //Average Rating
                        var courseRatings = (from cr in _context.CourseRatings
                                             where cr.CourseId == crs.Id
                                             select Convert.ToDecimal(cr.RatingValue)).ToList();
                        //Converts to array
                        decimal[] ratings = courseRatings.ToArray();

                        //Check if the array contains items
                        if (ratings.Length > 0)
                        {
                            averageRatings = ratings.Average();
                        }

                        //response object
                        resp.CourseData = objCourse;
                        resp.AverageRating = averageRatings;
                        resp.Duration = courseDuration.CourseDuration(crs.Id);

                        //response List
                        respList.Add(resp);

                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = respList};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Courses"};
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //--------------------Course Archiving, Activation and deactivaton by Learners--------------------------------

        public async Task<GenericResponseModel> archiveOrUnArchiveCourseEnrolledForAsync(Guid learnerId, long courseId)
        {
            try
            {
                //check if the CourseId is valid
                var checkResult = new CheckerValidation(_context).checkCourseById(courseId);
                if (checkResult == true)
                {
                    CourseEnrollees courseEnroll = _context.CourseEnrollees.Where(x => x.CourseId == courseId && x.LearnerId == learnerId).FirstOrDefault();

                    if (courseEnroll != null)
                    {
                        if (courseEnroll.IsArchived == true)
                        {
                            courseEnroll.IsArchived = false;
                            await _context.SaveChangesAsync();

                        }
                        else
                        {
                            courseEnroll.IsArchived = true;
                            await _context.SaveChangesAsync();
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Archived/UnArchived Successfully" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Course Was not Enrolled For" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course with the specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllArchivedCoursesEnrolledForAsync(Guid learnerId)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseEnrollees
                                 where cr.LearnerId == learnerId && cr.IsArchived == true
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.IsArchived,
                                     cr.IsActive,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Learner With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> getAllUnArchivedCoursesEnrolledForAsync(Guid learnerId)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    var result = from cr in _context.CourseEnrollees
                                 where cr.LearnerId == learnerId && cr.IsArchived == false
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.IsArchived,
                                     cr.IsActive,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Learner With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> updateStatusForCourseEnrolledForAsync(Guid learnerId, long courseId, long statusId)
        {
            try
            {
                //check if the CourseId is valid
                var checkCourse = new CheckerValidation(_context).checkCourseById(courseId);
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);

                if (checkCourse == true && checkLearner == true)
                {
                    if (statusId == (int)EnumUtility.ActiveInActive.Active)
                    {
                        CourseEnrollees courseEnroll = _context.CourseEnrollees.Where(x => x.CourseId == courseId && x.LearnerId == learnerId).FirstOrDefault();
                        courseEnroll.IsActive = true;

                        await _context.SaveChangesAsync();
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully" };
                    }
                    if (statusId == (int)EnumUtility.ActiveInActive.InActive)
                    {
                        CourseEnrollees courseEnroll = _context.CourseEnrollees.Where(x => x.CourseId == courseId && x.LearnerId == learnerId).FirstOrDefault();
                        courseEnroll.IsActive = false;

                        await _context.SaveChangesAsync();
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully" };
                    }
                  
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course/learner with the specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCoursesEnrolledForByStatusIdAsync(Guid learnerId, long statusId)
        {
            try
            {
                var checkResult = new CheckerValidation(_context).checkLearnerById(learnerId);
                if (checkResult == true)
                {
                    bool status = false;

                    if (statusId == (int)EnumUtility.ActiveInActive.Active)
                    {
                        status = true;
                    }
                    else if (statusId == (int)EnumUtility.ActiveInActive.InActive)
                    {
                        status = false;
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Status With the Specified ID" };
                    }

                    var result = from cr in _context.CourseEnrollees
                                 where cr.LearnerId == learnerId && cr.IsActive == status
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.Learners.FirstName,
                                     cr.Learners.LastName,
                                     cr.Learners.UserName,
                                     cr.Learners.Email,
                                     cr.CourseId,
                                     cr.Courses.CourseName,
                                     cr.Courses.CourseSubTitle,
                                     cr.Courses.CourseImageUrl,
                                     cr.Courses.CourseVideoPreviewUrl,
                                     cr.Courses.CourseType.CourseTypeName,
                                     cr.Courses.CourseLevelTypes.LevelTypeName,
                                     cr.Courses.CourseCategory.CourseCategoryName,
                                     cr.Courses.CourseCategory.CategoryImageUrl,
                                     cr.Courses.CourseCategory.CategoryDescription,
                                     cr.Courses.CourseCategoryId,
                                     cr.Courses.CourseSubCategoryId,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryImageUrl,
                                     cr.Courses.CourseSubCategory.CourseSubCategoryDescription,
                                     cr.Courses.CourseAmount,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.IsArchived,
                                     cr.IsActive,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Learner With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //----------------------Earning Percentage on Courses (SuperAdmin/Admin)---------------------------------------------------------

        public async Task<GenericResponseModel> getDefaultPercentageEarningsPerCourseAsync()
        {
            try
            {
                var result = from cr in _context.DefaultPercentageEarningsPerCourse select cr;
                             
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> updateDefaultPercentageEarningsPerCourseAsync(long Id, long percentage)
        {
            try
            {
                var getPercentage = _context.DefaultPercentageEarningsPerCourse.Where(x=>x.Id == Id).FirstOrDefault();

                if (getPercentage != null)
                {
                    //check if the percentage is more than 100%
                    if (percentage > 100)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Percentage Earnings cant be greater than 100%"};
                    }
                    else
                    {
                        getPercentage.Percentage = percentage;
                        getPercentage.LastDateUpdated = DateTime.Now;

                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully"};
                    }
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Record Found"};

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllPercentageEarnedOnCoursesAsync()
        {
            try
            {
                var result = from cr in _context.PercentageEarnedOnCourses
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Percentage,
                                 cr.DateCreated,
                                 cr.LastUpdated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getPercentageEarnedOnCoursesByIdAsync(long Id)
        {
            try
            {
                var result = from cr in _context.PercentageEarnedOnCourses where cr.Id == Id
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Percentage,
                                 cr.DateCreated,
                                 cr.LastUpdated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault()};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getPercentageEarnedOnCoursesByCourseIdAsync(long courseId)
        {
            try
            {
                var result = from cr in _context.PercentageEarnedOnCourses
                             where cr.CourseId == courseId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Percentage,
                                 cr.DateCreated,
                                 cr.LastUpdated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault()};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> updatePercentageEarnedOnCoursesAsync(long courseId, long percentage)
        {
            try
            {
                var getPercentage = _context.PercentageEarnedOnCourses.Where(x => x.CourseId == courseId).FirstOrDefault();

                if (getPercentage != null)
                {
                    if (percentage > 100)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Percentage Earnings on Courses cant be greater than 100%" };
                    }
                    else
                    {
                        getPercentage.Percentage = percentage;
                        getPercentage.LastUpdated = DateTime.Now;

                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully" };
                    }                        
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Record Found" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        //------------------------------- Course Return -------------------------------------------------------------

        public async Task<GenericResponseModel> refundCourseAsync(CourseRefundRequestModel obj)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(obj.CourseId);
                var checkLearner = new CheckerValidation(_context).checkLearnerById(obj.LearnerId);

                if (checkCourse == true && checkLearner == true)
                {
                    var courseEnroll = _context.CourseEnrollees.Where(x => x.CourseId == obj.CourseId && x.LearnerId == obj.LearnerId).FirstOrDefault();
                    if (courseEnroll != null)
                    {
                        var courseRefund = _context.CourseRefund.Where(x => x.CourseId == obj.CourseId && x.LearnerId == obj.LearnerId).FirstOrDefault();

                        if (courseRefund == null)
                        {
                            var crsRefund = new CourseRefund
                            {
                                CourseId = obj.CourseId,
                                LearnerId = obj.LearnerId,
                                ReasonForReturn = obj.ReasonForReturn,
                                ReviewStatus = false,
                                IsSettled = false,
                                DateReturned = DateTime.Now
                            };

                            await _context.CourseRefund.AddAsync(crsRefund);
                            await _context.SaveChangesAsync();

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful" };
                        }

                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Request For Course Refund Already Made" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Course Was not Enrolled For"};
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Cousre/Learner With the Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> deleteRefundCourseAsync(long refundCourseId)
        {
            try
            {
                var courseRefund = _context.CourseRefund.Where(x => x.Id == refundCourseId).FirstOrDefault();
                if (courseRefund != null)
                {
                     _context.CourseRefund.Remove(courseRefund);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Sucessfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course Refund With Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllRefundCoursesAsync()
        {
            try
            {
                var result = from cr in _context.CourseRefund
                             select new
                             {
                                 cr.Id,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.ReasonForReturn,
                                 cr.ReviewStatus,
                                 cr.IsSettled,
                                 cr.DateSettled,
                                 cr.DateReturned
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getRefundCoursesByIdAsync(long refundCourseId)
        {
            try
            {
                var result = from cr in _context.CourseRefund where cr.Id == refundCourseId
                             select new
                             {
                                 cr.Id,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.ReasonForReturn,
                                 cr.ReviewStatus,
                                 cr.IsSettled,
                                 cr.DateSettled,
                                 cr.DateReturned
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getRefundCourseByCourseIdAsync(long courseId)
        {
            try
            {
                var result = from cr in _context.CourseRefund where cr.CourseId == courseId
                             select new
                             {
                                 cr.Id,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.ReasonForReturn,
                                 cr.ReviewStatus,
                                 cr.IsSettled,
                                 cr.DateSettled,
                                 cr.DateReturned
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getRefundCourseByLearnerIdAsync(Guid learnerId)
        {
            try
            {
                var result = from cr in _context.CourseRefund
                             where cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
                                 cr.ReasonForReturn,
                                 cr.ReviewStatus,
                                 cr.IsSettled,
                                 cr.DateSettled,
                                 cr.DateReturned
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> courseProgressAsync(Guid learnerId, long courseId, long videoId)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(courseId);
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);

                if (checkCourse == true && checkLearner == true)
                {
                    CourseEnrollees courseEnroll = _context.CourseEnrollees.Where(x => x.CourseId == courseId && x.LearnerId == learnerId).FirstOrDefault();

                    if (courseEnroll != null)
                    {
                        //get list of watched videos
                        var progressVideo = await _context.CourseEnrolleProgressVideos.Where(x => x.CourseEnrolleeId == courseEnroll.Id && x.VideoId == videoId).FirstOrDefaultAsync();
                        if (progressVideo == null)
                        {

                            //count the number of videos in the course
                            IList<CourseTopicVideos> courseVideo = (_context.CourseTopicVideos.Where(x => x.CourseId == courseId)).ToList();

                            if (courseVideo.Count() > 0)
                            {
                                //check if the percentage of completion is 100
                                if (courseEnroll.PercentageCompletion < 99)
                                {
                                    int noOfVideos = courseVideo.Count();
                                    int videoSelected = 1;

                                    decimal percentageCompleted = (videoSelected * 100) / noOfVideos;
                                    decimal percentageCompletion = courseEnroll.PercentageCompletion;

                                    //update the percentage completion
                                    decimal updatedPercentageCompletion = percentageCompletion + percentageCompleted;

                                    courseEnroll.PercentageCompletion = updatedPercentageCompletion;
                                    await _context.SaveChangesAsync();

                                    CourseEnrolleProgressVideo courseEnrolleProgressVideo = new CourseEnrolleProgressVideo
                                    {
                                        CourseEnrolleeId = courseEnroll.Id,
                                        VideoId = videoId,
                                        CreatedAt = DateTime.Now,
                                        UpdatedAt = DateTime.Now
                                    };
                                    _context.CourseEnrolleProgressVideos.Add(courseEnrolleProgressVideo);
                                    await _context.SaveChangesAsync();

                                    //check if the updated percentage is 100; update the date completed if percentage is 100
                                    if (updatedPercentageCompletion >= 99)
                                    {
                                        //generate uniqueId
                                        Guid uniqueId = Guid.NewGuid();

                                        //date completed
                                        string dateCompleted = DateTime.Now.Date.ToShortDateString();

                                        //this concantenate and generates the certificate's number
                                        string certificateNumber = "EC-" + "" + dateCompleted.Replace("/", "") + "-" + uniqueId;

                                        //create the certificate on completion of the course
                                        Certificates cert = new Certificates
                                        {
                                            LearnerId = learnerId,
                                            CourseId = courseId,
                                            CertificateNumber = certificateNumber,
                                            DateGenerated = DateTime.Now,
                                        };

                                        await _context.Certificates.AddAsync(cert);
                                        //update the DateCompleted to the current date at which the course was completed by the learner
                                        courseEnroll.DateCompleted = DateTime.Now;

                                        await _context.SaveChangesAsync();

                                    }

                                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful!", Data = percentageCompleted };

                                }

                                return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Course has been Completed!" };
                            }
                        }
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Learner With the Specified ID did not enroll for this Course" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Cousre/Learner With the Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCourseProgressAsync(Guid learnerId, long courseId)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(courseId);
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);

                if (checkCourse == true && checkLearner == true)
                {
                    var result = from cr in _context.CourseEnrollees
                                 where cr.CourseId == courseId && cr.LearnerId == learnerId
                                 select new
                                 {
                                     cr.Id,
                                     cr.LearnerId,
                                     cr.CourseId,
                                     cr.IsCompleted,
                                     cr.IsInProgress,
                                     cr.PercentageCompletion,
                                     cr.DateCompleted,
                                     cr.DateEnrolled,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault()};
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Learner With the Specified ID did not enroll for this Course" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Cousre/Learner With the Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getCourseCertificateAsync(Guid learnerId, long courseId)
        {
            try
            {
                var checkCourse = new CheckerValidation(_context).checkCourseById(courseId);
                var checkLearner = new CheckerValidation(_context).checkLearnerById(learnerId);

                if (checkCourse == true && checkLearner == true)
                {
                    //get the certificate details
                    Certificates cert = _context.Certificates.Where(x => x.CourseId == courseId && x.LearnerId == learnerId).FirstOrDefault();

                    if (cert != null)
                    {
                        //to get thw date completed
                        CourseEnrollees courseEnroll = _context.CourseEnrollees.Where(x => x.CourseId == cert.CourseId && x.LearnerId == cert.LearnerId).FirstOrDefault();
                        //to get the course completed
                        Courses course = _context.Courses.Where(x => x.Id == cert.CourseId).FirstOrDefault();
                        //to get the course facilitator/Instructor
                        Facilitators facilitator = _context.Facilitators.Where(x => x.Id == course.FacilitatorId).FirstOrDefault();
                        //to get the learner
                        Learners learner = _context.Learners.Where(x => x.Id == cert.LearnerId).FirstOrDefault();

                        string learnerFullName = learner.FirstName + " " + learner.LastName; //add othernames later
                        string facilitatorFullName = facilitator.FirstName + " " + facilitator.LastName;
                        string courseName = course.CourseName;
                        DateTime dateCompleted = courseEnroll.DateCompleted;
                        string certificateNumber = cert.CertificateNumber;

                        //the certificate response object
                        CertificateResponseModel certificateData = new CertificateResponseModel();

                        certificateData.LernerFullName = learnerFullName;
                        certificateData.FacilitatorFullName = facilitatorFullName;
                        certificateData.CourseName = courseName;
                        certificateData.CompletionDate = dateCompleted;
                        certificateData.CertificateNumber = certificateNumber;

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = certificateData};

                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Certificate has not been generated for this course taken by the Learner with the specified ID" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Cousre/Learner With the Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
