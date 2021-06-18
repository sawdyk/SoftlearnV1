using Microsoft.EntityFrameworkCore;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class ReportDashboardRepo : IReportDashboardRepo
    {
        private readonly AppDbContext _context;

        public ReportDashboardRepo(AppDbContext context)
        {
            this._context = context;
        }

        //-------------------------Parent--------------------------------------------

        public async Task<GenericResponseModel> getNoOfCampusesForChildrenAsync(Guid parentId)
        {
            try
            {
                IList<long> campusList = new List<long>();
                var parent = await _context.Parents.Where(x => x.Id == parentId).FirstOrDefaultAsync();

                if (parent == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Parent doesn't exist" };
                }
                //Get children attached to parents
                var parentStudent = await _context.ParentsStudentsMap.Where(x => x.ParentId == parentId).ToListAsync();

                if (parentStudent.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No child is attached to this parent" };
                }
                else
                {
                    foreach(var child in parentStudent)
                    {
                        //Get child assigned to class
                        var student = await _context.GradeStudents.Where(x => x.StudentId == child.StudentId && x.HasGraduated == false).FirstOrDefaultAsync();
                        if(student != null)
                        {
                            if (!campusList.Contains(student.CampusId))
                            {
                                campusList.Add(student.CampusId);
                            }
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = campusList.Count() };

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

        public async Task<GenericResponseModel> getNoOfChildrenAsync(Guid parentId)
        {
            try
            {
                IList<object> studentList = new List<object>();
                var parent = await _context.Parents.Where(x => x.Id == parentId).FirstOrDefaultAsync();

                if (parent == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Parent doesn't exist" };
                }
                //Get children attached to parents
                var parentStudent = await _context.ParentsStudentsMap.Where(x => x.ParentId == parentId).ToListAsync();

                if (parentStudent.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No child is attached to this parent" };
                }
                else
                {
                    foreach (var child in parentStudent)
                    {
                        //Get child assigned to class
                        var student = await _context.GradeStudents.Where(x => x.StudentId == child.StudentId && x.HasGraduated == false).FirstOrDefaultAsync();
                        if (student != null)
                        {
                            studentList.Add(student);
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = studentList.Count() };

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

        public async Task<GenericResponseModel> getNoOfClassesForChildrenAsync(Guid parentId)
        {
            try
            {
                IList<long> classList = new List<long>();
                var parent = await _context.Parents.Where(x => x.Id == parentId).FirstOrDefaultAsync();

                if (parent == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Parent doesn't exist" };
                }
                //Get children attached to parents
                var parentStudent = await _context.ParentsStudentsMap.Where(x => x.ParentId == parentId).ToListAsync();

                if (parentStudent.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No child is attached to this parent" };
                }
                else
                {
                    foreach (var child in parentStudent)
                    {
                        //Get child detail
                        var student = await _context.GradeStudents.Where(x => x.StudentId == child.StudentId && x.HasGraduated == false).FirstOrDefaultAsync();
                        if (student != null)
                        {
                            if (!classList.Contains(student.ClassId))
                            {
                                classList.Add(student.ClassId);
                            }
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = classList.Count() };

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
        
        public async Task<GenericResponseModel> getTotalAmountPaidForCurrentTermAsync(Guid parentId)
        {
            try
            {
                long totalAmountPaid = 0;
                var parent = await _context.Parents.Where(x => x.Id == parentId).FirstOrDefaultAsync();

                if (parent == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Parent doesn't exist" };
                }
                //Get current term for school
                var academicSession = await _context.AcademicSessions.Where(x => x.IsCurrent == true && x.SchoolId == parent.SchoolId).FirstOrDefaultAsync();
                if (academicSession == null)
                {
                    return new GenericResponseModel { StatusCode = 402, StatusMessage = "Current term is not set" };
                }
                //Get children attached to parents
                var parentStudent = await _context.ParentsStudentsMap.Where(x => x.ParentId == parentId).ToListAsync();

                if (parentStudent.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "No child is attached to this parent" };
                }
                else
                {
                    foreach (var child in parentStudent)
                    {
                        //Get child detail
                        var student = await _context.GradeStudents.Where(x => x.StudentId == child.StudentId && x.HasGraduated == false).FirstOrDefaultAsync();
                        if (student != null)
                        {
                            //Get approved payment
                            var approvedPayment = await _context.SchoolFeesPayments.Where(x => x.StudentId == student.StudentId && x.TermId == academicSession.TermId).FirstOrDefaultAsync();
                            if (approvedPayment != null)
                            {
                                totalAmountPaid += approvedPayment.AmountPaid;
                            }
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = totalAmountPaid };

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

        //-------------------------Class Teacher--------------------------------------------

        public async Task<GenericResponseModel> getNoOfStudentsInTeacherClassAsync(Guid teacherId)
        {
            try
            {
                long totalStudent = 0;
                var teacherUser = await _context.SchoolUsers.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
                if (teacherUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Teacher doesn't exist" };
                }
                //Get all classGrade for teacher
                var gradeTeacher = await _context.GradeTeachers.Where(x => x.SchoolUserId == teacherId).ToListAsync();

                if (gradeTeacher.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Teacher hasn't been assigned to any class" };
                }
                else
                {
                    foreach (var teacher in gradeTeacher)
                    {
                        //Get child detail
                        var students = await _context.GradeStudents.Where(x => x.ClassGradeId == teacher.ClassGradeId && x.HasGraduated == false).ToListAsync();

                        totalStudent += students.Count();
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = totalStudent };

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
        public async Task<GenericResponseModel> getNoOfFemaleStudentsInTeacherClassAsync(Guid teacherId)
        {
            try
            {
                IList<object> totalFemaleStudent = new List<object>();
                var teacherUser = await _context.SchoolUsers.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
                if (teacherUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Teacher doesn't exist" };
                }
                //Get all classGrade for teacher
                var gradeTeacher = await _context.GradeTeachers.Where(x => x.SchoolUserId == teacherId).ToListAsync();

                if (gradeTeacher.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Teacher hasn't been assigned to any class" };
                }
                else
                {
                    foreach (var teacher in gradeTeacher)
                    {
                        //Get child detail
                        var students = await _context.GradeStudents.Where(x => x.ClassGradeId == teacher.ClassGradeId && x.HasGraduated == false).ToListAsync();
                        foreach(var student in students)
                        {
                            var studentRecord = await _context.Students.Where(x => x.Id == student.StudentId && x.GenderId == Convert.ToInt64(EnumUtility.Gender.Female)).FirstOrDefaultAsync();
                            if(studentRecord != null)
                            {
                                totalFemaleStudent.Add(studentRecord);
                            }
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = totalFemaleStudent.Count() };

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

        public async Task<GenericResponseModel> getNoOfMaleStudentsInTeacherClassAsync(Guid teacherId)
        {
            try
            {
                IList<object> totalFemaleStudent = new List<object>();
                var teacherUser = await _context.SchoolUsers.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
                if (teacherUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Teacher doesn't exist" };
                }
                //Get all classGrade for teacher
                var gradeTeacher = await _context.GradeTeachers.Where(x => x.SchoolUserId == teacherId).ToListAsync();

                if (gradeTeacher.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Teacher hasn't been assigned to any class" };
                }
                else
                {
                    foreach (var teacher in gradeTeacher)
                    {
                        //Get child detail
                        var students = await _context.GradeStudents.Where(x => x.ClassGradeId == teacher.ClassGradeId && x.HasGraduated == false).ToListAsync();
                        foreach (var student in students)
                        {
                            var studentRecord = await _context.Students.Where(x => x.Id == student.StudentId && x.GenderId == Convert.ToInt64(EnumUtility.Gender.Male)).FirstOrDefaultAsync();
                            if (studentRecord != null)
                            {
                                totalFemaleStudent.Add(studentRecord);
                            }
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = totalFemaleStudent.Count() };

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
        public async Task<GenericResponseModel> getNoOfSubjectsInTeacherClassAsync(Guid teacherId)
        {
            try
            {
                long totalSubject = 0;
                IList<object> subjectList = new List<object>();
                var teacherUser = await _context.SchoolUsers.Where(x => x.Id == teacherId).FirstOrDefaultAsync();
                if (teacherUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Teacher doesn't exist" };
                }
                //Get all classGrade for teacher
                var gradeTeacher = await _context.GradeTeachers.Where(x => x.SchoolUserId == teacherId).ToListAsync();

                if (gradeTeacher.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Teacher hasn't been assigned to any class" };
                }
                else
                {
                    foreach (var teacher in gradeTeacher)
                    {
                        //Get child detail
                        var subjects = await _context.SchoolSubjects.Where(x => x.ClassId == teacher.ClassId).ToListAsync();
                        foreach(var subject in subjects)
                        {
                            if (!subjectList.Contains(subject))
                            {
                                subjectList.Add(subject);
                            }
                        }

                        //totalSubject += subjects.Count();
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = subjectList.Count() };

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

        //-------------------------School Admin--------------------------------------------

        public async Task<GenericResponseModel> getNoOfNonTeachingStaffsInSchoolAsync(Guid schoolAdminId)
        {
            try
            {
                IList<object> userList = new List<object>();
                var adminUser = await _context.SchoolUsers.Where(x => x.Id == schoolAdminId).FirstOrDefaultAsync();
                if (adminUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Admin User doesn't exist" };
                }
                var checkRole = await _context.SchoolUserRoles.Where(x => x.UserId == adminUser.Id && x.RoleId == (long)EnumUtility.SchoolRoles.SuperAdministrator).FirstOrDefaultAsync();
                if (checkRole == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Only school super admin can perform this operation" };
                }
                //get all school users
                var schoolUsers = await _context.SchoolUsers.Where(x => x.SchoolId == adminUser.SchoolId).ToListAsync();
                foreach(var schoolUser in schoolUsers)
                {
                    var userRole = await _context.SchoolUserRoles.Where(x => x.UserId == schoolUser.Id && !(x.RoleId == (long)EnumUtility.SchoolRoles.ClassTeacher || x.RoleId == (long)EnumUtility.SchoolRoles.SubjectTeacher)).FirstOrDefaultAsync();
                    if (userRole != null)
                    {
                        userList.Add(userRole);
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = userList.Count() };

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

        public async Task<GenericResponseModel> getNoOfSchoolCampusesAsync(Guid schoolAdminId)
        {
            try
            {
                IList<object> userList = new List<object>();
                var adminUser = await _context.SchoolUsers.Where(x => x.Id == schoolAdminId).FirstOrDefaultAsync();
                if (adminUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Admin User doesn't exist" };
                }
                var checkRole = await _context.SchoolUserRoles.Where(x => x.UserId == adminUser.Id && x.RoleId == (long)EnumUtility.SchoolRoles.SuperAdministrator).FirstOrDefaultAsync();
                if(checkRole == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Only school super admin can perform this operation" };
                }
                var schoolCampuses = await _context.SchoolCampuses.Where(x => x.SchoolId == adminUser.SchoolId).ToListAsync();

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = schoolCampuses.Count() };

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

        public async Task<GenericResponseModel> getNoOfStudentsInSchoolAsync(Guid schoolAdminId)
        {
            try
            {
                IList<object> userList = new List<object>();
                var adminUser = await _context.SchoolUsers.Where(x => x.Id == schoolAdminId).FirstOrDefaultAsync();
                if (adminUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Admin User doesn't exist" };
                }
                var checkRole = await _context.SchoolUserRoles.Where(x => x.UserId == adminUser.Id && x.RoleId == (long)EnumUtility.SchoolRoles.SuperAdministrator).FirstOrDefaultAsync();
                if (checkRole == null)
                {
                    return new GenericResponseModel { StatusCode = 401, StatusMessage = "Only school super admin can perform this operation" };
                }
                var gradeStudents = await _context.GradeStudents.Where(x => x.SchoolId == adminUser.SchoolId && x.HasGraduated == false).ToListAsync();

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = gradeStudents.Count() };

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

        public async Task<GenericResponseModel> getNoOfTeachersInSchoolAsync(Guid schoolAdminId)
        {
            try
            {
                long totalSubject = 0;
                IList<object> userList = new List<object>();
                var adminUser = await _context.SchoolUsers.Where(x => x.Id == schoolAdminId).FirstOrDefaultAsync();
                if (adminUser == null)
                {
                    return new GenericResponseModel { StatusCode = 400, StatusMessage = "Admin User doesn't exist" };
                }
                var schoolUsers = await _context.SchoolUsers.Where(x => x.SchoolId == adminUser.SchoolId).ToListAsync();
                foreach (var schoolUser in schoolUsers)
                {
                    var userRole = await _context.SchoolUserRoles.Where(x => x.UserId == schoolUser.Id && (x.RoleId == (long)EnumUtility.SchoolRoles.ClassTeacher || x.RoleId == (long)EnumUtility.SchoolRoles.SubjectTeacher)).FirstOrDefaultAsync();
                    if (userRole != null)
                    {
                        userList.Add(userRole);
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = userList.Count() };

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
