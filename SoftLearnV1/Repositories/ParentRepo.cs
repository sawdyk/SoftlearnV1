using Microsoft.Extensions.Configuration;
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
    public class ParentRepo : IParentRepo
    {
        private readonly AppDbContext _context;
        private IConfiguration _config;
        private readonly EmailTemplate _emailTemplate;
        private readonly IEmailRepo _emailRepo;

        public ParentRepo(AppDbContext context, IConfiguration config, EmailTemplate emailTemplate, IEmailRepo emailRepo)
        {
            _context = context;
            _config = config;
            this._emailTemplate = emailTemplate;
            this._emailRepo = emailRepo;
        }

        public async Task<SchoolUsersLoginResponseModel> parentLoginAsync(LoginRequestModel obj)
        {
            try
            {
                //user data and schoolBasicInfo data objects
                SchoolBasicInfoLoginResponseModel schData = new SchoolBasicInfoLoginResponseModel();
                SchoolUserInfoResponseModel userData = new SchoolUserInfoResponseModel();

                //final data to be sent as response 
                SchoolUsersLoginResponseModel respData = new SchoolUsersLoginResponseModel();

                //JWT
                Jwt jwtObj = new Jwt(_config);

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                //var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Parents));
                var getUser = _context.Parents.FirstOrDefault(u => u.Email == obj.Email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password


                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    //else if (getUser != null && getUser.PasswordHash == decryptedPassword && accountCheckResult == true)
                    //{
                    //    return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                    //}
                    if(getUser.EmailConfirmed == false)
                    {
                        return new SchoolUsersLoginResponseModel { StatusCode = 408, StatusMessage = "Your account has not been activated!, kindly check your mail and reset your password" };
                    }
                    else
                    {
                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.UserName = getUser.UserName;
                        userData.Email = getUser.Email;
                        userData.EmailConfirmed = getUser.EmailConfirmed;
                        userData.IsActive = getUser.IsActive;
                        userData.LastLoginDate = getUser.LastLoginDate;
                        userData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        userData.LastUpdatedDate = getUser.LastUpdatedDate;

                        //Gets the School Information
                        var userSchool = _context.SchoolInformation.FirstOrDefault(u => u.Id == getUser.SchoolId);
                        //Get the schoolType Name
                        var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                        //Get the Campus Name
                        var getCampus = _context.SchoolCampuses.FirstOrDefault(u => u.Id == getUser.CampusId);

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
                        respData.StatusMessage = "Login Successful";
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

        public async Task<GenericResponseModel> resendPasswordResetLinkAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Parents));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.Parents));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    Parents parents = _context.Parents.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == parents.Id);
                    string linkGenerated = string.Empty;
                    var subject = "ExpertPlat Parent Registration";

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        linkGenerated = getUserCode.Link;

                        //send Mail to user for account activation
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                        string MailContent = _emailTemplate.SchoolUserPasswordReset(parents.FirstName, linkGenerated);

                        EmailMessage message = new EmailMessage(parents.Email, MailContent, subject);
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

        //Parents details by Email
        public async Task<GenericResponseModel> getParentDetailsByEmailAsync(string email, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Parents));
                if (emailCheckResult == true)
                {
                    var result = from prt in _context.Parents
                                 where prt.Email == email && prt.SchoolId == schoolId && prt.CampusId == campusId
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
                                     prt.LastPasswordChangedDate,
                                     prt.LastLoginDate,
                                     prt.LastUpdatedDate,
                                     prt.DateCreated,
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified Email Address" };


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

        //Parents details by ID
        public async Task<GenericResponseModel> getParentDetailsByIdAsync(Guid parentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkParentById(parentId);
                if (checkResult == true)
                {
                    var result = from prt in _context.Parents
                                 where prt.Id == parentId && prt.SchoolId == schoolId && prt.CampusId == campusId
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
                                     prt.LastPasswordChangedDate,
                                     prt.LastLoginDate,
                                     prt.LastUpdatedDate,
                                     prt.DateCreated,
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };


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

        //All parents in the school
        public async Task<GenericResponseModel> getAllParentAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    var result = from prt in _context.Parents
                                 where prt.SchoolId == schoolId && prt.CampusId == campusId
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
                                     prt.LastPasswordChangedDate,
                                     prt.LastLoginDate,
                                     prt.LastUpdatedDate,
                                     prt.DateCreated,
                                 };

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };


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

        //All parents children in school
        public async Task<ParentChildResponseModel> getAllParentChildAsync(Guid parentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSch = check.checkSchoolById(schoolId);
                var checkPrt = check.checkParentById(parentId);

                if (checkSch != true)
                {
                    return new ParentChildResponseModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkPrt != true)
                {
                    return new ParentChildResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    //Parent details
                    var parentDetails = from prt in _context.Parents
                                        where prt.SchoolId == schoolId && prt.CampusId == campusId
                                        select new
                                        {
                                            prt.Id,
                                            prt.SchoolId,
                                            prt.CampusId,
                                            prt.Email,
                                            prt.PhoneNumber,
                                            prt.hasChild,
                                            prt.IsActive,
                                            prt.LastPasswordChangedDate,
                                            prt.LastLoginDate,
                                            prt.LastUpdatedDate,
                                            prt.DateCreated,
                                        };

                    //Child List details
                    var childListDetails = from prt in _context.ParentsStudentsMap
                                           where prt.ParentId == parentId && prt.SchoolId == schoolId && prt.CampusId == campusId
                                           select new
                                           {
                                               prt.Id,
                                               prt.SchoolId,
                                               prt.CampusId,
                                               prt.ParentId,
                                               prt.Students.FirstName,
                                               prt.Students.LastName,
                                               prt.Students.UserName,
                                               prt.Students.AdmissionNumber,
                                               prt.Students.IsAssignedToClass,
                                               prt.Students.hasParent,
                                               studentId = prt.Students.Id,
                                               prt.DateCreated,
                                           };

                    return new ParentChildResponseModel { StatusCode = 200, StatusMessage = "Successful", ParentDetails = parentDetails.FirstOrDefault(), childDetails = childListDetails.ToList() };
                }

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ParentChildResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
        //-------------------------------------ChildrenProfile-----------------------------------------------
        public async Task<GenericResponseModel> getChildrenProfileAsync(ChildrenProfileRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(obj.ParentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (obj.ChildrenId.Count > 0)
                    {
                        foreach (Guid childId in obj.ChildrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == obj.ParentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Child List details
                            var childListDetails = from prt in _context.ParentsStudentsMap
                                                   where prt.ParentId == obj.ParentId && prt.StudentId == childId
                                                   select new
                                                   {
                                                       prt.Id,
                                                       prt.SchoolId,
                                                       prt.CampusId,
                                                       prt.ParentId,
                                                       prt.StudentId,
                                                       prt.Students.FirstName,
                                                       prt.Students.LastName,
                                                       prt.Students.MiddleName,
                                                       prt.Students.UserName,
                                                       prt.Students.AdmissionNumber,
                                                       prt.Students.IsAssignedToClass,
                                                       prt.Students.hasParent,
                                                       prt.Students.City,
                                                       prt.Students.DateOfBirth,
                                                       prt.Students.Gender.GenderName,
                                                       prt.Students.GenderId,
                                                       prt.Students.HomeAddress,
                                                       studentId = prt.Students.Id,
                                                       prt.Students.IsActive,
                                                       prt.Students.LastLoginDate,
                                                       prt.Students.LastPasswordChangedDate,
                                                       prt.Students.LastUpdatedDate,
                                                       prt.Students.LocalGovt,
                                                       prt.Students.ProfilePictureUrl,
                                                       prt.Students.Religion,
                                                       prt.Students.SchoolCampuses.CampusName,
                                                       prt.Students.SchoolInformation.SchoolName,
                                                       prt.Students.StaffStatus,
                                                       prt.Students.State,
                                                       prt.Students.StateOfOrigin,
                                                       prt.Students.Status,
                                                       prt.Students.StudentStatus,
                                                       prt.Students.YearOfAdmission,
                                                       prt.DateCreated,
                                                   };

                            data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = childListDetails.FirstOrDefault() });
                        }
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }
        //-------------------------------------ChildrenAttendance-----------------------------------------------
        public async Task<GenericResponseModel> getChildrenAttendanceBySessionIdAsync(IList<Guid> childrenId, Guid parentId, long sessionId)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSession = check.checkSessionById(sessionId);
                if (checkSession != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Session with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Attendance details
                            var attendanceDetails = from atd in _context.StudentAttendance
                                                    where atd.SessionId == sessionId && atd.StudentId == childId
                                                    select new
                                                    {
                                                        atd.Id,
                                                        atd.SchoolId,
                                                        atd.CampusId,
                                                        atd.StudentId,
                                                        atd.AdmissionNumber,
                                                        atd.Students.FirstName,
                                                        atd.Students.LastName,
                                                        atd.Terms.TermName,
                                                        atd.Sessions.SessionName,
                                                        atd.Classes.ClassName,
                                                        atd.ClassGrades.GradeName,
                                                        atd.AttendancePeriodIdMorning,
                                                        atd.AttendancePeriodIdAfternoon,
                                                        atd.AttendanceDate
                                                    };

                            data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() });
                        }
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }

        public async Task<GenericResponseModel> getChildrenAttendanceByTermIdAsync(IList<Guid> childrenId, Guid parentId, long termId)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkTerm = check.checkTermById(termId);
                if (checkTerm != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Term with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Attendance details
                            var attendanceDetails = from atd in _context.StudentAttendance
                                                    where atd.TermId == termId && atd.StudentId == childId
                                                    select new
                                                    {
                                                        atd.Id,
                                                        atd.SchoolId,
                                                        atd.CampusId,
                                                        atd.StudentId,
                                                        atd.AdmissionNumber,
                                                        atd.Students.FirstName,
                                                        atd.Students.LastName,
                                                        atd.Terms.TermName,
                                                        atd.Sessions.SessionName,
                                                        atd.Classes.ClassName,
                                                        atd.ClassGrades.GradeName,
                                                        atd.AttendancePeriodIdMorning,
                                                        atd.AttendancePeriodIdAfternoon,
                                                        atd.AttendanceDate
                                                    };

                            data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() });
                        }
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }

        public async Task<GenericResponseModel> getChildrenAttendanceByDateAsync(IList<Guid> childrenId, Guid parentId, DateTime startDate, DateTime endDate)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Attendance details
                            var attendanceDetails = from atd in _context.StudentAttendance
                                                    where atd.StudentId == childId && atd.AttendanceDate >= startDate && atd.AttendanceDate < endDate.AddDays(1)
                                                    select new
                                                    {
                                                        atd.Id,
                                                        atd.SchoolId,
                                                        atd.CampusId,
                                                        atd.StudentId,
                                                        atd.AdmissionNumber,
                                                        atd.Students.FirstName,
                                                        atd.Students.LastName,
                                                        atd.Terms.TermName,
                                                        atd.Sessions.SessionName,
                                                        atd.Classes.ClassName,
                                                        atd.ClassGrades.GradeName,
                                                        atd.AttendancePeriodIdMorning,
                                                        atd.AttendancePeriodIdAfternoon,
                                                        atd.AttendanceDate
                                                    };

                            data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() });
                        }
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }
        //------------------------------------------ChildrenSubject--------------------------------------------------
        public async Task<GenericResponseModel> getChildrenSubjectAsync(IList<Guid> childrenId, Guid parentId)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }
                            var childGrade = _context.GradeStudents.Where(x => x.StudentId == childId).FirstOrDefault();
                            if (childGrade != null)
                            {

                                //Subject
                                var subjects = from sub in _context.SchoolSubjects
                                               where sub.ClassId == childGrade.ClassId
                                               select new
                                               {
                                                   sub.Classes.CampusId,
                                                   sub.ClassId,
                                                   sub.DepartmentId,
                                                   sub.Id,
                                                   sub.IsActive,
                                                   sub.SchoolCampuses.CampusName,
                                                   sub.SchoolId,
                                                   sub.SchoolInformation.SchoolName,
                                                   sub.SubjectCode,
                                                   sub.SubjectDepartment.DepartmentName,
                                                   sub.SubjectName
                                               };


                                data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = subjects.ToList() });
                            }
                            else
                            {
                                data.Add(new GenericResponseModel { StatusCode = 405, StatusMessage = "Child has not been assigned to any Grade Class" });
                            }
                        }
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }
        //-------------------------------------ChildAttendance-----------------------------------------------
        public async Task<GenericResponseModel> getChildAttendanceBySessionIdAsync(Guid childId, Guid parentId, long sessionId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSession = check.checkSessionById(sessionId);
                if (checkSession != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Session with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 405, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }

                //Attendance details
                var attendanceDetails = from atd in _context.StudentAttendance
                                        where atd.SessionId == sessionId && atd.StudentId == childId
                                        select new
                                        {
                                            atd.Id,
                                            atd.SchoolId,
                                            atd.CampusId,
                                            atd.StudentId,
                                            atd.AdmissionNumber,
                                            atd.Students.FirstName,
                                            atd.Students.LastName,
                                            atd.Terms.TermName,
                                            atd.Sessions.SessionName,
                                            atd.Classes.ClassName,
                                            atd.ClassGrades.GradeName,
                                            atd.AttendancePeriodIdMorning,
                                            atd.AttendancePeriodIdAfternoon,
                                            atd.AttendanceDate
                                        };

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() };

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

        public async Task<GenericResponseModel> getChildAttendanceByTermIdAsync(Guid childId, Guid parentId, long termId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkTerm = check.checkTermById(termId);
                if (checkTerm != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Term with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 405, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }

                //Attendance details
                var attendanceDetails = from atd in _context.StudentAttendance
                                        where atd.TermId == termId && atd.StudentId == childId
                                        select new
                                        {
                                            atd.Id,
                                            atd.SchoolId,
                                            atd.CampusId,
                                            atd.StudentId,
                                            atd.AdmissionNumber,
                                            atd.Students.FirstName,
                                            atd.Students.LastName,
                                            atd.Terms.TermName,
                                            atd.Sessions.SessionName,
                                            atd.Classes.ClassName,
                                            atd.ClassGrades.GradeName,
                                            atd.AttendancePeriodIdMorning,
                                            atd.AttendancePeriodIdAfternoon,
                                            atd.AttendanceDate
                                        };

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() };

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

        public async Task<GenericResponseModel> getChildAttendanceByDateAsync(Guid childId, Guid parentId, DateTime startDate, DateTime endDate)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 405, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }

                //Attendance details
                var attendanceDetails = from atd in _context.StudentAttendance
                                        where atd.StudentId == childId && atd.AttendanceDate >= startDate && atd.AttendanceDate < endDate.AddDays(1)
                                        select new
                                        {
                                            atd.Id,
                                            atd.SchoolId,
                                            atd.CampusId,
                                            atd.StudentId,
                                            atd.AdmissionNumber,
                                            atd.Students.FirstName,
                                            atd.Students.LastName,
                                            atd.Terms.TermName,
                                            atd.Sessions.SessionName,
                                            atd.Classes.ClassName,
                                            atd.ClassGrades.GradeName,
                                            atd.AttendancePeriodIdMorning,
                                            atd.AttendancePeriodIdAfternoon,
                                            atd.AttendanceDate
                                        };

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() };

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
        //------------------------------------------ChildSubject--------------------------------------------------
        public async Task<GenericResponseModel> getChildSubjectAsync(Guid childId, Guid parentId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericResponseModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericResponseModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }
                var childGrade = _context.GradeStudents.Where(x => x.StudentId == childId).FirstOrDefault();
                if (childGrade != null)
                {

                    //Subject
                    var subjects = from sub in _context.SchoolSubjects
                                   where sub.ClassId == childGrade.ClassId
                                   select new
                                   {
                                       sub.Classes.CampusId,
                                       sub.ClassId,
                                       sub.DepartmentId,
                                       sub.Id,
                                       sub.IsActive,
                                       sub.SchoolCampuses.CampusName,
                                       sub.SchoolId,
                                       sub.SchoolInformation.SchoolName,
                                       sub.SubjectCode,
                                       sub.SubjectDepartment.DepartmentName,
                                       sub.SubjectName
                                   };


                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = subjects.ToList() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 405, StatusMessage = "Child has not been assigned to any Grade Class" };
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
    }
}
