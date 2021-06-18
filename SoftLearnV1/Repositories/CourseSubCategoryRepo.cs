using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CourseSubCategoryRepo : ICourseSubCategoryRepo
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryRepo _cloudinary;

        public CourseSubCategoryRepo(AppDbContext context, ICloudinaryRepo cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }
        //--------------------------------COURSE SUBCATEGORY-----------------------------------------------------------

        public async Task<GenericResponseModel> createCourseSubCategoryAsync(CourseSubCategoryRequestModel obj)
        {
            try
            {
                var checkSubCategoryExist = _context.CourseSubCategory.Where(x => x.CourseCategoryId == obj.CourseCategoryId && x.CourseSubCategoryName == obj.CourseSubCategoryName).FirstOrDefault();

                if (checkSubCategoryExist == null)
                {
                    //Save the SubCategory
                    var newSubCat = new CourseSubCategory
                    {
                        CourseCategoryId = obj.CourseCategoryId,
                        CourseSubCategoryName = obj.CourseSubCategoryName,
                        CourseSubCategoryImageUrl = obj.CourseSubCategoryImageUrl,
                        CourseSubCategoryDescription = obj.CourseSubCategoryDescription,
                    };
                    await _context.CourseSubCategory.AddAsync(newSubCat);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This SubCategory Already Exists For this Course Category" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course SubCategory Created Successfully" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }


        public async Task<GenericResponseModel> getCourseSubCategoryByIdAsync(long courseSubCategoryId)
        {
            try
            {
                var result = from ct in _context.CourseSubCategory
                             where ct.Id == courseSubCategoryId
                             select new
                             {
                                 ct.Id,
                                 ct.CourseCategoryId,
                                 ct.CourseCategory.CourseCategoryName,
                                 ct.CourseCategory.CategoryImageUrl,
                                 ct.CourseSubCategoryName,
                                 ct.CourseSubCategoryImageUrl,
                                 ct.CourseSubCategoryDescription,
                             };
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericResponseModel> getAllCourseSubCategoryAsync()
        {
            try
            {
                var result = from ct in _context.CourseSubCategory
                             select new
                             {
                                 ct.Id,
                                 ct.CourseCategoryId,
                                 ct.CourseCategory.CourseCategoryName,
                                 ct.CourseCategory.CategoryImageUrl,
                                 ct.CourseSubCategoryName,
                                 ct.CourseSubCategoryImageUrl,
                                 ct.CourseSubCategoryDescription,
                             };
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericResponseModel> updateCourseSubCategoryAsync(long courseSubCategoryId, CourseSubCategoryRequestModel obj)
        {
            try
            {
                CourseSubCategory courseSubCategory = _context.CourseSubCategory.Where(x => x.Id == courseSubCategoryId).FirstOrDefault();

                if (courseSubCategory != null)
                {
                    //Update the SubCategory
                    courseSubCategory.CourseCategoryId = obj.CourseCategoryId;
                    courseSubCategory.CourseSubCategoryName = obj.CourseSubCategoryName;
                    courseSubCategory.CourseSubCategoryImageUrl = obj.CourseSubCategoryImageUrl;
                    courseSubCategory.CourseSubCategoryDescription = obj.CourseSubCategoryDescription;
                   
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course SubCategory Updated Successfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course SubCategory With the Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> deleteCourseSubCategoryAsync(long courseSubCategoryId)
        {
            try
            {
                CourseSubCategory courseSubCategory = _context.CourseSubCategory.Where(x => x.Id == courseSubCategoryId).FirstOrDefault();

                if (courseSubCategory != null)
                {
                    _context.CourseSubCategory.Remove(courseSubCategory);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course SubCategory Deleted Successfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course SubCategory With the Specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getAllCourseSubCategoryByCourseCategoryIdAsync(long courseCategoryId)
        {
            try
            {
                var result = from ct in _context.CourseSubCategory
                             where ct.CourseCategoryId == courseCategoryId
                             select new
                             {
                                 ct.Id,
                                 ct.CourseCategoryId,
                                 ct.CourseCategory.CourseCategoryName,
                                 ct.CourseCategory.CategoryImageUrl,
                                 ct.CourseSubCategoryName,
                                 ct.CourseSubCategoryImageUrl,
                                 ct.CourseSubCategoryDescription,
                             };
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericResponseModel> getAllCourseSubCategoryByCourseCategoryIdAsync(int pageNumber, int pageSize, long courseCategoryId)
        {
            try
            {
                var result = (from ct in _context.CourseSubCategory
                              where ct.CourseCategoryId == courseCategoryId
                              orderby ct.Id ascending
                              select new
                              {
                                  ct.Id,
                                  ct.CourseCategoryId,
                                  ct.CourseCategory.CourseCategoryName,
                                  ct.CourseCategory.CategoryImageUrl,
                                  ct.CourseSubCategoryName,
                                  ct.CourseSubCategoryImageUrl,
                                  ct.CourseSubCategoryDescription,
                              }).Skip((pageNumber - 1) * pageSize).Take(pageSize);
                //reduce the pageNumber by 1 and multiply by the pageSize(5) (Skip 0 Take 5, Skip 5 Take 5, Skip 10 Take 5 etc)

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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
