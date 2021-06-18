﻿using Microsoft.Extensions.Configuration;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.Security;
using SoftLearnV1.Services.Email;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class ScoresConfigRepo : IScoresConfigRepo
    {
        private readonly AppDbContext _context;

        public ScoresConfigRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseModel> getAllScoreCategoryAsync()
        {
            try
            {
                //returns all the Score Category
                var result = from cl in _context.ScoreCategory select cl;

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getScoreCategoryByIdAsync(long scoreCategoryId)
        {
            try
            {
                //returns all the Score Category by Id
                var result = from cl in _context.ScoreCategory where cl.Id == scoreCategoryId select cl;

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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        //-------------------------------------

        public async Task<GenericResponseModel> createScoreGradesAsync(ScoreGradeCreateRequestModel obj)
        {
            try
            {
                var scrgrade = _context.ScoreGrading.Where(c => c.LowestRange == obj.LowestRange && c.HighestRange == obj.HighestRange 
                && c.Grade == obj.Grade && c.Remark == obj.Remark && c.SchoolId == obj.SchoolId && c.CampusId == obj.CampusId && c.ClassId == obj.ClassId).FirstOrDefault();

                if (scrgrade == null)
                {
                    var scGrd = new ScoreGrading
                    {
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        ClassId = obj.ClassId,
                        LowestRange = obj.LowestRange,
                        HighestRange = obj.HighestRange,
                        Grade = obj.Grade,
                        Remark = obj.Remark,
                        DateCreated = DateTime.Now,
                    };

                    await _context.ScoreGrading.AddAsync(scGrd);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Grade Created Successfully",};

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Score Grading Already Exist",};
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

        public async Task<GenericResponseModel> getAllScoreGradesAsync(long schoolId, long campusId)
        {
            try
            {
                //returns all the Score Grading
                var result = from cl in _context.ScoreGrading where cl.SchoolId == schoolId && cl.CampusId == campusId
                            select new
                            {
                                cl.Id,
                                cl.SchoolId,
                                cl.CampusId,
                                cl.ClassId,
                                cl.Classes.ClassName,
                                cl.LowestRange,
                                cl.HighestRange,
                                cl.Grade,
                                cl.Remark,
                                cl.DateCreated
                            };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(),};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> getScoreGradeByClassIdAsync(long classId, long schoolId, long campusId)
        {
            try
            {
                //returns all the Score Grading
                var result = from cl in _context.ScoreGrading
                             where cl.SchoolId == schoolId && cl.CampusId == campusId && cl.ClassId == classId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.LowestRange,
                                 cl.HighestRange,
                                 cl.Grade,
                                 cl.Remark,
                                 cl.DateCreated
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getScoreGradeByIdAsync(long scoreGradeId)
        {
            try
            {
                //returns all the Score Grading
                var result = from cl in _context.ScoreGrading
                             where cl.Id == scoreGradeId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.LowestRange,
                                 cl.HighestRange,
                                 cl.Grade,
                                 cl.Remark,
                                 cl.DateCreated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(),};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> updateScoreGradeAsync(long scoreGradeId, ScoreGradeCreateRequestModel obj)
        {
            try
            {
               //check if the score Grade exists
                var grd = _context.ScoreGrading.Where(c => c.Id == scoreGradeId).FirstOrDefault();

                if (grd != null)
                {
                    //check if the score grade already exists
                    var checkResult = _context.ScoreGrading.Where(c => c.LowestRange == obj.LowestRange && c.HighestRange == obj.HighestRange
                    && c.Grade == obj.Grade && c.Remark == obj.Remark && c.SchoolId == obj.SchoolId && c.CampusId == obj.CampusId && c.ClassId == obj.ClassId).FirstOrDefault();

                    //update the score grade if it doesnt exists
                    if (checkResult == null)
                    {
                        grd.SchoolId = obj.SchoolId;
                        grd.CampusId = obj.CampusId;
                        grd.ClassId = obj.ClassId;
                        grd.LowestRange = obj.LowestRange;
                        grd.HighestRange = obj.HighestRange;
                        grd.Grade = obj.Grade;
                        grd.Remark = obj.Remark;

                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Grade Updated Successfully", };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "This Score Grade Already Exists", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Score Grade with the specified Id", };
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

        public async Task<GenericResponseModel> deleteScoreGradeAsync(long scoreGradeId)
        {
            try
            {
                //check if the score Grade exists
                var grd = _context.ScoreGrading.Where(c => c.Id == scoreGradeId).FirstOrDefault();

                if (grd != null)
                {
                    _context.ScoreGrading.Remove(grd);
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Grade Deleted Successfully", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Score Grade with the specified Id", };
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

        //----------------------------------------SCORE CATEGORY CONFIGURATION--------------------------------------------------------------------------

        public async Task<GenericResponseModel> createScoreCategoryConfigAsync(ScoreCategoryConfigRequestModel obj)
        {
            try
            {
                //check if the score category already exists
                ScoreCategoryConfig catConfig = _context.ScoreCategoryConfig.Where(c => c.CategoryId == obj.CategoryId && c.SchoolId == obj.SchoolId
               && c.CampusId == obj.CampusId && c.ClassId == obj.ClassId && c.SessionId == obj.SessionId && c.TermId == obj.TermId).FirstOrDefault();

                if (catConfig == null)
                {
                    //get the totalpercentage
                    decimal totalPercentage = _context.ScoreCategoryConfig.Where(c => c.SchoolId == obj.SchoolId
                    && c.CampusId == obj.CampusId && c.ClassId == obj.ClassId && c.SessionId == obj.SessionId && c.TermId == obj.TermId).Sum(p => p.Percentage);

                    //adds the total percentage with the new percentage
                    decimal configPercentage = totalPercentage + obj.Percentage;

                    if (configPercentage <= 100)
                    {
                        //create score category if it doesnt exist
                        var cat = new ScoreCategoryConfig
                        {
                            CategoryId = obj.CategoryId,
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            ClassId = obj.ClassId,
                            TermId = obj.TermId,
                            SessionId = obj.SessionId,
                            Percentage = obj.Percentage,
                            DateCreated = DateTime.Now,
                        };

                        await _context.ScoreCategoryConfig.AddAsync(cat);
                        await _context.SaveChangesAsync();

                        //returns the Score Sub Category Config
                        var result = from cl in _context.ScoreCategoryConfig
                                     where cl.Id == cat.Id
                                     select new
                                     {
                                         cl.Id,
                                         cl.CategoryId,
                                         cl.ScoreCategory.CategoryName,
                                         cl.SchoolId,
                                         cl.CampusId,
                                         cl.ClassId,
                                         cl.Classes.ClassName,
                                         cl.Percentage,
                                         cl.TermId,
                                         cl.Terms.TermName,
                                         cl.SessionId,
                                         cl.Sessions.SessionName,
                                         cl.DateCreated
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Category Created Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Score Category total percentage Cannot be greater than 100%", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Score Category Already Exist", };
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

        public async Task<GenericResponseModel> getAllScoreCategoryConfigAsync(long schoolId, long campusId)
        {
            try
            {
                //returns all the Score Category Config
                var result = from cl in _context.ScoreCategoryConfig
                             where cl.SchoolId == schoolId && cl.CampusId == campusId
                             select new
                             {
                                 cl.Id,
                                 cl.CategoryId,
                                 cl.ScoreCategory.CategoryName,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.Percentage,
                                 cl.TermId,
                                 cl.Terms.TermName,
                                 cl.SessionId,
                                 cl.Sessions.SessionName,
                                 cl.DateCreated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> getScoreCategoryConfigByIdAsync(long scoreCategoryConfigId)
        {
            try
            {
                //returns all the Score Category Config
                var result = from cl in _context.ScoreCategoryConfig
                             where cl.Id == scoreCategoryConfigId
                             select new
                             {
                                 cl.Id,
                                 cl.CategoryId,
                                 cl.ScoreCategory.CategoryName,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.Percentage,
                                 cl.TermId,
                                 cl.Terms.TermName,
                                 cl.SessionId,
                                 cl.Sessions.SessionName,
                                 cl.DateCreated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> updateScoreCategoryConfigAsync(long scoreCategoryConfigId, ScoreCategoryConfigRequestModel obj)
        {
            try
            {
                ScoreCategoryConfig getConfig = _context.ScoreCategoryConfig.Where(c => c.Id == scoreCategoryConfigId).FirstOrDefault();

                if (getConfig != null)
                {
                    //get the totalpercentage
                    decimal totalPercentage = _context.ScoreCategoryConfig.Where(c => c.SchoolId == obj.SchoolId
                    && c.CampusId == obj.CampusId && c.ClassId == obj.ClassId && c.SessionId == obj.SessionId && c.TermId == obj.TermId).Sum(p => p.Percentage);

                    //adds the total percentage with the new percentage
                    decimal configPercentage = totalPercentage + obj.Percentage;

                    if (configPercentage <= 100)
                    {
                        getConfig.CategoryId = obj.CategoryId;
                        getConfig.SchoolId = obj.SchoolId;
                        getConfig.CampusId = obj.CampusId;
                        getConfig.ClassId = obj.ClassId;
                        getConfig.TermId = obj.TermId;
                        getConfig.SessionId = obj.SessionId;
                        getConfig.Percentage = obj.Percentage;

                        await _context.SaveChangesAsync();

                        //returns the Score Sub Category Config
                        var result = from cl in _context.ScoreCategoryConfig
                                     where cl.Id == getConfig.Id
                                     select new
                                     {
                                         cl.Id,
                                         cl.CategoryId,
                                         cl.ScoreCategory.CategoryName,
                                         cl.SchoolId,
                                         cl.CampusId,
                                         cl.ClassId,
                                         cl.Classes.ClassName,
                                         cl.Percentage,
                                         cl.TermId,
                                         cl.Terms.TermName,
                                         cl.SessionId,
                                         cl.Sessions.SessionName,
                                         cl.DateCreated
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Category Updated Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Score Category total percentage Cannot be greater than 100%", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Score Category With Specified ID does not Exist",};
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

        public async Task<GenericResponseModel> deleteScoreCategoryConfigAsync(long scoreCategoryConfigId)
        {
            try
            {
                //check if the score Category exists
                ScoreCategoryConfig config = _context.ScoreCategoryConfig.Where(c => c.Id == scoreCategoryConfigId).FirstOrDefault();

                if (config != null)
                {
                    _context.ScoreCategoryConfig.Remove(config);
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Category Deleted Successfully", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Score Category with the specified Id", };
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

        //----------------------------------------SCORESUBCATEGORY CONFIGURATION--------------------------------------------------------------------------

        public async Task<GenericResponseModel> createScoreSubCategoryConfigAsync(ScoreSubCategoryConfigRequestModel obj)
        {
            try
            {
                //check if the Score Sub category exists
                var catConfig = _context.ScoreSubCategoryConfig.Where(c => c.SubCategoryName == obj.SubCategoryName && c.CategoryId == obj.CategoryId && c.SchoolId == obj.SchoolId
                && c.CampusId == obj.CampusId && c.ClassId == obj.ClassId && c.SessionId == obj.SessionId && c.TermId == obj.TermId).FirstOrDefault();

                if (catConfig == null)
                {
                    //create score Sub category if it doesnt exist
                    var cat = new ScoreSubCategoryConfig
                    {
                        CategoryId = obj.CategoryId,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        ClassId = obj.ClassId,
                        TermId = obj.TermId,
                        SessionId = obj.SessionId,
                        SubCategoryName = obj.SubCategoryName,
                        ScoreObtainable = obj.ScoreObtainable,
                        DateCreated = DateTime.Now,
                    };

                    await _context.ScoreSubCategoryConfig.AddAsync(cat);
                    await _context.SaveChangesAsync();


                    //returns the Score Sub Category Config
                    var result = from cl in _context.ScoreSubCategoryConfig
                                 where cl.Id == cat.Id
                                 select new
                                 {
                                     cl.Id,
                                     cl.CategoryId,
                                     cl.SchoolId,
                                     cl.CampusId,
                                     cl.ClassId,
                                     cl.Classes.ClassName,
                                     cl.ScoreCategory.CategoryName,
                                     cl.SubCategoryName,
                                     cl.ScoreObtainable,
                                     cl.TermId,
                                     cl.Terms.TermName,
                                     cl.SessionId,
                                     cl.Sessions.SessionName,
                                     cl.DateCreated
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Sub Category Created Successfully", Data = result.FirstOrDefault()};
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Score Sub Category Already Exist", };
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

        public async Task<GenericResponseModel> getAllScoreSubCategoryConfigAsync(long schoolId, long campusId)
        {
            try
            {
                //returns all the Score Category Config
                var result = from cl in _context.ScoreSubCategoryConfig
                             where cl.SchoolId == schoolId && cl.CampusId == campusId
                             select new
                             {
                                 cl.Id,
                                 cl.CategoryId,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.ScoreCategory.CategoryName,
                                 cl.SubCategoryName,
                                 cl.ScoreObtainable,
                                 cl.TermId,
                                 cl.Terms.TermName,
                                 cl.SessionId,
                                 cl.Sessions.SessionName,
                                 cl.DateCreated
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> getScoreSubCategoryConfigByIdAsync(long scoreSubCategoryConfigId)
        {
            try
            {
                //returns all the Score Category Config
                var result = from cl in _context.ScoreSubCategoryConfig
                             where cl.Id == scoreSubCategoryConfigId
                             select new
                             {
                                 cl.Id,
                                 cl.CategoryId,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.ScoreCategory.CategoryName,
                                 cl.SubCategoryName,
                                 cl.ScoreObtainable,
                                 cl.TermId,
                                 cl.Terms.TermName,
                                 cl.SessionId,
                                 cl.Sessions.SessionName,
                                 cl.DateCreated
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> updateScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId, ScoreSubCategoryConfigRequestModel obj)
        {
            try
            {
                //check if the score Category exists
                var con = _context.ScoreSubCategoryConfig.Where(c => c.Id == scoreSubCategoryConfigId).FirstOrDefault();

                if (con != null)
                {
                    con.CategoryId = obj.CategoryId;
                    con.SchoolId = obj.SchoolId;
                    con.CampusId = obj.CampusId;
                    con.ClassId = obj.ClassId;
                    con.TermId = obj.TermId;
                    con.SessionId = obj.SessionId;
                    con.SubCategoryName = obj.SubCategoryName;
                    con.ScoreObtainable = obj.ScoreObtainable;

                    await _context.SaveChangesAsync();

                    //returns the Score Sub Category Config
                    var result = from cl in _context.ScoreSubCategoryConfig
                                 where cl.Id == con.Id
                                 select new
                                 {
                                     cl.Id,
                                     cl.CategoryId,
                                     cl.SchoolId,
                                     cl.CampusId,
                                     cl.ClassId,
                                     cl.Classes.ClassName,
                                     cl.ScoreCategory.CategoryName,
                                     cl.SubCategoryName,
                                     cl.ScoreObtainable,
                                     cl.TermId,
                                     cl.Terms.TermName,
                                     cl.SessionId,
                                     cl.Sessions.SessionName,
                                     cl.DateCreated
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score Sub Category Updated Successfully", Data = result .FirstOrDefault()};
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Score Sub Category with the specified Id", };
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

        public async Task<GenericResponseModel> deleteScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId)
        {
            try
            {
                //check if the score Grade exists
                var config = _context.ScoreSubCategoryConfig.Where(c => c.Id == scoreSubCategoryConfigId).FirstOrDefault();

                if (config != null)
                {
                    _context.ScoreSubCategoryConfig.Remove(config);
                    await _context.SaveChangesAsync();
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Score SubCategory Deleted Successfully", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Score SubCategory with the specified Id", };
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

       
    }
}
