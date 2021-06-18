using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class ParentReportRepo : IParentReportRepo
    {
        private readonly AppDbContext _context;

        public ParentReportRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------ExamPerformance---------------------------------------------------------------
        public async Task<GenericResponseModel> getExamPerformanceByTermAsync(Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(termId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Term Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                var result = from cr in _context.ExaminationScores
                             where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                             && cr.SessionId == sessionId && cr.TermId == termId && cr.StudentId == childId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId,
                                 cr.DateUpdated,
                                 cr.DateUploaded,
                                 cr.MarkObtainable,
                                 cr.MarkObtained,
                                 cr.SchoolSubjects.SubjectName,
                                 cr.SubjectId,
                                 cr.TeacherId,
                                 cr.SessionId,
                                 cr.TermId
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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

        //----------------------------TestPerformance---------------------------------------------------------------
        public async Task<GenericResponseModel> getTestPerformanceByTermAsync(Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(termId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Term Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                var result = from cr in _context.ContinousAssessmentScores
                             where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                             && cr.SessionId == sessionId && cr.TermId == termId && cr.StudentId == childId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId,
                                 cr.DateUpdated,
                                 cr.DateUploaded,
                                 cr.MarkObtainable,
                                 cr.MarkObtained,
                                 cr.SchoolSubjects.SubjectName,
                                 cr.SubjectId,
                                 cr.TeacherId,
                                 cr.SessionId,
                                 cr.TermId
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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
        //----------------------------TopScore---------------------------------------------------------------
        public async Task<GenericResponseModel> getTopExamPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(termId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Term Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                var result = (from cr in _context.ExaminationScores
                             where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                             && cr.SessionId == sessionId && cr.TermId == termId && cr.StudentId == childId
                             orderby cr.MarkObtained descending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId,
                                 cr.DateUpdated,
                                 cr.DateUploaded,
                                 cr.MarkObtainable,
                                 cr.MarkObtained,
                                 cr.SchoolSubjects.SubjectName,
                                 cr.SubjectId,
                                 cr.TeacherId,
                                 cr.SessionId,
                                 cr.TermId
                             }).Take(topNumber);

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTopTestPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(termId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Term Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                //Get data
                var result = (from cr in _context.ContinousAssessmentScores
                             where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                             && cr.SessionId == sessionId && cr.TermId == termId && cr.StudentId == childId
                             orderby cr.MarkObtained descending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId,
                                 cr.DateUpdated,
                                 cr.DateUploaded,
                                 cr.MarkObtainable,
                                 cr.MarkObtained,
                                 cr.SchoolSubjects.SubjectName,
                                 cr.SubjectId,
                                 cr.TeacherId,
                                 cr.SessionId,
                                 cr.TermId
                             }).Take(topNumber);

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTopTotalPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(termId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Term Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                var result = (from cr in _context.ReportCardData
                              where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                              && cr.SessionId == sessionId && cr.TermId == termId && cr.StudentId == childId
                              orderby cr.TotalScore descending
                              select new
                              {
                                  cr.Students.AdmissionNumber,
                                  cr.Students.FirstName,
                                  cr.Students.LastName,
                                  cr.Students.MiddleName,
                                  cr.ClassId,
                                  cr.ClassGradeId,
                                  cr.StudentId,
                                  cr.TotalScore,
                                  cr.SchoolSubjects.SubjectName,
                                  cr.SubjectId,
                                  cr.SessionId,
                                  cr.TermId
                              }).Take(topNumber);

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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
        //----------------------------Trend---------------------------------------------------------------
        public async Task<GenericResponseModel> getTrendReportbySubjectAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkSubjectById(subjectId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Subject Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                var result = from cr in _context.ReportCardData
                              where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                              && cr.SessionId == sessionId && cr.SubjectId == subjectId && cr.StudentId == childId
                              orderby cr.TotalScore ascending
                              select new
                              {
                                  cr.Students.AdmissionNumber,
                                  cr.Students.FirstName,
                                  cr.Students.LastName,
                                  cr.Students.MiddleName,
                                  cr.ClassId,
                                  cr.ClassGradeId,
                                  cr.StudentId,
                                  cr.TotalScore,
                                  cr.SchoolSubjects.SubjectName,
                                  cr.SubjectId,
                                  cr.SessionId,
                                  cr.TermId
                              };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTrendReportbySubjectExamAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkSubjectById(subjectId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Subject Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                //Get data
                var result = from cr in _context.ExaminationScores
                             where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                             && cr.SessionId == sessionId && cr.SubjectId == subjectId && cr.StudentId == childId
                             orderby cr.MarkObtained descending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId,
                                 cr.DateUpdated,
                                 cr.DateUploaded,
                                 cr.MarkObtainable,
                                 cr.MarkObtained,
                                 cr.SchoolSubjects.SubjectName,
                                 cr.SubjectId,
                                 cr.TeacherId,
                                 cr.SessionId,
                                 cr.TermId
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTrendReportbySubjectTestAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(sessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkSubjectById(subjectId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 311, StatusMessage = "Subject Doesn't Exist" };
                }
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 312, StatusMessage = "School Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Class Doesn't Exist" };
                }
                var checkGrade = new CheckerValidation(_context).checkClassGradeById(gradeId);
                if (checkGrade == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Grade Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(childId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Child Doesn't Exist" };
                }
                //Get data
                var result = from cr in _context.ContinousAssessmentScores
                              where cr.SchoolId == schoolId && cr.ClassId == classId && cr.ClassGradeId == gradeId
                              && cr.SessionId == sessionId && cr.SubjectId == subjectId && cr.StudentId == childId
                              orderby cr.MarkObtained descending
                              select new
                              {
                                  cr.Students.AdmissionNumber,
                                  cr.Students.FirstName,
                                  cr.Students.LastName,
                                  cr.Students.MiddleName,
                                  cr.ClassId,
                                  cr.ClassGradeId,
                                  cr.StudentId,
                                  cr.DateUpdated,
                                  cr.DateUploaded,
                                  cr.MarkObtainable,
                                  cr.MarkObtained,
                                  cr.SchoolSubjects.SubjectName,
                                  cr.SubjectId,
                                  cr.TeacherId,
                                  cr.SessionId,
                                  cr.TermId
                              };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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
