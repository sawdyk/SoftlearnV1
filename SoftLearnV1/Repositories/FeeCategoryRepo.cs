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
    public class FeeCategoryRepo : IFeeCategoryRepo
    {
        private readonly AppDbContext _context;

        public FeeCategoryRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------FeeCategory---------------------------------------------------------------

        public async Task<GenericResponseModel> createFeeCategoryAsync(FeeCategoryRequestModel obj)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }
                //check if category to be created already exists
                var checkResult = _context.FeeCategory.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.CategoryName.ToLower() == obj.CategoryName.ToLower()).FirstOrDefault();

                //if the category doesnt exist, Create the category
                if (checkResult == null)
                {
                    var category = new FeeCategory
                    {
                        CampusId = obj.CampusId,
                        SchoolId = obj.SchoolId,
                        CategoryName = obj.CategoryName,
                        LastUpdated = DateTime.Now,
                        DateCreated = DateTime.Now,
                    };

                    await _context.FeeCategory.AddAsync(category);
                    await _context.SaveChangesAsync();

                    //get the Category Created
                    var getCategory = from cr in _context.FeeCategory
                                      where cr.Id == category.Id
                                      select new
                                      {
                                          cr.CampusId,
                                          cr.DateCreated,
                                          cr.Id,
                                          cr.LastUpdated,
                                          cr.SchoolCampuses.CampusName,
                                          cr.SchoolId,
                                          cr.SchoolInformation.SchoolName,
                                          cr.CategoryName,
                                      };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Category Created Successfully", Data = getCategory.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Category Already Exists" };
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
        public async Task<GenericResponseModel> updateFeeCategoryAsync(long categoryId, FeeCategoryRequestModel obj)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }
                //check if category to be updated already exists
                var checkResult = _context.FeeCategory.FirstOrDefault(x => x.Id == categoryId);

                //if the category exist, Update the category
                if (checkResult != null)
                {
                    checkResult.CampusId = obj.CampusId;
                    checkResult.SchoolId = obj.SchoolId;
                    checkResult.CategoryName = obj.CategoryName;
                    checkResult.LastUpdated = DateTime.Now;
                    await _context.SaveChangesAsync();

                    //get the Category Created
                    var getCategory = from cr in _context.FeeCategory
                                      where cr.Id == categoryId
                                      select new
                                      {
                                          cr.CampusId,
                                          cr.DateCreated,
                                          cr.Id,
                                          cr.LastUpdated,
                                          cr.SchoolCampuses.CampusName,
                                          cr.SchoolId,
                                          cr.SchoolInformation.SchoolName,
                                          cr.CategoryName,
                                      };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Category Updated Successfully", Data = getCategory.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Category Doesn't Exist" };
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

        public async Task<GenericResponseModel> deleteFeeCategoryAsync(long categoryId)
        {
            try
            {
                var subCategory = _context.FeeSubCategory.Where(x => x.FeeCategoryId == categoryId).ToList();
                if (subCategory.Count <= 0)
                {
                    var obj = _context.FeeCategory.Where(x => x.Id == categoryId).FirstOrDefault();
                    if (obj != null)
                    {
                        _context.FeeCategory.Remove(obj);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Category With the Specified ID!" };
                }
                return new GenericResponseModel { StatusCode = 400, StatusMessage = "Record can't be deleted because some records are depending on it!" };
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

        public async Task<GenericResponseModel> getAllFeeCategoryByCampusIdAsync(long campusId)
        {
            try
            {
                //check if the campusId is valid
                var checkResult = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeCategory
                                 where cr.CampusId == campusId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.CategoryName,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Campus with the specified ID" };

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

        public async Task<GenericResponseModel> getAllFeeCategoryBySchoolIdAsync(long schoolId)
        {
            try
            {
                //check if the schoolId is valid
                var checkResult = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeCategory
                                 where cr.SchoolId == schoolId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.CategoryName,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No School with the specified ID" };

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

        public async Task<GenericResponseModel> getFeeCategoryByIdAsync(long categoryId)
        {
            try
            {
                //check if the categoryId is valid
                var checkResult = new CheckerValidation(_context).checkFeeCategoryById(categoryId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeCategory
                                 where cr.Id == categoryId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.CategoryName
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Category with the specified ID" };

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
        //----------------------------FeeSubCategory---------------------------------------------------------------
        public async Task<GenericResponseModel> createFeeSubCategoryAsync(FeeSubCategoryRequestModel obj)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }
                var checkCategory = new CheckerValidation(_context).checkFeeCategoryById(obj.FeeCategoryId);
                if (checkCategory == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Category Doesn't Exist" };
                }
                //check if sub category to be created already exists
                var checkResult = _context.FeeSubCategory.Where(x => x.FeeCategoryId == obj.FeeCategoryId && x.FeeCode.ToLower() == obj.FeeCode.ToLower() && x.SubCategoryName.ToLower() == obj.SubCategoryName.ToLower()).FirstOrDefault();

                //if the sub category doesnt exist, Create the category
                if (checkResult == null)
                {
                    var subCategory = new FeeSubCategory
                    {
                        Description = obj.Description,
                        FeeCategoryId = obj.FeeCategoryId,
                        FeeCode = obj.FeeCode,
                        CampusId = obj.CampusId,
                        SchoolId = obj.SchoolId,
                        SubCategoryName = obj.SubCategoryName,
                        LastUpdated = DateTime.Now,
                        DateCreated = DateTime.Now,
                    };

                    await _context.FeeSubCategory.AddAsync(subCategory);
                    await _context.SaveChangesAsync();

                    //get the SubCategory Created
                    var getSubCategory = from cr in _context.FeeSubCategory
                                         where cr.Id == subCategory.Id
                                         select new
                                         {
                                             cr.CampusId,
                                             cr.DateCreated,
                                             cr.Id,
                                             cr.LastUpdated,
                                             cr.SchoolCampuses.CampusName,
                                             cr.SchoolId,
                                             cr.SchoolInformation.SchoolName,
                                             cr.SubCategoryName,
                                             cr.Description,
                                             cr.FeeCategory.CategoryName,
                                             cr.FeeCategoryId,
                                             cr.FeeCode,
                                         };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Sub Category Created Successfully", Data = getSubCategory.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Sub Category Already Exists" };
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

        public async Task<GenericResponseModel> updateFeeSubCategoryAsync(long subCategoryId, FeeSubCategoryRequestModel obj)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }
                var checkCategory = new CheckerValidation(_context).checkFeeCategoryById(obj.FeeCategoryId);
                if (checkCategory == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Category Doesn't Exist" };
                }
                //check if sub category to be updated already exists
                var checkResult = _context.FeeSubCategory.FirstOrDefault(x => x.Id == subCategoryId);

                //if the sub category exist, Update the sub category
                if (checkResult != null)
                {
                    checkResult.CampusId = obj.CampusId;
                    checkResult.SchoolId = obj.SchoolId;
                    checkResult.SubCategoryName = obj.SubCategoryName;
                    checkResult.FeeCode = obj.FeeCode;
                    checkResult.Description = obj.Description;
                    checkResult.FeeCategoryId = obj.FeeCategoryId;
                    checkResult.LastUpdated = DateTime.Now;
                    await _context.SaveChangesAsync();

                    //get the Sub Category Created
                    var getSubCategory = from cr in _context.FeeSubCategory
                                         where cr.Id == subCategoryId
                                         select new
                                         {
                                             cr.CampusId,
                                             cr.DateCreated,
                                             cr.Id,
                                             cr.LastUpdated,
                                             cr.SchoolCampuses.CampusName,
                                             cr.SchoolId,
                                             cr.SchoolInformation.SchoolName,
                                             cr.SubCategoryName,
                                             cr.Description,
                                             cr.FeeCategory.CategoryName,
                                             cr.FeeCategoryId,
                                             cr.FeeCode,
                                         };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Sub Category Updated Successfully", Data = getSubCategory.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Sub Category Doesn't Exist" };
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

        public async Task<GenericResponseModel> deleteFeeSubCategoryAsync(long subCategoryId)
        {
            try
            {
                var templates = _context.FeeTemplateList.Where(x => x.FeeSubCategoryId == subCategoryId).ToList();
                var fees = _context.SchoolFees.Where(x => x.FeeSubCategoryId == subCategoryId).ToList();
                if (templates.Count <= 0 && fees.Count <= 0)
                {
                    var obj = _context.FeeSubCategory.Where(x => x.Id == subCategoryId).FirstOrDefault();
                if (obj != null)
                {
                    _context.FeeSubCategory.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Sub Category With the Specified ID!" };
                }
                return new GenericResponseModel { StatusCode = 400, StatusMessage = "Record can't be deleted because some records are depending on it!" };
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

        public async Task<GenericResponseModel> getAllFeeSubCategoryByCategoryIdAsync(long categoryId)
        {
            try
            {
                //check if the categoryId is valid
                var checkResult = new CheckerValidation(_context).checkFeeCategoryById(categoryId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeSubCategory
                                 where cr.FeeCategoryId == categoryId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.SubCategoryName,
                                     cr.Description,
                                     cr.FeeCategory.CategoryName,
                                     cr.FeeCategoryId,
                                     cr.FeeCode,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Category with the specified ID" };

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

        public async Task<GenericResponseModel> getAllFeeSubCategoryByCampusIdAsync(long campusId)
        {
            try
            {
                //check if the campusId is valid
                var checkResult = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeSubCategory
                                 where cr.CampusId == campusId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.SubCategoryName,
                                     cr.Description,
                                     cr.FeeCategory.CategoryName,
                                     cr.FeeCategoryId,
                                     cr.FeeCode,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Campus with the specified ID" };

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

        public async Task<GenericResponseModel> getAllFeeSubCategoryBySchoolIdAsync(long schoolId)
        {
            try
            {
                //check if the schoolId is valid
                var checkResult = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeSubCategory
                                 where cr.SchoolId == schoolId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.SubCategoryName,
                                     cr.Description,
                                     cr.FeeCategory.CategoryName,
                                     cr.FeeCategoryId,
                                     cr.FeeCode,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No School with the specified ID" };

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

        public async Task<GenericResponseModel> getFeeSubCategoryByIdAsync(long subCategoryId)
        {
            try
            {
                //check if the sub categoryId is valid
                var checkResult = new CheckerValidation(_context).checkFeeSubCategoryById(subCategoryId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeSubCategory
                                 where cr.Id == subCategoryId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.SubCategoryName,
                                     cr.Description,
                                     cr.FeeCategory.CategoryName,
                                     cr.FeeCategoryId,
                                     cr.FeeCode,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Sub Category with the specified ID" };

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
