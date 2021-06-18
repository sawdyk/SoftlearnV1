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
    public class PaymentDisbursementRepo : IPaymentDisbursementRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private IConfiguration _config;
        private readonly EmailTemplate _emailTemplate;

        public PaymentDisbursementRepo(AppDbContext context, IEmailRepo emailRepo, IConfiguration config, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _config = config;
            this._emailTemplate = emailTemplate;
        }

        public async Task<PayStackBulkTransferResponseModel> facilitatorsTotalEarningsAsync(IList<PaymentDisbursementRequestModel> objList)
        {
            try
            {
                //response data
                IList<GenericResponseModel> responseList = new List<GenericResponseModel>();
                IList<object> payStackResp = new List<object>();

                foreach (PaymentDisbursementRequestModel obj in objList)
                {
                    //check if the Id is valid
                    var facEarn = _context.FacilitatorsTotalEarnings.Where(x => x.Id == obj.TotalEarningsId).FirstOrDefault();

                    if (facEarn != null)
                    {
                        //check the facilitators Account details
                        var checkFacilitatorAcctDetails = _context.FacilitatorAccountDetails.Where(x => x.FacilitatorId == facEarn.FacilitatorId && x.IsActive == true).FirstOrDefault();

                        if (checkFacilitatorAcctDetails != null)
                        {
                            //check the facilitators Earnings
                            var checkFacilitatorEarnings = _context.FacilitatorsTotalEarnings.Where(x => x.Id == facEarn.Id && x.FacilitatorId == facEarn.FacilitatorId && x.IsSettled == false).FirstOrDefault();

                            if (checkFacilitatorEarnings != null)
                            {
                                PayStackSingleTransferRequestModel transReq = new PayStackSingleTransferRequestModel();
                                transReq.amount = Convert.ToInt64(checkFacilitatorEarnings.TotalAmountEarned) * 100; //total Amount earned by the facilitator multiplied by 100
                                transReq.recipient = checkFacilitatorAcctDetails.RecipientCode; //the facilitator recipient code after verification of account number and transfer receipt creation


                                //Connsumes PayStack Bulk Transfers API
                                HttpClientConfig httpClientConfig = new HttpClientConfig();
                                var apiResponse = await httpClientConfig.ApiPostRequest("https://api.paystack.co/transfer", transReq, _config["PayStack:SecretTestKey"]);
                                var response = JsonConvert.DeserializeObject<SingleTransferResponseModel>(apiResponse);

                                if (response.status == true)
                                {
                                    payStackResp.Add(response);

                                    //log the transaction to the database
                                    FacilitatorPaymentDisbursements facDisburse = new FacilitatorPaymentDisbursements
                                    {
                                        FacilitatorId = checkFacilitatorAcctDetails.FacilitatorId,
                                        Status = response.status,
                                        Message = response.message,
                                        Reference = response.data.reference,
                                        Integration = response.data.integration,
                                        Domain = response.data.domain,
                                        Amount = (response.data.amount) / 100, //divide the amount from paystack by 100 to get the actual amount transferred
                                        Currency = response.data.currency,
                                        Source = response.data.source,
                                        Reason = response.data.reason,
                                        Recipient = response.data.recipient,
                                        DataStatus = response.data.status,
                                        TransferCode = response.data.transfer_code,
                                        DataId = response.data.id,
                                        CreatedAt = response.data.createdAt,
                                        UpdatedAt = response.data.updatedAt,
                                    };

                                    await _context.FacilitatorPaymentDisbursements.AddAsync(facDisburse);

                                    //update the facilitators isSettled to true
                                    checkFacilitatorEarnings.IsSettled = true;
                                    checkFacilitatorEarnings.DateSettled = DateTime.Now;

                                    await _context.SaveChangesAsync();

                                    //get the facilitator's Email Address
                                    Facilitators fac = _context.Facilitators.Where(c => c.Id == facEarn.FacilitatorId).FirstOrDefault();

                                    //Send Mail to Facilitator
                                    //EmailTemplate emailTemp = new EmailTemplate();
                                    //var MailContent = emailTemp.EmailHtmlTemplate(codeGenerated);
                                    //***************************** Add a table listing all the courses bought for the particular date
                                    var subject = "ExpertPlat Payment";
                                    string body = $"A total Amount of {checkFacilitatorEarnings.TotalAmountEarned} earned on your courses on {checkFacilitatorEarnings.DateEarned.Date} has been paid into your account and Transaction is being Processed.";
                                    string MailContent = _emailTemplate.PaymentDisbursementEmailContent(fac.FirstName, body);
                                    EmailMessage message = new EmailMessage(fac.Email, MailContent, subject);
                                    _emailRepo.SendEmail(message);


                                    //return the transaction details logged
                                    var result = from cr in _context.FacilitatorPaymentDisbursements.Where(c => c.Id == facDisburse.Id).AsNoTracking() select cr;

                                    GenericResponseModel resp1 = new GenericResponseModel();
                                    resp1.StatusCode = 200;
                                    resp1.StatusMessage = string.Format("Payment For Facilitator with ID: {0} and Recipient_Code {1} was Successful", checkFacilitatorAcctDetails.FacilitatorId, checkFacilitatorAcctDetails.RecipientCode);
                                    resp1.Data = result.FirstOrDefault();
                                    responseList.Add(resp1);

                                }
                                else
                                {
                                    GenericResponseModel resp1 = new GenericResponseModel();
                                    resp1.StatusCode = 409;
                                    resp1.StatusMessage = "An Error Occured Trying to Process This Transfer Request";
                                    responseList.Add(resp1);
                                }
                            }
                            else
                            {
                                GenericResponseModel resp1 = new GenericResponseModel();
                                resp1.StatusCode = 409;
                                resp1.StatusMessage = string.Format("Facilitator with ID: {0} has been settled", facEarn.FacilitatorId);
                                responseList.Add(resp1);
                            }
                        }
                        else
                        {
                            GenericResponseModel resp1 = new GenericResponseModel();
                            resp1.StatusCode = 409;
                            resp1.StatusMessage = string.Format("Facilitator with ID: {0} does not have an account or set a default account", facEarn.FacilitatorId);
                            responseList.Add(resp1);
                        }
                    }
                    else
                    {
                        GenericResponseModel resp1 = new GenericResponseModel();
                        resp1.StatusCode = 409;
                        resp1.StatusMessage = string.Format("Facilitator Earnings with ID: {0} does not exists", obj.TotalEarningsId);
                        responseList.Add(resp1);
                    }
                }


                return new PayStackBulkTransferResponseModel { StatusCode = 200, StatusMessage = "Success", Data = responseList, PayStackData = payStackResp };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new PayStackBulkTransferResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<PayStackBulkTransferResponseModel> learnersCourseRefundAsync(IList<LearnerPaymentDisbursementRequestModel> objList)
        {
            try
            {
                //response data
                IList<GenericResponseModel> responseList = new List<GenericResponseModel>();
                IList<object> payStackResp = new List<object>();

                foreach (LearnerPaymentDisbursementRequestModel obj in objList)
                {
                    //check if the Id is valid
                    var lrnRef = _context.CourseRefund.Where(x => x.Id == obj.CourseRefundId).FirstOrDefault();

                    if (lrnRef != null)
                    {
                        //check the Learners Account details
                        var checkLearnerAcctDetails = _context.LearnerAccountDetails.Where(x => x.LearnerId == lrnRef.LearnerId).FirstOrDefault();

                        if (checkLearnerAcctDetails != null)
                        {
                            //check the facilitators Earnings
                            var checkLearnerCourseRefund = _context.CourseRefund.Where(x => x.Id == lrnRef.Id && x.LearnerId == lrnRef.LearnerId && x.IsSettled == false).FirstOrDefault();

                            if (checkLearnerCourseRefund != null)
                            {
                                //get the course obj using the courseId
                                Courses course = _context.Courses.Where(x => x.Id == checkLearnerCourseRefund.CourseId && x.IsApproved == true && x.IsVerified == true).FirstOrDefault();
                               
                                if (course != null)
                                {
                                    long courseAmount = course.CourseAmount; //the original Amount of the course to be refunded

                                    PayStackSingleTransferRequestModel transReq = new PayStackSingleTransferRequestModel();
                                    transReq.amount = courseAmount;
                                    transReq.recipient = checkLearnerAcctDetails.RecipientCode; //the facilitator recipient code after verification of account number and transfer receipt creation

                                    //Connsumes PayStack Bulk Transfers API
                                    HttpClientConfig httpClientConfig = new HttpClientConfig();
                                    var apiResponse = await httpClientConfig.ApiPostRequest("https://api.paystack.co/transfer", transReq, _config["PayStack:SecretTestKey"]);
                                    var response = JsonConvert.DeserializeObject<SingleTransferResponseModel>(apiResponse);

                                    if (response.status == true)
                                    {
                                        payStackResp.Add(response);

                                        //log the transaction to the database
                                        LearnersPaymentDisbursements lrnDisburse = new LearnersPaymentDisbursements
                                        {
                                            LearnerId = checkLearnerAcctDetails.LearnerId,
                                            Status = response.status,
                                            Message = response.message,
                                            Reference = response.data.reference,
                                            Integration = response.data.integration,
                                            Domain = response.data.domain,
                                            Amount = (response.data.amount) / 100, //divide the amount from paystack by 100 to get the actual amount transferred
                                            Currency = response.data.currency,
                                            Source = response.data.source,
                                            Reason = response.data.reason,
                                            Recipient = response.data.recipient,
                                            DataStatus = response.data.status,
                                            TransferCode = response.data.transfer_code,
                                            DataId = response.data.id,
                                            CreatedAt = response.data.createdAt,
                                            UpdatedAt = response.data.updatedAt,
                                        };

                                        await _context.LearnersPaymentDisbursements.AddAsync(lrnDisburse);

                                        //update the Learners isSettled to true
                                        checkLearnerCourseRefund.IsSettled = true;
                                        checkLearnerCourseRefund.DateSettled = DateTime.Now;

                                        await _context.SaveChangesAsync();

                                        //return the transaction details logged
                                        var result = from cr in _context.LearnersPaymentDisbursements.Where(c => c.Id == lrnDisburse.Id).AsNoTracking() select cr;

                                        GenericResponseModel resp1 = new GenericResponseModel();
                                        resp1.StatusCode = 200;
                                        resp1.StatusMessage = string.Format("Refund For Learner with ID: {0} and Recipient_Code {1} was Successful", lrnRef.LearnerId, checkLearnerAcctDetails.RecipientCode);
                                        resp1.Data = result.FirstOrDefault();
                                        responseList.Add(resp1);
                                    }
                                    else
                                    {
                                        GenericResponseModel resp1 = new GenericResponseModel();
                                        resp1.StatusCode = 409;
                                        resp1.StatusMessage = "An Error Occured Trying to Process This Transfer Request";
                                        responseList.Add(resp1);
                                    }
                                }
                                else
                                {
                                    GenericResponseModel resp1 = new GenericResponseModel();
                                    resp1.StatusCode = 409;
                                    resp1.StatusMessage = string.Format("Course does not Exist or Course has not been Approved and Verified");
                                    responseList.Add(resp1);
                                }
                            }
                            else
                            {
                                GenericResponseModel resp1 = new GenericResponseModel();
                                resp1.StatusCode = 409;
                                resp1.StatusMessage = string.Format("Learner with ID: {0} has been settled", lrnRef.LearnerId);
                                responseList.Add(resp1);
                            }
                        }
                        else
                        {
                            GenericResponseModel resp1 = new GenericResponseModel();
                            resp1.StatusCode = 409;
                            resp1.StatusMessage = string.Format("Learner with ID: {0} does not have acount details", lrnRef.LearnerId);
                            responseList.Add(resp1);
                        }
                    }
                    else
                    {
                        GenericResponseModel resp1 = new GenericResponseModel();
                        resp1.StatusCode = 409;
                        resp1.StatusMessage = string.Format("Learner Course Refund with ID: {0} does not exists", obj.CourseRefundId);
                        responseList.Add(resp1);
                    }
                }


                return new PayStackBulkTransferResponseModel { StatusCode = 200, StatusMessage = "Success", Data = responseList, PayStackData = payStackResp };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new PayStackBulkTransferResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
