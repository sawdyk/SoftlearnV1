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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SoftLearnV1.Repositories
{
    public class FacilitatorRepo : IFacilitatorRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private IConfiguration _config;
        private readonly EmailTemplate _emailTemplate;

        public FacilitatorRepo(AppDbContext context, IEmailRepo emailRepo, IConfiguration config, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _config = config;
            this._emailTemplate = emailTemplate;
        }

        public async Task<GenericResponseModel> facilitatorSignUpAsync(FacilitatorRegRequestModel newFac)
        {
            try
            {
                FacilitatorRegResponseModel respData = new FacilitatorRegResponseModel();
                GenericResponseModel respObj = new GenericResponseModel();
                Jwt jwtObj = new Jwt(_config);


                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(newFac.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(newFac.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                if (emailCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken" };
                }
                if (accountCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated" };
                }
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(newFac.Password, salt);

                    //save the facilitator details
                    var fac = new Facilitators
                    {
                        FirstName = newFac.FirstName,
                        LastName = newFac.LastName,
                        UserName = newFac.Email,
                        Email = newFac.Email,
                        EmailConfirmed = false,
                        PhoneNumber = newFac.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        FacilitatorTypeId = (int)EnumUtility.FacilitatorType.External,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Facilitators.AddAsync(fac);
                    await _context.SaveChangesAsync();

                    //generate the code for email confirmation
                    var confirmationCode = new RandomNumberGenerator();
                    string codeGenerated = confirmationCode.randomCodesGen();

                    //save the code generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = fac.Id,
                        Code = codeGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();

                    //EmailTemplate emailTemp = new EmailTemplate();
                    //var MailContent = emailTemp.EmailHtmlTemplate(codeGenerated);
                    //code to send Mail to user for account activation
                    var subject = "ExpertPlat Registration";
                    string MailContent = _emailTemplate.EmailContent(fac.FirstName, codeGenerated);
                    //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                    EmailMessage message = new EmailMessage(fac.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);

                    //The data collected from the user on successful creation
                    respData.UserId = fac.Id.ToString();
                    respData.FirstName = fac.FirstName;
                    respData.LastName = fac.LastName;
                    respData.Email = fac.Email;

                    //Generate JSON WEB TOKEN for a valid User
                    var tokenString = jwtObj.GenerateJWTToken();

                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "Account created successfully, use the code sent to your mail to activate and verify your account";
                    respObj.Token = tokenString;
                    respObj.Data = respData;

                }

                return respObj;
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

        public async Task<GenericResponseModel> facilitatorSignUpInternalAsync(InternalFacilitatorRegRequestModel newFac)
        {
            try
            {
                //generate the code/password for email confirmation
                var confirmationCode = new RandomNumberGenerator();
                string passwordGenerated = confirmationCode.randomCodesGen();

                FacilitatorRegResponseModel respData = new FacilitatorRegResponseModel();
                GenericResponseModel respObj = new GenericResponseModel();
                Jwt jwtObj = new Jwt(_config);


                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(newFac.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(newFac.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                if (emailCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken" };
                }
                if (accountCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated" };
                }
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(passwordGenerated, salt);

                    //save the facilitator details
                    var fac = new Facilitators
                    {
                        FirstName = newFac.FirstName,
                        LastName = newFac.LastName,
                        UserName = newFac.Email,
                        Email = newFac.Email,
                        EmailConfirmed = false,
                        PhoneNumber = newFac.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        FacilitatorTypeId = (int)EnumUtility.FacilitatorType.Internal,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Facilitators.AddAsync(fac);
                    await _context.SaveChangesAsync();


                    //save the link generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = fac.Id,
                        Code = passwordGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();

                    string link = _config["LearningModule:Url"] + "reset-password?userId=" + fac.Id + "&userType=internalFacilitator&sessionId=" + emailConfirmation.Id;
                    emailConfirmation.Link = link;
                    await _context.SaveChangesAsync();

                    var subject = "ExpertPlat Registration";
                    string MailContent = _emailTemplate.SchoolUserPasswordReset(fac.FirstName, link);

                    EmailMessage message = new EmailMessage(fac.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);


                    //The data collected from the user on successful creation
                    respData.UserId = fac.Id.ToString();
                    respData.FirstName = fac.FirstName;
                    respData.LastName = fac.LastName;
                    respData.Email = fac.Email;

                    //Generate JSON WEB TOKEN for a valid User
                    var tokenString = jwtObj.GenerateJWTToken();

                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "Account created successfully, use the code sent to your mail to activate and verify your account";
                    respObj.Token = tokenString;
                    respObj.Data = respData;

                }

                return respObj;
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
        public async Task<GenericResponseModel> resetPasswordAsync(Guid userId, long sessionId, string newPassword, string userType)
        {
            try
            {
                if (userType.ToLower() == "internalfacilitator")
                {
                    //Get user by Id
                    var facilitator = await _context.Facilitators.Where(x => x.Id == userId).FirstOrDefaultAsync();
                    if (facilitator == null)
                    {
                        return new GenericResponseModel { StatusCode = 404, StatusMessage = "Facilitator doesn't exist" };
                    }
                    var emailConfirmation = await _context.EmailConfirmationCodes.Where(x => x.UserId == userId && x.Id == sessionId).FirstOrDefaultAsync();
                    if (emailConfirmation == null)
                    {
                        return new GenericResponseModel { StatusCode = 405, StatusMessage = "Invalid sessionId or UserId" };
                    }
                    //Hash the old password and compare with the hashed password in the db
                    var paswordHasher = new PasswordHasher();
                    //Hash the password and salt
                    string oldPasswordHash = paswordHasher.hashedPassword(emailConfirmation.Code, facilitator.Salt);
                    if (oldPasswordHash != facilitator.PasswordHash)
                    {
                        return new GenericResponseModel { StatusCode = 400, StatusMessage = "Invalid Old Password" };
                    }
                    //Hash the new password with old salt
                    string newPasswordHash = paswordHasher.hashedPassword(newPassword, facilitator.Salt);

                    //Update the user password
                    facilitator.PasswordHash = newPasswordHash;
                    facilitator.LastPasswordChangedDate = DateTime.Now;
                    facilitator.EmailConfirmed = true;

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
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    Facilitators facilitators = _context.Facilitators.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == facilitators.Id);
                    string linkGenerated = string.Empty;
                    var subject = "ExpertPlat Registration";

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        linkGenerated = getUserCode.Link;
                        //send Mail to user for account activation
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                        string MailContent = _emailTemplate.SchoolUserPasswordReset(facilitators.FirstName, linkGenerated);

                        EmailMessage message = new EmailMessage(facilitators.Email, MailContent, subject);
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

        public async Task<GenericResponseModel> facilitatorLoginAsync(LoginRequestModel obj)
        {
            try
            {
                FacilitatorLoginResponseModel respData = new FacilitatorLoginResponseModel();
                GenericResponseModel respObj = new GenericResponseModel();
                Jwt jwtObj = new Jwt(_config);

                CheckerValidation emailcheck = new CheckerValidation(_context);

                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));
                var getUser = _context.Facilitators.FirstOrDefault(u => u.Email == obj.Email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password" };
                    }
                    if (getUser != null && getUser.PasswordHash == decryptedPassword && accountCheckResult == true)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated" };
                    }
                    else
                    {
                        respData.UserId = getUser.Id.ToString();
                        respData.FirstName = getUser.FirstName;
                        respData.LastName = getUser.LastName;
                        respData.Email = getUser.Email;
                        respData.EmailConfirmed = getUser.EmailConfirmed;
                        respData.IsActive = getUser.IsActive;
                        respData.LastLoginDate = getUser.LastLoginDate;
                        respData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        respData.LastUpdatedDate = getUser.LastUpdatedDate;
                        respData.FacilitatorTypeId = getUser.FacilitatorTypeId;

                        //Generate JSON WEB TOKEN for a valid User
                        var tokenString = jwtObj.GenerateJWTToken();

                        //The data to be sent as response
                        respObj.StatusCode = 200;
                        respObj.StatusMessage = "Successful";
                        respObj.Token = tokenString;
                        respObj.Data = respData;

                        //Log the user Activity
                        //Guid userId = getUser.Id;

                        //ActivityLogger actLog = new ActivityLogger();
                        //var logActivity = actLog.logActivity(userId, "Online Learning", "Login Activity");
                        //await _context.ActivityLog.AddAsync(logActivity);
                        //await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password" };
                }

                return respObj;

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

        public async Task<GenericResponseModel> getAllFacilitatorAsync(long? facilitatorTypeId)
        {
            try
            {
                var result = from fac in _context.Facilitators
                             .Include(f => f.Courses)
                             select new
                             {
                                 fac.Id,
                                 fac.FirstName,
                                 fac.LastName,
                                 fac.UserName,
                                 fac.Email,
                                 fac.EmailConfirmed,
                                 fac.PhoneNumber,
                                 fac.InstitutionAttended,
                                 fac.CourseOfStudy,
                                 fac.CertificateObtained,
                                 fac.Profession,
                                 fac.Bio,
                                 fac.ProfilePictureUrl,
                                 fac.LastPasswordChangedDate,
                                 fac.LastLoginDate,
                                 fac.LastUpdatedDate,
                                 fac.DateCreated,
                                 fac.Courses,
                                 fac.FacilitatorTypeId,
                                 fac.FacilitatorType.FacilitatorTypeName
                             };
                if (facilitatorTypeId == (int)EnumUtility.FacilitatorType.Internal || facilitatorTypeId == (int)EnumUtility.FacilitatorType.External)
                {
                    result = result.Where(x => x.FacilitatorTypeId == facilitatorTypeId);
                }
                if (result.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList<object>(), };
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

        public async Task<GenericResponseModel> getFacilitatorByIdAsync(Guid facilitatorId)
        {
            try
            {
                var result = from fac in _context.Facilitators
                             .Include(f => f.Courses)
                             where fac.Id == facilitatorId
                             select new
                             {
                                 fac.Id,
                                 fac.FirstName,
                                 fac.LastName,
                                 fac.UserName,
                                 fac.Email,
                                 fac.EmailConfirmed,
                                 fac.PhoneNumber,
                                 fac.InstitutionAttended,
                                 fac.CourseOfStudy,
                                 fac.CertificateObtained,
                                 fac.Profession,
                                 fac.Bio,
                                 fac.ProfilePictureUrl,
                                 fac.LastPasswordChangedDate,
                                 fac.LastLoginDate,
                                 fac.LastUpdatedDate,
                                 fac.DateCreated,
                                 fac.Courses,
                                 fac.FacilitatorTypeId,
                                 fac.FacilitatorType.FacilitatorTypeName
                             };

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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

        public async Task<GenericResponseModel> activateAccountAsync(AccountActivationRequestModel obj)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                if (emailCheckResult == true && accountCheckResult == false) //check if account has been verified and activated
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true) //email exist
                {
                    Facilitators getUser = _context.Facilitators.FirstOrDefault(u => u.Email == obj.Email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);

                    if (getUserCode != null && getUserCode.Code == obj.Code)
                    {
                        getUser.EmailConfirmed = true; //Update the user account as confirmed (EmailConfirmed set to true)

                        _context.EmailConfirmationCodes.Remove(getUserCode);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Code Entered" };
                    }

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Verification Successful" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This User doesnt exist" };

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
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    Facilitators getUser = _context.Facilitators.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);
                    string codeGenerated = string.Empty;
                    var subject = "ExpertPlat Registration";

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        codeGenerated = getUserCode.Code;

                        //send Mail to user for account activation
                        string MailContent = _emailTemplate.EmailContent(getUser.FirstName, codeGenerated);
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

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
                        string MailContent = _emailTemplate.EmailContent(getUser.FirstName, codeGenerated);
                        //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

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

        public async Task<GenericResponseModel> updateProfileDetailsAsync(UpdateFacilitatorProfileRequestModel obj)
        {
            try
            {
                CheckerValidation checkFacilitator = new CheckerValidation(_context);
                var checkResult = checkFacilitator.checkFacilitatorById(obj.FacilitatorId);

                if (checkResult == true)
                {
                    Facilitators fac = _context.Facilitators.FirstOrDefault(f => f.Id == obj.FacilitatorId);
                    fac.FirstName = obj.FirstName;
                    fac.LastName = obj.LastName;
                    fac.UserName = obj.UserName;
                    fac.PhoneNumber = obj.PhoneNumber;
                    fac.InstitutionAttended = obj.InstitutionAttended;
                    fac.CourseOfStudy = obj.CourseOfStudy;
                    fac.CertificateObtained = obj.CertificateObtained;
                    fac.Profession = obj.Profession;
                    fac.Bio = obj.Bio;
                    fac.LastUpdatedDate = DateTime.Now;
                    //Update the changes Made
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Profile Details Updated Successfully" };

                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Facilitator With the Specified ID" };
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

        public async Task<GenericResponseModel> forgotPasswordAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                if (emailCheckResult == true) //email exist 
                {
                    Facilitators getUser = _context.Facilitators.FirstOrDefault(u => u.Email == email);

                    //generate a new code for forgot password
                    var confirmationCode = new RandomNumberGenerator();
                    string codeGenerated = confirmationCode.randomCodesGen();

                    //save the code generated
                    var forgotPassword = new ForgotPasswordCodes
                    {
                        UserId = getUser.Id,
                        Code = codeGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(forgotPassword);
                    await _context.SaveChangesAsync();

                    //send Mail to user for account activation
                    //string MailContent = "use this code " + codeGenerated + " to reset/change your password";
                    string MailContent = _emailTemplate.ResetPasswordEmailContent(getUser.FirstName, codeGenerated);
                    var subject = "ExpertPlat Reset Password";

                    EmailMessage message = new EmailMessage(getUser.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Reset Password Code Sent Successfully!" };

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

        public async Task<GenericResponseModel> changePasswordAsync(ChangePasswordRequestModel obj)
        {
            try
            {
                var codeCheck = _context.ForgotPasswordCodes.FirstOrDefault(u => u.Code == obj.Code);

                if (codeCheck != null) //code exist
                {
                    Facilitators getUser = _context.Facilitators.FirstOrDefault(u => u.Id == codeCheck.UserId);

                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(obj.NewPassword, salt);

                    //Update the facilitator salt and password
                    getUser.Salt = salt;
                    getUser.PasswordHash = passwordHash;
                    getUser.LastPasswordChangedDate = DateTime.Now;

                    //delete the forgotpassword code after successful Update
                    _context.ForgotPasswordCodes.Remove(codeCheck);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Password Changed Successfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Code Entered" };
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

        public async Task<GenericResponseModel> updateProfilePictureAsync(Guid facilitatorId, string profilePictureUrl)
        {
            try
            {
                CheckerValidation checkFacilitator = new CheckerValidation(_context);
                var checkResult = checkFacilitator.checkFacilitatorById(facilitatorId);

                if (checkResult == true)
                {
                    var fac = _context.Facilitators.FirstOrDefault(f => f.Id == facilitatorId);
                    fac.ProfilePictureUrl = profilePictureUrl;

                    //Update the changes Made
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Profile Picture Uploaded Successfully" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Facilitator With the Specified ID" };

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

        //---------------------------------ACCOUNT DETAILS----------------------------------------------------------------

        public async Task<GenericResponseModel> createAccountDetailsAsync(FacilitatorAccountDetailsRequestModel obj)
        {
            try
            {
                var check = _context.FacilitatorAccountDetails.Where(x => x.FacilitatorId == obj.FacilitatorId && x.BankName == obj.BankName
                && x.AccountName == obj.AccountName && x.AccountNumber == obj.AccountNumber).FirstOrDefault();

                //
                CheckerValidation checkFacilitator = new CheckerValidation(_context);
                var checkResult = checkFacilitator.checkFacilitatorById(obj.FacilitatorId);

                if (checkResult == true)
                {
                    string FacilitatorFullName = string.Empty;
                    //gets the facilitator's details
                    Facilitators fac = _context.Facilitators.Where(x => x.Id == obj.FacilitatorId).FirstOrDefault();
                    FacilitatorFullName = fac.FirstName + " " + fac.LastName;

                    if (check == null)
                    {
                        //PayStack AccountNumber Verification
                        HttpClientConfig httpClientConfig = new HttpClientConfig();
                        var apiResponse = await httpClientConfig.ApiGetRequest("https://api.paystack.co/bank/resolve?account_number=" + obj.AccountNumber + "&bank_code=" + obj.BankCode, _config["PayStack:SecretTestKey"]);
                        var response = JsonConvert.DeserializeObject<VerifyAccountNoResponseModel>(apiResponse);

                        //if verification is successful, consume creation of transfer receipt
                        if (response.status == true)
                        {
                            //Consume PayStack transfer recipient endpoint to create transfer recipient and get the recipient_code from the response
                            //the recipient_code will be used to represent the account number that will be used to make subsequent transfers

                            TransferReceipentRequestModel transReq = new TransferReceipentRequestModel();
                            transReq.name = FacilitatorFullName;
                            transReq.account_number = response.data.account_number; //account number from the verification response
                            transReq.bank_code = obj.BankCode; //Bank Code from the request Object

                            HttpClientConfig clientConfig = new HttpClientConfig();
                            var apiResp = await clientConfig.ApiPostRequest("https://api.paystack.co/transferrecipient", transReq, _config["PayStack:SecretTestKey"]);
                            var receiptResponse = JsonConvert.DeserializeObject<TransferRecipientResponseModel>(apiResp);

                            //if the creation of transfer receipt is true, save record in the database
                            if (receiptResponse.status == true)
                            {
                                //Save the Account details
                                var acct = new FacilitatorAccountDetails
                                {
                                    FacilitatorId = obj.FacilitatorId,
                                    BankName = obj.BankName,
                                    AccountNumber = obj.AccountNumber,
                                    AccountName = obj.AccountName,
                                    IsActive = false,
                                    IsVerified = true,
                                    RecipientCode = receiptResponse.data.recipient_code,
                                    BankCode = receiptResponse.data.details.bank_code,
                                    DateCreated = DateTime.Now,
                                };
                                await _context.FacilitatorAccountDetails.AddAsync(acct);
                                await _context.SaveChangesAsync();

                                //return the account Created
                                var result = from cr in _context.FacilitatorAccountDetails.Where(c => c.Id == acct.Id)
                                             select new
                                             {
                                                 cr.Id,
                                                 cr.FacilitatorId,
                                                 cr.Facilitators.FirstName,
                                                 cr.Facilitators.LastName,
                                                 cr.BankName,
                                                 cr.AccountName,
                                                 cr.AccountNumber,
                                                 cr.IsActive,
                                                 cr.IsVerified,
                                                 cr.RecipientCode,
                                                 cr.BankCode,
                                                 cr.DateCreated,
                                                 cr.DateUpdated
                                             };

                                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Details Created Successfully!", Data = result.FirstOrDefault() };

                            }

                            return new GenericResponseModel { StatusCode = 400, StatusMessage = receiptResponse.message };
                        }

                        return new GenericResponseModel { StatusCode = 400, StatusMessage = response.message };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Details Already Exists!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Facilitator With the Specified ID" };
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


        public async Task<GenericResponseModel> getAccountDetailsByIdAsync(Guid facilitatorId, long accountId)
        {
            try
            {
                var result = from cr in _context.FacilitatorAccountDetails
                             where cr.Id == accountId && cr.FacilitatorId == facilitatorId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.BankName,
                                 cr.AccountName,
                                 cr.AccountNumber,
                                 cr.IsActive,
                                 cr.IsVerified,
                                 cr.RecipientCode,
                                 cr.BankCode,
                                 cr.DateCreated,
                                 cr.DateUpdated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllAccountDetailsByFacilitatorIdAsync(Guid facilitatorId)
        {
            try
            {
                var result = from cr in _context.FacilitatorAccountDetails
                             where cr.FacilitatorId == facilitatorId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.BankName,
                                 cr.AccountName,
                                 cr.AccountNumber,
                                 cr.IsActive,
                                 cr.IsVerified,
                                 cr.RecipientCode,
                                 cr.BankCode,
                                 cr.DateCreated,
                                 cr.DateUpdated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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


        public async Task<GenericResponseModel> setAccountDetailsAsDefaultAsync(Guid facilitatorId, long accountId)
        {
            try
            {
                //get the current session that is set to true
                var getAccount = _context.FacilitatorAccountDetails.Where(s => s.FacilitatorId == facilitatorId && s.IsActive == true).FirstOrDefault();

                if (getAccount != null)
                {
                    //update the session to false
                    getAccount.IsActive = false;

                    //update the new session whose parameter is supplied to true
                    var setAccountAsDefault = _context.FacilitatorAccountDetails.FirstOrDefault(s => s.Id == accountId && s.FacilitatorId == facilitatorId);
                    setAccountAsDefault.IsActive = true;

                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Details Set Successfully" };

                }
                else
                {
                    //update the new session whose parameter is supplied to true
                    var setAccountAsDefault = _context.FacilitatorAccountDetails.FirstOrDefault(s => s.Id == accountId && s.FacilitatorId == facilitatorId);
                    setAccountAsDefault.IsActive = true;

                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Details Set Successfully" };

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

        public async Task<GenericResponseModel> updateAccountDetailsAsync(FacilitatorAccountDetailsRequestModel obj, long accountId)
        {
            try
            {
                var getAcct = _context.FacilitatorAccountDetails.Where(x => x.Id == accountId && x.FacilitatorId == obj.FacilitatorId).FirstOrDefault();

                if (getAcct != null)
                {
                    var check = _context.FacilitatorAccountDetails.Where(x => x.FacilitatorId == obj.FacilitatorId && x.BankName == obj.BankName
                    && x.AccountName == obj.AccountName && x.AccountNumber == obj.AccountNumber).FirstOrDefault();

                    string FacilitatorFullName = string.Empty;
                    //gets the facilitator's details
                    Facilitators fac = _context.Facilitators.Where(x => x.Id == obj.FacilitatorId).FirstOrDefault();
                    FacilitatorFullName = fac.FirstName + " " + fac.LastName;

                    if (check == null)
                    {
                        //PayStack AccountNumber Verification
                        HttpClientConfig httpClientConfig = new HttpClientConfig();
                        var apiResponse = await httpClientConfig.ApiGetRequest("https://api.paystack.co/bank/resolve?account_number=" + obj.AccountNumber + "&bank_code=" + obj.BankCode, _config["PayStack:SecretTestKey"]);
                        var response = JsonConvert.DeserializeObject<VerifyAccountNoResponseModel>(apiResponse);

                        //if verification is successful, consume creation of transfer receipt
                        if (response.status == true)
                        {
                            //Consume PayStack transfer recipient endpoint to create transfer recipient and get the recipient_code from the response
                            //the recipient_code will be used to represent the account number that will be used to make subsequent transfers

                            TransferReceipentRequestModel transReq = new TransferReceipentRequestModel();
                            transReq.name = FacilitatorFullName;
                            transReq.account_number = response.data.account_number; //account number from the verification response
                            transReq.bank_code = obj.BankCode; //Bank Code from the request Object

                            HttpClientConfig clientConfig = new HttpClientConfig();
                            var apiResp = await clientConfig.ApiPostRequest("https://api.paystack.co/transferrecipient", transReq, _config["PayStack:SecretTestKey"]);
                            var receiptResponse = JsonConvert.DeserializeObject<TransferRecipientResponseModel>(apiResp);

                            //if the creation of transfer receipt is true, save record in the database
                            if (receiptResponse.status == true)
                            {
                                //Update the Account details
                                getAcct.FacilitatorId = obj.FacilitatorId;
                                getAcct.BankName = obj.BankName;
                                getAcct.AccountNumber = obj.AccountNumber;
                                getAcct.AccountName = obj.AccountName;
                                getAcct.IsVerified = true;
                                getAcct.RecipientCode = receiptResponse.data.recipient_code;
                                getAcct.BankCode = receiptResponse.data.details.bank_code;
                                getAcct.DateUpdated = DateTime.Now;

                                await _context.SaveChangesAsync();

                                //return the account Created
                                var result = from cr in _context.FacilitatorAccountDetails.Where(c => c.Id == getAcct.Id)
                                             select new
                                             {
                                                 cr.Id,
                                                 cr.FacilitatorId,
                                                 cr.Facilitators.FirstName,
                                                 cr.Facilitators.LastName,
                                                 cr.BankName,
                                                 cr.AccountName,
                                                 cr.AccountNumber,
                                                 cr.IsActive,
                                                 cr.IsVerified,
                                                 cr.RecipientCode,
                                                 cr.BankCode,
                                                 cr.DateCreated,
                                                 cr.DateUpdated
                                             };

                                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Details Updated Successfully", Data = result.FirstOrDefault() };

                            }

                            return new GenericResponseModel { StatusCode = 400, StatusMessage = receiptResponse.message };
                        }

                        return new GenericResponseModel { StatusCode = 400, StatusMessage = response.message };
                    }

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Details Already Exists" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Account Details With the Specified ID" };
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

        public async Task<GenericResponseModel> deleteAccountDetailsAsync(long accountId)
        {
            try
            {
                //check if the accountId is valid
                var getAcct = _context.FacilitatorAccountDetails.Where(x => x.Id == accountId).FirstOrDefault();
                if (getAcct != null)
                {
                    var acct = _context.FacilitatorAccountDetails.Where(c => c.Id == accountId).FirstOrDefault();

                    _context.FacilitatorAccountDetails.Remove(acct);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Account Details Deleted Successfully" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Account Details with the specified ID" };

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

        //-------------------------Percentage Earned on Courses----------------------------------------

        public async Task<GenericResponseModel> getAllPercentageEarnedOnCoursesAsync(Guid facilitatorId)
        {
            try
            {
                var result = from cr in _context.PercentageEarnedOnCourses.AsNoTracking()
                             where cr.FacilitatorId == facilitatorId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Percentage,
                                 cr.DateCreated,
                                 cr.LastUpdated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getPercentageEarnedOnCoursesByCourseIdAsync(Guid facilitatorId, long courseId)
        {
            try
            {
                var result = from cr in _context.PercentageEarnedOnCourses.AsNoTracking()
                             where cr.FacilitatorId == facilitatorId && cr.CourseId == courseId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Percentage,
                                 cr.DateCreated,
                                 cr.LastUpdated
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllEarningsPerCourseAsync(Guid facilitatorId)
        {
            try
            {
                var result = from cr in _context.FacilitatorsEarningsPerCourse
                             where cr.FacilitatorId == facilitatorId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Amount,
                                 cr.Percentage,
                                 cr.AmountEarned,
                                 cr.DateEarned,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllEarningsPerCourseByCourseIdAsync(Guid facilitatorId, long courseId)
        {
            try
            {
                var result = from cr in _context.FacilitatorsEarningsPerCourse
                             where cr.FacilitatorId == facilitatorId && cr.CourseId == courseId
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Amount,
                                 cr.Percentage,
                                 cr.AmountEarned,
                                 cr.DateEarned,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedAsync(Guid facilitatorId, DateTime date)
        {
            try
            {
                var result = from cr in _context.FacilitatorsEarningsPerCourse
                             where cr.FacilitatorId == facilitatorId && cr.DateEarned == date.Date
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Amount,
                                 cr.Percentage,
                                 cr.AmountEarned,
                                 cr.DateEarned,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedAndCourseIdAsync(Guid facilitatorId, long courseId, DateTime date)
        {
            try
            {
                var result = from cr in _context.FacilitatorsEarningsPerCourse
                             where cr.FacilitatorId == facilitatorId && cr.CourseId == courseId && cr.DateEarned == date.Date
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Amount,
                                 cr.Percentage,
                                 cr.AmountEarned,
                                 cr.DateEarned,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedRangeAsync(Guid facilitatorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = from cr in _context.FacilitatorsEarningsPerCourse
                             where cr.FacilitatorId == facilitatorId && (cr.DateEarned >= fromDate.Date && cr.DateEarned <= toDate.Date)
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Amount,
                                 cr.Percentage,
                                 cr.AmountEarned,
                                 cr.DateEarned,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllEarningsPerCourseByDateEarnedRangeAndCourseIdAsync(Guid facilitatorId, long courseId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = from cr in _context.FacilitatorsEarningsPerCourse
                             where cr.FacilitatorId == facilitatorId && cr.CourseId == courseId && (cr.DateEarned >= fromDate.Date && cr.DateEarned <= toDate.Date)
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.CourseId,
                                 cr.Courses.CourseName,
                                 cr.Courses.CourseCategory.CourseCategoryName,
                                 cr.Courses.CourseSubCategory.CourseSubCategoryName,
                                 cr.Amount,
                                 cr.Percentage,
                                 cr.AmountEarned,
                                 cr.DateEarned,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        //------------------------------Total Earnings------------------------------------------------------------

        public async Task<GenericResponseModel> getTotalEarningsByDateEarnedAsync(Guid facilitatorId, DateTime date)
        {
            try
            {
                var result = from cr in _context.FacilitatorsTotalEarnings
                             where cr.FacilitatorId == facilitatorId && cr.DateEarned == date.Date
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.TotalAmountEarned,
                                 cr.IsSettled,
                                 cr.DateEarned,
                                 cr.LastDateUpdated,
                                 cr.DateSettled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTotalEarningsByDateEarnedRangeAsync(Guid facilitatorId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = from cr in _context.FacilitatorsTotalEarnings
                             where cr.FacilitatorId == facilitatorId && (cr.DateEarned >= fromDate.Date && cr.DateEarned <= toDate.Date)
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.TotalAmountEarned,
                                 cr.IsSettled,
                                 cr.DateEarned,
                                 cr.LastDateUpdated,
                                 cr.DateSettled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTotalEarningsSettledAsync(Guid facilitatorId)
        {
            try
            {
                var result = from cr in _context.FacilitatorsTotalEarnings
                             where cr.FacilitatorId == facilitatorId && cr.IsSettled == true
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.TotalAmountEarned,
                                 cr.IsSettled,
                                 cr.DateEarned,
                                 cr.LastDateUpdated,
                                 cr.DateSettled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getTotalEarningsUnSettledAsync(Guid facilitatorId)
        {
            try
            {
                var result = from cr in _context.FacilitatorsTotalEarnings
                             where cr.FacilitatorId == facilitatorId && cr.IsSettled == false
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.TotalAmountEarned,
                                 cr.IsSettled,
                                 cr.DateEarned,
                                 cr.LastDateUpdated,
                                 cr.DateSettled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getFacilitatorsSettledEarningsAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = from cr in _context.FacilitatorsTotalEarnings
                             where cr.IsSettled == true && (cr.DateEarned >= fromDate.Date && cr.DateEarned <= toDate.Date)
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.TotalAmountEarned,
                                 cr.IsSettled,
                                 cr.DateEarned,
                                 cr.LastDateUpdated,
                                 cr.DateSettled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getFacilitatorsUnSettledEarningsAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = from cr in _context.FacilitatorsTotalEarnings
                             where cr.IsSettled == false && (cr.DateEarned >= fromDate.Date && cr.DateEarned <= toDate.Date)
                             select new
                             {
                                 cr.Id,
                                 cr.FacilitatorId,
                                 cr.Facilitators.FirstName,
                                 cr.Facilitators.LastName,
                                 cr.TotalAmountEarned,
                                 cr.IsSettled,
                                 cr.DateEarned,
                                 cr.LastDateUpdated,
                                 cr.DateSettled,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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
