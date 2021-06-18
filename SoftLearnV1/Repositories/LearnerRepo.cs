using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Newtonsoft.Json;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class LearnerRepo : ILearnerRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private IConfiguration _config;
        private readonly IHostingEnvironment _env;
        private readonly EmailTemplate _emailTemplate;

        public LearnerRepo(AppDbContext context, IEmailRepo emailRepo, IConfiguration config, IHostingEnvironment env, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _config = config;
            this._env = env;
            this._emailTemplate = emailTemplate;
        }
        public async Task<GenericResponseModel> learnerSignUpAsync(LearnerRegRequestModel obj)
        {
            try
            {
                LearnerRegResponseModel respData = new LearnerRegResponseModel();
                GenericResponseModel respObj = new GenericResponseModel();
                Jwt jwtObj = new Jwt(_config);


                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));

                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));

                if (emailCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken!" };
                }
                else if (accountCheckResult == true)
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                }
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(obj.Password, salt);

                    //save the lrnilitator details
                    var lrn = new Learners
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Email = obj.Email,
                        EmailConfirmed = false,
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        LevelTypeId = obj.LevelTypeId,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Learners.AddAsync(lrn);
                    await _context.SaveChangesAsync();

                    //generate the code for email confirmation
                    var confirmationCode = new RandomNumberGenerator();
                    string codeGenerated = confirmationCode.randomCodesGen();

                    //save the code generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = lrn.Id,
                        Code = codeGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();
                    var subject = "ExpertPlat Registration";
                    string MailContent = _emailTemplate.EmailContent(lrn.FirstName, codeGenerated);
                    

                    //_emailRepo.SendNewEmail(lrn.Email, subject, body);

                    //EmailTemplate emailTemp = new EmailTemplate();
                    //var MailContent = emailTemp.EmailHtmlTemplate(codeGenerated);
                    //code to send Mail to user for account activation
                    //string MailContent = "Welcome to SOFTLEARN, use this code " + codeGenerated + " to activate your account";

                    EmailMessage message = new EmailMessage(lrn.Email, MailContent, subject);
                    _emailRepo.SendEmail(message);

                    //The data collected from the user on successful creation
                    respData.UserId = lrn.Id.ToString();
                    respData.FirstName = lrn.FirstName;
                    respData.LastName = lrn.LastName;
                    respData.Email = lrn.Email;

                    //Generate JSON WEB TOKEN for a valid User
                    var tokenString = jwtObj.GenerateJWTToken();

                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "Account created successfully, use the code sent to your mail to activate and verify your account!";
                    respObj.Token = tokenString;
                    respObj.Data = respData;

                }

                return respObj;
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

        public async Task<GenericResponseModel> learnerLoginAsync(LoginRequestModel obj)
        {
            try
            {
                LearnerLoginResponseModel respData = new LearnerLoginResponseModel();
                GenericResponseModel respObj = new GenericResponseModel();
                Jwt jwtObj = new Jwt(_config);

                CheckerValidation emailcheck = new CheckerValidation(_context);

                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));

                Learners getUser = _context.Learners.FirstOrDefault(u => u.Email == obj.Email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    else if (getUser != null && getUser.PasswordHash == decryptedPassword && accountCheckResult == true)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                    }
                    else
                    {
                        //get the learner level
                        var userLevel = _context.CourseLevelTypes.FirstOrDefault(u => u.Id == getUser.LevelTypeId);

                        respData.UserId = getUser.Id.ToString();
                        respData.FirstName = getUser.FirstName;
                        respData.LastName = getUser.LastName;
                        respData.UserName = getUser.UserName;
                        respData.Email = getUser.Email;
                        respData.EmailConfirmed = getUser.EmailConfirmed;
                        respData.LevelTypeId = getUser.LevelTypeId;
                        respData.LevelType = userLevel.LevelTypeName;
                        respData.IsActive = getUser.IsActive;
                        respData.LastLoginDate = getUser.LastLoginDate;
                        respData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        respData.LastUpdatedDate = getUser.LastUpdatedDate;

                        //Generate JSON WEB TOKEN for a valid User
                        var tokenString = jwtObj.GenerateJWTToken();

                        //The data to be sent as response
                        respObj.StatusCode = 200;
                        respObj.StatusMessage = "Successful!";
                        respObj.Token = tokenString;
                        respObj.Data = respData;
                    }
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                }

                return respObj;

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

        public async Task<GenericResponseModel> getAllLearnersAsync()
        {
            try
            {
                var result = from lrn in _context.Learners
                             select new
                             {
                                 lrn.Id,
                                 lrn.FirstName,
                                 lrn.LastName,
                                 lrn.UserName,
                                 lrn.Email,
                                 lrn.EmailConfirmed,
                                 lrn.PhoneNumber,
                                 lrn.Bio,
                                 lrn.ProfilePictureUrl,
                                 lrn.LastPasswordChangedDate,
                                 lrn.LastLoginDate,
                                 lrn.LastUpdatedDate,
                                 lrn.DateCreated,
                             };
                if (result.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList<object>(), };
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


        public async Task<GenericResponseModel> getLearnerByIdAsync(Guid learnerId)
        {
            try
            {
                var result = from lrn in _context.Learners where lrn.Id == learnerId
                             select new
                             {
                                 lrn.Id,
                                 lrn.FirstName,
                                 lrn.LastName,
                                 lrn.UserName,
                                 lrn.Email,
                                 lrn.EmailConfirmed,
                                 lrn.PhoneNumber,
                                 lrn.Bio,
                                 lrn.ProfilePictureUrl,
                                 lrn.LastPasswordChangedDate,
                                 lrn.LastLoginDate,
                                 lrn.LastUpdatedDate,
                                 lrn.DateCreated,
                             };
              
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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



        public async Task<GenericResponseModel> activateAccountAsync(AccountActivationRequestModel obj)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));

                if (emailCheckResult == true && accountCheckResult == false) //check if account has been verified and activated
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true) //email exist
                {
                    Learners getUser = _context.Learners.FirstOrDefault(u => u.Email == obj.Email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);

                    if (getUserCode != null && getUserCode.Code == obj.Code)
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

        public async Task<GenericResponseModel> resendActivationCodeAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    Learners getUser = _context.Learners.FirstOrDefault(u => u.Email == email);
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

        public async Task<GenericResponseModel> forgotPasswordAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Learners));

                if (emailCheckResult == true) //email exist 
                {
                    Learners getUser = _context.Learners.FirstOrDefault(u => u.Email == email);

                    //generate a new code for forgot password
                    var confirmationCode = new RandomNumberGenerator();
                    string codeGenerated = confirmationCode.randomCodesGen();
                    var subject = "ExpertPlat Reset Password";

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

        public async Task<GenericResponseModel> changePasswordAsync(ChangePasswordRequestModel obj)
        {
            try
            {
                var codeCheck = _context.ForgotPasswordCodes.FirstOrDefault(u => u.Code == obj.Code);

                if (codeCheck != null) //code exist
                {
                    Learners getUser = _context.Learners.FirstOrDefault(u => u.Id == codeCheck.UserId);

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
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "Invalid Code Entered" };
                }
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

        public async Task<GenericResponseModel> updateProfileDetailsAsync(UpdateLearnerProfileRequestModel obj)
        {
            try
            {
                CheckerValidation checkLearner = new CheckerValidation(_context);
                var checkResult = checkLearner.checkLearnerById(obj.LearnerId);

                if (checkResult == true)
                {
                    Learners lrn = _context.Learners.FirstOrDefault(l => l.Id == obj.LearnerId);
                    lrn.FirstName = obj.FirstName;
                    lrn.LastName = obj.LastName;
                    lrn.UserName = obj.UserName;
                    lrn.PhoneNumber = obj.PhoneNumber;
                    lrn.Bio = obj.Bio;
                    lrn.LastUpdatedDate = DateTime.Now;
                    //Update the changes Made
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Profile Details Updated Successfully" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Learner With the Specified ID" };
                }
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

        public async Task<GenericResponseModel> updateProfilePictureAsync(Guid learnerId, string profilePictureUrl)
        {
            try
            {
                CheckerValidation checkLearner = new CheckerValidation(_context);
                var checkResult = checkLearner.checkLearnerById(learnerId);

                if (checkResult == true)
                {
                    var lrn = _context.Learners.FirstOrDefault(l => l.Id == learnerId);
                    lrn.ProfilePictureUrl = profilePictureUrl;

                    //Update the changes Made
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Profile Picture Uploaded Successfully" };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Learner With the Specified ID" };
                }
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

        //-------------------------Bank Account Details----------------------------------------

        public async Task<GenericResponseModel> createAccountDetailsAsync(LearnerAccountDetailsRequestModel obj)
        {
            try
            {
                //validations
                CheckerValidation checkLearner = new CheckerValidation(_context);
                var checkResult = checkLearner.checkLearnerById(obj.LearnerId);

                if (checkResult == true)
                {
                    //check if account details has been created (i.e.  Limit the learners to create only one account details for course refund)
                    LearnerAccountDetails checkAcctExist = _context.LearnerAccountDetails.Where(x => x.LearnerId == obj.LearnerId).FirstOrDefault();
                    if (checkAcctExist == null)
                    {
                        var check = _context.LearnerAccountDetails.Where(x => x.LearnerId == obj.LearnerId && x.BankName == obj.BankName
                        && x.AccountName == obj.AccountName && x.AccountNumber == obj.AccountNumber).FirstOrDefault();
                  
                        string LearnerFullName = string.Empty;
                        //gets the facilitator's details
                        Learners lrn = _context.Learners.Where(x => x.Id == obj.LearnerId).FirstOrDefault();
                        LearnerFullName = lrn.FirstName + " " + lrn.LastName;

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
                                transReq.name = LearnerFullName;
                                transReq.account_number = response.data.account_number; //account number from the verification response
                                transReq.bank_code = obj.BankCode; //Bank Code from the request Object

                                HttpClientConfig clientConfig = new HttpClientConfig();
                                var apiResp = await clientConfig.ApiPostRequest("https://api.paystack.co/transferrecipient", transReq, _config["PayStack:SecretTestKey"]);
                                var receiptResponse = JsonConvert.DeserializeObject<TransferRecipientResponseModel>(apiResp);

                                //if the creation of transfer receipt is true, save record in the database
                                if (receiptResponse.status == true)
                                {
                                    //Save the Account details
                                    var acct = new LearnerAccountDetails
                                    {
                                        LearnerId = obj.LearnerId,
                                        BankName = obj.BankName,
                                        AccountNumber = obj.AccountNumber,
                                        AccountName = obj.AccountName,
                                        IsActive = true,
                                        IsVerified = true,
                                        RecipientCode = receiptResponse.data.recipient_code,
                                        BankCode = receiptResponse.data.details.bank_code,
                                        DateCreated = DateTime.Now,
                                    };
                                    await _context.LearnerAccountDetails.AddAsync(acct);
                                    await _context.SaveChangesAsync();

                                    //return the account Created
                                    var result = from cr in _context.LearnerAccountDetails.Where(c => c.Id == acct.Id)
                                                 select new
                                                 {
                                                     cr.Id,
                                                     cr.LearnerId,
                                                     cr.Learners.FirstName,
                                                     cr.Learners.LastName,
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

                    return new GenericResponseModel { StatusCode = 409, StatusMessage = "An Account DetaIls Already Exists!" };
                }

                return new GenericResponseModel { StatusCode = 409, StatusMessage = "No Learner With the Specified ID" };

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

        public async Task<GenericResponseModel> getAccountDetailsByIdAsync(Guid learnerId, long accountId)
        {
            try
            {
                var result = from cr in _context.LearnerAccountDetails
                             where cr.Id == accountId && cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
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

        public async Task<GenericResponseModel> getAllAccountDetailsByLearnerIdAsync(Guid learnerId)
        {
            try
            {
                var result = from cr in _context.LearnerAccountDetails
                             where cr.LearnerId == learnerId
                             select new
                             {
                                 cr.Id,
                                 cr.LearnerId,
                                 cr.Learners.FirstName,
                                 cr.Learners.LastName,
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


        public async Task<GenericResponseModel> updateAccountDetailsAsync(LearnerAccountDetailsRequestModel obj, long accountId)
        {
            try
            {
                var getAcct = _context.LearnerAccountDetails.Where(x => x.Id == accountId && x.LearnerId == obj.LearnerId).FirstOrDefault();

                if (getAcct != null)
                {
                    var check = _context.LearnerAccountDetails.Where(x => x.LearnerId == obj.LearnerId && x.BankName == obj.BankName
                    && x.AccountName == obj.AccountName && x.AccountNumber == obj.AccountNumber).FirstOrDefault();

                    string LearnerFullName = string.Empty;
                    //gets the facilitator's details
                    Learners lrn = _context.Learners.Where(x => x.Id == obj.LearnerId).FirstOrDefault();
                    LearnerFullName = lrn.FirstName + " " + lrn.LastName;

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
                            transReq.name = LearnerFullName;
                            transReq.account_number = response.data.account_number; //account number from the verification response
                            transReq.bank_code = obj.BankCode; //Bank Code from the request Object

                            HttpClientConfig clientConfig = new HttpClientConfig();
                            var apiResp = await clientConfig.ApiPostRequest("https://api.paystack.co/transferrecipient", transReq, _config["PayStack:SecretTestKey"]);
                            var receiptResponse = JsonConvert.DeserializeObject<TransferRecipientResponseModel>(apiResp);

                            //if the creation of transfer receipt is true, save record in the database
                            if (receiptResponse.status == true)
                            {
                                //Update the Account details
                                getAcct.LearnerId = obj.LearnerId;
                                getAcct.BankName = obj.BankName;
                                getAcct.AccountNumber = obj.AccountNumber;
                                getAcct.AccountName = obj.AccountName;
                                getAcct.IsActive = true;
                                getAcct.IsVerified = true;
                                getAcct.RecipientCode = receiptResponse.data.recipient_code;
                                getAcct.BankCode = receiptResponse.data.details.bank_code;
                                getAcct.DateUpdated = DateTime.Now;

                                await _context.SaveChangesAsync();

                                //return the account Created
                                var result = from cr in _context.LearnerAccountDetails.Where(c => c.Id == getAcct.Id)
                                             select new
                                             {
                                                 cr.Id,
                                                 cr.LearnerId,
                                                 cr.Learners.FirstName,
                                                 cr.Learners.LastName,
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
                var getAcct = _context.LearnerAccountDetails.Where(x => x.Id == accountId).FirstOrDefault();
                if (getAcct != null)
                {
                    var acct = _context.LearnerAccountDetails.Where(c => c.Id == accountId).FirstOrDefault();

                    _context.LearnerAccountDetails.Remove(acct);
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
    }
}
