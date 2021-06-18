using Microsoft.EntityFrameworkCore;
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
    public class SchoolRepo : ISchoolRepo
    {
        private readonly AppDbContext _context;
        private IConfiguration _config;
        private readonly EmailTemplate _emailTemplate;
        private readonly IEmailRepo _emailRepo;

        public SchoolRepo(AppDbContext context, IEmailRepo emailRepo, IConfiguration config, EmailTemplate emailTemplate)
        {
            _context = context;
            _config = config;
            this._emailTemplate = emailTemplate;
            _emailRepo = emailRepo;
        }

        //default password
        private static string defaultPassword()
        {
            return "uwuwjjfhdyg73838FG#";
        }

        public async Task<SchoolSignUpResponseModel> schoolSignUpAsync(SchoolSignUpRequestModel obj)
        {
            try
            {
                SchoolUserInfoResponseModel schUserRespData = new SchoolUserInfoResponseModel();
                SchoolBasicInfoResponseModel schBasicRespData = new SchoolBasicInfoResponseModel();

                SchoolSignUpResponseModel respObj = new SchoolSignUpResponseModel();
                Jwt jwtObj = new Jwt(_config);


                CheckerValidation checker = new CheckerValidation(_context);
                var emailCheckResult = checker.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                var accountCheckResult = checker.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                //the school name
                var schoolNameCheckResult = checker.checkIfSchoolNameExist(obj.SchoolName);
                //the school Code
                var schoolCodeCheckResult = checker.checkIfSchoolCodeExist(obj.SchoolCode);


                if (schoolNameCheckResult == true)
                {
                    return new SchoolSignUpResponseModel { StatusCode = 409, StatusMessage = "This School Name Already Exists!" };
                }
                if (schoolCodeCheckResult == true)
                {
                    return new SchoolSignUpResponseModel { StatusCode = 409, StatusMessage = "A School Already Uses This Code!" };
                }
                if (emailCheckResult == true)
                {
                    return new SchoolSignUpResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken!" };
                }
                if (accountCheckResult == true)
                {
                    return new SchoolSignUpResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                }
                else
                {
                    //school info
                    var newSch = new SchoolInformation
                    {
                        SchoolName = obj.SchoolName,
                        SchoolCode = obj.SchoolCode,
                        SchoolTypeId = obj.SchoolTypeId,
                        IsActive = true,
                        IsApproved = false,
                        IsVerified = false,
                        DateCreated = DateTime.Now
                    };
                    await _context.SchoolInformation.AddAsync(newSch);
                    await _context.SaveChangesAsync();


                    //Campus info
                    var camp = new SchoolCampuses
                    {
                        SchoolId = newSch.Id,
                        CampusName = obj.CampusName,
                        CampusAddress = obj.CampusAddress,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };
                    await _context.SchoolCampuses.AddAsync(camp);
                    await _context.SaveChangesAsync();

                    //Password Security
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(obj.Password, salt);

                    //save the School User details
                    var schUsr = new SchoolUsers
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Email = obj.Email,
                        EmailConfirmed = false,
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = newSch.Id,
                        CampusId = camp.Id,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUsers.AddAsync(schUsr);
                    await _context.SaveChangesAsync();

                    //schoolUser Role
                    var usrRol = new SchoolUserRoles
                    {
                        UserId = schUsr.Id,
                        RoleId = Convert.ToInt64(EnumUtility.SchoolRoles.SuperAdministrator),
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUserRoles.AddAsync(usrRol);
                    await _context.SaveChangesAsync();

                    //generate the code for email confirmation
                    var confirmationCode = new RandomNumberGenerator();
                    string codeGenerated = confirmationCode.randomCodesGen();

                    //save the code generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = schUsr.Id,
                        Code = codeGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();


                    //code to send Mail to user for account activation
                    //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";
                    var subject = "ExpertPlat Registration";
                    string MailContent = _emailTemplate.EmailContent(schUsr.FirstName, codeGenerated);

                    EmailMessage message = new EmailMessage(schUsr.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);

                    //The data collected from the user 
                    schUserRespData.UserId = schUsr.Id.ToString();
                    schUserRespData.FirstName = schUsr.FirstName;
                    schUserRespData.LastName = schUsr.LastName;
                    schUserRespData.Email = schUsr.Email;
                    schUserRespData.PhoneNumber = schUsr.PhoneNumber;


                    //Get the schoolType Name
                    var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == newSch.SchoolTypeId);

                    //The data collected from the School Information
                    schBasicRespData.SchoolSuperAdministratorId = schUsr.Id;
                    schBasicRespData.SchoolId = newSch.Id;
                    schBasicRespData.SchoolName = newSch.SchoolName;
                    schBasicRespData.SchoolCode = newSch.SchoolCode;
                    schBasicRespData.SchoolTypeId = newSch.SchoolTypeId;
                    schBasicRespData.SchoolTypeName = getSchType.SchoolTypeName;


                    //Generate JSON WEB TOKEN for a valid User
                    var tokenString = jwtObj.GenerateJWTToken();

                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "Account created successfully, use the code sent to your mail to activate and verify your account!";
                    respObj.Token = tokenString;
                    respObj.AdministratorDetails = schUserRespData;
                    respObj.SchoolDetails = schBasicRespData;

                }

                return respObj;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SchoolSignUpResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }


        public async Task<GenericResponseModel> activateAccountAsync(AccountActivationRequestModel obj)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var emailCheckResult = emailcheck.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true && accountCheckResult == false) //check if email exist and account has been verified and activated
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true) //email exist
                {
                    SchoolUsers getUser = _context.SchoolUsers.FirstOrDefault(u => u.Email == obj.Email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);

                    if (getUserCode != null && getUserCode.Code == obj.Code.Trim())
                    {
                        getUser.EmailConfirmed = true; //Update the user account as confirmed (EmailConfirmed set to true)

                        _context.EmailConfirmationCodes.Remove(getUserCode);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Code Entered!" };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Verification Successful!" };
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


        public async Task<GenericResponseModel> resendActivationCodeAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    SchoolUsers getUser = _context.SchoolUsers.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);
                    string codeGenerated = string.Empty;
                    var subject = "ExpertPlat Registration";

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        codeGenerated = getUserCode.Code;

                        //send Mail to user for account activation
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                        string MailContent = _emailTemplate.EmailContent(getUser.FirstName, codeGenerated);

                        EmailMessage message = new EmailMessage(getUser.Email, MailContent, subject);
                        _emailRepo.SendEmail(message);

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Activation Code Sent Successfully!" };

                    }
                    else
                    {
                        //generate a new code for email confirmation and account activation
                        var confirmationCode = new RandomNumberGenerator();
                        codeGenerated = confirmationCode.randomCodesGen();

                        //save the code generated
                        var emailConfirmation = new EmailConfirmationCodes
                        {
                            UserId = getUser.Id,
                            Code = codeGenerated,
                            DateGenerated = DateTime.Now
                        };
                        await _context.AddAsync(emailConfirmation);
                        await _context.SaveChangesAsync();

                        //EmailTemplate emailTemp = new EmailTemplate();
                        //var MailContent = emailTemp.EmailHtmlTemplate(codeGenerated);
                        //code to send Mail to user for account activation
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";
                        string MailContent = _emailTemplate.EmailContent(getUser.FirstName, codeGenerated);

                        EmailMessage message = new EmailMessage(getUser.Email, MailContent, subject);
                        _emailRepo.SendEmail(message);

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Activation Code Sent Successfully!" };

                    }

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


        //------------------------- SCHOOL CAMPUS --------------------------------------------------------------
        public async Task<GenericResponseModel> createSchoolCampusAsync(SchoolCampusCreateRequestModel obj)
        {
            try
            {
                CheckerValidation checker = new CheckerValidation(_context);
                var campusNameCheckResult = checker.checkIfSchoolCampusNameExist(obj.CampusName);

                if (campusNameCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Campus Name Already Exists" };
                }
                else
                {
                    //Campus info
                    var camp = new SchoolCampuses
                    {
                        SchoolId = obj.SchoolId,
                        CampusName = obj.CampusName,
                        CampusAddress = obj.CampusAddress,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };
                    await _context.SchoolCampuses.AddAsync(camp);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Campus Created Successfully" };

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


        public async Task<GenericResponseModel> getAllSchoolCampusAsync(long schoolId)
        {
            try
            {
                var result = from camp in _context.SchoolCampuses
                             where camp.SchoolId == schoolId
                             select new
                             {
                                 camp.Id,
                                 camp.SchoolId,
                                 camp.CampusName,
                                 camp.CampusAddress,
                                 camp.IsActive,
                                 camp.DateCreated
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getSchoolCampusByIdAsync(long campusId)
        {
            try
            {
                var result = from camp in _context.SchoolCampuses
                             where camp.Id == campusId
                             select new
                             {
                                 camp.Id,
                                 camp.SchoolId,
                                 camp.CampusName,
                                 camp.CampusAddress,
                                 camp.IsActive,
                                 camp.DateCreated
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
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        //------------------------ SCHOOL USERS --------------------------------------------------
        public async Task<SchoolUsersCreateResponseModel> createSchoolUsersAsync(SchoolUsersCreateRequestModel obj)
        {
            try
            {
                //generate the code for email confirmation
                var confirmationCode = new RandomNumberGenerator();
                string passwordGenerated = confirmationCode.randomCodesGen();

                SchoolUserInfoResponseModel userRespData = new SchoolUserInfoResponseModel();
                SchoolBasicInfoLoginResponseModel schData = new SchoolBasicInfoLoginResponseModel();

                SchoolUsersCreateResponseModel respObj = new SchoolUsersCreateResponseModel();

                CheckerValidation checker = new CheckerValidation(_context);
                var emailCheckResult = checker.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true)
                {
                    return new SchoolUsersCreateResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken!" };
                }
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(passwordGenerated, salt);

                    //save the SchoolAdmin details
                    var schUsr = new SchoolUsers
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Email = obj.Email,
                        EmailConfirmed = false,
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUsers.AddAsync(schUsr);
                    await _context.SaveChangesAsync();

                    //save the SchoolUser Roles
                    foreach (var roleId in obj.RoleIds)
                    {
                        var usrRol = new SchoolUserRoles
                        {
                            UserId = schUsr.Id,
                            RoleId = roleId.Id,
                            DateCreated = DateTime.Now
                        };

                        await _context.SchoolUserRoles.AddAsync(usrRol);
                        await _context.SaveChangesAsync();
                    }

                    //The data collected from the user 
                    userRespData.UserId = schUsr.Id.ToString();
                    userRespData.FirstName = schUsr.FirstName;
                    userRespData.LastName = schUsr.LastName;
                    userRespData.Email = schUsr.Email;

                    //Gets the School Information
                    var userSchool = _context.SchoolInformation.FirstOrDefault(u => u.Id == obj.SchoolId);
                    //Get the schoolType Name
                    var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                    //Get the Campus Name
                    var getCampus = _context.SchoolCampuses.FirstOrDefault(u => u.Id == obj.CampusId);
                    //Get Roles Assigned to Teacher (TeacherRoles)
                    var getRoles = from rol in _context.SchoolUserRoles
                                   where rol.UserId == schUsr.Id
                                   select new
                                   {
                                       rol.Id,
                                       rol.UserId,
                                       rol.RoleId,
                                       rol.SchoolRoles.RoleName
                                   };

                    //school Details
                    schData.SchoolId = userSchool.Id;
                    schData.SchoolName = userSchool.SchoolName;
                    schData.SchoolCode = userSchool.SchoolCode;
                    schData.SchoolTypeName = getSchType.SchoolTypeName;
                    schData.CampusName = getCampus.CampusName;
                    schData.CampusAddress = getCampus.CampusAddress;


                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "User Created Successfully";
                    respObj.SchoolDetails = schData;
                    respObj.SchoolUserDetails = userRespData;
                    respObj.Roles = getRoles.ToList();


                    //save the link generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = schUsr.Id,
                        Code = passwordGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();

                    string link = _config["SchoolModule:Url"] + "setpassword?userId=" + schUsr.Id + "&userType=schooluser&sessionId=" + emailConfirmation.Id;
                    emailConfirmation.Link = link;
                    await _context.SaveChangesAsync();

                    var subject = "ExpertPlat Registration";
                    string MailContent = _emailTemplate.SchoolUserPasswordReset(obj.FirstName, link);

                    EmailMessage message = new EmailMessage(obj.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);

                }

                return respObj;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SchoolUsersCreateResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> resetSchoolUserPasswordAsync(Guid userId, long sessionId, string newPassword, string userType)
        {
            try
            {
                if (userType == "schooluser")
                {
                    //Get user by Id
                    var schoolUser = await _context.SchoolUsers.Where(x => x.Id == userId).FirstOrDefaultAsync();
                    if (schoolUser == null)
                    {
                        return new GenericResponseModel { StatusCode = 404, StatusMessage = "User doesn't exist" };
                    }
                    var emailConfirmation = await _context.EmailConfirmationCodes.Where(x => x.UserId == userId && x.Id == sessionId).FirstOrDefaultAsync();
                    if (emailConfirmation == null)
                    {
                        return new GenericResponseModel { StatusCode = 405, StatusMessage = "Invalid sessionId or UserId" };
                    }
                    //Hash the old password and compare with the hashed password in the db
                    var paswordHasher = new PasswordHasher();
                    //Hash the password and salt
                    string oldPasswordHash = paswordHasher.hashedPassword(emailConfirmation.Code, schoolUser.Salt);
                    if (oldPasswordHash != schoolUser.PasswordHash)
                    {
                        return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Old Password" };
                    }
                    //Hash the new password with old salt
                    string newPasswordHash = paswordHasher.hashedPassword(newPassword, schoolUser.Salt);

                    //Update the user password
                    schoolUser.PasswordHash = newPasswordHash;
                    schoolUser.LastPasswordChangedDate = DateTime.Now;
                    schoolUser.EmailConfirmed = true;

                    //delete the forgotpassword code after successful Update

                    _context.EmailConfirmationCodes.Remove(emailConfirmation);

                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Password Changed Successfully" };
                }
                else if (userType == "student")
                {
                    //Get user by Id
                    var student = await _context.Students.Where(x => x.Id == userId).FirstOrDefaultAsync();
                    if (student == null)
                    {
                        return new GenericResponseModel { StatusCode = 404, StatusMessage = "Student doesn't exist" };
                    }
                    var emailConfirmation = await _context.EmailConfirmationCodes.Where(x => x.UserId == userId && x.Id == sessionId).FirstOrDefaultAsync();
                    if (emailConfirmation == null)
                    {
                        return new GenericResponseModel { StatusCode = 405, StatusMessage = "Invalid sessionId or UserId" };
                    }
                    //Hash the old password and compare with the hashed password in the db
                    var paswordHasher = new PasswordHasher();
                    //Hash the password and salt
                    string oldPasswordHash = paswordHasher.hashedPassword(emailConfirmation.Code, student.Salt);
                    if (oldPasswordHash != student.PasswordHash)
                    {
                        return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Old Password" };
                    }
                    //Hash the new password with old salt
                    string newPasswordHash = paswordHasher.hashedPassword(newPassword, student.Salt);

                    //Update the user password
                    student.PasswordHash = newPasswordHash;
                    student.LastPasswordChangedDate = DateTime.Now;
                    student.EmailConfirmed = true;

                    //delete the forgotpassword code after successful Update
                    
                        _context.EmailConfirmationCodes.Remove(emailConfirmation);
                    
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Password Changed Successfully" };
                }
                else if (userType == "parent")
                {
                    //Get user by Id
                    var parent = await _context.Parents.Where(x => x.Id == userId).FirstOrDefaultAsync();
                    if (parent == null)
                    {
                        return new GenericResponseModel { StatusCode = 404, StatusMessage = "Parent doesn't exist" };
                    }
                    var emailConfirmation = await _context.EmailConfirmationCodes.Where(x => x.UserId == userId && x.Id == sessionId).FirstOrDefaultAsync();
                    if (emailConfirmation == null)
                    {
                        return new GenericResponseModel { StatusCode = 405, StatusMessage = "Invalid sessionId or UserId" };
                    }
                    //Hash the old password and compare with the hashed password in the db
                    var paswordHasher = new PasswordHasher();
                    //Hash the password and salt
                    string oldPasswordHash = paswordHasher.hashedPassword(emailConfirmation.Code, parent.Salt);
                    if (oldPasswordHash != parent.PasswordHash)
                    {
                        return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Old Password" };
                    }
                    //Hash the new password with old salt
                    string newPasswordHash = paswordHasher.hashedPassword(newPassword, parent.Salt);

                    //Update the user password
                    parent.PasswordHash = newPasswordHash;
                    parent.LastPasswordChangedDate = DateTime.Now;
                    parent.EmailConfirmed = true;

                    //delete the forgotpassword code after successful Update
                        _context.EmailConfirmationCodes.Remove(emailConfirmation);
                    
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Password Changed Successfully" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "User Type is invalid!" };
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
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    SchoolUsers getUser = _context.SchoolUsers.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);
                    string linkGenerated = string.Empty;
                    var subject = "ExpertPlat Registration";

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        linkGenerated = getUserCode.Link;
                        //send Mail to user for account activation
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                        string MailContent = _emailTemplate.SchoolUserPasswordReset(getUser.FirstName, linkGenerated);

                        EmailMessage message = new EmailMessage(getUser.Email, MailContent, subject);
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

        //---------------------------SCHOOL USERS LOGIN---------------------------------------------------

        public async Task<SchoolUsersLoginResponseModel> schoolUserLoginAsync(LoginRequestModel obj)
        {
            try
            {
                //user data and schoolBasicInfo data objects
                SchoolBasicInfoLoginResponseModel schData = new SchoolBasicInfoLoginResponseModel();
                SchoolUserInfoResponseModel userData = new SchoolUserInfoResponseModel();

                //final data to be sent as response to the client
                SchoolUsersLoginResponseModel respData = new SchoolUsersLoginResponseModel();

                //JWT
                Jwt jwtObj = new Jwt(_config);

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.SchoolUsers.Where(u => u.Email == obj.Email).FirstOrDefault();


                if (getUser != null)
                {
                    var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(getUser.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                    //get the school user Role
                    var getSchUserRoleId = _context.SchoolUserRoles.Where(r => r.UserId == getUser.Id).FirstOrDefault();

                    //get the school user RoleId
                    long schUserRoleId = (long)getSchUserRoleId.RoleId;


                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    if (getUser.PasswordHash != decryptedPassword)
                    {
                        return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    if (accountCheckResult == true)
                    {
                        return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated, kindly check your email!" };
                    }
                    else
                    {
                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
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

                        var getRole = from rol in _context.SchoolUserRoles
                                      where rol.UserId == getUser.Id
                                      select new
                                      {
                                          rol.UserId,
                                          rol.RoleId,
                                          rol.SchoolRoles.RoleName
                                      };

                        schData.SchoolId = userSchool.Id;
                        schData.SchoolName = userSchool.SchoolName;
                        schData.SchoolCode = userSchool.SchoolCode;
                        schData.SchoolTypeName = getSchType.SchoolTypeName;
                        schData.SchoolLogoUrl = userSchool.SchoolLogoUrl;
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
                        respData.Roles = getRole.ToList();

                    }
                }
                else
                {
                    return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username" };
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

        public async Task<GenericResponseModel> getSchoolUsersByRoleIdAsync(long schoolId, long campusId, long roleId)
        {
            try
            {
                var result = from usRol in _context.SchoolUserRoles
                             join usr in _context.SchoolUsers on usRol.UserId equals usr.Id
                             where usr.SchoolId == schoolId
&& usr.CampusId == campusId && usRol.RoleId == roleId
                             select usr;

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

        public async Task<GenericResponseModel> getAllSchoolRolesAsync()
        {
            try
            {
                var result = from rol in _context.SchoolRoles select rol;

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

        public async Task<GenericResponseModel> getSchoolRolesByRoleIdAsync(long schoolRoleId)
        {
            try
            {
                var result = from rol in _context.SchoolRoles where rol.Id == schoolRoleId select rol;

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

        //all School roles excluding parents superAdministrator, students, class and Subject Teacher
        public async Task<GenericResponseModel> getSchoolRolesForSchoolUserCreationAsync()
        {
            try
            {
                var result = from rol in _context.SchoolRoles
                             where rol.Id != Convert.ToInt64(EnumUtility.SchoolRoles.ClassTeacher)
                            && rol.Id != Convert.ToInt64(EnumUtility.SchoolRoles.SubjectTeacher)
                            && rol.Id != Convert.ToInt64(EnumUtility.SchoolRoles.SuperAdministrator)
                             select rol;

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


        public async Task<GenericResponseModel> assignRolesToSchoolUsersAsync(AssignRolesToSchoolUsersRequestModel obj)
        {
            try
            {
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == obj.SchoolUserId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    foreach (var rol in obj.RoleIds)
                    {
                        var getRoles = _context.SchoolUserRoles.Where(s => s.UserId == getSchUser.Id && s.RoleId == rol.Id).FirstOrDefault();

                        if (getRoles != null)
                        {
                            //update the roles
                            getRoles.UserId = obj.SchoolUserId;
                            getRoles.RoleId = rol.Id;

                            await _context.SaveChangesAsync();

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Role(s) Assigned to User Updated Successfully", };
                        }
                        else
                        {
                            //save new Role
                            var usrRol = new SchoolUserRoles
                            {
                                UserId = obj.SchoolUserId,
                                RoleId = rol.Id,
                                DateCreated = DateTime.Now
                            };

                            await _context.SchoolUserRoles.AddAsync(usrRol);
                            await _context.SaveChangesAsync();

                            return new GenericResponseModel { StatusCode = 200, StatusMessage = "Role(s) Assigned to User Successfully", };

                        }
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "User Details Updated Successfully", };

                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No User With the Specified ID", };
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


        public async Task<GenericResponseModel> updateSchoolUserDetailsAsync(Guid schoolUserId, UpdateSchoolUsersDetailsRequestModel obj)
        {
            try
            {
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == schoolUserId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    getSchUser.FirstName = obj.FirstName;
                    getSchUser.LastName = obj.LastName;
                    getSchUser.PhoneNumber = obj.PhoneNumber;
                    getSchUser.SchoolId = obj.SchoolId;
                    getSchUser.CampusId = obj.CampusId;

                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "User Details Updated Successfully", };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No User With the Specified ID", };
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


        public async Task<GenericResponseModel> updateSchoolDetailsAsync(long schoolId, UpdateSchoolDetailsRequestModel obj)
        {
            try
            {
                var getSch = _context.SchoolInformation.Where(s => s.Id == schoolId).FirstOrDefault();
                if (getSch != null)
                {
                    CheckerValidation chk = new CheckerValidation(_context);
                    //Check if another school is using the school name
                    var checkSchool = await _context.SchoolInformation.Where(x => x.SchoolName == obj.SchoolName && x.Id != schoolId).FirstOrDefaultAsync();

                    if (checkSchool != null)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "A School with this Name Already Exists for another school", };
                    }
                    else
                    {
                        getSch.SchoolName = obj.SchoolName;
                        getSch.SchoolLogoUrl = obj.SchoolLogouRL;

                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "School Details Updated Successfully", };
                    }

                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No School With the Specified ID", };
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

        public async Task<GenericResponseModel> updateCampusDetailsAsync(long campusId, SchoolCampusCreateRequestModel obj)
        {

            try
            {
                var getCamp = _context.SchoolCampuses.Where(s => s.Id == campusId).FirstOrDefault();
                if (getCamp != null)
                {
                    CheckerValidation chk = new CheckerValidation(_context);
                    var campNameExist = chk.checkIfSchoolCampusNameExist(obj.CampusName);

                    if (campNameExist == true)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "A SchoolCampus with this Name Already Exists", };
                    }
                    else
                    {
                        getCamp.SchoolId = obj.SchoolId;
                        getCamp.CampusName = obj.CampusName;
                        getCamp.CampusAddress = obj.CampusAddress;

                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Campus Details Updated Successfully", };
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Campus With the Specified ID", };
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

        public async Task<GenericResponseModel> deleteSchoolUsersAsync(Guid schoolUserId, long schoolId, long campusId)
        {
            try
            {
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == schoolUserId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    _context.SchoolUsers.Remove(getSchUser);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "User Details Deleted Successfully", };

                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No User With the Specified ID", };
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

        public async Task<GenericResponseModel> deleteRolesAssignedToSchoolUsersAsync(DeleteRolesAssignedToSchoolUsersRequestModel obj)
        {
            try
            {
                //if user exists
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == obj.SchoolUserId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    //the roles assigned
                    var getUserRole = _context.SchoolUserRoles.Where(s => s.UserId == obj.SchoolUserId && s.RoleId == obj.RoleId).FirstOrDefault();
                    if (getUserRole != null)
                    {
                        _context.SchoolUserRoles.Remove(getUserRole);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Role Assigned to User Deleted Successfully", };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "No User/Role With the Specified ID", };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No User With the Specified ID", };
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
