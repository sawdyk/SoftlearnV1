﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Text;
using SoftLearnV1.SchoolReusables;

namespace SoftLearnV1.Repositories
{
    public class ReportCardConfigurationRepo : IReportCardConfigurationRepo
    {
        private readonly AppDbContext _context;
        public ReportCardConfigurationRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericResponseModel> createCommentsListAsync(CommentListRequestModel obj)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    foreach (CommentList com in obj.Comments)
                    {
                        //check if the comment does not exists
                        var checkComment = _context.ReportCardCommentsList.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.Comment == com.Comment).FirstOrDefault();

                        if (checkComment == null)
                        {
                            //add new comments
                            var newCom = new ReportCardCommentsList
                            {
                                SchoolId = obj.SchoolId,
                                CampusId = obj.CampusId,
                                UploadedById = obj.UploadedById,
                                Comment = com.Comment,
                                DateCreated = DateTime.Now,
                                LastDateUpdated = DateTime.Now,
                            };

                            await _context.ReportCardCommentsList.AddAsync(newCom);
                            await _context.SaveChangesAsync();

                            //return the commenta added
                            var result = (from co in _context.ReportCardCommentsList
                                          where co.Id == newCom.Id
                                          select new
                                          {
                                              co.Id,
                                              co.SchoolId,
                                              co.CampusId,
                                              co.UploadedById,
                                              UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                              co.Comment,
                                              co.DateCreated,
                                              co.LastDateUpdated
                                          }).FirstOrDefault();

                            data.Add(result);

                            response.StatusCode = 200;
                            response.StatusMessage = "Comment(s) Added Successfully!";
                            response.Data = data.ToList();

                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "One or more Comments Already Exists!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "School or Campus With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> getCommentByIdAsync(long schoolId, long campusId, long commentId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    var result = from co in _context.ReportCardCommentsList
                                 where co.Id == commentId && co.SchoolId == schoolId && co.CampusId == campusId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.Comment,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> getAllCommentsAsync(long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    var result = from co in _context.ReportCardCommentsList
                                 where co.SchoolId == schoolId && co.CampusId == campusId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.Comment,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> updateCommentsAsync(long commentId, UpdateCommentRequestModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //check the comment with the specified ID
                    var checkComment = _context.ReportCardCommentsList.Where(x => x.Id == commentId).FirstOrDefault();
                    if (checkComment != null)
                    {
                        //Check if the comment already exist
                        var checkExist = _context.ReportCardCommentsList.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.Comment == obj.Comment).FirstOrDefault();
                        if (checkExist == null)
                        {
                            //Update the comment if it exists
                            checkComment.SchoolId = obj.SchoolId;
                            checkComment.CampusId = obj.CampusId;
                            checkComment.UploadedById = obj.UploadedById;
                            checkComment.Comment = obj.Comment;
                            checkComment.LastDateUpdated = DateTime.Now;
                            await _context.SaveChangesAsync();

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Comment Updated Successfully" };
                        }

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "This Comments Already Exists" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Comment With Specified ID does not exist" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };
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

        public async Task<GenericResponseModel> deleteCommentsAsync(long commentId)
        {
            try
            {
                //check the comment with the specified ID
                var comment = _context.ReportCardCommentsList.Where(x => x.Id == commentId).FirstOrDefault();
                if (comment != null)
                {
                    _context.ReportCardCommentsList.Remove(comment);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Comment Deleted Successfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Comment With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> uploadReportCardSignatureAsync(ReportCardSignatureRequestModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //check if signature already exists
                    var checkSign = _context.ReportCardSignature.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (checkSign == null)
                    {
                        //add new signature
                        var newSign = new ReportCardSignature
                        {
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            UploadedById = obj.UploadedById,
                            SignatureUrl = obj.SignatureUrl,
                            DateCreated = DateTime.Now,
                            LastDateUpdated = DateTime.Now,
                        };

                        await _context.ReportCardSignature.AddAsync(newSign);
                        await _context.SaveChangesAsync();

                        //return the result
                        var result = (from co in _context.ReportCardSignature
                                      where co.Id == newSign.Id
                                      select new
                                      {
                                          co.Id,
                                          co.SchoolId,
                                          co.CampusId,
                                          co.UploadedById,
                                          UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                          co.SignatureUrl,
                                          co.DateCreated,
                                          co.LastDateUpdated
                                      }).FirstOrDefault();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Report Card Signature Added Successfully!", Data = result };

                    }
                    else
                    {
                        checkSign.SchoolId = obj.SchoolId;
                        checkSign.CampusId = obj.CampusId;
                        checkSign.UploadedById = obj.UploadedById;
                        checkSign.SignatureUrl = obj.SignatureUrl;
                        checkSign.LastDateUpdated = DateTime.Now;
                        await _context.SaveChangesAsync();

                        //return the result
                        var result = (from co in _context.ReportCardSignature
                                      where co.Id == checkSign.Id
                                      select new
                                      {
                                          co.Id,
                                          co.SchoolId,
                                          co.CampusId,
                                          co.UploadedById,
                                          UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                          co.SignatureUrl,
                                          co.DateCreated,
                                          co.LastDateUpdated

                                      }).FirstOrDefault();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Report Card Signature Updated Successfully", Data = result };
                    }
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> getReportCardSignatureAsync(long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    var result = from co in _context.ReportCardSignature
                                 where co.SchoolId == schoolId && co.CampusId == campusId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.SignatureUrl,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> nextTermBeginsAsync(NextTermBeginsRequestModel obj)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    var checkNextTerm = _context.ReportCardNextTermBegins.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (checkNextTerm == null)
                    {
                        //add new nextTermBeginsDate
                        var newNext = new ReportCardNextTermBegins
                        {
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            UploadedById = obj.UploadedById,
                            NextTermBeginsDate = obj.NextTermBeginsDate,
                            DateCreated = DateTime.Now,
                            LastDateUpdated = DateTime.Now,
                        };

                        await _context.ReportCardNextTermBegins.AddAsync(newNext);
                        await _context.SaveChangesAsync();

                        //return the result
                        var result = (from co in _context.ReportCardNextTermBegins
                                      where co.Id == newNext.Id
                                      select new
                                      {
                                          co.Id,
                                          co.SchoolId,
                                          co.CampusId,
                                          co.UploadedById,
                                          UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                          co.NextTermBeginsDate,
                                          co.DateCreated,
                                          co.LastDateUpdated
                                      }).FirstOrDefault();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Report Card Next Term Begins Added Successfully!", Data = result };

                    }
                    else
                    {
                        //Updates the NextTermBeginsDate
                        checkNextTerm.SchoolId = obj.SchoolId;
                        checkNextTerm.CampusId = obj.CampusId;
                        checkNextTerm.UploadedById = obj.UploadedById;
                        checkNextTerm.NextTermBeginsDate = obj.NextTermBeginsDate;
                        checkNextTerm.LastDateUpdated = DateTime.Now;
                        await _context.SaveChangesAsync();

                        //return the result
                        var result = (from co in _context.ReportCardNextTermBegins
                                      where co.Id == checkNextTerm.Id
                                      select new
                                      {
                                          co.Id,
                                          co.SchoolId,
                                          co.CampusId,
                                          co.UploadedById,
                                          UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                          co.NextTermBeginsDate,
                                          co.DateCreated,
                                          co.LastDateUpdated
                                      }).FirstOrDefault();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Report Card Next Term Begins Updated Successfully", Data = result };
                    }
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> getNextTermBeginsAsync(long schoolId, long campusId)
        {

            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    var result = from co in _context.ReportCardNextTermBegins
                                 where co.SchoolId == schoolId && co.CampusId == campusId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.NextTermBeginsDate,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "School or Campus With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> addCommentsOnReportCardForAllStudentsAsync(CommentsOnReportsCardForAllStudent obj)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);


                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    foreach (CommentsAndRemarks com in obj.Comments)
                    {
                        //check if the student belong to the class and ClassGrade
                        var checkStudent = _context.GradeStudents.Where(x => x.StudentId == com.StudentId && x.SchoolId == obj.SchoolId
                        && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.SessionId == obj.SessionId && x.HasGraduated == false).FirstOrDefault();

                        if (checkStudent != null)
                        {
                            //get the student admissionNumber
                            string admissionNumber = _context.Students.Where(x => x.Id == checkStudent.StudentId).FirstOrDefault().AdmissionNumber;

                            //check if the comment exists
                            var checkComment = _context.ReportCardComments.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                            && x.ClassGradeId == obj.ClassGradeId && x.CommentConfigId == obj.CommentConfigId && x.StudentId == com.StudentId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                            if (checkComment == null)
                            {
                                //add new comment
                                var newCom = new ReportCardComments
                                {
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    StudentId = com.StudentId,
                                    AdmissionNumber = admissionNumber,
                                    UploadedById = obj.UploadedById,
                                    CommentConfigId = obj.CommentConfigId,
                                    TermId = obj.TermId,
                                    SessionId = obj.SessionId,
                                    Comment = com.Comment,
                                    Remark = com.Remark,
                                    DateCreated = DateTime.Now,
                                    LastDateUpdated = DateTime.Now,
                                };

                                await _context.ReportCardComments.AddAsync(newCom);
                                await _context.SaveChangesAsync();

                                //return the list of comments added
                                var result = (from co in _context.ReportCardComments
                                              where co.Id == newCom.Id
                                              select new
                                              {
                                                  co.Id,
                                                  co.SchoolId,
                                                  co.CampusId,
                                                  co.Classes.ClassName,
                                                  co.ClassGrades.GradeName,
                                                  co.Terms.TermName,
                                                  co.Sessions.SessionName,
                                                  co.StudentId,
                                                  StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                                  co.AdmissionNumber,
                                                  co.UploadedById,
                                                  UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                                  co.Comment,
                                                  co.Remark,
                                                  co.DateCreated,
                                                  co.LastDateUpdated
                                              }).FirstOrDefault();

                                data.Add(result);

                                response.StatusCode = 200;
                                response.StatusMessage = "Comment(s) Added Successfully!";
                                response.Data = data.ToList();

                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "One or more Comments Already Exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "One or more Student does not belong to this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "Some Parameters With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> addCommentsOnReportCardForSingleStudentAsync(CommentsOnReportsCardForSingleStudent obj)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);


                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    //check if the selected student belongs to a class
                    var checkStudent = _context.GradeStudents.Where(x => x.StudentId == obj.Comments.StudentId && x.SchoolId == obj.SchoolId
                    && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.SessionId == obj.SessionId && x.HasGraduated == false).FirstOrDefault();

                    if (checkStudent != null)
                    {
                        //get the student admissionNumber
                        string admissionNumber = _context.Students.Where(x => x.Id == checkStudent.StudentId).FirstOrDefault().AdmissionNumber;

                        //check if the comment exists
                        var checkComment = _context.ReportCardComments.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                        && x.ClassGradeId == obj.ClassGradeId && x.CommentConfigId == obj.CommentConfigId && x.StudentId == obj.Comments.StudentId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                        if (checkComment == null)
                        {
                            //add new comment
                            var newCom = new ReportCardComments
                            {
                                SchoolId = obj.SchoolId,
                                CampusId = obj.CampusId,
                                ClassId = obj.ClassId,
                                ClassGradeId = obj.ClassGradeId,
                                StudentId = obj.Comments.StudentId,
                                AdmissionNumber = admissionNumber,
                                UploadedById = obj.UploadedById,
                                CommentConfigId = obj.CommentConfigId,
                                TermId = obj.TermId,
                                SessionId = obj.SessionId,
                                Comment = obj.Comments.Comment,
                                Remark = obj.Comments.Remark,
                                DateCreated = DateTime.Now,
                                LastDateUpdated = DateTime.Now,
                            };

                            await _context.ReportCardComments.AddAsync(newCom);
                            await _context.SaveChangesAsync();

                            //return the comment added
                            var result = from co in _context.ReportCardComments
                                         where co.Id == newCom.Id
                                         select new
                                         {
                                             co.Id,
                                             co.SchoolId,
                                             co.CampusId,
                                             co.Classes.ClassName,
                                             co.ClassGrades.GradeName,
                                             co.Terms.TermName,
                                             co.Sessions.SessionName,
                                             co.StudentId,
                                             StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                             co.AdmissionNumber,
                                             co.UploadedById,
                                             UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                             co.Comment,
                                             co.Remark,
                                             co.DateCreated,
                                             co.LastDateUpdated
                                         };

                            response.StatusCode = 200;
                            response.StatusMessage = "Comment Added Successfully!";
                            response.Data = result.FirstOrDefault();

                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "Comments Already Exists!";
                        }
                    }
                    else
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "Student does not belong to this Class!";
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "Some Parameters With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> getAllCommentOnReportCardAsync(long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    var result = from co in _context.ReportCardComments
                                 where co.SchoolId == schoolId && co.CampusId == campusId && co.CommentConfigId == commentConfigId && co.ClassId == classId
                                 && co.ClassGradeId == classGradeId && co.TermId == termId && co.SessionId == sessionId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.Classes.ClassName,
                                     co.ClassGrades.GradeName,
                                     co.Terms.TermName,
                                     co.Sessions.SessionName,
                                     co.StudentId,
                                     StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                     co.AdmissionNumber,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.Comment,
                                     co.Remark,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> getCommentOnReportCardByIdAsync(long commentOnReportCardId)
        {
            try
            {
                //check if the comment with specified ID exists
                var checkComment = _context.ReportCardComments.Where(x => x.Id == commentOnReportCardId).FirstOrDefault();

                if (checkComment != null)
                {
                    var result = from co in _context.ReportCardComments
                                 where co.Id == commentOnReportCardId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.Classes.ClassName,
                                     co.ClassGrades.GradeName,
                                     co.Terms.TermName,
                                     co.Sessions.SessionName,
                                     co.StudentId,
                                     StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                     co.AdmissionNumber,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.Comment,
                                     co.Remark,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Comment With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> getCommentOnReportCardByStudentIdAsync(Guid studentId, long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    var result = from co in _context.ReportCardComments
                                 where co.StudentId == studentId && co.SchoolId == schoolId && co.CampusId == campusId && co.CommentConfigId == commentConfigId && co.ClassId == classId
                                 && co.ClassGradeId == classGradeId && co.TermId == termId && co.SessionId == sessionId
                                 select new
                                 {
                                     co.Id,
                                     co.SchoolId,
                                     co.CampusId,
                                     co.Classes.ClassName,
                                     co.ClassGrades.GradeName,
                                     co.Terms.TermName,
                                     co.Sessions.SessionName,
                                     co.StudentId,
                                     StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                     co.AdmissionNumber,
                                     co.UploadedById,
                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                     co.Comment,
                                     co.Remark,
                                     co.DateCreated,
                                     co.LastDateUpdated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters With Specified ID does not exist" };

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

        public async Task<GenericResponseModel> getAllReportCardCommenConfigAsync()
        {
            try
            {
                var result = from co in _context.ReportCardCommentConfig select co;

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

        public async Task<GenericResponseModel> getAllReportCardCommenConfigByIdAsync(long commentConfigId)
        {
            try
            {
                var result = from co in _context.ReportCardCommentConfig where co.Id == commentConfigId select co;

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


        public async Task<GenericResponseModel> updateCommentsOnReportCardForAllStudentsAsync(CommentsOnReportsCardForAllStudent obj)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    foreach (CommentsAndRemarks com in obj.Comments)
                    {
                        //check if the student belong to the class and ClassGrade
                        var checkStudent = _context.GradeStudents.Where(x => x.StudentId == com.StudentId && x.SchoolId == obj.SchoolId
                        && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.SessionId == obj.SessionId && x.HasGraduated == false).FirstOrDefault();

                        if (checkStudent != null)
                        {
                            //get the student admissionNumber
                            string admissionNumber = _context.Students.Where(x => x.Id == checkStudent.StudentId).FirstOrDefault().AdmissionNumber;

                            //check if the comment exists
                            var checkComment = _context.ReportCardComments.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                            && x.ClassGradeId == obj.ClassGradeId && x.CommentConfigId == obj.CommentConfigId && x.StudentId == com.StudentId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                            //check if a comment with comment and remark already exists
                            var checkExists = _context.ReportCardComments.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                            && x.ClassGradeId == obj.ClassGradeId && x.CommentConfigId == obj.CommentConfigId && x.StudentId == com.StudentId && x.TermId == obj.TermId
                            && x.Comment == com.Comment && x.Remark == com.Remark && x.SessionId == obj.SessionId).FirstOrDefault();

                            if (checkComment != null)
                            {
                                if (checkExists == null)
                                {
                                    checkComment.SchoolId = obj.SchoolId;
                                    checkComment.CampusId = obj.CampusId;
                                    checkComment.ClassId = obj.ClassId;
                                    checkComment.ClassGradeId = obj.ClassGradeId;
                                    checkComment.StudentId = com.StudentId;
                                    checkComment.AdmissionNumber = admissionNumber;
                                    checkComment.UploadedById = obj.UploadedById;
                                    checkComment.CommentConfigId = obj.CommentConfigId;
                                    checkComment.TermId = obj.TermId;
                                    checkComment.SessionId = obj.SessionId;
                                    checkComment.Comment = com.Comment;
                                    checkComment.Remark = com.Remark;
                                    checkComment.LastDateUpdated = DateTime.Now;

                                    await _context.SaveChangesAsync();

                                    //return the list of comments Updated
                                    var result = from co in _context.ReportCardComments
                                                 where co.Id == checkComment.Id
                                                 select new
                                                 {
                                                     co.Id,
                                                     co.SchoolId,
                                                     co.CampusId,
                                                     co.Classes.ClassName,
                                                     co.ClassGrades.GradeName,
                                                     co.Terms.TermName,
                                                     co.Sessions.SessionName,
                                                     co.StudentId,
                                                     StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                                     co.AdmissionNumber,
                                                     co.UploadedById,
                                                     UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                                     co.Comment,
                                                     co.Remark,
                                                     co.DateCreated,
                                                     co.LastDateUpdated
                                                 };

                                    data.Add(result);

                                    response.StatusCode = 200;
                                    response.StatusMessage = "Comment(s) Updated Successfully!";
                                    response.Data = data.FirstOrDefault();
                                }
                                else
                                {
                                    response.StatusCode = 200;
                                    response.StatusMessage = "Comment(s) Already Exists!";
                                }
                            }
                            else
                            {
                                response.StatusCode = 409;
                                response.StatusMessage = "One or more Comments does not Exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "One or More Student does not belong to this Class!";
                        }
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "Some Parameters With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> updateCommentsOnReportCardForSingleStudentAsync(CommentsOnReportsCardForSingleStudent obj)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    //check if the student belong to the class and ClassGrade
                    var checkStudent = _context.GradeStudents.Where(x => x.StudentId == obj.Comments.StudentId && x.SchoolId == obj.SchoolId
                    && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId && x.ClassGradeId == obj.ClassGradeId && x.SessionId == obj.SessionId && x.HasGraduated == false).FirstOrDefault();

                    if (checkStudent != null)
                    {
                        //get the student admissionNumber
                        string admissionNumber = _context.Students.Where(x => x.Id == checkStudent.StudentId).FirstOrDefault().AdmissionNumber;

                        //check if the comment exists
                        var checkComment = _context.ReportCardComments.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                        && x.ClassGradeId == obj.ClassGradeId && x.CommentConfigId == obj.CommentConfigId && x.StudentId == obj.Comments.StudentId && x.TermId == obj.TermId && x.SessionId == obj.SessionId).FirstOrDefault();

                        //check if a comment with comment and remark already exists
                        var checkExists = _context.ReportCardComments.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId
                        && x.ClassGradeId == obj.ClassGradeId && x.CommentConfigId == obj.CommentConfigId && x.StudentId == obj.Comments.StudentId && x.TermId == obj.TermId
                        && x.Comment == obj.Comments.Comment && x.Remark == obj.Comments.Remark && x.SessionId == obj.SessionId).FirstOrDefault();

                        if (checkComment != null)
                        {
                            if (checkExists == null)
                            {
                                checkComment.SchoolId = obj.SchoolId;
                                checkComment.CampusId = obj.CampusId;
                                checkComment.ClassId = obj.ClassId;
                                checkComment.ClassGradeId = obj.ClassGradeId;
                                checkComment.StudentId = obj.Comments.StudentId;
                                checkComment.AdmissionNumber = admissionNumber;
                                checkComment.UploadedById = obj.UploadedById;
                                checkComment.CommentConfigId = obj.CommentConfigId;
                                checkComment.TermId = obj.TermId;
                                checkComment.SessionId = obj.SessionId;
                                checkComment.Comment = obj.Comments.Comment;
                                checkComment.Remark = obj.Comments.Remark;
                                checkComment.LastDateUpdated = DateTime.Now;

                                await _context.SaveChangesAsync();

                                //return the list of comments Updated
                                var result = from co in _context.ReportCardComments
                                             where co.Id == checkComment.Id
                                             select new
                                             {
                                                 co.Id,
                                                 co.SchoolId,
                                                 co.CampusId,
                                                 co.Classes.ClassName,
                                                 co.ClassGrades.GradeName,
                                                 co.Terms.TermName,
                                                 co.Sessions.SessionName,
                                                 co.StudentId,
                                                 StudentName = co.Students.FirstName + " " + co.Students.LastName,
                                                 co.AdmissionNumber,
                                                 co.UploadedById,
                                                 UploadedBy = co.SchoolUsers.FirstName + " " + co.SchoolUsers.LastName,
                                                 co.Comment,
                                                 co.Remark,
                                                 co.DateCreated,
                                                 co.LastDateUpdated
                                             };

                                data.Add(result);

                                response.StatusCode = 200;
                                response.StatusMessage = "Comment(s) Updated Successfully!";
                                response.Data = data.FirstOrDefault();
                            }
                            else
                            {
                                response.StatusCode = 200;
                                response.StatusMessage = "Comment Already Exists!";
                            }
                        }
                        else
                        {
                            response.StatusCode = 409;
                            response.StatusMessage = "One or more Comments does not Exists!";
                        }
                    }
                    else
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "Student does not belong to this Class!";
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "Some Parameters With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> deleteCommentsOnReportCardForAllStudentAsync(long commentConfigId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    //check if the comment exists
                    IList<ReportCardComments> checkComment = (_context.ReportCardComments.Where(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId
                    && x.ClassGradeId == classGradeId && x.CommentConfigId == commentConfigId && x.TermId == termId && x.SessionId == sessionId)).ToList();

                    if (checkComment.Count() > 0)
                    {
                        _context.ReportCardComments.RemoveRange(checkComment);
                        await _context.SaveChangesAsync();

                        response.StatusCode = 409;
                        response.StatusMessage = "Comments Deleted Successfully!";
                    }
                    else
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "No Record Available!";
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "Some Parameters With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> deleteCommentsOnReportCardForSingleStudentAsync(long commentConfigId, Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkClass = check.checkClassById(classId);
                var checkClassGrade = check.checkClassGradeById(classGradeId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true && checkClassGrade == true)
                {
                    //check if the comment exists
                    ReportCardComments checkComment = _context.ReportCardComments.Where(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId
                    && x.ClassGradeId == classGradeId && x.CommentConfigId == commentConfigId && x.StudentId == studentId && x.TermId == termId && x.SessionId == sessionId).FirstOrDefault();

                    if (checkComment != null)
                    {
                        _context.ReportCardComments.Remove(checkComment);
                        await _context.SaveChangesAsync();

                        response.StatusCode = 409;
                        response.StatusMessage = "Comments Deleted Successfully!";
                    }
                    else
                    {
                        response.StatusCode = 409;
                        response.StatusMessage = "No Record Available!";
                    }
                }
                else
                {
                    response.StatusCode = 409;
                    response.StatusMessage = "Some Parameters With the Specified ID does not Exist";
                }

                return response;
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

        public async Task<GenericResponseModel> createReportCardTemplateAsync(ReportCardTemplateRequestModel obj)
        {
            try
            {
                IList<object> data = new List<object>();
                var response = new GenericResponseModel();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.CampusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkClass == true)
                {
                    //check if the Template exists
                    ReportCardTemplates templates = _context.ReportCardTemplates.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId
                    && x.ClassId == obj.ClassId && x.SchoolSubTypeId == obj.SchoolSubTypeId).FirstOrDefault();

                    if (templates == null)
                    {
                        ReportCardTemplates temp = new ReportCardTemplates
                        {
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            ClassId = obj.ClassId,
                            SchoolSubTypeId = obj.SchoolSubTypeId,
                            Description = obj.Description,
                            DateCreated = DateTime.Now,
                        };

                        await _context.ReportCardTemplates.AddAsync(temp);
                        await _context.SaveChangesAsync();

                        //return the template created
                        var result = from tm in _context.ReportCardTemplates
                                     where tm.Id == temp.Id
                                     select new
                                     {
                                         tm.Id,
                                         tm.SchoolId,
                                         tm.CampusId,
                                         tm.ClassId,
                                         tm.Classes.ClassName,
                                         tm.SchoolSubTypes.SubTypeName,
                                         tm.Description,
                                         tm.DateCreated
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Report Card Template Created Successfully!", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Report Card Template Already Exist!" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };
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

        public async Task<GenericResponseModel> getReportCardTemplateByIdAsync(long reportCardTemplateId)
        {
            try
            {
                //Check if the template exists
                var getTemplate = _context.ReportCardTemplates.Where(x => x.Id == reportCardTemplateId).FirstOrDefault();

                if (getTemplate != null)
                {
                    //get the report card template
                    var result = from tm in _context.ReportCardTemplates
                                 where tm.Id == reportCardTemplateId
                                 select new
                                 {
                                     tm.Id,
                                     tm.SchoolId,
                                     tm.CampusId,
                                     tm.ClassId,
                                     tm.Classes.ClassName,
                                     tm.SchoolSubTypes.SubTypeName,
                                     tm.Description,
                                     tm.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Report Card Template With the Specified ID", };
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

        public async Task<GenericResponseModel> getReportCardTemplateAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //get the report card template
                    var result = from tm in _context.ReportCardTemplates
                                 where tm.SchoolId == schoolId && tm.CampusId == campusId
                                 select new
                                 {
                                     tm.Id,
                                     tm.SchoolId,
                                     tm.CampusId,
                                     tm.ClassId,
                                     tm.Classes.ClassName,
                                     tm.SchoolSubTypes.SubTypeName,
                                     tm.Description,
                                     tm.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };
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


        //----------------------------------------------------REPORT CARD CONFIGURATION------------------------------------------------------------------------------------------------------

        public async Task<GenericResponseModel> createReportCardConfigurationAsync(ReportCardConfigRequestModel obj)
        {
            try
            {
                CreateReportCardConfigurationResponseModel responseModel = new CreateReportCardConfigurationResponseModel();
                IList<long> successfulClassId = new List<long>();
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkTerm = check.checkTermById(obj.TermId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkTerm == true)
                {
                    //check if the config already exists for the specified term
                    //ReportCardConfiguration checkConfig = _context.ReportCardConfiguration.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.TermId == obj.TermId).FirstOrDefault();

                    ////if it doesnt exists, create new config
                    //if (checkConfig == null)
                    //{
                    //if config is for first or second term
                    if (obj.TermId == (int)EnumUtility.Terms.FirstTerm || obj.TermId == (int)EnumUtility.Terms.SecondTerm)
                    {
                        ReportCardConfiguration rptConfig = new ReportCardConfiguration()
                        {
                            ShowDepartment = obj.ShowDepartment,
                            ShowSubject = obj.ShowSubject,
                            ShowCAScore = obj.ShowCAScore,
                            ShowExamScore = obj.ShowExamScore,
                            ComputeCA_Cumulative = obj.ComputeCA_Cumulative,
                            ShowCA_Cumulative = obj.ShowCA_Cumulative,
                            MultipleLegend = obj.MultipleLegend,
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            TermId = obj.TermId,
                            CreatedBy = obj.CreatedOrUpdatedBy,
                            DateCreated = DateTime.Now,
                            LastUpdatedBy = obj.CreatedOrUpdatedBy,
                            LastUpdatedDate = DateTime.Now,
                            RefFirstTermScoreCompute = false,
                            RefFirstTermScoreShow = false,
                            RefSecondTermScoreCompute = false,
                            RefSecondTermScoreShow = false,
                            ComputeOverallTotalAverage = false,
                            ShowComputeOverallTotalAverage = false,
                        };

                        await _context.ReportCardConfiguration.AddAsync(rptConfig);
                        await _context.SaveChangesAsync();
                        //Set existing configuration to inActive
                        foreach (var classId in obj.ClassIds)
                        {
                            var classExist = await _context.Classes.Where(x => x.Id == classId && x.SchoolId == obj.SchoolId).FirstOrDefaultAsync();
                            if (classExist != null)
                            {
                                ReportCardConfigurationForClass cardConfigurationForClass = new ReportCardConfigurationForClass
                                {
                                    ClassId = classId,
                                    ConfigurationId = rptConfig.Id,
                                    CreatedAt = DateTime.Now,
                                    SchoolId = obj.SchoolId,
                                    TermId = obj.TermId,
                                    UpdatedAt = DateTime.Now
                                };
                                await _context.ReportCardConfigurationForClasses.AddAsync(cardConfigurationForClass);
                                await _context.SaveChangesAsync();
                                //deactivate existing ones
                                var classConfigs = await _context.ReportCardConfigurationForClasses.Where(x => x.SchoolId == obj.SchoolId && x.TermId == obj.TermId && x.ClassId == classId && x.IsActive == true).ToListAsync();
                                foreach (var classConfig in classConfigs)
                                {
                                    var classConf = await _context.ReportCardConfigurationForClasses.Where(x => x.Id == classConfig.Id).FirstOrDefaultAsync();
                                    classConf.IsActive = false;
                                    await _context.SaveChangesAsync();
                                }
                                //Set the new one as active
                                cardConfigurationForClass.IsActive = true;
                                await _context.SaveChangesAsync();
                                successfulClassId.Add(classId);
                            }
                        }

                        //get the report card config
                        var result = from rp in _context.ReportCardConfiguration
                                     where rp.Id == rptConfig.Id
                                     select new
                                     {
                                         rp.Id,
                                         rp.ShowDepartment,
                                         rp.ShowSubject,
                                         rp.ShowCAScore,
                                         rp.ShowExamScore,
                                         rp.ComputeCA_Cumulative,
                                         rp.ShowCA_Cumulative,
                                         rp.MultipleLegend,
                                         rp.SchoolId,
                                         rp.CampusId,
                                         rp.TermId,
                                         rp.Terms.TermName,
                                         rp.CreatedBy,
                                         rp.DateCreated,
                                         rp.LastUpdatedBy,
                                         rp.LastUpdatedDate,
                                         rp.RefFirstTermScoreCompute,
                                         rp.RefFirstTermScoreShow,
                                         rp.RefSecondTermScoreCompute,
                                         rp.RefSecondTermScoreShow,
                                         rp.ComputeOverallTotalAverage,
                                         rp.ShowComputeOverallTotalAverage,
                                     };
                        responseModel.ConfigurationDetail = result;
                        responseModel.SuccessfulClassId = successfulClassId;
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Configuration Created Successfully", Data = responseModel };

                    }

                    //if config is for third term
                    if (obj.TermId == (int)EnumUtility.Terms.ThirdTerm)
                    {
                        ReportCardConfiguration rptConfig = new ReportCardConfiguration()
                        {
                            ShowDepartment = obj.ShowDepartment,
                            ShowSubject = obj.ShowSubject,
                            ShowCAScore = obj.ShowCAScore,
                            ShowExamScore = obj.ShowExamScore,
                            ComputeCA_Cumulative = obj.ComputeCA_Cumulative,
                            ShowCA_Cumulative = obj.ShowCA_Cumulative,
                            MultipleLegend = obj.MultipleLegend,
                            SchoolId = obj.SchoolId,
                            CampusId= obj.CampusId,
                            TermId = obj.TermId,
                            CreatedBy = obj.CreatedOrUpdatedBy,
                            DateCreated = DateTime.Now,
                            LastUpdatedBy = obj.CreatedOrUpdatedBy,
                            LastUpdatedDate = DateTime.Now,
                            RefFirstTermScoreCompute = obj.RefFirstTermScoreCompute,
                            RefFirstTermScoreShow = obj.RefFirstTermScoreShow,
                            RefSecondTermScoreCompute = obj.RefSecondTermScoreCompute,
                            RefSecondTermScoreShow = obj.RefSecondTermScoreShow,
                            ComputeOverallTotalAverage = obj.ComputeOverallTotalAverage,
                            ShowComputeOverallTotalAverage = obj.ShowComputeOverallTotalAverage,
                        };

                        await _context.ReportCardConfiguration.AddAsync(rptConfig);
                        await _context.SaveChangesAsync();

                        foreach (var classId in obj.ClassIds)
                        {
                            var classExist = await _context.Classes.Where(x => x.Id == classId && x.SchoolId == obj.SchoolId).FirstOrDefaultAsync();
                            if (classExist != null)
                            {
                                ReportCardConfigurationForClass cardConfigurationForClass = new ReportCardConfigurationForClass
                                {
                                    ClassId = classId,
                                    ConfigurationId = rptConfig.Id,
                                    CreatedAt = DateTime.Now,
                                    SchoolId = obj.SchoolId,
                                    TermId = obj.TermId,
                                    UpdatedAt = DateTime.Now
                                };
                                await _context.ReportCardConfigurationForClasses.AddAsync(cardConfigurationForClass);
                                await _context.SaveChangesAsync();
                                //deactivate existing ones
                                var classConfigs = await _context.ReportCardConfigurationForClasses.Where(x => x.SchoolId == obj.SchoolId && x.TermId == obj.TermId && x.ClassId == classId && x.IsActive == true).ToListAsync();
                                foreach (var classConfig in classConfigs)
                                {
                                    var classConf = await _context.ReportCardConfigurationForClasses.Where(x => x.Id == classConfig.Id).FirstOrDefaultAsync();
                                    classConf.IsActive = false;
                                    await _context.SaveChangesAsync();
                                }
                                //Set the new one as active
                                cardConfigurationForClass.IsActive = true;
                                await _context.SaveChangesAsync();
                                successfulClassId.Add(classId);
                            }
                        }

                        //get the report card config
                        var result = from rp in _context.ReportCardConfiguration
                                     where rp.Id == rptConfig.Id
                                     select new
                                     {
                                         rp.Id,
                                         rp.ShowDepartment,
                                         rp.ShowSubject,
                                         rp.ShowCAScore,
                                         rp.ShowExamScore,
                                         rp.ComputeCA_Cumulative,
                                         rp.ShowCA_Cumulative,
                                         rp.MultipleLegend,
                                         rp.SchoolId,
                                         rp.CampusId,
                                         rp.TermId,
                                         rp.Terms.TermName,
                                         rp.CreatedBy,
                                         rp.DateCreated,
                                         rp.LastUpdatedBy,
                                         rp.LastUpdatedDate,
                                         rp.RefFirstTermScoreCompute,
                                         rp.RefFirstTermScoreShow,
                                         rp.RefSecondTermScoreCompute,
                                         rp.RefSecondTermScoreShow,
                                         rp.ComputeOverallTotalAverage,
                                         rp.ShowComputeOverallTotalAverage,
                                     };
                        responseModel.ConfigurationDetail = result;
                        responseModel.SuccessfulClassId = successfulClassId;
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Configuration Created Successfully", Data = responseModel };
                    }
                    //}

                    //return new GenericResponseModel { StatusCode = 409, StatusMessage = "Configuration Already Exists"};
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };
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

        public async Task<GenericResponseModel> getAllReportCardConfigurationAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {

                    //get the report card config
                    var result = from rp in _context.ReportCardConfiguration
                                 where rp.SchoolId == schoolId && rp.CampusId == campusId
                                 select new
                                 {
                                     rp.Id,
                                     rp.ShowDepartment,
                                     rp.ShowSubject,
                                     rp.ShowCAScore,
                                     rp.ShowExamScore,
                                     rp.ComputeCA_Cumulative,
                                     rp.ShowCA_Cumulative,
                                     rp.MultipleLegend,
                                     rp.SchoolId,
                                     rp.CampusId,
                                     rp.TermId,
                                     rp.Terms.TermName,
                                     rp.CreatedBy,
                                     rp.DateCreated,
                                     rp.LastUpdatedBy,
                                     rp.LastUpdatedDate,
                                     rp.RefFirstTermScoreCompute,
                                     rp.RefFirstTermScoreShow,
                                     rp.RefSecondTermScoreCompute,
                                     rp.RefSecondTermScoreShow,
                                     rp.ComputeOverallTotalAverage,
                                     rp.ShowComputeOverallTotalAverage,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> getAllClassesReportCardConfigurationAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                //check if all parameters supplied is Valid
                if (checkSchool == true)
                {

                    //get the report card config
                    var result = from rp in _context.ReportCardConfigurationForClasses
                                 where rp.SchoolId == schoolId
                                 orderby rp.Id descending
                                 select new
                                 {
                                     rp.Id,
                                     rp.ClassId,
                                     rp.ConfigurationId,
                                     rp.CreatedAt,
                                     rp.IsActive,
                                     rp.SchoolId,
                                     rp.TermId,
                                     rp.UpdatedAt,
                                     rp.ReportCardConfiguration.ShowDepartment,
                                     rp.ReportCardConfiguration.ShowSubject,
                                     rp.ReportCardConfiguration.ShowCAScore,
                                     rp.ReportCardConfiguration.ShowExamScore,
                                     rp.ReportCardConfiguration.ComputeCA_Cumulative,
                                     rp.ReportCardConfiguration.ShowCA_Cumulative,
                                     rp.ReportCardConfiguration.MultipleLegend,
                                     rp.ReportCardConfiguration.CampusId,
                                     rp.ReportCardConfiguration.Terms.TermName,
                                     rp.ReportCardConfiguration.CreatedBy,
                                     rp.ReportCardConfiguration.DateCreated,
                                     rp.ReportCardConfiguration.LastUpdatedBy,
                                     rp.ReportCardConfiguration.LastUpdatedDate,
                                     rp.ReportCardConfiguration.RefFirstTermScoreCompute,
                                     rp.ReportCardConfiguration.RefFirstTermScoreShow,
                                     rp.ReportCardConfiguration.RefSecondTermScoreCompute,
                                     rp.ReportCardConfiguration.RefSecondTermScoreShow,
                                     rp.ReportCardConfiguration.ComputeOverallTotalAverage,
                                     rp.ReportCardConfiguration.ShowComputeOverallTotalAverage,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> getReportCardConfigurationByIdAsync(long schoolId, long campusId, long reportCardConfigId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {

                    //get the report card config
                    var result = from rp in _context.ReportCardConfiguration
                                 where rp.Id == reportCardConfigId && rp.SchoolId == schoolId && rp.CampusId == campusId
                                 select new
                                 {
                                     rp.Id,
                                     rp.ShowDepartment,
                                     rp.ShowSubject,
                                     rp.ShowCAScore,
                                     rp.ShowExamScore,
                                     rp.ComputeCA_Cumulative,
                                     rp.ShowCA_Cumulative,
                                     rp.MultipleLegend,
                                     rp.SchoolId,
                                     rp.CampusId,
                                     rp.TermId,
                                     rp.Terms.TermName,
                                     rp.CreatedBy,
                                     rp.DateCreated,
                                     rp.LastUpdatedBy,
                                     rp.LastUpdatedDate,
                                     rp.RefFirstTermScoreCompute,
                                     rp.RefFirstTermScoreShow,
                                     rp.RefSecondTermScoreCompute,
                                     rp.RefSecondTermScoreShow,
                                     rp.ComputeOverallTotalAverage,
                                     rp.ShowComputeOverallTotalAverage,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> getReportCardClassConfigurationByIdAsync(long reportCardClassConfigId)
        {
            try
            {
                    //get the report card config
                    var result = from rp in _context.ReportCardConfigurationForClasses
                                 where rp.Id == reportCardClassConfigId
                                 select new
                                 {
                                     rp.Id,
                                     rp.ClassId,
                                     rp.ConfigurationId,
                                     rp.CreatedAt,
                                     rp.IsActive,
                                     rp.SchoolId,
                                     rp.TermId,
                                     rp.UpdatedAt,
                                     rp.ReportCardConfiguration.ShowDepartment,
                                     rp.ReportCardConfiguration.ShowSubject,
                                     rp.ReportCardConfiguration.ShowCAScore,
                                     rp.ReportCardConfiguration.ShowExamScore,
                                     rp.ReportCardConfiguration.ComputeCA_Cumulative,
                                     rp.ReportCardConfiguration.ShowCA_Cumulative,
                                     rp.ReportCardConfiguration.MultipleLegend,
                                     rp.ReportCardConfiguration.CampusId,
                                     rp.ReportCardConfiguration.Terms.TermName,
                                     rp.ReportCardConfiguration.CreatedBy,
                                     rp.ReportCardConfiguration.DateCreated,
                                     rp.ReportCardConfiguration.LastUpdatedBy,
                                     rp.ReportCardConfiguration.LastUpdatedDate,
                                     rp.ReportCardConfiguration.RefFirstTermScoreCompute,
                                     rp.ReportCardConfiguration.RefFirstTermScoreShow,
                                     rp.ReportCardConfiguration.RefSecondTermScoreCompute,
                                     rp.ReportCardConfiguration.RefSecondTermScoreShow,
                                     rp.ReportCardConfiguration.ComputeOverallTotalAverage,
                                     rp.ReportCardConfiguration.ShowComputeOverallTotalAverage,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Successful, No Data Available" };

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

        public async Task<GenericResponseModel> getReportCardConfigurationByTermIdAsync(long schoolId, long campusId, long termId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {

                    //get the report card config
                    var result = from rp in _context.ReportCardConfiguration
                                 where rp.TermId == termId && rp.SchoolId == schoolId && rp.CampusId == campusId
                                 select new
                                 {
                                     rp.Id,
                                     rp.ShowDepartment,
                                     rp.ShowSubject,
                                     rp.ShowCAScore,
                                     rp.ShowExamScore,
                                     rp.ComputeCA_Cumulative,
                                     rp.ShowCA_Cumulative,
                                     rp.MultipleLegend,
                                     rp.SchoolId,
                                     rp.CampusId,
                                     rp.TermId,
                                     rp.Terms.TermName,
                                     rp.CreatedBy,
                                     rp.DateCreated,
                                     rp.LastUpdatedBy,
                                     rp.LastUpdatedDate,
                                     rp.RefFirstTermScoreCompute,
                                     rp.RefFirstTermScoreShow,
                                     rp.RefSecondTermScoreCompute,
                                     rp.RefSecondTermScoreShow,
                                     rp.ComputeOverallTotalAverage,
                                     rp.ShowComputeOverallTotalAverage,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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


        public async Task<GenericResponseModel> updateReportCardConfigurationAsync(ReportCardConfigRequestModel obj, long reportCardConfigId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkTerm = check.checkTermById(obj.TermId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkTerm == true)
                {
                    //check if the reportCardConfig Exists
                    ReportCardConfiguration rptConfig = _context.ReportCardConfiguration.Where(x => x.Id == reportCardConfigId).FirstOrDefault();

                    if (rptConfig != null)
                    {
                        //if config is for first or second term
                        if (obj.TermId == (int)EnumUtility.Terms.FirstTerm || obj.TermId == (int)EnumUtility.Terms.SecondTerm)
                        {
                            rptConfig.ShowDepartment = obj.ShowDepartment;
                            rptConfig.ShowSubject = obj.ShowSubject;
                            rptConfig.ShowCAScore = obj.ShowCAScore;
                            rptConfig.ShowExamScore = obj.ShowExamScore;
                            rptConfig.ComputeCA_Cumulative = obj.ComputeCA_Cumulative;
                            rptConfig.ShowCA_Cumulative = obj.ShowCA_Cumulative;
                            rptConfig.MultipleLegend = obj.MultipleLegend;
                            rptConfig.SchoolId = obj.SchoolId;
                            rptConfig.CampusId = obj.CampusId;
                            rptConfig.TermId = obj.TermId;
                            rptConfig.LastUpdatedBy = obj.CreatedOrUpdatedBy;
                            rptConfig.LastUpdatedDate = DateTime.Now;
                            rptConfig.RefFirstTermScoreCompute = false;
                            rptConfig.RefFirstTermScoreShow = false;
                            rptConfig.RefSecondTermScoreCompute = false;
                            rptConfig.RefSecondTermScoreShow = false;
                            rptConfig.ComputeOverallTotalAverage = false;
                            rptConfig.ShowComputeOverallTotalAverage = false;

                            await _context.SaveChangesAsync();

                            //get the report card config
                            var result = from rp in _context.ReportCardConfiguration
                                         where rp.Id == rptConfig.Id
                                         select new
                                         {
                                             rp.Id,
                                             rp.ShowDepartment,
                                             rp.ShowSubject,
                                             rp.ShowCAScore,
                                             rp.ShowExamScore,
                                             rp.ComputeCA_Cumulative,
                                             rp.ShowCA_Cumulative,
                                             rp.MultipleLegend,
                                             rp.SchoolId,
                                             rp.CampusId,
                                             rp.TermId,
                                             rp.Terms.TermName,
                                             rp.CreatedBy,
                                             rp.DateCreated,
                                             rp.LastUpdatedBy,
                                             rp.LastUpdatedDate,
                                             rp.RefFirstTermScoreCompute,
                                             rp.RefFirstTermScoreShow,
                                             rp.RefSecondTermScoreCompute,
                                             rp.RefSecondTermScoreShow,
                                             rp.ComputeOverallTotalAverage,
                                             rp.ShowComputeOverallTotalAverage,
                                         };

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Configuration Created Successfully", Data = result.FirstOrDefault() };

                        }

                        //if config is for third term
                        if (obj.TermId == (int)EnumUtility.Terms.ThirdTerm)
                        {
                            rptConfig.ShowDepartment = obj.ShowDepartment;
                            rptConfig.ShowSubject = obj.ShowSubject;
                            rptConfig.ShowCAScore = obj.ShowCAScore;
                            rptConfig.ShowExamScore = obj.ShowExamScore;
                            rptConfig.ComputeCA_Cumulative = obj.ComputeCA_Cumulative;
                            rptConfig.ShowCA_Cumulative = obj.ShowCA_Cumulative;
                            rptConfig.MultipleLegend = obj.MultipleLegend;
                            rptConfig.SchoolId = obj.SchoolId;
                            rptConfig.CampusId = obj.CampusId;
                            rptConfig.TermId = obj.TermId;
                            rptConfig.LastUpdatedBy = obj.CreatedOrUpdatedBy;
                            rptConfig.LastUpdatedDate = DateTime.Now;
                            rptConfig.RefFirstTermScoreCompute = obj.RefFirstTermScoreCompute;
                            rptConfig.RefFirstTermScoreShow = obj.RefFirstTermScoreShow;
                            rptConfig.RefSecondTermScoreCompute = obj.RefSecondTermScoreCompute;
                            rptConfig.RefSecondTermScoreShow = obj.RefSecondTermScoreShow;
                            rptConfig.ComputeOverallTotalAverage = obj.ComputeOverallTotalAverage;
                            rptConfig.ShowComputeOverallTotalAverage = obj.ShowComputeOverallTotalAverage;

                            await _context.SaveChangesAsync();

                            //get the report card config
                            var result = from rp in _context.ReportCardConfiguration
                                         where rp.Id == rptConfig.Id
                                         select new
                                         {
                                             rp.Id,
                                             rp.ShowDepartment,
                                             rp.ShowSubject,
                                             rp.ShowCAScore,
                                             rp.ShowExamScore,
                                             rp.ComputeCA_Cumulative,
                                             rp.ShowCA_Cumulative,
                                             rp.MultipleLegend,
                                             rp.SchoolId,
                                             rp.CampusId,
                                             rp.TermId,
                                             rp.Terms.TermName,
                                             rp.CreatedBy,
                                             rp.DateCreated,
                                             rp.LastUpdatedBy,
                                             rp.LastUpdatedDate,
                                             rp.RefFirstTermScoreCompute,
                                             rp.RefFirstTermScoreShow,
                                             rp.RefSecondTermScoreCompute,
                                             rp.RefSecondTermScoreShow,
                                             rp.ComputeOverallTotalAverage,
                                             rp.ShowComputeOverallTotalAverage,
                                         };

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Configuration Updated Successfully", Data = result.FirstOrDefault() };
                        }
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Report Card Configuration With the Specified ID" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };
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

        public async Task<GenericResponseModel> deleteReportCardConfigurationAsync(long reportCardConfigId)
        {
            try
            {
                //check if the reportCardConfig Exists
                ReportCardConfiguration rptConfig = _context.ReportCardConfiguration.Where(x => x.Id == reportCardConfigId).FirstOrDefault();

                if (rptConfig != null)
                {
                    var classConfig = await _context.ReportCardConfigurationForClasses.Where(x => x.ConfigurationId == rptConfig.Id).ToListAsync();
                    foreach(var config in classConfig)
                    {
                        _context.ReportCardConfigurationForClasses.Remove(config);
                        await _context.SaveChangesAsync();
                    }
                    _context.ReportCardConfiguration.Remove(rptConfig);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Succesffully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Report Card Configuration With the Specified ID" };

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

        public async Task<GenericResponseModel> deleteReportCardConfigurationForClassAsync(long reportCardClassConfigId)
        {
            try
            {
                //check if the reportCardConfig Exists
                ReportCardConfigurationForClass rptConfig = _context.ReportCardConfigurationForClasses.Where(x => x.Id == reportCardClassConfigId).FirstOrDefault();

                if (rptConfig != null)
                {
                    _context.ReportCardConfigurationForClasses.Remove(rptConfig);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Succesffully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Report Card Class Configuration With the Specified ID" };

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

        public async Task<GenericResponseModel> changeConfigurationStatusForClassAsync(long reportCardClassConfigId)
        {
            try
            {
                //check if the reportCardConfig Exists
                ReportCardConfigurationForClass rptConfig = _context.ReportCardConfigurationForClasses.Where(x => x.Id == reportCardClassConfigId).FirstOrDefault();

                if (rptConfig != null)
                {
                    if (rptConfig.IsActive == true)
                    {
                        rptConfig.IsActive = false;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        //set others to false before setting this to true
                        var classConfigs = await _context.ReportCardConfigurationForClasses.Where(x => x.SchoolId == rptConfig.SchoolId && x.TermId == rptConfig.TermId && x.ClassId == rptConfig.ClassId && x.IsActive == true).ToListAsync();
                        foreach (var classConfig in classConfigs)
                        {
                            var classConf = await _context.ReportCardConfigurationForClasses.Where(x => x.Id == classConfig.Id).FirstOrDefaultAsync();
                            classConf.IsActive = false;
                            await _context.SaveChangesAsync();
                        }
                        rptConfig.IsActive = true;
                        await _context.SaveChangesAsync();
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Status Changed Succesffully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Report Card Class Configuration With the Specified ID" };

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

        //----------------------------------------------------REPORT CARD CONFIGURATION (LEGEND)------------------------------------------------------------------------------------------------------

        public async Task<GenericResponseModel> createReportCardConfigurationLegendAsync(ReportCardConfigurationLegendRequestModel obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkTerm = check.checkTermById(obj.TermId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkTerm == true)
                {
                    //check if the legend exists
                    ReportCardConfigurationLegend checkLgnd = _context.ReportCardConfigurationLegend.Where(x => x.LegendName == obj.LegendName && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.TermId == obj.TermId).FirstOrDefault();

                    if (checkLgnd == null)
                    {
                        //creates the legend
                        ReportCardConfigurationLegend rptLgnd = new ReportCardConfigurationLegend()
                        {
                            LegendName = obj.LegendName,
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            TermId = obj.TermId,
                            CreatedBy = obj.CreatedOrUpdatedBy,
                            DateCreated = DateTime.Now,
                            LastUpdatedBy = obj.CreatedOrUpdatedBy,
                            LastUpdatedDate = DateTime.Now,
                            StatusId = (int)EnumUtility.ActiveInActive.Active,
                        };

                        await _context.ReportCardConfigurationLegend.AddAsync(rptLgnd);
                        await _context.SaveChangesAsync();

                        //save each legend
                        foreach (LegendList lgd in obj.LegendList)
                        {
                            ReportCardConfigurationLegendList lgdList = new ReportCardConfigurationLegendList()
                            {
                                LegendId = rptLgnd.Id,
                                ReferenceRange = lgd.ReferenceRange,
                                ReferenceValue = lgd.ReferenceValue,
                            };

                            await _context.ReportCardConfigurationLegendList.AddAsync(lgdList);
                            await _context.SaveChangesAsync();
                        }

                        //get the legend and the legend list
                        var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                     .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                     where rp.Id == rptLgnd.Id
                                     select new
                                     {
                                         rp.Id,
                                         rp.LegendName,
                                         rp.SchoolId,
                                         rp.CampusId,
                                         rp.TermId,
                                         rp.Terms.TermName,
                                         rp.CreatedBy,
                                         rp.DateCreated,
                                         rp.LastUpdatedBy,
                                         rp.LastUpdatedDate,
                                         rp.StatusId,
                                         rp.ActiveInActiveStatus.StatusName,
                                         rp.ReportCardConfigurationLegendList,
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Created Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Legend Already Exists" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> getAllReportCardConfigurationLegendAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //get the legend and the legendList
                    var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                    .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                 where rp.SchoolId == schoolId && rp.CampusId == campusId
                                 select new
                                 {
                                     rp.Id,
                                     rp.LegendName,
                                     rp.SchoolId,
                                     rp.CampusId,
                                     rp.TermId,
                                     rp.Terms.TermName,
                                     rp.CreatedBy,
                                     rp.DateCreated,
                                     rp.LastUpdatedBy,
                                     rp.LastUpdatedDate,
                                     rp.StatusId,
                                     rp.ActiveInActiveStatus.StatusName,
                                     rp.ReportCardConfigurationLegendList,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> getReportCardConfigurationLegendByIdAsync(long schoolId, long campusId, long reportCardConfigLegendId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //get the legend and the legendList
                    var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                    .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                 where rp.Id == reportCardConfigLegendId && rp.SchoolId == schoolId && rp.CampusId == campusId
                                 select new
                                 {
                                     rp.Id,
                                     rp.LegendName,
                                     rp.SchoolId,
                                     rp.CampusId,
                                     rp.TermId,
                                     rp.Terms.TermName,
                                     rp.CreatedBy,
                                     rp.DateCreated,
                                     rp.LastUpdatedBy,
                                     rp.LastUpdatedDate,
                                     rp.StatusId,
                                     rp.ActiveInActiveStatus.StatusName,
                                     rp.ReportCardConfigurationLegendList,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> getReportCardConfigurationLegendByTermIdAsync(long schoolId, long campusId, long termId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);
                var checkTerm = check.checkTermById(termId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkTerm == true)
                {
                    //get the legend and the legendList
                    var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                    .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                 where rp.TermId == termId && rp.SchoolId == schoolId && rp.CampusId == campusId
                                 select new
                                 {
                                     rp.Id,
                                     rp.LegendName,
                                     rp.SchoolId,
                                     rp.CampusId,
                                     rp.TermId,
                                     rp.Terms.TermName,
                                     rp.CreatedBy,
                                     rp.DateCreated,
                                     rp.LastUpdatedBy,
                                     rp.LastUpdatedDate,
                                     rp.StatusId,
                                     rp.ActiveInActiveStatus.StatusName,
                                     rp.ReportCardConfigurationLegendList,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Data Available" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> updateReportCardConfigurationLegendAsync(long reportCardConfigLegendId, UpdateLegendRequestModel obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkTerm = check.checkTermById(obj.TermId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true && checkTerm == true)
                {
                    //check if the legend exists
                    ReportCardConfigurationLegend legend = _context.ReportCardConfigurationLegend.Where(x => x.Id == reportCardConfigLegendId).FirstOrDefault();

                    if (legend != null)
                    {
                        //updates the legend
                        legend.LegendName = obj.LegendName;
                        legend.SchoolId = obj.SchoolId;
                        legend.CampusId = obj.CampusId;
                        legend.TermId = obj.TermId;
                        legend.CreatedBy = obj.CreatedOrUpdatedBy;
                        legend.DateCreated = DateTime.Now;
                        legend.LastUpdatedBy = obj.CreatedOrUpdatedBy;
                        legend.LastUpdatedDate = DateTime.Now;
                        legend.StatusId = (int)EnumUtility.ActiveInActive.Active;

                        await _context.SaveChangesAsync();

                        //get the legend and the legend list
                        var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                     .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                     where rp.Id == legend.Id
                                     select new
                                     {
                                         rp.Id,
                                         rp.LegendName,
                                         rp.SchoolId,
                                         rp.CampusId,
                                         rp.TermId,
                                         rp.Terms.TermName,
                                         rp.CreatedBy,
                                         rp.DateCreated,
                                         rp.LastUpdatedBy,
                                         rp.LastUpdatedDate,
                                         rp.StatusId,
                                         rp.ActiveInActiveStatus.StatusName,
                                         rp.ReportCardConfigurationLegendList,
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Legend With the Specified ID" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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


        public async Task<GenericResponseModel> updateReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId, long schoolId, long campusId, LegendList obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //check if the legend exists
                    ReportCardConfigurationLegend legend = _context.ReportCardConfigurationLegend.Where(x => x.Id == reportCardConfigLegendId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();

                    if (legend != null)
                    {
                        // check if the legendList exists
                        ReportCardConfigurationLegendList legendList = _context.ReportCardConfigurationLegendList.Where(x => x.Id == legendListId && x.LegendId == legend.Id).FirstOrDefault();

                        if (legendList != null)
                        {
                            legendList.LegendId = legend.Id;
                            legendList.ReferenceRange = obj.ReferenceRange;
                            legendList.ReferenceValue = obj.ReferenceValue;

                            await _context.SaveChangesAsync();


                            //get the legend and the legend list
                            var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                         .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                         where rp.Id == legend.Id
                                         select new
                                         {
                                             rp.Id,
                                             rp.LegendName,
                                             rp.SchoolId,
                                             rp.CampusId,
                                             rp.TermId,
                                             rp.Terms.TermName,
                                             rp.CreatedBy,
                                             rp.DateCreated,
                                             rp.LastUpdatedBy,
                                             rp.LastUpdatedDate,
                                             rp.StatusId,
                                             rp.ActiveInActiveStatus.StatusName,
                                             rp.ReportCardConfigurationLegendList,
                                         };

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully", Data = result.FirstOrDefault() };
                        }

                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Legend List With the Specified ID" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Legend With the Specified ID" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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

        public async Task<GenericResponseModel> deleteReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId)
        {
            try
            {
                //check if the legend exists
                ReportCardConfigurationLegend legend = _context.ReportCardConfigurationLegend.Where(x => x.Id == reportCardConfigLegendId).FirstOrDefault();

                if (legend != null)
                {
                    // check if the legendList exists
                    ReportCardConfigurationLegendList legendList = _context.ReportCardConfigurationLegendList.Where(x => x.Id == legendListId && x.LegendId == legend.Id).FirstOrDefault();

                    if (legendList != null)
                    {
                        _context.ReportCardConfigurationLegendList.Remove(legendList);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully" };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Legend List With the Specified ID" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Legend With the Specified ID" };

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

        public async Task<GenericResponseModel> addReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long schoolId, long campusId, IList<LegendList> legendList)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                //check if all parameters supplied is Valid
                if (checkSchool == true && checkCampus == true)
                {
                    //check if the legend exists
                    ReportCardConfigurationLegend legend = _context.ReportCardConfigurationLegend.Where(x => x.Id == reportCardConfigLegendId).FirstOrDefault();

                    if (legend != null)
                    {
                        //save each legend
                        foreach (LegendList lgd in legendList)
                        {
                            ReportCardConfigurationLegendList lgdList = new ReportCardConfigurationLegendList()
                            {
                                LegendId = legend.Id,
                                ReferenceRange = lgd.ReferenceRange,
                                ReferenceValue = lgd.ReferenceValue,
                            };

                            await _context.ReportCardConfigurationLegendList.AddAsync(lgdList);
                            await _context.SaveChangesAsync();
                        }

                        //get the legend and the legend list
                        var result = from rp in _context.ReportCardConfigurationLegend.AsNoTracking()
                                     .Include(l => l.ReportCardConfigurationLegendList).AsNoTracking()
                                     where rp.Id == legend.Id
                                     select new
                                     {
                                         rp.Id,
                                         rp.LegendName,
                                         rp.SchoolId,
                                         rp.CampusId,
                                         rp.TermId,
                                         rp.Terms.TermName,
                                         rp.CreatedBy,
                                         rp.DateCreated,
                                         rp.LastUpdatedBy,
                                         rp.LastUpdatedDate,
                                         rp.StatusId,
                                         rp.ActiveInActiveStatus.StatusName,
                                         rp.ReportCardConfigurationLegendList,
                                     };

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Updated Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Legend With the Specified ID" };

                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Some Parameters you entered are invalid!" };

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


        public async Task<GenericResponseModel> deleteReportCardConfigurationLegendAsync(long reportCardConfigLegendId)
        {
            try
            {
                //check if the ReportCardConfigurationLegend Exists
                ReportCardConfigurationLegend rptLegend = _context.ReportCardConfigurationLegend.Where(x => x.Id == reportCardConfigLegendId).FirstOrDefault();

                if (rptLegend != null)
                {
                    //check if the ReportCardConfigurationLegend Exists
                    IList<ReportCardConfigurationLegendList> rptLegendList = (_context.ReportCardConfigurationLegendList.Where(x => x.LegendId == rptLegend.Id).ToList());

                    //deletes the legend lists
                    _context.ReportCardConfigurationLegendList.RemoveRange(rptLegendList);

                    //deletes the legend
                    _context.ReportCardConfigurationLegend.Remove(rptLegend);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Succesffully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Report Card Legend With the Specified ID" };
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
