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
    public class FeeTemplateRepo : IFeeTemplateRepo
    {
        private readonly AppDbContext _context;

        public FeeTemplateRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------FeeTemplate---------------------------------------------------------------
        public async Task<GenericResponseModel> createFeeTemplateAsync(FeeTemplateRequestModel obj)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if(checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }
                //check if template to be created already exists
                var checkResult = _context.FeeTemplate.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.TemplateName.ToLower() == obj.TemplateName.ToLower()).FirstOrDefault();

                //if the template doesnt exist, Create the template
                if (checkResult == null)
                {
                    var template = new FeeTemplate
                    {
                        CampusId = obj.CampusId,
                        SchoolId = obj.SchoolId,
                        TemplateName = obj.TemplateName,
                        Description = obj.Description,
                        LastUpdated = DateTime.Now,
                        DateCreated = DateTime.Now,
                    };

                    await _context.FeeTemplate.AddAsync(template);
                    await _context.SaveChangesAsync();

                    //get the Template Created
                    var getTemplate = from cr in _context.FeeTemplate
                                        where cr.Id == template.Id
                                        select new
                                        {
                                            cr.CampusId,
                                            cr.DateCreated,
                                            cr.Description,
                                            cr.Id,
                                            cr.LastUpdated,
                                            cr.SchoolCampuses.CampusName,
                                            cr.SchoolId,
                                            cr.SchoolInformation.SchoolName,
                                            cr.TemplateName
                                        };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Template Created Successfully", Data = getTemplate.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Template Already Exists" };
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

        public async Task<GenericResponseModel> updateFeeTemplateAsync(long templateId, FeeTemplateRequestModel obj)
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
                //check if template to be updated already exists
                var checkResult = _context.FeeTemplate.FirstOrDefault(x => x.Id == templateId);

                //if the template exist, Update the template
                if (checkResult != null)
                {
                    checkResult.CampusId = obj.CampusId;
                    checkResult.SchoolId = obj.SchoolId;
                    checkResult.TemplateName = obj.TemplateName;
                    checkResult.Description = obj.Description;
                    checkResult.LastUpdated = DateTime.Now;
                    await _context.SaveChangesAsync();

                    //get the Template Updated
                    var getTemplate = from cr in _context.FeeTemplate
                                      where cr.Id == templateId
                                      select new
                                      {
                                          cr.CampusId,
                                          cr.DateCreated,
                                          cr.Description,
                                          cr.Id,
                                          cr.LastUpdated,
                                          cr.SchoolCampuses.CampusName,
                                          cr.SchoolId,
                                          cr.SchoolInformation.SchoolName,
                                          cr.TemplateName
                                      };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Template Updated Successfully", Data = getTemplate.FirstOrDefault() };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Template Doesn't Exist" };
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

        public async Task<GenericResponseModel> deleteFeeTemplateAsync(long templateId)
        {
            try
            {
                var templates = _context.FeeTemplateList.Where(x => x.TemplateId == templateId).ToList();
                var fees = _context.SchoolFees.Where(x => x.TemplateId == templateId).ToList();
                if (templates.Count <= 0 && fees.Count <= 0)
                {
                    var obj = _context.FeeTemplate.Where(x => x.Id == templateId).FirstOrDefault();
                if (obj != null)
                {
                    _context.FeeTemplate.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Template With the Specified ID!" };
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

        public async Task<GenericResponseModel> getAllFeeTemplateByCampusIdAsync(long campusId)
        {
            try
            {
                //check if the campusId is valid
                var checkResult = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeTemplate
                                 where cr.CampusId == campusId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Description,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.TemplateName
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

        public async Task<GenericResponseModel> getAllFeeTemplateBySchoolIdAsync(long schoolId)
        {
            try
            {
                //check if the campusId is valid
                var checkResult = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeTemplate
                                 where cr.SchoolId == schoolId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Description,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.TemplateName
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

        public async Task<GenericResponseModel> getFeeTemplateByIdAsync(long templateId)
        {
            try
            {
                //check if the templateId is valid
                var checkResult = new CheckerValidation(_context).checkFeeTemplateById(templateId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeTemplate
                                 where cr.Id == templateId
                                 orderby cr.Id ascending
                                 select new
                                 {
                                     cr.CampusId,
                                     cr.DateCreated,
                                     cr.Description,
                                     cr.Id,
                                     cr.LastUpdated,
                                     cr.SchoolCampuses.CampusName,
                                     cr.SchoolId,
                                     cr.SchoolInformation.SchoolName,
                                     cr.TemplateName
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Template with the specified ID" };

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
        //----------------------------FeeTemplateList---------------------------------------------------------------
        public async Task<GenericResponseModel> createFeeTemplateListAsync(FeeTemplateListRequestModel obj)
        {
            IList<object> data = new List<object>();
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
                var checkTemplate = new CheckerValidation(_context).checkFeeTemplateById(obj.TemplateId);
                if (checkTemplate == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "TemplateId Doesn't Exist" };
                }
                foreach(long subCategoryId in obj.FeeSubCategoryIdList) { 
                var checkSubCategory = new CheckerValidation(_context).checkFeeSubCategoryById(subCategoryId);
                if (checkSubCategory == false)
                {
                        data.Add(new GenericResponseModel { StatusCode = 305, StatusMessage = "Sub Category Doesn't Exist" });
                        continue;
                }
                //check if template to be created already exists
                var checkResult = _context.FeeTemplateList.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.FeeSubCategoryId == subCategoryId && x.TemplateId == obj.TemplateId).FirstOrDefault();

                    //if the template doesnt exist, Create the template
                    if (checkResult == null)
                    {
                        var templateList = new FeeTemplateList
                        {
                            CampusId = obj.CampusId,
                            FeeSubCategoryId = subCategoryId,
                            SchoolId = obj.SchoolId,
                            TemplateId = obj.TemplateId,
                            LastUpdated = DateTime.Now,
                            DateCreated = DateTime.Now,
                        };

                        await _context.FeeTemplateList.AddAsync(templateList);
                        await _context.SaveChangesAsync();

                        //get the Template Created
                        var getTemplate = from cr in _context.FeeTemplateList
                                          where cr.Id == templateList.Id
                                          select new
                                          {
                                              cr.CampusId,
                                              cr.DateCreated,
                                              cr.Id,
                                              cr.LastUpdated,
                                              cr.SchoolCampuses.CampusName,
                                              cr.SchoolId,
                                              cr.SchoolInformation.SchoolName,
                                              cr.FeeTemplate.TemplateName,
                                              cr.TemplateId,
                                              cr.FeeSubCategory.SubCategoryName,
                                              cr.FeeSubCategoryId
                                          };
                        data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Template Created Successfully", Data = getTemplate.FirstOrDefault() });
                    }
                    else
                    {
                        data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Template Already Exists"});
                    }
                }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
                
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
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
            }
        }

        public Task<GenericResponseModel> updateFeeTemplateListAsync(long templateListId, FeeTemplateListRequestModel obj)
        {
            throw new NotImplementedException();
        }

        public async Task<GenericResponseModel> deleteFeeTemplateListAsync(long templateId)
        {
            try
            {
                var obj = _context.FeeTemplateList.Where(x => x.TemplateId == templateId).ToList();
                if (obj.Count > 0)
                {
                    _context.FeeTemplateList.RemoveRange(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = obj.Count.ToString() +" records Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Template With the Specified ID!" };
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

        public async Task<GenericResponseModel> deleteFeeInTemplateListAsync(long id)
        {
            try
            {
                var obj = _context.FeeTemplateList.Where(x => x.Id == id).FirstOrDefault();
                if (obj != null)
                {
                    _context.FeeTemplateList.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = " Record Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Fee With the Specified ID!" };
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

        public async Task<GenericResponseModel> getFeeTemplateListByCampusIdAsync(long campusId, long templateId)
        {
            try
            {
                //check if the campusId or templateId is valid
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                var checkTemplate = new CheckerValidation(_context).checkFeeTemplateById(templateId);
                if (checkCampus == true && checkTemplate == true)
                {
                    var result = from cr in _context.FeeTemplateList
                                 where cr.CampusId == campusId && cr.TemplateId == templateId
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
                                     cr.FeeTemplate.TemplateName,
                                     cr.TemplateId,
                                     cr.FeeSubCategory.SubCategoryName,
                                     cr.FeeSubCategoryId
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Invalid parameter(s)" };

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

        public async Task<GenericResponseModel> getAllFeeTemplateListByCampusIdAsync(long campusId)
        {
            try
            {
                //check if the campusId is valid
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == true)
                {
                    var result = from cr in _context.FeeTemplateList
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
                                     cr.FeeTemplate.TemplateName,
                                     cr.TemplateId,
                                     cr.FeeSubCategory.SubCategoryName,
                                     cr.FeeSubCategoryId
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Invalid parameter" };

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

        public async Task<GenericResponseModel> getFeeTemplateListBySchoolIdAsync(long schoolId, long templateId)
        {
            try
            {
                //check if the schoolId or templateId is valid
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                var checkTemplate = new CheckerValidation(_context).checkFeeTemplateById(templateId);
                if (checkSchool == true && checkTemplate == true)
                {
                    var result = from cr in _context.FeeTemplateList
                                 where cr.SchoolId == schoolId && cr.TemplateId == templateId
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
                                     cr.FeeTemplate.TemplateName,
                                     cr.TemplateId,
                                     cr.FeeSubCategory.SubCategoryName,
                                     cr.FeeSubCategoryId
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Invalid parameter(s)" };

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

        public async Task<GenericResponseModel> getAllFeeTemplateListBySchoolIdAsync(long schoolId)
        {
            try
            {
                //check if the schoolId is valid
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == true)
                {
                    var result = from cr in _context.FeeTemplateList
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
                                     cr.FeeTemplate.TemplateName,
                                     cr.TemplateId,
                                     cr.FeeSubCategory.SubCategoryName,
                                     cr.FeeSubCategoryId
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Invalid parameter" };

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

        public async Task<GenericResponseModel> getFeeTemplateListByIdAsync(long templateListId)
        {
            try
            {
                //check if the templateListId is valid
                var checkResult = new CheckerValidation(_context).checkFeeTemplateListById(templateListId);
                if (checkResult == true)
                {
                    var result = from cr in _context.FeeTemplateList
                                 where cr.Id == templateListId
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
                                     cr.FeeTemplate.TemplateName,
                                     cr.TemplateId,
                                     cr.FeeSubCategory.SubCategoryName,
                                     cr.FeeSubCategoryId
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Record With the Specified ID!" };

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
