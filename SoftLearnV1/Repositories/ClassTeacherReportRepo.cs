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
    public class ClassTeacherReportRepo : IClassTeacherReportRepo
    {
        private readonly AppDbContext _context;

        public ClassTeacherReportRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------Performance---------------------------------------------------------------
        
        public async Task<GenericResponseModel> getTestPerformanceBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            IList<TopStudentsBySubjectResponseModel> students = new List<TopStudentsBySubjectResponseModel>();
            try
            {
                decimal totalCA = 0;
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
                    //Get all the CA scores for the student
                    var datas = await _context.ContinousAssessmentScores.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.StudentId == student.StudentId && x.SubjectId == subjectId).ToListAsync();
                    if (datas.Count() > 0)
                    {
                        foreach (var data in datas)
                        {
                            totalCA += data.MarkObtained;
                        }
                        students.Add(new TopStudentsBySubjectResponseModel { AdmissionNumber = StudentData.AdmissionNumber, subjectId = subjectId, subjectName = checkSubject.SubjectName, studentId = student.StudentId, studentName = StudentData.FirstName + " " + StudentData.LastName, studentScore = totalCA });
                        totalCA = 0;
                    }
                }
                if (students.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found" };
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = students };

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
        public async Task<GenericResponseModel> getExamPerformanceBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            IList<TopStudentsBySubjectResponseModel> students = new List<TopStudentsBySubjectResponseModel>();
            try
            {
                decimal totalExam = 0;
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
                    //Get all the Exam scores for the student
                    var datas = await _context.ExaminationScores.Where(x => x.ClassId == classId && x.SessionId == sessionId && x.TermId == termId && x.StudentId == student.StudentId && x.SubjectId == subjectId).ToListAsync();
                    if (datas.Count() > 0)
                    {
                        foreach (var data in datas)
                        {
                            totalExam += data.MarkObtained;
                        }
                        students.Add(new TopStudentsBySubjectResponseModel { AdmissionNumber = StudentData.AdmissionNumber, subjectId = subjectId, subjectName = checkSubject.SubjectName, studentId = student.StudentId, studentName = StudentData.FirstName + " " + StudentData.LastName, studentScore = totalExam });
                        totalExam = 0;
                    }
                }
                if (students.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 404, StatusMessage = "No record found" };
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = students };

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
