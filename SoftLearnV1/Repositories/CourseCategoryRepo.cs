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
using Microsoft.EntityFrameworkCore;

namespace SoftLearnV1.Repositories
{
    public class CourseCategoryRepo : ICourseCategoryRepo
    {
        private readonly AppDbContext _context;
        private readonly ICloudinaryRepo _cloudinary;

        public CourseCategoryRepo(AppDbContext context, ICloudinaryRepo cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        //create a Course Category
        public async Task<GenericResponseModel> createCourseCategoryAsync(CourseCategoryCreateRequestModel obj)
        {
            try
            {
                var checkCategoryExist = _context.CourseCategory.Where(x => x.CourseCategoryName == obj.CourseCategoryName).FirstOrDefault();

                if (checkCategoryExist == null)
                {
                    //string courseCatgoryImageUrl = string.Empty;
                    //if (obj.CourseCategoryImage != null)
                    //{
                    //    //Image Upload to Cloudinary Instance
                    //    var imageUploadResult = await _cloudinary.CategoryImagesUpload(obj.CourseCategoryImage);
                    //    courseCatgoryImageUrl = imageUploadResult.Url.ToString();
                    //}

                    //Save the Category
                    var newCat = new CourseCategory
                    {
                        CourseCategoryName = obj.CourseCategoryName,
                        CategoryImageUrl = obj.CourseCategoryImage,
                        CategoryDescription = obj.CategoryDescription,
                    };
                    await _context.CourseCategory.AddAsync(newCat);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Course Category Already Exists" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Category Created Successfully" };

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
        public async Task<GenericResponseModel> updateCourseCategoryAsync(long courseCategoryId, CourseCategoryCreateRequestModel obj)
        {
            try
            {
                CourseCategory category = _context.CourseCategory.Where(x => x.Id == courseCategoryId).FirstOrDefault();

                if (category != null)
                {
                    //Update the Category
                    category.CourseCategoryName = obj.CourseCategoryName;
                    category.CategoryImageUrl = obj.CourseCategoryImage;
                    category.CategoryDescription = obj.CategoryDescription;

                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Category Updated Successfully"};
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course Category with the Specified ID"};
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

        public async Task<GenericResponseModel> deleteCourseCategoryAsync(long courseCategoryId)
        {
            try
            {
                CourseCategory category = _context.CourseCategory.Where(x => x.Id == courseCategoryId).FirstOrDefault();

                if (category != null)
                {
                    _context.CourseCategory.Remove(category);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Course Category Deleted Successfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Course Category with the Specified ID" };
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

        //Gets the Course Category By a specified ID
        public async Task<GenericResponseModel> getAllCourseCategoryAsync()
        {
            try
            {
                var result = from ct in _context.CourseCategory
                             .Include(c => c.CourseSubCategory)
                             select new
                             {
                                 ct.Id,
                                 ct.CourseCategoryName,
                                 ct.CategoryImageUrl,
                                 ct.CategoryDescription,
                                 ct.CourseSubCategory,
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

        //All Course Category with pagination
        public async Task<GenericResponseModel> getAllCourseCategoryAsync(int pageNumber, int pageSize)
        {
            try
            {
                var result = (from ct in _context.CourseCategory
                             .Include(c => c.CourseSubCategory)
                             orderby ct.Id ascending
                             select new
                             {
                                 ct.Id,
                                 ct.CourseCategoryName,
                                 ct.CategoryImageUrl,
                                 ct.CategoryDescription,
                                 ct.CourseSubCategory
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
      
        //Gets the Course Category by ID

        public async Task<GenericResponseModel> getCourseCategoryByIdAsync(long courseCategoryId)
        {
            try
            {
                var result = from ct in _context.CourseCategory
                             .Include(c=>c.CourseSubCategory)
                             where ct.Id == courseCategoryId
                             select new
                             {
                                 ct.Id,
                                 ct.CourseCategoryName,
                                 ct.CategoryImageUrl,
                                 ct.CategoryDescription,
                                 ct.CourseSubCategory
                             };
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No CourseCategory with the specified ID", };

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

        public async Task<GenericResponseModel> popularCourseCategoryAsync()
        {
            try
            {
                int top = 10;
                IList<object> result = new List<object>();
                IList<long> crsCatId = new List<long>();

                // top ten most frequent course categories 
                var courseId = (from cr in _context.CourseEnrollees
                             .GroupBy(c => c.CourseId)
                             .OrderByDescending(gp => gp.Count())
                             .Take(top) //take top ten
                             .Select(g => g.Key)
                               select cr).Distinct();

                //loop through all the courseId to get their categories
                foreach (var courseIds in courseId)
                {
                    //all the categoryId 
                    var courseCat = _context.Courses.Where(c => c.Id == courseIds).FirstOrDefault().CourseCategoryId;
                    crsCatId.Add(courseCat);
                }

                //loop through all the categories to get their obj
                foreach (var courseCats in crsCatId.Distinct())
                {
                    var obj = (from cr in _context.CourseCategory
                               where cr.Id == courseCats
                               select new
                               {
                                   cr.Id,
                                   cr.CourseCategoryName,
                                   cr.CategoryDescription,
                                   cr.CategoryImageUrl,
                               }).Distinct().FirstOrDefault();
                    //add the categories to a list       
                    result.Add(obj);
                }

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

        public async Task<GenericResponseModel> topCoursesInCourseCategoryAsync(long categoryId)
        {
            try
            {
                int top = 20;
                //using LINQ TO SQL SubQuery, returns all Courses learners enrolled for the most in a category
                var result = (from cr in _context.Courses
                              where cr.CourseCategoryId == categoryId
                              where (from cs in _context.CourseEnrollees select cs.CourseId).Contains(cr.Id)
                              select new
                                {
                                  cr.Id,
                                  cr.FacilitatorId,
                                  cr.Facilitators.FirstName,
                                  cr.Facilitators.LastName,
                                  cr.CourseName,
                                  cr.CourseSubTitle,
                                  cr.CourseImageUrl,
                                  cr.CourseType.CourseTypeName,
                                  cr.CourseLevelTypes.LevelTypeName,
                                  cr.CourseCategory.CourseCategoryName,
                                  cr.CourseCategory.CategoryImageUrl,
                                  cr.CourseCategory.CategoryDescription,
                                  cr.CourseSubCategory.CourseSubCategoryName,
                                  cr.CourseSubCategory.CourseSubCategoryImageUrl,
                                  cr.CourseSubCategory.CourseSubCategoryDescription,
                                  cr.CourseAmount,

                              }).Take(top);

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
    }
}
