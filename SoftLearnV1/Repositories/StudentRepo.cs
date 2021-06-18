using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.SchoolReusables;
using SoftLearnV1.Security;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SoftLearnV1.Services.Email;

namespace SoftLearnV1.Repositories
{
    public class StudentRepo : IStudentRepo
    {
        private readonly AppDbContext _context;
        private IConfiguration _config;
        private IHostingEnvironment env;
        private readonly ServerPath _serverPath;
        private readonly EmailTemplate _emailTemplate;
        private readonly IEmailRepo _emailRepo;

        public StudentRepo(AppDbContext context, IConfiguration config, IHostingEnvironment env, ServerPath serverPath, EmailTemplate emailTemplate, IEmailRepo emailRepo)
        {
            _context = context;
            _config = config;
            this.env = env;
            _serverPath = serverPath;
            this._emailTemplate = emailTemplate;
            this._emailRepo = emailRepo;
        }

        //student and parent default password
        private static string defaultPassword()
        {
            return "Password";
        }

        public async Task<GenericResponseModel> createStudentAsync(StudentCreationRequestModel obj)
        {
            try
            {
                //generate the code for email confirmation
                var confirmationCode1 = new RandomNumberGenerator();
                string passwordGeneratedParent = confirmationCode1.randomCodesGen();

                var confirmationCode2 = new RandomNumberGenerator();
                string passwordGeneratedStudent = confirmationCode2.randomCodesGen();
                //check if the parent email exist
                CheckerValidation check = new CheckerValidation(_context);
                var emailCheckResult = check.checkIfEmailExist(obj.ParentEmail, Convert.ToInt64(EnumUtility.UserCategoty.Parents));

                //check for exsting students in the school
                var getStudentIfExist = _context.Students.Where(x => x.LastName == obj.StudentLastName
                                        && x.FirstName == obj.StudentFirstName
                                        && x.MiddleName == obj.MiddleName
                                        && x.SchoolId == obj.SchoolId
                                        && x.CampusId == obj.CampusId).FirstOrDefault();

                if (emailCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Parent Email Address already exist, Kindly add the student to the exsting parent details!" };
                }
                if (getStudentIfExist != null)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = $"Student With this FullName: {obj.StudentLastName + " " + obj.StudentFirstName + " " + obj.MiddleName} Already exists, Kindly Update the Student Class and ClassGrade"};
                }
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHashParent = paswordHasher.hashedPassword(passwordGeneratedParent, salt);
                    string passwordHashStudent = paswordHasher.hashedPassword(passwordGeneratedStudent, salt);

                    //student AdmissionNumber/Username
                    string admissionNumber = new AdmissionNumberGenerator(_context).GenerateAdmissionNumber(obj.SchoolId);

                    //Save the Student details
                    var std = new Students
                    {
                        LastName = obj.StudentLastName,
                        FirstName = obj.StudentFirstName,
                        MiddleName = obj.MiddleName,
                        UserName = admissionNumber,
                        AdmissionNumber = admissionNumber,
                        Salt = salt,
                        PasswordHash = passwordHashStudent,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        GenderId = obj.GenderId,
                        Email = obj.ParentEmail,
                        EmailConfirmed = false,
                        StaffStatus = 0,
                        DateOfBirth = Convert.ToDateTime(obj.DateOfBirth),
                        YearOfAdmission = obj.YearOfAdmission,
                        StateOfOrigin = obj.StateOfOrigin,
                        LocalGovt = obj.LocalGovt,
                        Religion = obj.Religion,
                        HomeAddress = obj.HomeAddress,
                        City = obj.City,
                        State = obj.State,
                        StudentStatus = "",
                        ProfilePictureUrl = "",
                        Status = "",
                        IsAssignedToClass = false,
                        hasParent = true,
                        IsActive = true,
                        DateCreated = DateTime.Now
                        
                    };

                    await _context.Students.AddAsync(std);
                    await _context.SaveChangesAsync();

