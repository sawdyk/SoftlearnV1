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
    public class SystemUserRepo : ISystemUserRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly IConfiguration _config;
        private readonly EmailTemplate _emailTemplate;

        public SystemUserRepo(AppDbContext context, IEmailRepo emailRepo, IConfiguration config, EmailTemplate emailTemplate)
        {
            this._context = context;
            this._emailRepo = emailRepo;
            this._config = config;
            this._emailTemplate = emailTemplate;
        }

        public async Task<GenericResponseModel> createSystemUserAsync(SystemUserRegRequestModel model)
        {
            try
            {
                SystemUserResponseModel respData = new SystemUserResponseModel();
                GenericResponseModel respObj = new GenericResponseModel();
                Jwt jwtObj = new Jwt(_config);


                CheckerValidation emailcheck = new CheckerValidation(_context);
                if (model.RoleId == (int)EnumUtility.SystemRoles.Content_Creator)
                {
                    var emailCheckResult = emailcheck.checkIfEmailExist(model.Email, Convert.ToInt64(EnumUtility.UserCategoty.Facilitator));

                    if (emailCheckResult == true)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken" };
                    }
                    else
                    {
                        //generate the password for email confirmation
                        //var confirmationCode = new RandomNumberGenerator();
                        string passwordGenerated = RandomNumberGenerator.RandomString();

                        var paswordHasher = new PasswordHasher();
                        //the salt
                        string salt = paswordHasher.getSalt();
                        //Hash the password and salt
                        string passwordHash = paswordHasher.hashedPassword(passwordGenerated, salt);
                        
                        var fac = new Facilitators
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            EmailConfirmed = false,
                            PhoneNumber = model.PhoneNumber,
                            PhoneNumberConfirmed = false,
                            Salt = salt,
                            PasswordHash = passwordHash,
                            IsActive = true,
                            DateCreated = DateTime.Now,
                            LastUpdatedDate = DateTime.Now,
                            UserName = model.Email,
                            FacilitatorTypeId = (int)EnumUtility.FacilitatorType.Internal
                        };

                        await _context.Facilitators.AddAsync(fac);
                        await _context.SaveChangesAsync();


                        //code to send Mail to user for account activation
                        string body = "Welcome to ExpertPlat, use this password " + passwordGenerated + " to login to your account";
                        string MailContent = _emailTemplate.PaymentDisbursementEmailContent(fac.FirstName, body);
                        var subject = "ExpertPlat Registration";
                        EmailMessage message = new EmailMessage(model.Email, MailContent, subject);
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
                        respObj.StatusMessage = "Account created successfully, use the password sent to your mail to verify your account";
                        respObj.Token = tokenString;
                        respObj.Data = respData;

                    }
                }
                if (model.RoleId == (int)EnumUtility.SystemRoles.Super_Administrator || model.RoleId == (int)EnumUtility.SystemRoles.Administrator)
                {
                    //check if email has been used
                    var emailCheckResult = emailcheck.checkIfEmailExist(model.Email, Convert.ToInt64(EnumUtility.UserCategoty.SystemUser));

                    //check if a superAdministrator account has been created
                    var checkSuperAdmin = _context.SystemUserRoles.Where(r => r.RoleId == (int)EnumUtility.SystemRoles.Super_Administrator).FirstOrDefault();

                    if (emailCheckResult == true)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "This Email has been taken" };
                    }
                    else if (model.RoleId == (int)EnumUtility.SystemRoles.Super_Administrator && checkSuperAdmin != null)
                    {
                        return new GenericResponseModel { StatusCode = 409, StatusMessage = "A SuperAdministrator Account Already Exists!" };
                    }
                    else
                    {
                        //generate the default password
                        string password = DefaultPasswordGenerator.defaultPassword();

                        var paswordHasher = new PasswordHasher();
                        //the salt
                        string salt = paswordHasher.getSalt();
                        //Hash the password and salt
                        string passwordHash = paswordHasher.hashedPassword(password, salt);

                        //save the facilitator details
                        var systemUser = new SystemUsers
                        {
                            DateCreated = DateTime.Now,
                            Email = model.Email,
                            FirstName = model.FirstName,
                            IsActive = true,
                            LastName = model.LastName,
                            LastUpdatedDate = DateTime.Now,
                            PasswordHash = passwordHash,
                            Salt = salt,
                            UserName = model.Email
                        };

                        await _context.SystemUsers.AddAsync(systemUser);
                        await _context.SaveChangesAsync();

                        //save the system user role 
                        var systemUserRole = new SystemUserRoles
                        {
                            UserId = systemUser.Id,
                            RoleId = model.RoleId,
                            DateCreated = DateTime.Now,
                        };

                        await _context.SystemUserRoles.AddAsync(systemUserRole);
                        await _context.SaveChangesAsync();


                        //get the System User Role
                        var roleId = _context.SystemUserRoles.Where(c => c.UserId == systemUser.Id).FirstOrDefault().RoleId;
                        var roleName = _context.SystemRoles.Where(c => c.Id == roleId).FirstOrDefault().RoleName;

                        //The data collected from the user on successful creation
                        respData.UserId = systemUser.Id.ToString();
                        respData.FirstName = systemUser.FirstName;
                        respData.LastName = systemUser.LastName;
                        respData.Email = systemUser.Email;
                        respData.SystemUserRole = roleName;

                        //Generate JSON WEB TOKEN for a valid User
                        var tokenString = jwtObj.GenerateJWTToken();

                        //The data to be sent as response
                        respObj.StatusCode = 200;
                        respObj.StatusMessage = "Account created successfully";
                        respObj.Token = tokenString;
                        respObj.Data = respData;
                    }
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

        //-------------------------SignUp, SignIn and Account Activation----------------------------------------
        public async Task<SystemUserLoginResponseModel> systemUserLoginAsync(LoginRequestModel obj)
        {
            try
            {
                SystemUserInfoResponseModel userData = new SystemUserInfoResponseModel();

                //final data to be sent as response to the client
                SystemUserLoginResponseModel respData = new SystemUserLoginResponseModel();

                //JWT
                Jwt jwtObj = new Jwt(_config);

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.SystemUsers.FirstOrDefault(u => u.Email == obj.Email);
                
                if (getUser != null)
                {
                    //get the system user Role
                    var getSystemUserRoleId = _context.SystemUserRoles.Where(r => r.UserId == getUser.Id).FirstOrDefault().RoleId;

                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    if (getUser.PasswordHash != decryptedPassword)
                    {
                        return new SystemUserLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    else
                    {
                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.Email = getUser.Email;
                        userData.IsActive = getUser.IsActive;
                        userData.LastLoginDate = getUser.LastLoginDate;
                        userData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        userData.LastUpdatedDate = getUser.LastUpdatedDate;
                        userData.UserName = getUser.UserName;
                        userData.EmailConfirmed = true;

                        var getRole = from rol in _context.SystemUserRoles
                                      where rol.UserId == getUser.Id
                                      select new
                                      {
                                          rol.UserId,
                                          rol.RoleId,
                                          rol.SystemRoles.RoleName
                                      };

                        //Generate JSON WEB TOKEN for a valid User
                        var tokenString = jwtObj.GenerateJWTToken();

                        //The data to be sent as response
                        respData.StatusCode = 200;
                        respData.StatusMessage = "Login Successful!";
                        respData.Token = tokenString;
                        respData.UserDetails = userData;
                        respData.Roles = getRole.ToList();

                    }
                }
                else
                {
                    return new SystemUserLoginResponseModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                }

                return respData;

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SystemUserLoginResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericResponseModel> getAllSystemRolesAsync()
        {
            try
            {
                var result = from cl in _context.SystemRoles select cl;

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

        public async Task<GenericResponseModel> getSystemRolesByIdAsync(long systemRoleId)
        {
            try
            {
                var result = from cl in _context.SystemRoles where cl.Id == systemRoleId select cl;

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

    }
}

