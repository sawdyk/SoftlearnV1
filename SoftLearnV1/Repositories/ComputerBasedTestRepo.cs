using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.SchoolReusables;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class ComputerBasedTestRepo : IComputerBasedTestRepo
    {
        private readonly AppDbContext _context;
        private readonly IHostingEnvironment env;
        private readonly ServerPath _serverPath;

        public ComputerBasedTestRepo(AppDbContext context, IHostingEnvironment env, ServerPath serverPath)
        {
            this._context = context;
            this.env = env;
            _serverPath = serverPath;
        }

        //------------------------------------------------COMPUTER BASES TEST---------------------------------------------------------------------

        public async Task<GenericResponseModel> createComputerBasedTestAsync(CbtRequestModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //check if a CBT to be created already exists
                    var checkResult = _context.ComputerBasedTest.Where(x => x.Description == obj.Description && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId
                    && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.TypeId == obj.TypeId && x.CategoryId == obj.CategoryId && x.SubjectId == obj.SubjectId
                    && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                    //if the CBT doesnt exist, Create the CBT
                    if (checkResult == null)
                    {
                        var cbt = new ComputerBasedTest
                        {
                            Description = obj.Description,
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            ClassId = obj.ClassId,
                            ClassGradeId = obj.ClassGradeId,
                            TypeId = obj.TypeId,
                            CategoryId = obj.CategoryId,
                            SubjectId = obj.SubjectId,
                            TeacherId = obj.TeacherId,
                            TermId = obj.TermId,
                            SessionId = obj.SessionId,
                            Duration = obj.Duration,
                            PassMark = obj.PassMark,
                            TermsAndConditions = obj.TermsAndConditions,
                            StatusId = (long)EnumUtility.ActiveInActive.Active,
                            DateCreated = DateTime.Now,
                            LastDateUpdated = DateTime.Now,

                        };

                        await _context.ComputerBasedTest.AddAsync(cbt);
                        await _context.SaveChangesAsync();

                        //get the CBT Created
                        var getCbt = from cb in _context.ComputerBasedTest
                                     where cb.Id == cbt.Id
                                     select new
                                     {
                                         cb.Id,
                                         cb.Description,
                                         cb.SchoolId,
                                         cb.CampusId,
                                         cb.Classes.ClassName,
                                         cb.ClassGrades.GradeName,
                                         cb.CbtTypes.TypeName,
                                         cb.CbtCategory.CategoryName,
                                         cb.SchoolSubjects.SubjectName,
                                         TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                         cb.Terms.TermName,
                                         cb.Sessions.SessionName,
                                         cb.Duration,
                                         cb.PassMark,
                                         cb.TermsAndConditions,
                                         cb.ActiveInActiveStatus.StatusName,
                                         cb.DateCreated,
                                         cb.LastDateUpdated,
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Created Successfully", Data = getCbt.FirstOrDefault() };

                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Computer Based Test Already Exists" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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
       
        public async Task<GenericResponseModel> getComputerBasedTestAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long typeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.SubjectId == subjectId && cb.CategoryId == categoryId && cb.TypeId == typeId && cb.TermId == termId && cb.SessionId == sessionId
                                 && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestByCategoryIdAndSubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.SubjectId == subjectId && cb.CategoryId == categoryId && cb.TermId == termId && cb.SessionId == sessionId
                                 && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestByCategoryIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.CategoryId == categoryId && cb.TermId == termId && cb.SessionId == sessionId && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new 
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestByClassIdAndGradeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.TermId == termId && cb.SessionId == sessionId && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestByIdAsync(long cbtId)
        {
            try
            {
                //Validations
                var checkCbt = _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefault();

                //check if the CBT Exists
                if (checkCbt != null)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.Id == cbtId 
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.SubjectId == subjectId && cb.TermId == termId && cb.SessionId == sessionId && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestByTypeIdAndSubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long typeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.SubjectId == subjectId && cb.TypeId == typeId && cb.TypeId == typeId && cb.TermId == termId && cb.SessionId == sessionId
                                 && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestByTypeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long typeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.TypeId == typeId && cb.TermId == termId && cb.SessionId == sessionId && cb.StatusId == (long)EnumUtility.ActiveInActive.Active
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName,
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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


        public async Task<GenericResponseModel> getComputerBasedTestByStatusIdAsync(long schoolId, long campusId, long classId, long classGradeId, long statusId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    var result = from cb in _context.ComputerBasedTest
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.TermId == termId && cb.SessionId == sessionId && cb.StatusId == statusId
                                 select new
                                 {
                                     cb.Id,
                                     cb.Description,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.SchoolSubjects.SubjectName, 
                                     TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.Duration,
                                     cb.PassMark,
                                     cb.TermsAndConditions,
                                     cb.ActiveInActiveStatus.StatusName,
                                     cb.DateCreated,
                                     cb.LastDateUpdated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> setComputerBasedTestStatusAsync(long cbtId, long statusId)
        {
            try
            {
                //Validations
                var checkCbt = _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefault();

                //check if the CBT Exists
                if (checkCbt != null)
                {
                    if (statusId == (long)EnumUtility.ActiveInActive.Active)
                    {
                        checkCbt.StatusId = (long)EnumUtility.ActiveInActive.Active;
                        checkCbt.LastDateUpdated = DateTime.Now;

                        await _context.SaveChangesAsync();
                    }
                    else if (statusId == (long)EnumUtility.ActiveInActive.InActive)
                    {
                        checkCbt.StatusId = (long)EnumUtility.ActiveInActive.InActive;
                        checkCbt.LastDateUpdated = DateTime.Now;

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Status ID Specified, or No Status With the Specified ID" };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Status Updated Successfully" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
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

        public async Task<GenericResponseModel> updateComputerBasedTestAsync(long cbtId, CbtRequestModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGarade = check.checkClassGradeById(obj.ClassGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //Validations
                    var checkCbt = _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefault();

                    //check if the CBT Exists
                    if (checkCbt != null)
                    {
                        checkCbt.Description = obj.Description;
                        checkCbt.SchoolId = obj.SchoolId;
                        checkCbt.CampusId = obj.CampusId;
                        checkCbt.ClassId = obj.ClassId;
                        checkCbt.ClassGradeId = obj.ClassGradeId;
                        checkCbt.TypeId = obj.TypeId;
                        checkCbt.CategoryId = obj.CategoryId;
                        checkCbt.SubjectId = obj.SubjectId;
                        checkCbt.TeacherId = obj.TeacherId;
                        checkCbt.TermId = obj.TermId;
                        checkCbt.SessionId = obj.SessionId;
                        checkCbt.Duration = obj.Duration;
                        checkCbt.PassMark = obj.PassMark;
                        checkCbt.TermsAndConditions = obj.TermsAndConditions;
                        checkCbt.LastDateUpdated = DateTime.Now;

                        await _context.SaveChangesAsync();

                        //get the CBT Updated
                        var getCbt = from cb in _context.ComputerBasedTest
                                     where cb.Id == checkCbt.Id
                                     select new
                                     {
                                         cb.Id,
                                         cb.Description,
                                         cb.SchoolId,
                                         cb.CampusId,
                                         cb.Classes.ClassName,
                                         cb.ClassGrades.GradeName,
                                         cb.CbtTypes.TypeName,
                                         cb.CbtCategory.CategoryName,
                                         cb.SchoolSubjects.SubjectName,
                                         TeachersName = cb.SchoolUsers.FirstName + " " + cb.SchoolUsers.FirstName,
                                         cb.Terms.TermName,
                                         cb.Sessions.SessionName,
                                         cb.Duration,
                                         cb.PassMark,
                                         cb.TermsAndConditions,
                                         cb.ActiveInActiveStatus.StatusName,
                                         cb.DateCreated,
                                         cb.LastDateUpdated,
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Updated Successfully", Data = getCbt.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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
        public async Task<GenericResponseModel> deleteComputerBasedTestAsync(long cbtId)
        {
            try
            {
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefault();

                //check if the CBT Exists
                if (cbt != null)
                {
                    //get the CBT questions
                    IList<CbtQuestions> cbtQuestions = _context.CbtQuestions.Where(x => x.CbtId == cbt.Id).ToList();
                    //get the CBT Results
                    IList<CbtResults> cbtResults = _context.CbtResults.Where(x => x.CbtId == cbt.Id).ToList();

                    //deletes all questions pertaining to the CBT
                    if (cbtQuestions.Count > 0)
                    {
                        _context.CbtQuestions.RemoveRange(cbtQuestions);
                        await _context.SaveChangesAsync();
                    }

                    //deletes all result pertaining to the CBT
                    if (cbtResults.Count > 0)
                    {
                        _context.CbtResults.RemoveRange(cbtResults);
                        await _context.SaveChangesAsync();
                    }

                    //deletes the CBT
                    _context.ComputerBasedTest.Remove(cbt);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Deleted Successfully"};

                }
                
              return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
                
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

        //---------------------------------------------------COMPUTER BASED TEST QUESTIONS-----------------------------------------------------------

        public async Task<GenericResponseModel> createQuestionsAsync(CbtQuestionRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == obj.CbtId).FirstOrDefault();

                //check if the CBT Exists
                if (cbt != null)
                {
                    if (obj.Questions.Count > 0)
                    {
                        foreach (QuestionList quest in obj.Questions)
                        {
                            //check if the questionTypeId is valid
                            var questionType = new CheckerValidation(_context).checkQuestionTypeById(quest.QuestionTypeId);
                            if (questionType == false)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 405, StatusMessage = "Question Type Doesn't Exist" });
                                continue;
                            }
                            //check if a question to be created already exists
                            var checkResult = _context.CbtQuestions.Where(x => x.CbtId == obj.CbtId && x.QuestionTypeId == quest.QuestionTypeId && x.Answer == quest.Answer && x.Question == quest.Question).FirstOrDefault();

                            //if the course CBT question doesnt exist, Create the question
                            if (checkResult == null)
                            {
                                var question = new CbtQuestions();
                                question.CbtId = obj.CbtId;
                                question.QuestionTypeId = quest.QuestionTypeId;
                                question.Question = quest.Question;
                                question.DateCreated = DateTime.Now;
                                question.LastDateUpdated = DateTime.Now;

                                if (quest.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                                {
                                    question.Answer = quest.Answer;
                                }
                                else if (quest.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                                {
                                    question.Answer = quest.Answer;
                                    if (quest.Option1 == string.Empty || quest.Option2 == string.Empty || quest.Option3 == string.Empty || quest.Option4 == string.Empty)
                                    {
                                        data.Add(new GenericResponseModel { StatusCode = 400, StatusMessage = "Multiple Choice Questions Must have all options Specified" });
                                        continue;
                                    }
                                    else
                                    {
                                        question.Option1 = quest.Option1;
                                        question.Option2 = quest.Option2;
                                        question.Option3 = quest.Option3;
                                        question.Option4 = quest.Option4;
                                    }
                                }
                                else if (quest.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                                {
                                    if (quest.Answer.ToLower().Trim() == "true" || quest.Answer.ToLower().Trim() == "false")
                                    {
                                        question.Answer = quest.Answer;
                                    }
                                    else
                                    {
                                        data.Add(new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Answer, Answer must be true or false for the Selected Question Type" });
                                    }
                                }
                                await _context.CbtQuestions.AddAsync(question);
                                await _context.SaveChangesAsync();

                                //get the Question Created
                                var result = from cr in _context.CbtQuestions
                                             where cr.Id == question.Id
                                             select new
                                             {
                                                 cr.Id,
                                                 cr.CbtId,
                                                 cr.ComputerBasedTest.Description,
                                                 cr.Option1,
                                                 cr.Option2,
                                                 cr.Option3,
                                                 cr.Option4,
                                                 cr.Question,
                                                 cr.QuestionTypeId,
                                                 cr.QuestionTypes.QuestionTypeName,
                                                 cr.Answer,
                                                 cr.DateCreated,
                                                 cr.LastDateUpdated
                                             };

                                data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Question Created Successfully", Data = result.FirstOrDefault() });

                            }
                            else
                            {
                                data.Add(new GenericResponseModel { StatusCode = 409, StatusMessage = "This Question Already Exists for this Computer Based Test!" });
                            }
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Question Was Added to the List!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!", Data = data.ToList()};
            }
        }

        public async Task<GenericResponseModel> createBulkQuestionsFromExcelAsync(BulkCbtQuestionsRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                //check if the quizId is valid
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == obj.CbtId).FirstOrDefault();

                //check if the CBT Exists
                if (cbt != null)
                {
                    string subFolderName = "Others";
                    //the file path
                    //get the defined filepath (e.g. @"C:\inetpub\wwwroot\SoftlearnMedia")
                    string serverPath = _serverPath.ServerFolderPath((int)EnumUtility.AppName.SchoolApp, subFolderName);

                    //the file path
                    //var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", obj.File.FileName);
                    //copy the file to the stream and read from the file

                    //the file path to save the file
                    var FilePath = Path.Combine(serverPath, obj.File.FileName);

                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await obj.File.CopyToAsync(stream);
                    }

                    FileInfo existingFile = new FileInfo(FilePath);
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        //get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        int colCount = worksheet.Dimension.Columns;  //get Column Count
                        int rowCount = worksheet.Dimension.Rows;     //get row count

                        for (int row = 2; row <= rowCount; row++) // starts from the second row (Jumping the table headings)
                        {
                            //check if the question, answer or question type is valid
                            if (worksheet.Cells[row, 1].Value != null && worksheet.Cells[row, 6].Value != null && worksheet.Cells[row, 7].Value != null)
                            {
                                //check if the questionTypeId is valid
                                var checkQuestionType = _context.QuestionTypes.Where(x => x.QuestionTypeName.ToLower().Trim() == worksheet.Cells[row, 7].Value.ToString().ToLower().Trim()).FirstOrDefault();
                                if (checkQuestionType != null)
                                {
                                    string question = worksheet.Cells[row, 1].Value.ToString().Trim();
                                    string answer = worksheet.Cells[row, 6].Value.ToString().Trim();

                                    //check if a question to be created already exists
                                    var checkResult = _context.CbtQuestions.Where(x => x.CbtId == obj.CbtId && x.QuestionTypeId == checkQuestionType.Id && x.Answer.ToLower() == answer.ToLower() && x.Question.ToLower() == question.ToLower()).FirstOrDefault();

                                    //if the course quiz question doesnt exist, Create the question
                                    if (checkResult == null)
                                    {
                                        var newQuestion = new CbtQuestions();
                                        newQuestion.CbtId = obj.CbtId;
                                        newQuestion.QuestionTypeId = checkQuestionType.Id;
                                        newQuestion.Question = question;
                                        newQuestion.DateCreated = DateTime.Now;
                                        newQuestion.LastDateUpdated = DateTime.Now;

                                        if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                                        {
                                            newQuestion.Answer = answer;
                                        }
                                        else if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                                        {
                                            newQuestion.Answer = answer;
                                            if (worksheet.Cells[row, 2].Value == null || worksheet.Cells[row, 3].Value == null || worksheet.Cells[row, 4].Value == null || worksheet.Cells[row, 5].Value == null)
                                            {
                                                data.Add(new GenericResponseModel { StatusCode = 400, StatusMessage = "Multiple Choice Questions Must have all options Specified" });
                                                continue;
                                            }
                                            else
                                            {
                                                newQuestion.Option1 = worksheet.Cells[row, 2].Value.ToString().Trim();
                                                newQuestion.Option2 = worksheet.Cells[row, 3].Value.ToString().Trim();
                                                newQuestion.Option3 = worksheet.Cells[row, 4].Value.ToString().Trim();
                                                newQuestion.Option4 = worksheet.Cells[row, 5].Value.ToString().Trim();
                                            }
                                        }
                                        else if (checkQuestionType.Id == (int)EnumUtility.QuestionTypes.True_or_False)
                                        {
                                            if (answer.ToLower().Trim() == "true" || answer.ToLower().Trim() == "false")
                                            {
                                                newQuestion.Answer = answer;
                                            }
                                            else
                                            {
                                                data.Add(new GenericResponseModel { StatusCode = 401, StatusMessage = "Invalid Answer, Answer must be true or false" });
                                                continue;
                                            }
                                        }
                                        await _context.CbtQuestions.AddAsync(newQuestion);
                                        await _context.SaveChangesAsync();

                                        //get the Question Created
                                        var getCbtQuestions = from cr in _context.CbtQuestions
                                                                    where cr.Id == newQuestion.Id
                                                                    select new
                                                                    {
                                                                        cr.Id,
                                                                        cr.CbtId,
                                                                        cr.ComputerBasedTest.Description,
                                                                        cr.Option1,
                                                                        cr.Option2,
                                                                        cr.Option3,
                                                                        cr.Option4,
                                                                        cr.Question,
                                                                        cr.QuestionTypeId,
                                                                        cr.QuestionTypes.QuestionTypeName,
                                                                        cr.Answer,
                                                                        cr.DateCreated,
                                                                        cr.LastDateUpdated
                                                                    };

                                        data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Question Created Successfully", Data = getCbtQuestions.FirstOrDefault() });

                                    }
                                    else
                                    {
                                        data.Add(new GenericResponseModel { StatusCode = 201, StatusMessage = "Computer Based Test Question Already Exists" });
                                    }

                                }
                                else
                                {
                                    data.Add(new GenericResponseModel { StatusCode = 202, StatusMessage = "Question Type Doesn't Exist" });
                                }
                            }
                            else
                            {
                                data.Add(new GenericResponseModel { StatusCode = 203, StatusMessage = "Question, Answer or Question type invalid" });
                            }
                        }
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Computer Based Test With the Specified ID"};

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
            }
        }

        public async Task<GenericResponseModel> getQuestionsByIdAsync(long questionId)
        {
            try
            {
                //Validations
                var question = _context.CbtQuestions.Where(x => x.Id == questionId).FirstOrDefault();

                if (question != null)
                {
                    var result = from cr in _context.CbtQuestions
                                 where cr.Id == question.Id
                                 select new
                                 {
                                     cr.Id,
                                     cr.CbtId,
                                     cr.ComputerBasedTest.Description,
                                     cr.Option1,
                                     cr.Option2,
                                     cr.Option3,
                                     cr.Option4,
                                     cr.Question,
                                     cr.QuestionTypeId,
                                     cr.QuestionTypes.QuestionTypeName,
                                     cr.Answer,
                                     cr.DateCreated,
                                     cr.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Question With the Specified ID!" };
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

        public async Task<GenericResponseModel> getQuestionsByCbtIdAsync(long cbtId)
        {
            try
            {
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefault();

                if (cbt != null)
                {
                    var result = from cr in _context.CbtQuestions
                                 where cr.CbtId == cbt.Id
                                 select new
                                 {
                                     cr.Id,
                                     cr.CbtId,
                                     cr.ComputerBasedTest.Description,
                                     cr.Option1,
                                     cr.Option2,
                                     cr.Option3,
                                     cr.Option4,
                                     cr.Question,
                                     cr.QuestionTypeId,
                                     cr.QuestionTypes.QuestionTypeName,
                                     cr.Answer,
                                     cr.DateCreated,
                                     cr.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
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

        public async Task<GenericResponseModel> getQuestionsByQuestionTypeIdAsync(long cbtId, long questionTypeId)
        {
            try
            {
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefault();

                if (cbt != null)
                {
                    if (questionTypeId == (long)EnumUtility.QuestionTypes.Multiple_Choice)
                    {
                        var result = from cr in _context.CbtQuestions
                                     where cr.CbtId == cbt.Id && cr.QuestionTypeId == (long)EnumUtility.QuestionTypes.Multiple_Choice
                                     select new
                                     {
                                         cr.Id,
                                         cr.CbtId,
                                         cr.ComputerBasedTest.Description,
                                         cr.Option1,
                                         cr.Option2,
                                         cr.Option3,
                                         cr.Option4,
                                         cr.Question,
                                         cr.QuestionTypeId,
                                         cr.QuestionTypes.QuestionTypeName,
                                         cr.Answer,
                                         cr.DateCreated,
                                         cr.LastDateUpdated
                                     };
                        if (result.Count() > 0)
                        {
                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                    }
                    if (questionTypeId == (long)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                    {
                        var result = from cr in _context.CbtQuestions
                                     where cr.CbtId == cbt.Id && cr.QuestionTypeId == (long)EnumUtility.QuestionTypes.Fill_in_the_Gap
                                     select new
                                     {
                                         cr.Id,
                                         cr.CbtId,
                                         cr.ComputerBasedTest.Description,
                                         cr.Option1,
                                         cr.Option2,
                                         cr.Option3,
                                         cr.Option4,
                                         cr.Question,
                                         cr.QuestionTypeId,
                                         cr.QuestionTypes.QuestionTypeName,
                                         cr.Answer,
                                         cr.DateCreated,
                                         cr.LastDateUpdated
                                     };
                        if (result.Count() > 0)
                        {
                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                    }
                    if (questionTypeId == (long)EnumUtility.QuestionTypes.True_or_False)
                    {
                        var result = from cr in _context.CbtQuestions
                                     where cr.CbtId == cbt.Id && cr.QuestionTypeId == (long)EnumUtility.QuestionTypes.True_or_False
                                     select new
                                     {
                                         cr.Id,
                                         cr.CbtId,
                                         cr.ComputerBasedTest.Description,
                                         cr.Option1,
                                         cr.Option2,
                                         cr.Option3,
                                         cr.Option4,
                                         cr.Question,
                                         cr.QuestionTypeId,
                                         cr.QuestionTypes.QuestionTypeName,
                                         cr.Answer,
                                         cr.DateCreated,
                                         cr.LastDateUpdated
                                     };
                        if (result.Count() > 0)
                        {
                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Question ID Specified, or No QuestionType With the Specified ID" };
                    }
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
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
       
        public async Task<GenericResponseModel> updateQuestionAsync(long questionId, CbtQuestionCreationRequestModel obj)
        {
            try
            {
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == obj.CbtId).FirstOrDefault();
                var question = _context.CbtQuestions.Where(x => x.Id == questionId).FirstOrDefault();

                //check if the CBT Exists
                if (cbt != null)
                {
                    //check if the question Exists
                    if (question != null)
                    {
                        //check if the questionTypeId is valid
                        var questionType = new CheckerValidation(_context).checkQuestionTypeById(obj.QuestionTypeId);
                        if (questionType == false)
                        {
                            return new GenericResponseModel { StatusCode = 404, StatusMessage = "Question Type Doesn't Exist" };
                        }
                        //check if a question to be created already exists
                        var checkResult = _context.CbtQuestions.Where(x => x.CbtId == obj.CbtId && x.QuestionTypeId == obj.QuestionTypeId && x.Answer == obj.Answer && x.Question == obj.Question).FirstOrDefault();

                        //if the course CBT question doesnt exist, Create the question
                        if (checkResult == null)
                        {
                            question.CbtId = obj.CbtId;
                            question.QuestionTypeId = obj.QuestionTypeId;
                            question.Question = obj.Question;
                            question.LastDateUpdated = DateTime.Now;

                            if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Fill_in_the_Gap)
                            {
                                question.Answer = obj.Answer;
                                question.Option1 = null;
                                question.Option2 = null;
                                question.Option3 = null;
                                question.Option4 = null;
                            }
                            else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.Multiple_Choice)
                            {
                                question.Answer = obj.Answer;
                                if (obj.Option1 == string.Empty || obj.Option2 == string.Empty || obj.Option3 == string.Empty || obj.Option4 == string.Empty)
                                {
                                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Multiple Choice Questions Must have all options Specified" };
                                }
                                else
                                {
                                    question.Option1 = obj.Option1;
                                    question.Option2 = obj.Option2;
                                    question.Option3 = obj.Option3;
                                    question.Option4 = obj.Option4;
                                }
                            }
                            else if (obj.QuestionTypeId == (int)EnumUtility.QuestionTypes.True_or_False)
                            {
                                if (obj.Answer.ToLower().Trim() == "true" || obj.Answer.ToLower().Trim() == "false")
                                {
                                    question.Answer = obj.Answer;
                                    question.Option1 = null;
                                    question.Option2 = null;
                                    question.Option3 = null;
                                    question.Option4 = null;
                                }
                                else
                                {
                                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Answer, Answer must be true or false for the Selected Question Type" };
                                }
                            }

                            await _context.SaveChangesAsync();

                            //get the Question Updated
                            var result = from cr in _context.CbtQuestions
                                         where cr.Id == question.Id
                                         select new
                                         {
                                             cr.Id,
                                             cr.CbtId,
                                             cr.ComputerBasedTest.Description,
                                             cr.Option1,
                                             cr.Option2,
                                             cr.Option3,
                                             cr.Option4,
                                             cr.Question,
                                             cr.QuestionTypeId,
                                             cr.QuestionTypes.QuestionTypeName,
                                             cr.Answer,
                                             cr.DateCreated,
                                             cr.LastDateUpdated
                                         };

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Question Updated Successfully", Data = result.FirstOrDefault() };

                        }

                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Question Already Exists for this Computer Based Test!" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Question With the Specified ID!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };
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

        public async Task<GenericResponseModel> deleteQuestionAsync(long questionId)
        {
            try
            {
                var obj = _context.CbtQuestions.Where(x => x.Id == questionId).FirstOrDefault();
                if (obj != null)
                {
                    _context.CbtQuestions.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Question Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Question With the Specified ID!" };
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

        //---------------------------------------------------COMPUTER BASED TEST RESULTS-------------------------------------------------------------

        public async Task<GenericResponseModel> updateComputerBasedTestResultAsync(CbtResultCreationRequestModel obj)
        {
            try
            {
                //Validations
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == obj.CbtId).FirstOrDefault();
                if (cbt != null)
                {
                    //check if student result exists
                    var getCbtResult = await _context.CbtResults.Where(x => x.CbtId == obj.CbtId && x.StudentId == obj.StudentId).FirstOrDefaultAsync();

                    if (getCbtResult != null)
                    {
                        //total Number of questions in the Computer Based Test
                        IList<CbtQuestions> questions = _context.CbtQuestions.Where(x => x.CbtId == obj.CbtId).ToList();

                        int rightAnswers = 0;
                        int wrongAnswers = 0;
                        //int invalidQuestions = 0;
                        int totalQuestions = questions.Count;
                        long status = 0;
                        decimal percentageScore = 0;

                        //check if the Computer Based Test Contains Questions
                        if (questions.Count > 0)
                        {
                            foreach (QuestionAndAnswerList qs in obj.QuestionAndAnswer)
                            {
                                //check if the question for the CBT Exists
                                CbtQuestions question = await _context.CbtQuestions.Where(x => x.Id == qs.QuetionId && x.CbtId == obj.CbtId).FirstOrDefaultAsync();

                                if (question != null)
                                {
                                    //check if the answer submitted is equal to the answer specified for the question
                                    if (qs.Answer.ToLower().Trim() == question.Answer.ToLower().Trim())
                                    {
                                        rightAnswers++;
                                    }
                                    else
                                    {
                                        wrongAnswers++;
                                    }
                                }
                                //else
                                //{
                                //    invalidQuestions++;
                                //}
                            }

                            //calculate the student percentage score
                            percentageScore = (rightAnswers * 100) / totalQuestions;

                            //check the Student percentage score and Passmark alloted to the CBT and Assign a Status
                            if (percentageScore >= cbt.PassMark)
                            {
                                status = (long)EnumUtility.ScoreStatus.Passed;
                            }
                            else
                            {
                                status = (long)EnumUtility.ScoreStatus.Failed;
                            }

                            //Update the Computer Based Test Result for the Student
                            getCbtResult.CbtId = obj.CbtId;
                            getCbtResult.StudentId = obj.StudentId;
                            getCbtResult.SchoolId = cbt.SchoolId;
                            getCbtResult.CampusId = cbt.CampusId;
                            getCbtResult.ClassId = cbt.ClassId;
                            getCbtResult.ClassGradeId = cbt.ClassGradeId;
                            getCbtResult.CbtId = cbt.Id;
                            getCbtResult.TypeId = cbt.TypeId;
                            getCbtResult.CategoryId = cbt.CategoryId;
                            getCbtResult.TermId = cbt.TermId;
                            getCbtResult.SessionId = cbt.SessionId;
                            getCbtResult.NoOfQuestion = totalQuestions;
                            getCbtResult.RightAnswers = rightAnswers;
                            getCbtResult.WrongAnswers = wrongAnswers;
                            getCbtResult.TotalScore = rightAnswers;
                            getCbtResult.PercentageScore = percentageScore;
                            getCbtResult.StatuId = status;
                            getCbtResult.DateTaken = DateTime.Now;

                            await _context.SaveChangesAsync();

                            //get the CBT Result Created
                            var getResult = from cb in _context.CbtResults
                                            where cb.Id == getCbtResult.Id
                                            select new
                                            {
                                                cb.Id,
                                                cb.SchoolId,
                                                cb.CampusId,
                                                cb.Classes.ClassName,
                                                cb.ClassGrades.GradeName,
                                                cb.StudentId,
                                                StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                                cb.CbtTypes.TypeName,
                                                cb.CbtCategory.CategoryName,
                                                cb.Terms.TermName,
                                                cb.Sessions.SessionName,
                                                cb.CbtId,
                                                cb.ComputerBasedTest.Description,
                                                cb.NoOfQuestion,
                                                cb.PercentageScore,
                                                cb.TotalScore,
                                                cb.RightAnswers,
                                                cb.WrongAnswers,
                                                cb.ScoreStatus.ScoreStatusName,
                                                cb.DateTaken,
                                            };

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Result Updated Successfully", Data = getResult.FirstOrDefault() };
                        }

                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "No questions for the selected Computer Based Test" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Result does not Exist for the Student and the Computer Based Test" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByIdAsync(long cbtResultId)
        {
            try
            {
                //Validations
                var cbtResult = _context.CbtResults.Where(x => x.Id == cbtResultId).FirstOrDefault();

                //check if the School and CampusId is Valid
                if (cbtResult != null)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                    where cb.Id == cbtResult.Id
                                    select new
                                    {
                                        cb.Id,
                                        cb.SchoolId,
                                        cb.CampusId,
                                        cb.Classes.ClassName,
                                        cb.ClassGrades.GradeName,
                                        cb.StudentId,
                                        StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                        cb.CbtTypes.TypeName,
                                        cb.CbtCategory.CategoryName,
                                        cb.Terms.TermName,
                                        cb.Sessions.SessionName,
                                        cb.CbtId,
                                        cb.ComputerBasedTest.Description,
                                        cb.NoOfQuestion,
                                        cb.PercentageScore,
                                        cb.TotalScore,
                                        cb.RightAnswers,
                                        cb.WrongAnswers,
                                        cb.ScoreStatus.ScoreStatusName,
                                        cb.DateTaken,
                                    };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test Result With the Specified ID!" };

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

        public async Task<GenericResponseModel> getComputerBasedTestResultAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long typeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.CategoryId == categoryId && cb.TypeId == typeId && cb.TermId == termId && cb.SessionId == sessionId
                                 select new
                                 {
                                     cb.Id,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.StudentId,
                                     StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.CbtId,
                                     cb.ComputerBasedTest.Description,
                                     cb.NoOfQuestion,
                                     cb.PercentageScore,
                                     cb.TotalScore,
                                     cb.RightAnswers,
                                     cb.WrongAnswers,
                                     cb.ScoreStatus.ScoreStatusName,
                                     cb.DateTaken,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByTypeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long typeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.TypeId == typeId && cb.TermId == termId && cb.SessionId == sessionId
                                 select new
                                 {
                                     cb.Id,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.StudentId,
                                     StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.CbtId,
                                     cb.ComputerBasedTest.Description,
                                     cb.NoOfQuestion,
                                     cb.PercentageScore,
                                     cb.TotalScore,
                                     cb.RightAnswers,
                                     cb.WrongAnswers,
                                     cb.ScoreStatus.ScoreStatusName,
                                     cb.DateTaken,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByCategoryIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.CategoryId == categoryId && cb.TermId == termId && cb.SessionId == sessionId
                                 select new
                                 {
                                     cb.Id,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.StudentId,
                                     StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.CbtId,
                                     cb.ComputerBasedTest.Description,
                                     cb.NoOfQuestion,
                                     cb.PercentageScore,
                                     cb.TotalScore,
                                     cb.RightAnswers,
                                     cb.WrongAnswers,
                                     cb.ScoreStatus.ScoreStatusName,
                                     cb.DateTaken,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByCbtIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long typeId, long cbtId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.CbtId == cbtId && cb.TypeId == typeId && cb.CategoryId == categoryId && cb.TermId == termId && cb.SessionId == sessionId
                                 select new
                                 {
                                     cb.Id,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.StudentId,
                                     StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.CbtId,
                                     cb.ComputerBasedTest.Description,
                                     cb.NoOfQuestion,
                                     cb.PercentageScore,
                                     cb.TotalScore,
                                     cb.RightAnswers,
                                     cb.WrongAnswers,
                                     cb.ScoreStatus.ScoreStatusName,
                                     cb.DateTaken,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByStudentIdAsync(long schoolId, long campusId, long classId, long classGradeId, long cbtId, long categoryId, long typeId, Guid studentId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.CbtId == cbtId  && cb.StudentId == studentId && cb.TypeId == typeId && cb.CategoryId == categoryId && cb.TermId == termId && cb.SessionId == sessionId
                                 select new
                                 {
                                     cb.Id,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.StudentId,
                                     StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.CbtId,
                                     cb.ComputerBasedTest.Description,
                                     cb.NoOfQuestion,
                                     cb.PercentageScore,
                                     cb.TotalScore,
                                     cb.RightAnswers,
                                     cb.WrongAnswers,
                                     cb.ScoreStatus.ScoreStatusName,
                                     cb.DateTaken,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByIndividualStudentIdAsync(long schoolId, long campusId, long cbtId, Guid studentId)
        {
            try
            {
                //Validations
                var cbtObj = await _context.ComputerBasedTest.Where(x => x.Id == cbtId).FirstOrDefaultAsync();
                if(cbtObj == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid cbtId" };
                }
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //get School Current Session and Term
                long currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);
                long currentTermId = new SessionAndTerm(_context).getCurrentTermId(schoolId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    if (currentSessionId > 0 && currentTermId > 0)
                    {
                        //get the Student Class and ClassGrade
                        var getStudent = _context.GradeStudents.Where(x => x.StudentId == studentId && x.SessionId == currentSessionId).FirstOrDefault();

                        //Students Class and ClassGrade
                        long classId = getStudent.ClassId;
                        long classGradeId = getStudent.ClassGradeId;

                        if (getStudent != null)
                        {
                            //get the CBT Result 
                            var result = from cb in _context.CbtResults
                                         where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                         && cb.CbtId == cbtId && cb.StudentId == studentId && cb.TypeId == cbtObj.TypeId && cb.CategoryId == cbtObj.CategoryId && cb.TermId == currentTermId
                                         && cb.SessionId == currentSessionId
                                         select new
                                         {
                                             cb.Id,
                                             cb.SchoolId,
                                             cb.CampusId,
                                             cb.Classes.ClassName,
                                             cb.ClassGrades.GradeName,
                                             cb.StudentId,
                                             StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                             cb.CbtTypes.TypeName,
                                             cb.CbtCategory.CategoryName,
                                             cb.Terms.TermName,
                                             cb.Sessions.SessionName,
                                             cb.CbtId,
                                             cb.ComputerBasedTest.Description,
                                             cb.NoOfQuestion,
                                             cb.PercentageScore,
                                             cb.TotalScore,
                                             cb.RightAnswers,
                                             cb.WrongAnswers,
                                             cb.ScoreStatus.ScoreStatusName,
                                             cb.DateTaken,
                                         };

                            if (result.Count() > 0)
                            {
                                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                            }

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                        }

                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Student does not Belong to a Class for the session Specified" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "School Current Academic Session and Term has not been Set" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid School or CampusId" };
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

        public async Task<GenericResponseModel> getComputerBasedTestResultByClassIdAndGradeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long cbtId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGarade = check.checkClassGradeById(classGradeId);

                //check if the School and CampusId is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGarade == true)
                {
                    //get the CBT Result 
                    var result = from cb in _context.CbtResults
                                 where cb.SchoolId == schoolId && cb.CampusId == campusId && cb.ClassId == classId && cb.ClassGradeId == classGradeId
                                 && cb.CbtId == cbtId && cb.TermId == termId && cb.SessionId == sessionId
                                 select new
                                 {
                                     cb.Id,
                                     cb.SchoolId,
                                     cb.CampusId,
                                     cb.Classes.ClassName,
                                     cb.ClassGrades.GradeName,
                                     cb.StudentId,
                                     StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                     cb.CbtTypes.TypeName,
                                     cb.CbtCategory.CategoryName,
                                     cb.Terms.TermName,
                                     cb.Sessions.SessionName,
                                     cb.CbtId,
                                     cb.ComputerBasedTest.Description,
                                     cb.NoOfQuestion,
                                     cb.PercentageScore,
                                     cb.TotalScore,
                                     cb.RightAnswers,
                                     cb.WrongAnswers,
                                     cb.ScoreStatus.ScoreStatusName,
                                     cb.DateTaken,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "A Parameter with the specified ID does not exist!" };
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

        public async Task<GenericResponseModel> deleteComputerBasedTestResultAsync(long cbtResultId)
        {
            try
            {
                var obj = _context.CbtResults.Where(x => x.Id == cbtResultId).FirstOrDefault();
                if (obj != null)
                {
                    _context.CbtResults.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Result Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Result With the Specified ID!" };
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

        //--------------------------------------------------START COMPUTER BASED TEST----------------------------------------------------------------
        //call this endpoint to start CBT, by creating the result and calling the updateCbtResult to update the result on completion of the CBT
        public async Task<GenericResponseModel> takeComputerBasedTestAsync(CbtResultRequestModel obj)
        {
            try
            {
                //check if Computer Based Test Exists
                var cbt = _context.ComputerBasedTest.Where(x => x.Id == obj.CbtId).FirstOrDefault();

                //check if Student has taken the Computer Based Test 
                var checkResult = _context.CbtResults.Where(x => x.CbtId == obj.CbtId && x.StudentId == obj.StudentId).FirstOrDefault();

                if (cbt != null)
                {
                    if (checkResult == null)
                    {
                        //Create the Computer Based Test Result for the Student
                        CbtResults cbtResult = new CbtResults();
                        cbtResult.CbtId = obj.CbtId;
                        cbtResult.StudentId = obj.StudentId;
                        cbtResult.SchoolId = cbt.SchoolId;
                        cbtResult.CampusId = cbt.CampusId;
                        cbtResult.ClassId = cbt.ClassId;
                        cbtResult.ClassGradeId = cbt.ClassGradeId;
                        cbtResult.CbtId = cbt.Id;
                        cbtResult.TypeId = cbt.TypeId;
                        cbtResult.CategoryId = cbt.CategoryId;
                        cbtResult.TermId = cbt.TermId;
                        cbtResult.SessionId = cbt.SessionId;
                        cbtResult.NoOfQuestion = 0;
                        cbtResult.RightAnswers = 0;
                        cbtResult.WrongAnswers = 0;
                        cbtResult.TotalScore = 0;
                        cbtResult.PercentageScore = 0;
                        cbtResult.StatuId = (long)EnumUtility.ScoreStatus.Failed;
                        cbtResult.DateTaken = DateTime.Now;

                        await _context.CbtResults.AddAsync(cbtResult);
                        await _context.SaveChangesAsync();

                        //get the CBT Result Created
                        var getResult = from cb in _context.CbtResults
                                        where cb.Id == cbtResult.Id
                                        select new
                                        {
                                            cb.Id,
                                            cb.SchoolId,
                                            cb.CampusId,
                                            cb.Classes.ClassName,
                                            cb.ClassGrades.GradeName,
                                            cb.StudentId,
                                            StudentsName = cb.Students.FirstName + " " + cb.Students.LastName,
                                            cb.CbtTypes.TypeName,
                                            cb.CbtCategory.CategoryName,
                                            cb.Terms.TermName,
                                            cb.Sessions.SessionName,
                                            cb.CbtId,
                                            cb.ComputerBasedTest.Description,
                                            cb.NoOfQuestion,
                                            cb.PercentageScore,
                                            cb.TotalScore,
                                            cb.RightAnswers,
                                            cb.WrongAnswers,
                                            cb.ScoreStatus.ScoreStatusName,
                                            cb.DateTaken,
                                        };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Computer Based Test Result Created Successfully!", Data = getResult.FirstOrDefault()};
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Computer Based Test has Already been Taken by the Student!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Computer Based Test With the Specified ID!" };

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

        //-------------------------------------------------SYSTEM DEFINED/DEFAULTS------------------------------------------------------------------------

        public async Task<GenericResponseModel> getCbtTypesAsync()
        {
            try
            {
                var result = from cb in _context.CbtTypes select cb;

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

        public async Task<GenericResponseModel> getCbtTypesByIdAsync(long cbtTypeId)
        {
            try
            {
                var result = from cb in _context.CbtTypes where cb.Id == cbtTypeId select cb;

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available"};

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

        public async Task<GenericResponseModel> getCbtCategoryAsync()
        {
            try
            {
                var result = from cb in _context.CbtCategory select cb;

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
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

        public async Task<GenericResponseModel> getCbtCategoryByIdAsync(long cbtCategoryId)
        {
            try
            {
                var result = from cb in _context.CbtCategory where cb.Id == cbtCategoryId select cb;

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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
    }
}
