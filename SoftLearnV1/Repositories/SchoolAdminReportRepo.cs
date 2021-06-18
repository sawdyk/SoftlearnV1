using Microsoft.EntityFrameworkCore;
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
    public class SchoolAdminReportRepo : ISchoolAdminReportRepo
    {
        private readonly AppDbContext _context;

        public SchoolAdminReportRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------Trend---------------------------------------------------------------
        public async Task<GenericResponseModel> getTrendReportByClassAsync(int sessionId, int termId, long schoolId, long campusId)
        {
            IList<TrendReportByClassResponseModel> classResponseModelsList = new List<TrendReportByClassResponseModel>();
            try
            {
                decimal totalScore = 0;
                decimal averageScore = 0;
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
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "School Campus Doesn't Exist" };
                }
                //get all the classes in the school campus
                var allClass = await _context.Classes.Where(x => x.SchoolId == schoolId && x.CampusId == campusId).ToListAsync();
                if (allClass.Count() <= 0)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Classes have not been created for this school campus" };
                }
                foreach (var clas in allClass)
                {
                    //Get all the students in the class
                    var studentsInClass = await _context.GradeStudents.Where(x => x.ClassId == clas.Id && x.SessionId == sessionId).ToListAsync();
                    if (studentsInClass.Count() <= 0)
                    {
                        classResponseModelsList.Add(new TrendReportByClassResponseModel { code = 201, message = "No student in this class for the session selected", averageScore = averageScore, classId = clas.Id, className = clas.ClassName });
                        continue;
                    }
                    //Get all the scores in the class for the selected session and term
                    var datas = await _context.ReportCardData.Where(x => x.ClassId == clas.Id && x.SessionId == sessionId && x.TermId == termId).ToListAsync();
                    if (datas.Count() <= 0)
                    {
                        classResponseModelsList.Add(new TrendReportByClassResponseModel { code = 202, message = "No score record for this class", averageScore = averageScore, classId = clas.Id, className = clas.ClassName });
                        continue;
                    }
                    foreach (var data in datas)
                    {
                        totalScore += data.TotalScore;
                    }
                    averageScore = totalScore / studentsInClass.Count();
                    classResponseModelsList.Add(new TrendReportByClassResponseModel { code = 200, message = "Success", averageScore = averageScore, classId = clas.Id, className = clas.ClassName });
                    averageScore = 0;
                    continue;
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = classResponseModelsList };

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
                classResponseModelsList.Add(new TrendReportByClassResponseModel { code = 500, message = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = classResponseModelsList };
            }
        }

        public async Task<GenericResponseModel> getTrendReportBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId)
        {
            IList<TrendReportBySubjectResponseModel> subjectResponseModelsList = new List<TrendReportBySubjectResponseModel>();
            try
            {
                decimal totalScore = 0;
                decimal averageScore = 0;
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
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "School Campus Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Doesn't Exist" };
                }
                //get all the class subjects  in the school campus
                var allClassSubject = await _context.SchoolSubjects.Where(x => x.SchoolId == schoolId && x.CampusId == campusId && x.ClassId == classId).ToListAsync();
                if (allClassSubject.Count() <= 0)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Subjects have not been created for this class" };
                }
                foreach (var subject in allClassSubject)
                {
                    //Get all the subject scores in the class for the selected session and term
                    var datas = await _context.ReportCardData.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.SubjectId == subject.Id).ToListAsync();
                    if (datas.Count() <= 0)
                    {
                        subjectResponseModelsList.Add(new TrendReportBySubjectResponseModel { code = 202, message = "No score record for this subject", averageScore = averageScore, subjectId = subject.Id, subjectName = subject.SubjectName });
                        continue;
                    }
                    foreach (var data in datas)
                    {
                        totalScore += data.TotalScore;
                    }
                    averageScore = totalScore / datas.Count();
                    subjectResponseModelsList.Add(new TrendReportBySubjectResponseModel { code = 200, message = "Success", averageScore = averageScore, subjectId = subject.Id, subjectName = subject.SubjectName });
                    averageScore = 0;
                    continue;
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = subjectResponseModelsList };

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
                subjectResponseModelsList.Add(new TrendReportBySubjectResponseModel { code = 500, message = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = subjectResponseModelsList };
            }
        }
        //----------------------------Top Student---------------------------------------------------------------
        public async Task<GenericResponseModel> getTopStudentsByClassAsync(int topNumber, int sessionId, int termId, long schoolId, long campusId, long classId)
        {
            IList<TopStudentsByClassResponseModel> topStudents = new List<TopStudentsByClassResponseModel>();
            try
            {
                if(topNumber < 1)
                {
                    return new GenericResponseModel { StatusCode = 316, StatusMessage = "Number can't be lesser than 1" };
                }
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
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "School Campus Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Doesn't Exist" };
                }
                //Get all the students in the class for the session selected
                var studentsInClass = await _context.GradeStudents.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.SchoolId == schoolId && x.CampusId == campusId).ToListAsync();
                if (studentsInClass.Count() <= 0)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "No student in this class for the session selected" };
                }
                foreach (GradeStudents student in studentsInClass)
                {
                    //Get student details
                    Students StudentData = await _context.Students.Where(x => x.Id == student.StudentId).FirstOrDefaultAsync();
                    //Get all the scores for the student
                    var datas = await _context.ReportCardPosition.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.StudentId == student.StudentId).FirstOrDefaultAsync();
                    if (datas != null)
                    {
                        topStudents.Add(new TopStudentsByClassResponseModel { AdmissionNumber = StudentData.AdmissionNumber, classId = student.ClassId, className = student.Classes.ClassName, studentId = student.StudentId, studentName = StudentData.FirstName + " " + StudentData.LastName, studentScore = datas.TotalScore });
                    }
                }
                if(topStudents.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found" };
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = topStudents.OrderByDescending(x => x.studentScore).Take(topNumber) };

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

        public async Task<GenericResponseModel> getTopStudentsBySubjectAsync(int topNumber, int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            IList<TopStudentsBySubjectResponseModel> topStudents = new List<TopStudentsBySubjectResponseModel>();
            try
            {
                if (topNumber < 1)
                {
                    return new GenericResponseModel { StatusCode = 317, StatusMessage = "Number can't be lesser than 1" };
                }
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
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "School Campus Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Doesn't Exist" };
                }
                var checkSubject = await _context.SchoolSubjects.Where(x => x.Id == subjectId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefaultAsync();
                if (checkSubject == null)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Subject Doesn't Exist for the School Campus" };
                }
                //Get all the students in the class for the session selected
                var studentsInClass = await _context.GradeStudents.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.SchoolId == schoolId && x.CampusId == campusId).ToListAsync();
                if (studentsInClass.Count() <= 0)
                {
                    return new GenericResponseModel { StatusCode = 316, StatusMessage = "No student in this class for the session selected" };
                }
                foreach (GradeStudents student in studentsInClass)
                {
                    //Get student details
                    Students StudentData = await _context.Students.Where(x => x.Id == student.StudentId).FirstOrDefaultAsync();
                    //Get all the scores for the student
                    var datas = await _context.ReportCardData.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.StudentId == student.StudentId && x.SubjectId == subjectId).FirstOrDefaultAsync();
                    if (datas != null)
                    {
                        topStudents.Add(new TopStudentsBySubjectResponseModel { AdmissionNumber = StudentData.AdmissionNumber, subjectId = datas.SubjectId, subjectName = checkSubject.SubjectName, studentId = student.StudentId, studentName = StudentData.FirstName + " " + StudentData.LastName, studentScore = datas.TotalScore });
                    }
                }
                if (topStudents.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found" };
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = topStudents.OrderByDescending(x => x.studentScore).Take(topNumber) };

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
        //----------------------------Low Student---------------------------------------------------------------
        public async Task<GenericResponseModel> getLowStudentsByClassAsync(int lowNumber, int sessionId, int termId, long schoolId, long campusId, long classId)
        {
            IList<TopStudentsByClassResponseModel> lowStudents = new List<TopStudentsByClassResponseModel>();
            try
            {
                if (lowNumber < 1)
                {
                    return new GenericResponseModel { StatusCode = 316, StatusMessage = "Number can't be lesser than 1" };
                }
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
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "School Campus Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Doesn't Exist" };
                }
                //Get all the students in the class for the session selected
                var studentsInClass = await _context.GradeStudents.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.SchoolId == schoolId && x.CampusId == campusId).ToListAsync();
                if (studentsInClass.Count() <= 0)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "No student in this class for the session selected" };
                }
                foreach (GradeStudents student in studentsInClass)
                {
                    //Get student details
                    Students StudentData = await _context.Students.Where(x => x.Id == student.StudentId).FirstOrDefaultAsync();
                    //Get all the scores for the student
                    var datas = await _context.ReportCardPosition.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.StudentId == student.StudentId).FirstOrDefaultAsync();
                    if (datas != null)
                    {
                        lowStudents.Add(new TopStudentsByClassResponseModel { AdmissionNumber = StudentData.AdmissionNumber, classId = student.ClassId, className = student.Classes.ClassName, studentId = student.StudentId, studentName = StudentData.FirstName + " " + StudentData.LastName, studentScore = datas.TotalScore });
                    }
                }
                if (lowStudents.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found" };
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = lowStudents.OrderBy(x => x.studentScore).Take(lowNumber) };

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

        public async Task<GenericResponseModel> getLowStudentsBySubjectAsync(int lowNumber, int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            IList<TopStudentsBySubjectResponseModel> lowStudents = new List<TopStudentsBySubjectResponseModel>();
            try
            {
                if (lowNumber < 1)
                {
                    return new GenericResponseModel { StatusCode = 317, StatusMessage = "Number can't be lesser than 1" };
                }
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
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(campusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "School Campus Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(classId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 314, StatusMessage = "Class Doesn't Exist" };
                }
                var checkSubject = await _context.SchoolSubjects.Where(x => x.Id == subjectId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefaultAsync();
                if (checkSubject == null)
                {
                    return new GenericResponseModel { StatusCode = 315, StatusMessage = "Subject Doesn't Exist for the School Campus" };
                }
                //Get all the students in the class for the session selected
                var studentsInClass = await _context.GradeStudents.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.SchoolId == schoolId && x.CampusId == campusId).ToListAsync();
                if (studentsInClass.Count() <= 0)
                {
                    return new GenericResponseModel { StatusCode = 316, StatusMessage = "No student in this class for the session selected" };
                }
                foreach (GradeStudents student in studentsInClass)
                {
                    //Get student details
                    Students StudentData = await _context.Students.Where(x => x.Id == student.StudentId).FirstOrDefaultAsync();
                    //Get all the scores for the student
                    var datas = await _context.ReportCardData.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.StudentId == student.StudentId && x.SubjectId == subjectId).FirstOrDefaultAsync();
                    if (datas != null)
                    {
                        lowStudents.Add(new TopStudentsBySubjectResponseModel { AdmissionNumber = StudentData.AdmissionNumber, subjectId = datas.SubjectId, subjectName = checkSubject.SubjectName, studentId = student.StudentId, studentName = StudentData.FirstName + " " + StudentData.LastName, studentScore = datas.TotalScore });
                    }
                }
                if (lowStudents.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found" };
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = lowStudents.OrderBy(x => x.studentScore).Take(lowNumber) };

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