                    //Save the Parent details
                    var prt = new Parents
                    {

                        FirstName = obj.ParentFirstName,
                        LastName = obj.ParentLastName,
                        Email = obj.ParentEmail,
                        EmailConfirmed = false,
                        PhoneNumber = obj.ParentPhoneNumber,
                        Salt = salt,
                        PasswordHash = passwordHashParent,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        UserName = obj.ParentEmail,
                        GenderId = obj.ParentGenderId,
                        Nationality = obj.ParentNationality,
                        State = obj.ParentState,
                        City = obj.ParentCity,
                        HomeAddress = obj.ParentHomeAddress,
                        Occupation = obj.ParentOccupation,
                        StateOfOrigin = obj.ParentStateOfOrigin,
                        LocalGovt = obj.ParentLocalGovt,
                        Religion = obj.ParentReligion,
                        hasChild = true,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Parents.AddAsync(prt);
                    await _context.SaveChangesAsync();

                    //map student and parent
                    var mapp = new ParentsStudentsMap
                    {
                        ParentId = prt.Id,
                        StudentId = std.Id,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        DateCreated = DateTime.Now
                    };
                    await _context.ParentsStudentsMap.AddAsync(mapp);
                    await _context.SaveChangesAsync();

                    
                    //save the link generated
                    var emailConfirmationParent = new EmailConfirmationCodes
                    {
                        UserId = prt.Id,
                        Code = passwordGeneratedParent,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmationParent);
                    await _context.SaveChangesAsync();

                    //Parent
                    string parentlink = _config["SchoolModule:Url"] + "setpassword?userId=" + prt.Id + "&userType=parent&sessionId=" + emailConfirmationParent.Id;
                    emailConfirmationParent.Link = parentlink;
                    await _context.SaveChangesAsync();

                    var subjectParent = "ExpertPlat Parent Registration";
                    string MailContentParent = _emailTemplate.SchoolUserPasswordReset(prt.FirstName, parentlink);

                    EmailMessage messageParent = new EmailMessage(obj.ParentEmail, MailContentParent, subjectParent);
                    _emailRepo.SendEmail(messageParent);

                    
                    //save the link generated for student
                    var emailConfirmationStudent = new EmailConfirmationCodes
                    {
                        UserId = std.Id,
                        Code = passwordGeneratedStudent,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmationStudent);
                    await _context.SaveChangesAsync();

                    //Student
                    string studentlink = _config["SchoolModule:Url"] + "setpassword?userId=" + std.Id + "&userType=student&sessionId=" + emailConfirmationStudent.Id;
                    emailConfirmationStudent.Link = studentlink;
                    await _context.SaveChangesAsync();

                    var subjectStudent = "ExpertPlat Student Registration";
                    string MailContentStudent = _emailTemplate.SchoolUserPasswordReset(obj.StudentFirstName, studentlink);

                    EmailMessage messageStudent = new EmailMessage(obj.ParentEmail, MailContentStudent, subjectStudent);
                    _emailRepo.SendEmail(messageStudent);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Student Created Successfully!" };
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

        public async Task<GenericResponseModel> addStudentToExistingParentAsync(StudentParentExistCreationRequestModel obj)
        {
            try
            {
                //generate the code/password for email confirmation
                var confirmationCode = new RandomNumberGenerator();
                string passwordGenerated = confirmationCode.randomCodesGen();

                CheckerValidation check = new CheckerValidation(_context);
                var checkSch = check.checkSchoolById(obj.SchoolId);
                var checkCamp = check.checkSchoolCampusById(obj.CampusId);
                var checkPrt = check.checkParentById(obj.ParentId);

                //check if the student admissionNumber exist
               // var studentAdmissionNumberCheck = check.checkStudentByAdmissionNumber(obj.AdmissionNumber);

                if (checkSch != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCamp != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School Campus with the specified ID" };
                }
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                //if (studentAdmissionNumberCheck == true)
                //{
                //    return new GenericResponseModel { StatusCode = 409, StatusMessage = "A student with this Admission Number Already exist!" };
                //}
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(passwordGenerated, salt);

                    //get the parent details
                    var parentDetails = _context.Parents.Where(x => x.Id == obj.ParentId).FirstOrDefault();
                    //student AdmissionNumber/Username
                    string admissionNumber = new AdmissionNumberGenerator(_context).GenerateAdmissionNumber(obj.SchoolId);

                    //Save the Student details
                    var std = new Students
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        UserName = admissionNumber,
                        AdmissionNumber = admissionNumber,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        GenderId = obj.GenderId,
                        Email = parentDetails.Email,
                        EmailConfirmed = false,
                        StaffStatus = 0,
                        DateOfBirth = Convert.ToDateTime(obj.DateOfBirth),
                        YearOfAdmission = obj.YearOfAdmission,
                        StateOfOrigin = obj.StateOfOrigin,
                        LocalGovt = obj.LocalGovt,
                        Religion = obj.Religion,
                        HomeAddress = obj.HomeAddress,
                        City = obj.City,
                        State = obj.State,
                        StudentStatus = "",
                        ProfilePictureUrl = "",
                        Status = "",
                        IsAssignedToClass = false,
                        hasParent = true,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Students.AddAsync(std);
                    await _context.SaveChangesAsync();
                    

                    //map student and parent
                    var mapp = new ParentsStudentsMap
                    {
                        ParentId = parentDetails.Id,
                        StudentId = std.Id,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        DateCreated = DateTime.Now
                    };
                    await _context.ParentsStudentsMap.AddAsync(mapp);
                    await _context.SaveChangesAsync();
                    

                    
                    //save the link generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = std.Id,
                        Code = passwordGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();

                    string link = _config["SchoolModule:Url"] + "setpassword?userId=" + std.Id + "&userType=student&sessionId=" + emailConfirmation.Id;
                    emailConfirmation.Link = link;
                    await _context.SaveChangesAsync();

                    var subject = "ExpertPlat Student Registration";
                    string MailContent = _emailTemplate.SchoolUserPasswordReset(obj.FirstName, link);

                    EmailMessage message = new EmailMessage(parentDetails.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Student Created Successfully!" };
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

        public async Task<GenericResponseModel> resendPasswordResetLinkAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Students));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.Students));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    Students students = _context.Students.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == students.Id);
                    string linkGenerated = string.Empty;
                    var subject = "ExpertPlat Student Registration";

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        linkGenerated = getUserCode.Link;

                        //send Mail to user for account activation
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                        string MailContent = _emailTemplate.SchoolUserPasswordReset(students.FirstName, linkGenerated);

