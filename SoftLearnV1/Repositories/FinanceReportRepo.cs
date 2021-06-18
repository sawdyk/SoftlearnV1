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
using static SoftLearnV1.Utilities.EnumUtility;

namespace SoftLearnV1.Repositories
{
    public class FinanceReportRepo : IFinanceReportRepo
    {
        private readonly AppDbContext _context;

        public FinanceReportRepo(AppDbContext context)
        {
            this._context = context;
        }
        //----------------------------SchoolFeePaymentStatus---------------------------------------------------------------
        public async Task<GenericResponseModel> getFeePaymentStatusAsync(int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                IList<PaymentStatusRespondModel> statusList = new List<PaymentStatusRespondModel>();

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
                var result = from cr in _context.GradeStudents
                             where  cr.SchoolId == schoolId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId
                             };

                if (result.Count() > 0)
                {
                    if (classId != 0)
                    {
                        result = result.Where(x => x.ClassId == classId);
                    }
                    if(gradeId != 0)
                    {
                        result = result.Where(x => x.ClassGradeId == gradeId);
                    }
                    foreach(var student in result)
                    {
                        var payment = await _context.SchoolFeesPayments.Where(x => x.SchoolId == schoolId && x.TermId == termId && x.SessionId == sessionId && x.StudentId == student.StudentId).FirstOrDefaultAsync();
                        if(payment != null)
                        {
                            PaymentStatusRespondModel statusRespondModel = new PaymentStatusRespondModel
                            {
                                AdmissionNo = student.AdmissionNumber,
                                AmountPaid = payment.AmountPaid,
                                Balance = payment.Balance,
                                FirstName = student.FirstName,
                                InvoiceAmount = payment.InvoiceTotal,
                                InvoiceCode = payment.InvoiceCode,
                                IsInvoiceGenerated = true,
                                IsPaymentCompleted = payment.IsPaymentCompleted,
                                LastName = student.LastName,
                                MiddleName = student.MiddleName,
                                PaymentId = payment.Id,
                                StudentId = student.StudentId,
                            };
                            statusList.Add(statusRespondModel);
                        }
                        else
                        {
                            var invoice = await _context.InvoiceTotal.Where(x => x.SchoolId == schoolId && x.TermId == termId && x.SessionId == sessionId && x.StudentId == student.StudentId).FirstOrDefaultAsync();
                            if(invoice != null)
                            {
                                PaymentStatusRespondModel statusRespondModel = new PaymentStatusRespondModel
                                {
                                    AdmissionNo = student.AdmissionNumber,
                                    AmountPaid = 0,
                                    Balance = 0,
                                    FirstName = student.FirstName,
                                    InvoiceAmount = invoice.InvoiceSubTotal,
                                    InvoiceCode = invoice.InvoiceCode,
                                    IsInvoiceGenerated = true,
                                    IsPaymentCompleted = false,
                                    LastName = student.LastName,
                                    MiddleName = student.MiddleName,
                                    PaymentId = 0,
                                    StudentId = student.StudentId
                                };
                                statusList.Add(statusRespondModel);
                            }
                            else
                            {
                                PaymentStatusRespondModel statusRespondModel = new PaymentStatusRespondModel
                                {
                                    AdmissionNo = student.AdmissionNumber,
                                    AmountPaid = 0,
                                    Balance = 0,
                                    FirstName = student.FirstName,
                                    InvoiceAmount = 0,
                                    InvoiceCode = "Nil",
                                    IsInvoiceGenerated = false,
                                    IsPaymentCompleted = false,
                                    LastName = student.LastName,
                                    MiddleName = student.MiddleName,
                                    PaymentId = 0,
                                    StudentId = student.StudentId
                                };
                                statusList.Add(statusRespondModel);
                            }
                            
                        }
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = statusList.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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
        //----------------------------PaymentMethod---------------------------------------------------------------
        public async Task<GenericResponseModel> getAllFeePaymentByMethodAsync(int methodId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
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
                var checkMethod = new CheckerValidation(_context).checkPaymentMethodById(methodId);
                if (checkMethod == false)
                {
                    return new GenericResponseModel { StatusCode = 313, StatusMessage = "Method Doesn't Exist" };
                }
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.SchoolId == schoolId && cr.PaymentMethodId == methodId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId, 
                                 cr.AmountPaid,
                                 cr.ApprovedBy,
                                 cr.BankName,
                                 cr.DateCreated,
                                 cr.DepositorsAccountName,
                                 cr.Id,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.ReferenceCode,
                                 cr.SessionId,
                                 cr.TermId,
                                 cr.VerifiedBy
                             };

                if (result.Count() > 0)
                {
                    if (classId != 0)
                    {
                        result = result.Where(x => x.ClassId == classId);
                    }
                    if (gradeId != 0)
                    {
                        result = result.Where(x => x.ClassGradeId == gradeId);
                    }
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getAllFeePaymentTotalByMethodAsync(int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            try
            {
                long bankTotal = 0;
                long onlineTotal = 0;
                long cardTotal = 0;
                long cashTotal = 0;
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
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.SchoolId == schoolId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Students.AdmissionNumber,
                                 cr.Students.FirstName,
                                 cr.Students.LastName,
                                 cr.Students.MiddleName,
                                 cr.ClassId,
                                 cr.ClassGradeId,
                                 cr.StudentId,
                                 cr.AmountPaid,
                                 cr.ApprovedBy,
                                 cr.BankName,
                                 cr.DateCreated,
                                 cr.DepositorsAccountName,
                                 cr.Id,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.ReferenceCode,
                                 cr.SessionId,
                                 cr.TermId,
                                 cr.VerifiedBy
                             };

                if (result.Count() > 0)
                {
                    if (classId != 0)
                    {
                        result = result.Where(x => x.ClassId == classId);
                    }
                    if (gradeId != 0)
                    {
                        result = result.Where(x => x.ClassGradeId == gradeId);
                    }
                    foreach(var payments in result)
                    {
                        if(payments.PaymentMethodId == (int)PaymentMethod.Bank_Deposit)
                        {
                            bankTotal += payments.AmountPaid;
                        }else if (payments.PaymentMethodId == (int)PaymentMethod.Online_Transfer)
                        {
                            onlineTotal += payments.AmountPaid;
                        }
                        else if (payments.PaymentMethodId == (int)PaymentMethod.Card_Payment)
                        {
                            cardTotal += payments.AmountPaid;
                        }
                        else if (payments.PaymentMethodId == (int)PaymentMethod.Cash_Payment)
                        {
                            cashTotal += payments.AmountPaid;
                        }
                    }
                    TotalPaymentByMethodResponseModel totalPayment = new TotalPaymentByMethodResponseModel
                    {
                        TotalBank = bankTotal,
                        TotalCard = cardTotal,
                        TotalCash = cashTotal,
                        TotalOnline = onlineTotal
                    };
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = totalPayment };
                }

                return new GenericResponseModel { StatusCode = 201, StatusMessage = "No Available Record" };
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