                        EmailMessage message = new EmailMessage(students.Email, MailContent, subject);
                        _emailRepo.SendEmail(message);

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Activation Link Sent Successfully!" };

                    }
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Activation Link Was Not Created!" };
                }

                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This User doesnt exist!" };
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

        public async Task<SchoolUsersLoginResponseModel> studentLoginAsync(StudentLoginRequestModel obj)
        {
            try
            {
                //user data and schoolBasicInfo data objects
                SchoolBasicInfoLoginResponseModel schData = new SchoolBasicInfoLoginResponseModel();
                StudentInfoResponseModel userData = new StudentInfoResponseModel();

                //final data to be sent as response 
                SchoolUsersLoginResponseModel respData = new SchoolUsersLoginResponseModel();

                //JWT
                Jwt jwtObj = new Jwt(_config);

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.Students.FirstOrDefault(u => u.UserName == obj.Username || u.Email == obj.Username);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password


                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    if(getUser.EmailConfirmed == false)
                    {
                        return new SchoolUsersLoginResponseModel { StatusCode = 408, StatusMessage = "Your account has not been activated!, kindly check your mail and reset your password" };
                    }
                    else
                    {
                        //Gets the School Information
                        var userSchool = _context.SchoolInformation.FirstOrDefault(u => u.Id == getUser.SchoolId);
                        //Get the schoolType Name
                        var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                        //Get the Campus Name
                        var getCampus = _context.SchoolCampuses.FirstOrDefault(u => u.Id == getUser.CampusId);

                        //get the current session and term of the school
                        SessionAndTerm sessionTerm = new SessionAndTerm(_context);
                        var currentSessionId = sessionTerm.getCurrentSessionId(getUser.SchoolId);
                      
                        long classId = 0;
                        long classGradeId = 0;
                        string className = string.Empty;
                        string classGradeName = string.Empty;

                        //student Info
                        StudentClassInfo classInfo = new StudentClassInfo();

                        if (currentSessionId > 0)
                        {
                            //Get the Student Class and ClassGrade
                            GradeStudents getStudentClassAndGrade = _context.GradeStudents.FirstOrDefault(u => u.StudentId == getUser.Id && u.SessionId == currentSessionId);

                            if (getStudentClassAndGrade != null)
                            {
                                classId = getStudentClassAndGrade.ClassId;
                                classGradeId = getStudentClassAndGrade.ClassGradeId;

                                //get the class and ClassGrade
                                className = _context.Classes.FirstOrDefault(x => x.Id == classId && x.SchoolId == getUser.SchoolId && x.CampusId == getUser.CampusId).ClassName;
                                classGradeName = _context.ClassGrades.FirstOrDefault(x => x.Id == classGradeId && x.SchoolId == getUser.SchoolId && x.CampusId == getUser.CampusId).GradeName;

                                classInfo.Message = "Success";
                                classInfo.ClassId = classId;
                                classInfo.ClassName = className;
                                classInfo.ClassGradeId = classGradeId;
                                classInfo.ClassGradeName = classGradeName;
                            }
                            else
                            {
                                classInfo.Message = "Student has not been Assigned to a Class for the current Session";
                            }
                        }
                        else
                        {
                            classInfo.Message = "School Current Session has not been set and Student has not been Assigned to a Class";
                        }

                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.UserName = getUser.UserName;
                        userData.AdmissionNumber = getUser.AdmissionNumber;
                        userData.IsActive = getUser.IsActive;
                        userData.LastLoginDate = getUser.LastLoginDate;
                        userData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        userData.LastUpdatedDate = getUser.LastUpdatedDate;
                        userData.StudentClassInfo = classInfo;

                        //School and Campus details
                        schData.SchoolId = userSchool.Id;
                        schData.SchoolName = userSchool.SchoolName;
                        schData.SchoolCode = userSchool.SchoolCode;
                        schData.SchoolTypeName = getSchType.SchoolTypeName;
                        schData.CampusId = getCampus.Id;
                        schData.CampusName = getCampus.CampusName;
                        schData.CampusAddress = getCampus.CampusAddress;

                        //Generate JSON WEB TOKEN for a valid User
                        var tokenString = jwtObj.GenerateJWTToken();

                        //The data to be sent as response
                        respData.StatusCode = 200;
                        respData.StatusMessage = "Login Successful!";
                        respData.Token = tokenString;
                        respData.UserDetails = userData;
                        respData.schoolDetails = schData;
                    }
                }
                else
                {
                    return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                }

                return respData;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SchoolUsersLoginResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getStudentByIdAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkStudent = check.checkStudentById(studentId);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkStudent != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Student with the specified ID" };
                }
                if (checkSchool != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No SchoolCampus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.Id == studentId && std.SchoolId == schoolId && std.CampusId == campusId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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

        public async Task<GenericResponseModel> assignStudentToClassAsync(AssignStudentToClassRequestModel obj)
        {
            try
            {
                GenericResponseModel respData = new GenericResponseModel();

                //get the current sessionId
                var currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(obj.SchoolId);

                if (currentSessionId == 0) //if the current session has not been set
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Current Academic Session has not been set!" };
                }
                else
                {
                    foreach (StudentId studId in obj.StudentIds)
                    {
                        //check unassigned students
                        var checkStudent = _context.Students.Where(x => x.Id == studId.Id && x.IsAssignedToClass == false).FirstOrDefault();

                        if (checkStudent != null)
                        {
                            var std = new GradeStudents
                            {
                                StudentId = studId.Id,
                                ClassId = obj.ClassId,
                                ClassGradeId = obj.ClassGradeId,
                                SchoolId = obj.SchoolId,
                                CampusId = obj.CampusId,
                                SessionId = currentSessionId,
                                HasGraduated = false,
                                DateCreated = DateTime.Now
                            };

                            //update the student "IsAssignedToClass" to true
                            checkStudent.IsAssignedToClass = true;

                            await _context.GradeStudents.AddAsync(std);
                            await _context.SaveChangesAsync();

                            //return all the students assigned to the Class
                            var clasStudents = from sub in _context.GradeStudents
                                               where sub.ClassId == obj.ClassId && sub.ClassGradeId == obj.ClassGradeId
                                               && sub.SchoolId == obj.SchoolId && sub.CampusId == obj.CampusId
                                               select new
                                               {
                                                   sub.StudentId,
                                                   sub.SchoolId,
                                                   sub.CampusId,
                                                   sub.Students.FirstName,
                                                   sub.Students.LastName,
                                                   sub.Students.AdmissionNumber,
                                                   sub.Students.UserName,
                                                   sub.ClassId,
                                                   sub.Classes.ClassName,
                                                   sub.ClassGradeId,
                                                   sub.ClassGrades.GradeName,
                                                   sub.Sessions.SessionName,
                                                   sub.DateCreated
                                               };

                            respData.StatusCode = 200;
                            respData.StatusMessage = "Students Assigned Successfully";
                            respData.Data = clasStudents.ToList();
                        }
                        else
                        {
                            var clasStudents = from sub in _context.GradeStudents
                                               where sub.ClassId == obj.ClassId && sub.ClassGradeId == obj.ClassGradeId
                                               && sub.SchoolId == obj.SchoolId && sub.CampusId == obj.CampusId
                                               select new
                                               {
                                                   sub.StudentId,
                                                   sub.SchoolId,
                                                   sub.CampusId,
                                                   sub.Students.FirstName,
                                                   sub.Students.LastName,
                                                   sub.Students.AdmissionNumber,
                                                   sub.Students.UserName,
                                                   sub.ClassId,
                                                   sub.Classes.ClassName,
                                                   sub.ClassGradeId,
                                                   sub.ClassGrades.GradeName,
                                                   sub.Sessions.SessionName,
                                                   sub.DateCreated
                                               };


                            respData.StatusCode = 200;
                            respData.StatusMessage = "One or more selected Students has been assigned!";
                            respData.Data = clasStudents.ToList();
                        }
                    }
                }

                return respData;

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

        public async Task<GenericResponseModel> getStudentParentAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkStudent = check.checkStudentById(studentId);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkStudent != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Student with the specified ID" };
                }
                if (checkSchool != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No SchoolCampus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.ParentsStudentsMap
                                 where std.StudentId == studentId && std.SchoolId == schoolId && std.CampusId == campusId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.StudentId,
                                     std.ParentId,
                                     std.Parents.FirstName,
                                     std.Parents.LastName,
                                     std.Parents.UserName,
                                     std.Parents.Email,
                                     std.Parents.PhoneNumber,
                                     std.Parents.HomeAddress,
                                     std.Parents.StateOfOrigin,
                                     std.Parents.LocalGovt,
                                     std.Parents.Nationality,
                                     std.Parents.Occupation,
                                     std.Parents.Religion,
                                     std.Parents.State,
                                     std.Parents.IsActive,
                                     std.Parents.LastLoginDate,
                                     std.Parents.LastPasswordChangedDate,
                                     std.Parents.LastUpdatedDate,
                                     std.DateCreated,
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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

        public async Task<GenericResponseModel> getAllAssignedStudentAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.IsAssignedToClass == true
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> getAllUnAssignedStudentAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.IsAssignedToClass == false
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> getAllStudentInSchoolAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                if (checkSchool != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId 
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> getAllStudentInCampusAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId 
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> getStudentsBySessionIdAsync(long schoolId, long campusId, long sessionId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);


                if (checkSchool != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No SchoolCampus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.GradeStudents
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.SessionId == sessionId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.Students.FirstName,
                                     std.Students.LastName,
                                     std.Students.MiddleName,
                                     std.Students.UserName,
                                     std.Students.AdmissionNumber,
                                     std.Students.YearOfAdmission,
                                     std.Students.Status,
                                     std.Students.StaffStatus,
                                     std.Students.State,
                                     std.Students.City,
                                     std.Students.DateOfBirth,
                                     std.Students.StateOfOrigin,
                                     std.Students.LocalGovt,
                                     std.Students.ProfilePictureUrl,
                                     std.Students.HomeAddress,
                                     std.Students.Gender.GenderName,
                                     std.Students.hasParent,
                                     std.Students.IsActive,
                                     std.Students.LastPasswordChangedDate,
                                     std.Students.LastLoginDate,
                                     std.Students.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericResponseModel> moveStudentToNewClassAndClassGradeAsync(MoveStudentRequestModel obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);
                var checkSession = check.checkSessionById(obj.SessionId);

                //response data
                IList<GenericResponseModel> responseList = new List<GenericResponseModel>();

                //the teacherId
                var teacherId = _context.GradeTeachers.Where(g => g.ClassId == obj.ClassId && g.ClassGradeId == obj.ClassGradeId 
                && g.SchoolId == obj.SchoolId && g.CampusId == obj.CampusId).FirstOrDefault();

                if (checkSchool != true && checkCampus != true && checkClass != true && checkClassGrade != true && checkSession != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "One or More Paramters are Invalid" };
                }
                if (teacherId == null)
                {
                    return new GenericResponseModel { StatusCode = 500, StatusMessage = "A Teacher has not been assigned to this Class and ClassGrade!" };
                }
                else
                {
                    //if category selected is Alumni; move students selected to the alumni table
                    //and update the students status (graduated to true)
                    if (obj.ClassOrAlumniId == (long)EnumUtility.ClassOrAlumni.Alumni)
                    {
                        foreach (var stdId in obj.StudentIds)
                        {
                            var getAlumni = _context.Alumni.FirstOrDefault(x => x.StudentId == stdId.Id);
                            var getStudents = _context.GradeStudents.Where(x => x.StudentId == stdId.Id).FirstOrDefault();

                            if (getAlumni == null)
                            {
                                var alumni = new Alumni
                                {
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    SessionId = obj.SessionId,
                                    StudentId = stdId.Id,
                                    GradeTeacherId = teacherId.SchoolUserId,
                                    DateGraduated = DateTime.Now,
                                };
                               
                                await _context.Alumni.AddAsync(alumni);

                                //update the student hasGraduated to true
                                getStudents.HasGraduated = true;
                                await _context.SaveChangesAsync();

                                //response data
                                GenericResponseModel response = new GenericResponseModel();

                                response.StatusCode = 200;
                                response.StatusMessage = "Student(s) Moved to Alumni Successfully!";

                                responseList.Add(response);

                            }
                            else
                            {
                                getAlumni.StudentId = stdId.Id;
                                getAlumni.SchoolId = obj.SchoolId;
                                getAlumni.CampusId = obj.CampusId;
                                getAlumni.SessionId = obj.SessionId;
                                getAlumni.ClassId = obj.ClassId;
                                getAlumni.ClassGradeId = obj.ClassGradeId;
                                getAlumni.GradeTeacherId = teacherId.SchoolUserId;
                                getAlumni.DateGraduated = DateTime.Now;

                                await _context.SaveChangesAsync();

                                GenericResponseModel response = new GenericResponseModel();

                                response.StatusCode = 200;
                                response.StatusMessage = "Student(s) Moved to Alumni Successfully!";

                                responseList.Add(response);
                            }
                            
                        }
                    }
                    //if category selected is Class; add new students to the session
                    else if (obj.ClassOrAlumniId == (long)EnumUtility.ClassOrAlumni.Class)
                    {
                        foreach (var stdId in obj.StudentIds)
                        {
                            var studentExists = _context.GradeStudents.Where(x => x.StudentId == stdId.Id && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                            if (studentExists != null)
                            {
                                GenericResponseModel response = new GenericResponseModel();

                                response.StatusCode = 409;
                                response.StatusMessage = $"Student with ID: {studentExists.StudentId} Already exists in this Session";

                                responseList.Add(response);
                            }
                            else
                            {
                                var grdStd = new GradeStudents
                                {
                                    StudentId = stdId.Id,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    SessionId = obj.SessionId,
                                    HasGraduated = false,
                                    DateCreated = DateTime.Now,
                                };

                                await _context.GradeStudents.AddAsync(grdStd);
                                await _context.SaveChangesAsync();

                                GenericResponseModel response = new GenericResponseModel();
                                response.StatusCode = 200;
                                response.StatusMessage = $"Student with ID: {stdId.Id} Moved to a new Class and ClassGrade Successfully";

                                responseList.Add(response);
                            }
                        }
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = responseList};

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


        public async Task<StudentBulkCreationResponseModel> createStudentFromExcelAsync(BulkStudentRequestModel obj)
        {
            //generate the code/password for email confirmation
            var confirmationCode1 = new RandomNumberGenerator();
            string passwordGeneratedParent = confirmationCode1.randomCodesGen();

            var confirmationCode2 = new RandomNumberGenerator();
            string passwordGeneratedStudent = confirmationCode2.randomCodesGen();
            IList<object> data = new List<object>();
            try
            {
                StudentBulkCreationResponseModel response = new StudentBulkCreationResponseModel();
                long numberOfStudentsCreated = 0;
                long numberOfExistingStudents = 0;
                IList<object> listOfParentThatExists = new List<object>();
                IList<object> listOfStudentThatExists = new List<object>();
                IList<object> listOfStudentsCreated = new List<object>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //check if the School and CampusId is Valid
                if (checkSchool == false && checkCampus == false)
                {
                    return new StudentBulkCreationResponseModel { StatusCode = 400, StatusMessage = "No School Or Campus With the specified ID" };
                }
                else if (obj.File == null || obj.File.Length <= 0)
                {
                    return new StudentBulkCreationResponseModel { StatusCode = 400, StatusMessage = "No File Selected!, Please Select the Student Bulk Upload Template" };
                }
                else if (!Path.GetExtension(obj.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new StudentBulkCreationResponseModel { StatusCode = 400, StatusMessage = "Not a Supported File Format!" };
                }
                else
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

                        //student AdmissionNumber/Username Instance
                        AdmissionNumberGenerator admNumber = new AdmissionNumberGenerator(_context);

                        //the default password Instance
                        string password = defaultPassword();
                        //Password Encryption Instance
                        var paswordHasher = new PasswordHasher();

                        for (int row = 2; row <= rowCount; row++) // starts from the second row (Jumping the table headings)
                        {
                            //the parents email Address
                            string parentsEmailAddress = worksheet.Cells[row, 17].Value.ToString(); //gets the parent email Address
                            if (parentsEmailAddress != null)
                            {

                                //Check the Student FirstName, Gender, Date of birth, parent Email Address

                                //AdmissionNumber
                                var admissionNumber = admNumber.GenerateAdmissionNumber(obj.SchoolId);

                                //the salt
                                string salt = paswordHasher.getSalt();
                                //Hash the password and salt
                                string passwordHash = paswordHasher.hashedPassword(password, salt);

                                long studentGenderId = 1; //set a default genderId (Male)

                                if (worksheet.Cells[row, 4].Value != null)
                                {
                                    studentGenderId = (long)Converters.stringToGender(worksheet.Cells[row, 4].Value.ToString());
                                }

                                //check for exsting students in the school
                                var getExistingStudents = from x in _context.Students
                                                          where x.LastName == worksheet.Cells[row, 1].Value.ToString()
                            && x.FirstName == worksheet.Cells[row, 2].Value.ToString()
                            && x.MiddleName == worksheet.Cells[row, 3].Value.ToString()
                            && x.SchoolId == obj.SchoolId
                            && x.CampusId == obj.CampusId
                                                          select x;

                                //skip and add students to duplicate table if the students exists 
                                if (getExistingStudents.Count() > 0)
                                {
                                    foreach (Students std in getExistingStudents)
                                    {
                                        //check if the students exists in the duplicate table
                                        StudentDuplicates objDupStd = _context.StudentDuplicates.FirstOrDefault(x => x.NewStudentFullName == worksheet.Cells[row, 1].Value.ToString() + " " + worksheet.Cells[row, 2].Value.ToString() + " " + worksheet.Cells[row, 3].Value.ToString()
                                        && x.ExistingStudentId == std.Id
                                        && x.SchoolId == obj.SchoolId
                                        && x.CampusId == obj.CampusId);

                                        //adds the students duplicate record
                                        if (objDupStd == null)
                                        {
                                            StudentDuplicates objDup = new StudentDuplicates();
                                            objDup.NewStudentFullName = worksheet.Cells[row, 1].Value.ToString() + " " + worksheet.Cells[row, 2].Value.ToString() + " " + worksheet.Cells[row, 3].Value.ToString();
                                            objDup.ExistingStudentId = std.Id;
                                            objDup.SchoolId = obj.SchoolId;
                                            objDup.CampusId = obj.CampusId;
                                            objDup.DateCreated = DateTime.Now;
                                            await _context.StudentDuplicates.AddAsync(objDup);
                                            await _context.SaveChangesAsync();
                                        }
                                        else //updates the students duplicate record
                                        {
                                            objDupStd.NewStudentFullName = worksheet.Cells[row, 1].Value.ToString() + " " + worksheet.Cells[row, 2].Value.ToString() + " " + worksheet.Cells[row, 3].Value.ToString();
                                            objDupStd.ExistingStudentId = std.Id;
                                            objDupStd.SchoolId = obj.SchoolId;
                                            objDupStd.CampusId = obj.CampusId;
                                            objDupStd.DateCreated = DateTime.Now;
                                            await _context.SaveChangesAsync();
                                        }

                                        //the student data existing
                                        var existingStudent = (from sd in _context.Students
                                                               where sd.Id == std.Id
                                                               select new
                                                               {
                                                                   sd.Id,
                                                                   sd.SchoolId,
                                                                   sd.CampusId,
                                                                   sd.FirstName,
                                                                   sd.LastName,
                                                                   sd.UserName,
                                                                   sd.AdmissionNumber,
                                                                   sd.hasParent,
                                                                   sd.IsActive,
                                                                   sd.LastPasswordChangedDate,
                                                                   sd.LastLoginDate,
                                                                   sd.LastUpdatedDate,
                                                                   sd.DateCreated,
                                                               }).FirstOrDefault();

                                        //add the existing students to a list as response
                                        listOfStudentThatExists.Add(existingStudent);
                                        numberOfExistingStudents++;
                                    }
                                }
                                else
                                {
                                    var std = new Students
                                    {
                                        LastName = worksheet.Cells[row, 1].Value.ToString(),
                                        FirstName = worksheet.Cells[row, 2].Value.ToString(),
                                        MiddleName = worksheet.Cells[row, 3].Value.ToString(),
                                        GenderId = studentGenderId,
                                        StaffStatus = 0,
                                        DateOfBirth = Convert.ToDateTime(worksheet.Cells[row, 6].Value.ToString()),
                                        YearOfAdmission = worksheet.Cells[row, 7].Value.ToString(),
                                        StateOfOrigin = worksheet.Cells[row, 8].Value.ToString(),
                                        LocalGovt = worksheet.Cells[row, 9].Value.ToString(),
                                        Religion = worksheet.Cells[row, 10].Value.ToString(),
                                        HomeAddress = worksheet.Cells[row, 11].Value.ToString(),
                                        City = worksheet.Cells[row, 12].Value.ToString(),
                                        State = worksheet.Cells[row, 13].Value.ToString(),
                                        UserName = admissionNumber,
                                        AdmissionNumber = admissionNumber,
                                        Email = parentsEmailAddress,
                                        EmailConfirmed = false,
                                        Salt = salt,
                                        PasswordHash = passwordHash,
                                        StudentStatus = "",
                                        SchoolId = obj.SchoolId,
                                        CampusId = obj.CampusId,
                                        IsAssignedToClass = false,
                                        hasParent = true,
                                        IsActive = true,
                                        ProfilePictureUrl = "",
                                        Status = "",
                                        DateCreated = DateTime.Now,
                                    };

                                    await _context.Students.AddAsync(std);
                                    await _context.SaveChangesAsync();

                                    //the students created
                                    var stud = (from sd in _context.Students
                                                where sd.Id == std.Id
                                                select new
                                                {
                                                    sd.Id,
                                                    sd.SchoolId,
                                                    sd.CampusId,
                                                    sd.FirstName,
                                                    sd.LastName,
                                                    sd.UserName,
                                                    sd.AdmissionNumber,
                                                    sd.hasParent,
                                                    sd.IsActive,
                                                    sd.LastPasswordChangedDate,
                                                    sd.LastLoginDate,
                                                    sd.LastUpdatedDate,
                                                    sd.DateCreated,
                                                }).FirstOrDefault();

                                    //add all students created to a list
                                    listOfStudentsCreated.Add(stud);


                                    
                                    //save the link generated
                                    var emailConfirmationStudent = new EmailConfirmationCodes
                                    {
                                        UserId = std.Id,
                                        Code = passwordGeneratedStudent,
                                        DateGenerated = DateTime.Now
                                    };
                                    await _context.AddAsync(emailConfirmationStudent);
                                    await _context.SaveChangesAsync();

                                    //Student
                                    string studentlink = _config["SchoolModule:Url"] + "setpassword?userId=" + std.Id + "&userType=student&sessionId=" + emailConfirmationStudent.Id;
                                    emailConfirmationStudent.Link = studentlink;
                                    await _context.SaveChangesAsync();

                                    var subjectStudent = "ExpertPlat Student Registration";
                                    string MailContentStudent = _emailTemplate.SchoolUserPasswordReset(std.FirstName, studentlink);

                                    EmailMessage messageStudent = new EmailMessage(parentsEmailAddress, MailContentStudent, subjectStudent);
                                    _emailRepo.SendEmail(messageStudent);


                                    //Check if the parent exists
                                    var parentExists = _context.Parents.Where(P => P.Email == parentsEmailAddress && P.SchoolId == obj.SchoolId && P.CampusId == obj.CampusId).FirstOrDefault();

                                    if (parentExists != null)
                                    {
                                        //map student to existing parent
                                        var mapp = new ParentsStudentsMap
                                        {
                                            ParentId = parentExists.Id,
                                            StudentId = std.Id,
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            DateCreated = DateTime.Now
                                        };

                                        await _context.ParentsStudentsMap.AddAsync(mapp);
                                        await _context.SaveChangesAsync();
                                        
                                        //save the link generated
                                        var emailConfirmationParent = new EmailConfirmationCodes
                                        {
                                            UserId = parentExists.Id,
                                            Code = passwordGeneratedParent,
                                            DateGenerated = DateTime.Now
                                        };
                                        await _context.AddAsync(emailConfirmationParent);
                                        await _context.SaveChangesAsync();

                                        //Parent
                                        string parentlink = _config["SchoolModule:Url"] + "setpassword?userId=" + parentExists.Id + "&userType=parent&sessionId=" + emailConfirmationParent.Id;
                                        emailConfirmationParent.Link = parentlink;
                                        await _context.SaveChangesAsync();

                                        var subjectParent = "ExpertPlat Parent Registration";
                                        string MailContentParent = _emailTemplate.SchoolUserPasswordReset(parentExists.FirstName, parentlink);

                                        EmailMessage messageParent = new EmailMessage(parentExists.Email, MailContentParent, subjectParent);
                                        _emailRepo.SendEmail(messageParent);

                                        //the list of parent that exits
                                        var prts = (from prt in _context.Parents
                                                    where prt.Email == parentExists.Email
                                                    select new
                                                    {
                                                        prt.Id,
                                                        prt.SchoolId,
                                                        prt.CampusId,
                                                        prt.FirstName,
                                                        prt.LastName,
                                                        prt.UserName,
                                                        prt.Email,
                                                        prt.PhoneNumber,
                                                        prt.hasChild,
                                                        prt.IsActive,
                                                        prt.DateCreated,
                                                    }).FirstOrDefault();
                                        //add all existing parent to a list and send as part of API respponse
                                        listOfParentThatExists.Add(prts);
                                    }
                                    else
                                    {
                                        long parentGenderId = 1; //set a default genderId (Male)

                                        if (worksheet.Cells[row, 16].Value != null)
                                        {
                                            parentGenderId = (long)Converters.stringToGender(worksheet.Cells[row, 16].Value.ToString());
                                        }

                                        //Save new parents details
                                        var parent = new Parents
                                        {
                                            FirstName = worksheet.Cells[row, 14].Value.ToString(),
                                            LastName = worksheet.Cells[row, 15].Value.ToString(),
                                            UserName = parentsEmailAddress,
                                            GenderId = parentGenderId,
                                            Email = parentsEmailAddress,
                                            EmailConfirmed = false,
                                            PhoneNumber = worksheet.Cells[row, 18].Value.ToString(),
                                            Salt = salt,
                                            PasswordHash = passwordHash,
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            Nationality = worksheet.Cells[row, 19].Value.ToString(),
                                            State = worksheet.Cells[row, 20].Value.ToString(),
                                            City = worksheet.Cells[row, 21].Value.ToString(),
                                            HomeAddress = worksheet.Cells[row, 22].Value.ToString(),
                                            Occupation = worksheet.Cells[row, 23].Value.ToString(),
                                            StateOfOrigin = worksheet.Cells[row, 24].Value.ToString(),
                                            LocalGovt = worksheet.Cells[row, 25].Value.ToString(),
                                            Religion = worksheet.Cells[row, 26].Value.ToString(),
                                            IsActive = true,
                                            hasChild = true,
                                            DateCreated = DateTime.Now,
                                        };

                                        await _context.Parents.AddAsync(parent);
                                        await _context.SaveChangesAsync();

                                        //map student and parent
                                        var mapp = new ParentsStudentsMap
                                        {
                                            ParentId = parent.Id,
                                            StudentId = std.Id,
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            DateCreated = DateTime.Now
                                        };

                                        await _context.ParentsStudentsMap.AddAsync(mapp);
                                        await _context.SaveChangesAsync();

                                        
                                        //save the link generated
                                        var emailConfirmationParent = new EmailConfirmationCodes
                                        {
                                            UserId = parent.Id,
                                            Code = passwordGeneratedParent,
                                            DateGenerated = DateTime.Now
                                        };
                                        await _context.AddAsync(emailConfirmationParent);
                                        await _context.SaveChangesAsync();

                                        //Mail to Parent
                                        string parentlink = _config["SchoolModule:Url"] + "setpassword?userId=" + parent.Id + "&userType=parent&sessionId=" + emailConfirmationParent.Id;
                                        emailConfirmationParent.Link = parentlink;
                                        await _context.SaveChangesAsync();

                                        var subjectParent = "ExpertPlat Parent Registration";
                                        string MailContentParent = _emailTemplate.SchoolUserPasswordReset(parent.FirstName, parentlink);

                                        EmailMessage messageParent = new EmailMessage(parent.Email, MailContentParent, subjectParent);
                                        _emailRepo.SendEmail(messageParent);
                                    }

                                    //increments the numbers of students created from the excel file
                                    numberOfStudentsCreated++;
                                }

                                response.StatusCode = 200;
                                response.StatusMessage = "Uploaded Successfully for students with parent email!, Student(s) with existing details and Parent Details has been updated Successfully!";
                                response.NumberOfStudentsCreated = numberOfStudentsCreated;
                                response.StudentsData = listOfStudentsCreated.ToList();
                                response.NumberOfExistingParents = listOfParentThatExists.Count();
                                response.ExistingParentsInfo = listOfParentThatExists.ToList();
                                response.ExistingStudentsInfo = listOfStudentThatExists.ToList();
                            }
                        }
                    }
                }

                return response;

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new StudentBulkCreationResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> updateStudentDetailsAsync(Guid studentId, UpdateStudentRequestModel obj)
        {
            try
            {
                var getStudent = _context.Students.Where(s => s.Id == studentId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                if (getStudent != null)
                {
                    getStudent.LastName = obj.StudentLastName;
                    getStudent.FirstName = obj.StudentFirstName;
                    getStudent.MiddleName = obj.MiddleName;
                    getStudent.SchoolId = obj.SchoolId;
                    getStudent.CampusId = obj.CampusId;
                    getStudent.GenderId = obj.GenderId;
                    getStudent.StaffStatus = 0;
                    getStudent.DateOfBirth = Convert.ToDateTime(obj.DateOfBirth);
                    getStudent.YearOfAdmission = obj.YearOfAdmission;
                    getStudent.StateOfOrigin = obj.StateOfOrigin;
                    getStudent.LocalGovt = obj.LocalGovt;
                    getStudent.Religion = obj.Religion;
                    getStudent.HomeAddress = obj.HomeAddress;
                    getStudent.City = obj.City;
                    getStudent.State = obj.State;
                    getStudent.StudentStatus = "";
                    getStudent.ProfilePictureUrl = obj.ProfilePictureUrl;
                    getStudent.Status = "";

                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Student Details Updated Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Student With the Specified ID" };

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

        public async Task<GenericResponseModel> deleteStudentsAssignedToClassAsync(DeleteStudentAssignedRequestModel obj)
        {
            try
            {
                var getStudent = _context.Students.Where(s => s.Id == obj.StudentId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getStudent != null)
                {
                    var countExists = from st in _context.GradeStudents where st.StudentId == obj.StudentId && st.SchoolId == obj.SchoolId && st.CampusId == obj.CampusId select st;

                    //if student with the specified ID exists more than once in the GradeStudents table, the student is deleted from the class, classgrade 
                    //and the session specified and the status (IsAssigned) remains true 
                    if (countExists.Count() > 1)
                    {
                        var studentInGrade = _context.GradeStudents.Where(s => s.StudentId == obj.StudentId && s.ClassId == obj.ClassId
                        && s.ClassGradeId == obj.ClassGradeId && s.SessionId == obj.SessionId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                        if (studentInGrade != null)
                        {
                            _context.GradeStudents.Remove(studentInGrade);
                            await _context.SaveChangesAsync();

                            return new GenericResponseModel { StatusCode = 500, StatusMessage = "Student Deleted from the Class and ClassGrade Assigned!" };

                        }

                    }
                    else //if student with the specified ID exists in the GradeStudents table only once, the student is deleted and the status (IsAssigned) is updated to false
                    {
                        var studentInGrade = _context.GradeStudents.Where(s => s.StudentId == obj.StudentId && s.ClassId == obj.ClassId
                            && s.ClassGradeId == obj.ClassGradeId && s.SessionId == obj.SessionId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                        if (studentInGrade != null)
                        {
                            //updates isAssignedToClass to false
                            getStudent.IsAssignedToClass = false;

                            _context.GradeStudents.Remove(studentInGrade);
                            await _context.SaveChangesAsync();

                            return new GenericResponseModel { StatusCode = 500, StatusMessage = "Student Deleted from the Class and ClassGrade Assigned!" };

                        }

                    }
                }

                return new GenericResponseModel { StatusCode = 500, StatusMessage = "No Student with the specified Id!" };

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


        public async Task<GenericResponseModel> deleteStudentAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                var getStudent = _context.Students.Where(s => s.Id == studentId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                if (getStudent != null)
                {
                    _context.Students.Remove(getStudent);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 500, StatusMessage = "Student and other Information related to student deleted Successfully!" };

                }

                return new GenericResponseModel { StatusCode = 500, StatusMessage = "No Student with the specified Id!" };

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

        public async Task<GenericResponseModel> getAllStudentDuplicatesAsync(long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    //the list of student duplicates
                    var result = from std in _context.StudentDuplicates
                                where std.SchoolId == schoolId && std.CampusId == campusId
                                select new
                                {
                                    std.Id,
                                    std.ExistingStudentId,
                                    std.NewStudentFullName, //concantenation of firstname, surname and middlename
                                    std.SchoolId,
                                    std.CampusId,
                                    std.DateCreated
                                };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid School/CampusId!" };

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

        public async Task<GenericResponseModel> getStudentDuplicateByStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    //the list of student duplicates
                    var result = from std in _context.StudentDuplicates
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.ExistingStudentId == studentId
                                 select new
                                 {
                                     std.Id,
                                     std.ExistingStudentId,
                                     std.NewStudentFullName, //concantenation of firstname, surname and middlename
                                     std.SchoolId,
                                     std.CampusId,
                                     std.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid School/CampusId!" };

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

        public async Task<GenericResponseModel> updateStudentDuplicateAsync(StudentDuplicateRequestModel obj)
        {
            try
            {
                //response lists
                IList<GenericResponseModel> responseList = new List<GenericResponseModel>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);
                var checkSession = check.checkSessionById(obj.SessionId);

                if (checkSchool != true && checkCampus != true && checkClass != true && checkClassGrade != true && checkSession != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "One or More Paramters are Invalid" };
                }
                else
                {
                    foreach (StudentId std in obj.StudentIds)
                    {
                        //check if student exists in duplicate table
                        StudentDuplicates studentDuplicate = _context.StudentDuplicates.Where(s => s.ExistingStudentId == std.Id && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                        if (studentDuplicate != null)
                        {
                            //check if student exists in the session
                            var studentExists = _context.GradeStudents.Where(x => x.StudentId == std.Id && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                            if (studentExists == null)
                            {
                                var grdStd = new GradeStudents
                                {
                                    StudentId = std.Id,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    SessionId = obj.SessionId,
                                    HasGraduated = false,
                                    DateCreated = DateTime.Now,
                                };

                                await _context.GradeStudents.AddAsync(grdStd);
                                await _context.SaveChangesAsync();

                                //update students isAssigned to true
                                var gettSudent = _context.Students.Where(x => x.Id == std.Id && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                                if (gettSudent != null)
                                {
                                    gettSudent.IsAssignedToClass = true;
                                    await _context.SaveChangesAsync();
                                }

                                //delete the student record from the studentDuplicate Table
                                var getStudentDuplicate = _context.StudentDuplicates.Where(s => s.ExistingStudentId == std.Id && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                                if (getStudentDuplicate != null)
                                {
                                    _context.StudentDuplicates.Remove(getStudentDuplicate);
                                    await _context.SaveChangesAsync();
                                }

                                GenericResponseModel response = new GenericResponseModel
                                {
                                    StatusCode = 200,
                                    StatusMessage = $"Student with ID: {std.Id} Data has been Updated Successfully",
                                };
                                responseList.Add(response);
                            }
                            else
                            {
                                GenericResponseModel response = new GenericResponseModel
                                {
                                    StatusCode = 409,
                                    StatusMessage = $"Student with ID: {studentExists.StudentId} Already exists in this Session",
                                };
                                responseList.Add(response);
                            }
                        }
                        else
                        {
                            GenericResponseModel response = new GenericResponseModel
                            {
                                StatusCode = 409,
                                StatusMessage = $"Student with ID: {std.Id} does not have a duplicate Record",
                            };
                            responseList.Add(response);
                        }
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = responseList };
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

        public async Task<GenericResponseModel> deleteStudentDuplicateAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                var getStudentDuplicate = _context.StudentDuplicates.Where(s => s.ExistingStudentId == studentId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                if (getStudentDuplicate != null)
                {
                    _context.StudentDuplicates.Remove(getStudentDuplicate);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Student Duplicate Record Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Student with the specified ID does not have a duplicate record!"};

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
