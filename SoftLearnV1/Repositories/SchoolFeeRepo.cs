using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.Utilities;

namespace SoftLearnV1.InterfaceRepositories
{
    public class SchoolFeeRepo : ISchoolFeeRepo
    {
        private readonly AppDbContext _context;
        private readonly InvoiceNumberGenerator _invoiceNumber;

        public SchoolFeeRepo(AppDbContext context, InvoiceNumberGenerator invoiceNumber)
        {
            this._context = context;
            this._invoiceNumber = invoiceNumber;
        }

        public async Task<GenericResponseModel> createSchoolFeeAsync(SchoolFeeRequestModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }

                var checkSession = new CheckerValidation(_context).checkSessionById(obj.SessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(obj.TermId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Term Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(obj.ClassId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Class Doesn't Exist" };
                }
                foreach (TemplateList template in obj.TemplateList)
                {
                    var checkSubCategory = new CheckerValidation(_context).checkFeeSubCategoryById(template.FeeSubCategoryId);
                    if (checkSubCategory == false)
                    {
                        data.Add(new GenericResponseModel { StatusCode = 305, StatusMessage = "Sub Category Doesn't Exist" });
                        continue;
                    }
                    var checkTemplate = new CheckerValidation(_context).checkFeeTemplateById(template.TemplateId);
                    if (checkTemplate == false)
                    {
                        data.Add(new GenericResponseModel { StatusCode = 303, StatusMessage = "Template Doesn't Exist" });
                        continue;
                    }
                    //check if SchoolFee to be created already exists
                    var checkResult = _context.SchoolFees.Where(x => x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.ClassId == obj.ClassId && x.SessionId == obj.SessionId && x.TermId == obj.TermId && x.FeeSubCategoryId == template.FeeSubCategoryId).FirstOrDefault();

                    //if the template doesnt exist, Create the template
                    if (checkResult == null)
                    {
                        var schoolFee = new SchoolFee
                        {
                            Amount = template.Amount,
                            IsMandatory = template.IsMandatory,
                            CampusId = obj.CampusId,
                            FeeSubCategoryId = template.FeeSubCategoryId,
                            SchoolId = obj.SchoolId,
                            ClassId = obj.ClassId,
                            SessionId = obj.SessionId,
                            TermId = obj.TermId,
                            TemplateId = template.TemplateId,
                            LastUpdated = DateTime.Now,
                            DateCreated = DateTime.Now,
                        };

                        await _context.SchoolFees.AddAsync(schoolFee);
                        await _context.SaveChangesAsync();

                        //get the Fee Created
                        var getSchoolFee = from cr in _context.SchoolFees
                                           where cr.Id == schoolFee.Id
                                           select new
                                           {
                                               cr.Amount,
                                               cr.CampusId,
                                               cr.ClassId,
                                               cr.Classes.ClassName,
                                               cr.DateCreated,
                                               cr.Id,
                                               cr.IsMandatory,
                                               cr.LastUpdated,
                                               cr.SchoolCampuses.CampusName,
                                               cr.SchoolId,
                                               cr.SchoolInformation.SchoolName,
                                               cr.SessionId,
                                               cr.Sessions.SessionName,
                                               cr.FeeTemplate.TemplateName,
                                               cr.TermId,
                                               cr.Terms.TermName,
                                               cr.TemplateId,
                                               cr.FeeSubCategory.SubCategoryName,
                                               cr.FeeSubCategoryId
                                           };
                        data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "School Fee Created Successfully", Data = getSchoolFee.FirstOrDefault() });
                    }
                    else
                    {
                        data.Add(new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Already Exists" });
                    }
                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };

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
                data.Add(new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" });
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Success", Data = data.ToList() };
            }
        }
        public async Task<GenericResponseModel> updateSchoolFeeAsync(long schoolFeeId, SchoolFeeRequestModel obj)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(obj.SchoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var checkCampus = new CheckerValidation(_context).checkSchoolCampusById(obj.CampusId);
                if (checkCampus == false)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Campus Doesn't Exist" };
                }

                var checkSession = new CheckerValidation(_context).checkSessionById(obj.SessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(obj.TermId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Term Doesn't Exist" };
                }
                var checkClass = new CheckerValidation(_context).checkClassById(obj.ClassId);
                if (checkClass == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Class Doesn't Exist" };
                }

                //check if SchoolFee to be created already exists
                var checkResult = _context.SchoolFees.Where(x => x.Id == schoolFeeId).FirstOrDefault();

                //if the fee exist, Update the fee
                if (checkResult != null)
                {
                    checkResult.Amount = obj.UpdateAmount;
                    checkResult.IsMandatory = obj.UpdateIsMandatory;
                    checkResult.CampusId = obj.CampusId;
                    checkResult.SchoolId = obj.SchoolId;
                    checkResult.ClassId = obj.ClassId;
                    checkResult.SessionId = obj.SessionId;
                    checkResult.TermId = obj.TermId;
                    checkResult.LastUpdated = DateTime.Now;
                    await _context.SaveChangesAsync();

                    //get the Fee Updated
                    var getSchoolFee = from cr in _context.SchoolFees
                                       where cr.Id == schoolFeeId
                                       select new
                                       {
                                           cr.Amount,
                                           cr.CampusId,
                                           cr.ClassId,
                                           cr.Classes.ClassName,
                                           cr.DateCreated,
                                           cr.Id,
                                           cr.IsMandatory,
                                           cr.LastUpdated,
                                           cr.SchoolCampuses.CampusName,
                                           cr.SchoolId,
                                           cr.SchoolInformation.SchoolName,
                                           cr.SessionId,
                                           cr.Sessions.SessionName,
                                           cr.FeeTemplate.TemplateName,
                                           cr.TermId,
                                           cr.Terms.TermName,
                                           cr.TemplateId,
                                           cr.FeeSubCategory.SubCategoryName,
                                           cr.FeeSubCategoryId
                                       };
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "School Fee Updated Successfully", Data = getSchoolFee.FirstOrDefault() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Fee Doesn't Exist" };
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

        public async Task<GenericResponseModel> deleteSchoolFeeAsync(long schoolFeeId)
        {
            try
            {
                var obj = _context.SchoolFees.Where(x => x.Id == schoolFeeId).FirstOrDefault();
                if (obj != null)
                {
                    _context.SchoolFees.Remove(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Fee With the Specified ID!" };
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
        public async Task<GenericResponseModel> deleteSchoolFeesByTemplateIdAsync(long templateId)
        {
            try
            {
                var obj = _context.SchoolFees.Where(x => x.TemplateId == templateId).ToList();
                if (obj.Count > 0)
                {
                    _context.SchoolFees.RemoveRange(obj);
                    await _context.SaveChangesAsync();

                    return new GenericResponseModel { StatusCode = 200, StatusMessage = obj.Count.ToString() + " records Deleted Successfully!" };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Template With the Specified ID!" };
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

        public async Task<GenericResponseModel> getAllSchoolFeesAsync(GetSchoolFeesRequestModel obj)
        {
            try
            {
                var checkChild = new CheckerValidation(_context).checkStudentById(obj.ChildId);
                if (checkChild == false)
                {
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Child Doesn't Exist" };
                }
                var checkParent = new CheckerValidation(_context).checkParentById(obj.ParentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 308, StatusMessage = "Parent Doesn't Exist" };
                }
                ParentsStudentsMap parentsStudentsMap = _context.ParentsStudentsMap.Where(x => x.ParentId == obj.ParentId && x.StudentId == obj.ChildId).FirstOrDefault();
                if (parentsStudentsMap == null)
                {
                    return new GenericResponseModel { StatusCode = 309, StatusMessage = "Wrong parent and child combination" };
                }
                var checkSession = new CheckerValidation(_context).checkSessionById(obj.SessionId);
                if (checkSession == false)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(obj.TermId);
                if (checkTerm == false)
                {
                    return new GenericResponseModel { StatusCode = 304, StatusMessage = "Term Doesn't Exist" };
                }
                var childInClass = _context.GradeStudents.Where(x => x.StudentId == obj.ChildId).FirstOrDefault();
                if (childInClass == null)
                {
                    return new GenericResponseModel { StatusCode = 305, StatusMessage = "Your child has not been assigned to any class" };
                }
                var result = from cr in _context.SchoolFees
                             where cr.CampusId == childInClass.CampusId && cr.SchoolId == childInClass.SchoolId && cr.ClassId == childInClass.ClassId
                             && cr.SessionId == obj.SessionId && cr.TermId == obj.TermId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Amount,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.IsMandatory,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.FeeTemplate.TemplateName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.TemplateId,
                                 cr.FeeSubCategory.SubCategoryName,
                                 cr.FeeSubCategoryId
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getSchoolFeesByIdAsync(long schoolFeeId)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolFeeById(schoolFeeId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "SchoolFee Doesn't Exist" };
                }
                var result = from cr in _context.SchoolFees
                             where cr.Id == schoolFeeId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Amount,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.IsMandatory,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.FeeTemplate.TemplateName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.TemplateId,
                                 cr.FeeSubCategory.SubCategoryName,
                                 cr.FeeSubCategoryId
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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


        public async Task<GenericResponseModel> getAllSchoolFeesBySchoolIdAsync(long schoolId)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var result = from cr in _context.SchoolFees
                             where cr.SchoolId == schoolId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Amount,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.IsMandatory,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.FeeTemplate.TemplateName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.TemplateId,
                                 cr.FeeSubCategory.SubCategoryName,
                                 cr.FeeSubCategoryId
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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
        //----------------------------Invoice---------------------------------------------------------------
        public async Task<InvoiceResponseModel> generateInvoiceAsync(InvoiceGenerationRequestModel obj)
        {
            try
            {
                var checkSession = new CheckerValidation(_context).checkSessionById(obj.SessionId);
                if (checkSession == false)
                {
                    return new InvoiceResponseModel { StatusCode = 310, StatusMessage = "Session Doesn't Exist" };
                }
                var checkTerm = new CheckerValidation(_context).checkTermById(obj.TermId);
                if (checkTerm == false)
                {
                    return new InvoiceResponseModel { StatusCode = 311, StatusMessage = "Term Doesn't Exist" };
                }
                var checkChild = new CheckerValidation(_context).checkStudentById(obj.ChildId);
                if (checkChild == false)
                {
                    return new InvoiceResponseModel { StatusCode = 300, StatusMessage = "Child Doesn't Exist" };
                }
                var checkParent = new CheckerValidation(_context).checkParentById(obj.ParentId);
                if (checkParent == false)
                {
                    return new InvoiceResponseModel { StatusCode = 308, StatusMessage = "Parent Doesn't Exist" };
                }
                ParentsStudentsMap parentsStudentsMap = _context.ParentsStudentsMap.Where(x => x.ParentId == obj.ParentId && x.StudentId == obj.ChildId).FirstOrDefault();
                if (parentsStudentsMap == null)
                {
                    return new InvoiceResponseModel { StatusCode = 309, StatusMessage = "Wrong parent and child combination" };
                }
                var childInClass = _context.GradeStudents.Where(x => x.StudentId == obj.ChildId).FirstOrDefault();
                if (childInClass == null)
                {
                    return new InvoiceResponseModel { StatusCode = 305, StatusMessage = "Your child has not been assigned to any class" };
                }
                //check if invoice has been generated b4
                InvoiceTotal invoiceTotal = _context.InvoiceTotal.Where(x => x.CampusId == childInClass.CampusId && x.ClassId == childInClass.ClassId && x.ParentId == obj.ParentId && x.SchoolId == childInClass.SchoolId && x.SessionId == obj.SessionId && x.StudentId == obj.ChildId && x.TermId == obj.TermId).FirstOrDefault();
                if (invoiceTotal != null)
                {
                    SchoolFeesPayments feesPayments = _context.SchoolFeesPayments.Where(x => x.InvoiceCode == invoiceTotal.InvoiceCode).FirstOrDefault();
                    if (feesPayments != null && feesPayments.IsPaymentCompleted == true)
                    {
                        return new InvoiceResponseModel { StatusCode = 301, StatusMessage = "An Invoice has been generated for this Child and Payments has been made completely for this Term and session" };
                    }
                    else if (feesPayments != null && feesPayments.IsPaymentCompleted == false)
                    {
                        return new InvoiceResponseModel { StatusCode = 303, StatusMessage = "An Invoice Exists For this Student, and Payment has been made but not completed. Kindly Check the Invoice to balance Payment before generating a new Invoice!" };
                    }
                    else
                    {
                        return new InvoiceResponseModel { StatusCode = 302, StatusMessage = "An Invoice Exists For this Student, and payment has not been made. You may Kindly delete an existing Invoice before generating a new Invoice!" };
                    }
                }
                else
                {
                    decimal minimumTotal = 0;
                    decimal total = 0;
                    string invoiceCode = _invoiceNumber.GetInvoiceNumber(childInClass.SchoolId).ToString();
                    //check if the fee list submitted contains all the mandatory fee defined
                    var schoolFees = _context.SchoolFees.Where(x => x.SchoolId == childInClass.SchoolId && x.CampusId == childInClass.CampusId && x.ClassId == childInClass.ClassId && x.SessionId == obj.SessionId && x.TermId == obj.TermId && x.IsMandatory == true).ToList();
                    if (schoolFees.Count() < 0)
                    {
                        return new InvoiceResponseModel { StatusCode = 304, StatusMessage = "No fee found" };
                    }
                    foreach (var fee in schoolFees)
                    {
                        if (!obj.feeList.Select(x => x.subCategoryId).Contains(fee.FeeSubCategoryId))
                        {
                            return new InvoiceResponseModel { StatusCode = 305, StatusMessage = "List of fees submitted does not contain all the mandatory fees" };
                        }
                        //minimum total expected for mandatory list
                        minimumTotal += fee.Amount;
                    }
                    //Get total amount of subcategory selected and check if they all exist
                    foreach (var fee in obj.feeList)
                    {
                        var schoolFee = _context.SchoolFees.Where(x => x.SchoolId == childInClass.SchoolId && x.CampusId == childInClass.CampusId && x.ClassId == childInClass.ClassId && x.SessionId == obj.SessionId && x.TermId == obj.TermId && x.FeeSubCategoryId == fee.subCategoryId && x.Amount == fee.Amount).FirstOrDefault();
                        if (schoolFee == null)
                        {
                            return new InvoiceResponseModel { StatusCode = 306, StatusMessage = "One of the selected fees and amount doesn't  exist" };
                        }
                        total += fee.Amount;
                    }
                    //compare
                    if (total < minimumTotal)
                    {
                        return new InvoiceResponseModel { StatusCode = 307, StatusMessage = "Total amount generated is lesser than the expected minimum  amount" };
                    }

                    //Create the invoice summary
                    InvoiceTotal invoice = new InvoiceTotal
                    {
                        CampusId = childInClass.CampusId,
                        ClassGradeId = childInClass.ClassGradeId,
                        ClassId = childInClass.ClassId,
                        DateGenerated = DateTime.Now,
                        InvoiceSubTotal = Convert.ToInt64(total),
                        InvoiceCode = invoiceCode,
                        ParentId = obj.ParentId,
                        SchoolId = childInClass.SchoolId,
                        SessionId = obj.SessionId,
                        StudentId = obj.ChildId,
                        TermId = obj.TermId
                    };
                    await _context.InvoiceTotal.AddAsync(invoice);
                    await _context.SaveChangesAsync();
                    //create list under the invoice created above
                    foreach (var fee in obj.feeList)
                    {
                        InvoiceList invoiceList = new InvoiceList
                        {
                            Amount = fee.Amount,
                            CampusId = childInClass.CampusId,
                            ClassGradeId = childInClass.ClassGradeId,
                            ClassId = childInClass.ClassId,
                            DateGenerated = DateTime.Now,
                            FeeSubCategoryId = fee.subCategoryId,
                            InvoiceCode = invoiceCode,
                            InvoiceTotalId = invoice.Id,
                            ParentId = obj.ParentId,
                            SchoolId = childInClass.SchoolId,
                            SessionId = obj.SessionId,
                            StudentId = obj.ChildId,
                            TermId = obj.TermId,
                        };
                        await _context.InvoiceList.AddAsync(invoiceList);
                        await _context.SaveChangesAsync();
                    }
                    //get the Invoice Created
                    var getInvoice = from cr in _context.InvoiceTotal
                                     where cr.Id == invoice.Id
                                     select new
                                     {
                                         cr.Id,
                                         cr.InvoiceSubTotal,
                                         cr.CampusId,
                                         cr.ClassId,
                                         cr.Classes.ClassName,
                                         cr.DateGenerated,
                                         cr.SchoolCampuses.CampusName,
                                         cr.SchoolId,
                                         cr.SchoolInformation.SchoolName,
                                         cr.SessionId,
                                         cr.Sessions.SessionName,
                                         cr.TermId,
                                         cr.Terms.TermName,
                                         cr.ClassGradeId,
                                         cr.ClassGrades.GradeName,
                                         cr.InvoiceCode,
                                         cr.ParentId,
                                         cr.StudentId,
                                         Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                         cr.Students.AdmissionNumber
                                     };
                    //get the Invoice List Created
                    var getInvoiceList = from cr in _context.InvoiceList
                                         where cr.InvoiceTotalId == invoice.Id
                                         select new
                                         {
                                             cr.Amount,
                                             cr.CampusId,
                                             cr.ClassId,
                                             cr.Classes.ClassName,
                                             cr.DateGenerated,
                                             cr.Id,
                                             cr.SchoolCampuses.CampusName,
                                             cr.SchoolId,
                                             cr.SchoolInformation.SchoolName,
                                             cr.SessionId,
                                             cr.Sessions.SessionName,
                                             cr.TermId,
                                             cr.Terms.TermName,
                                             cr.FeeSubCategory.SubCategoryName,
                                             cr.FeeSubCategoryId,
                                             cr.ClassGradeId,
                                             cr.ClassGrades.GradeName,
                                             cr.InvoiceCode,
                                             cr.InvoiceTotalId,
                                             cr.ParentId,
                                             cr.StudentId
                                         };
                    return new InvoiceResponseModel { StatusCode = 200, StatusMessage = "Invoice Generated Successfully", Invoice = getInvoice.FirstOrDefault(), InvoiceList = getInvoiceList.ToList() };
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
                return new InvoiceResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericResponseModel> deleteInvoiceAsync(string invoiceCode)
        {
            try
            {
                var obj = _context.InvoiceTotal.Where(x => x.InvoiceCode.Trim() == invoiceCode.Trim()).FirstOrDefault();
                if (obj != null)
                {
                    var payment = _context.SchoolTemporaryPayments.Where(x => x.InvoiceCode == invoiceCode).ToList();
                    if (payment.Count == 0)
                    {
                        var invoiceList = _context.InvoiceList.Where(x => x.InvoiceCode == invoiceCode).ToList();
                        if (invoiceList.Count > 0)
                        {
                            _context.InvoiceList.RemoveRange(invoiceList);
                            await _context.SaveChangesAsync();
                        }
                        _context.InvoiceTotal.Remove(obj);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = "Deleted Successfully!" };
                    }
                    return new GenericResponseModel { StatusCode = 403, StatusMessage = "Invoice can't be deleted because there's a transaction on it!" };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Invoice With the Specified Invoice Code!" };
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
        public async Task<GenericResponseModel> getAllParentInvoiceAsync(Guid parentId)
        {
            try
            {
                var checkParent = new CheckerValidation(_context).checkParentById(parentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Parent Doesn't Exist" };
                }
                var result = from cr in _context.InvoiceTotal
                             where cr.ParentId == parentId
                             orderby cr.Id descending
                             select new
                             {
                                 cr.Id,
                                 cr.InvoiceSubTotal,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateGenerated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.ClassGradeId,
                                 cr.ClassGrades.GradeName,
                                 cr.InvoiceCode,
                                 cr.ParentId,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                 cr.Students.AdmissionNumber
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getAllSchoolInvoiceAsync(long schoolId)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var result = from cr in _context.InvoiceTotal
                             where cr.SchoolId == schoolId
                             orderby cr.Id descending
                             select new
                             {
                                 cr.Id,
                                 cr.InvoiceSubTotal,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateGenerated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.ClassGradeId,
                                 cr.ClassGrades.GradeName,
                                 cr.InvoiceCode,
                                 cr.ParentId,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                 cr.Students.AdmissionNumber
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> getInvoiceByIdAsync(long invoiceId)
        {
            try
            {
                var checkInvoice = new CheckerValidation(_context).checkInvoiceById(invoiceId);
                if (checkInvoice == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Invoice Doesn't Exist" };
                }
                var result = from cr in _context.InvoiceTotal
                             where cr.Id == invoiceId
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Id,
                                 cr.InvoiceSubTotal,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateGenerated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.ClassGradeId,
                                 cr.ClassGrades.GradeName,
                                 cr.InvoiceCode,
                                 cr.ParentId,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                 cr.Students.AdmissionNumber
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericResponseModel { StatusCode = 300, StatusMessage = "No Available Record" };
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
        //----------------------------Payment---------------------------------------------------------------
        public async Task<GenericResponseModel> getAllPaymentMethodAsync()
        {
            try
            {
                var result = from cr in _context.PaymentMethods
                             orderby cr.Id ascending
                             select new
                             {
                                 cr.Id,
                                 cr.MethodName
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericResponseModel { StatusCode = 404, StatusMessage = "No Available Record" };
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

        public async Task<GenericResponseModel> savePaymentAsync(PaymentRequestModel obj)
        {
            try
            {
                var checkParent = new CheckerValidation(_context).checkParentById(obj.ParentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Parent Doesn't Exist" };
                }
                var checkPaymentMethod = new CheckerValidation(_context).checkPaymentMethodById(obj.PaymentMethodId);
                if (checkPaymentMethod == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Payment Method Doesn't Exist" };
                }
                InvoiceTotal invoice = _context.InvoiceTotal.Where(x => x.InvoiceCode.ToLower().Trim() == obj.InvoiceCode.ToLower().Trim() && x.ParentId == obj.ParentId).FirstOrDefault();
                if (invoice == null)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Invoice Code doesn't exist for this parent" };
                }

                //check if record has been saved b4
                var checkResult = _context.SchoolTemporaryPayments.Where(x => x.InvoiceCode == invoice.InvoiceCode && x.ParentId == invoice.ParentId && x.ReferenceCode == obj.ReferenceCode).FirstOrDefault();

                //if the record doesnt exist, Create the record
                if (checkResult == null)
                {
                    var payment = new SchoolTemporaryPayments
                    {
                        AmountPaid = obj.AmountPaid,
                        BankName = obj.BankName,
                        CampusId = invoice.CampusId,
                        CardType = obj.CardType,
                        SchoolId = invoice.SchoolId,
                        ClassId = invoice.ClassId,
                        SessionId = invoice.SessionId,
                        TermId = invoice.TermId,
                        ClassGradeId = invoice.ClassGradeId,
                        InvoiceCode = invoice.InvoiceCode,
                        IsApproved = false,
                        IsVerified = false,
                        DepositorsAccountName = obj.DepositorsAccountName,
                        ParentId = invoice.ParentId,
                        PaymentMethodId = obj.PaymentMethodId,
                        ReferenceCode = obj.ReferenceCode,
                        StudentId = invoice.StudentId,
                        PaymentDate = obj.PaymentDate,
                        LastUpdated = DateTime.Now,
                        DateCreated = DateTime.Now,
                    };

                    await _context.SchoolTemporaryPayments.AddAsync(payment);
                    await _context.SaveChangesAsync();

                    //get the Payment Created
                    var getPayment = from cr in _context.SchoolTemporaryPayments
                                     where cr.Id == payment.Id
                                     select new
                                     {
                                         cr.AmountPaid,
                                         cr.CampusId,
                                         cr.ClassId,
                                         cr.Classes.ClassName,
                                         cr.DateCreated,
                                         cr.Id,
                                         cr.BankName,
                                         cr.LastUpdated,
                                         cr.SchoolCampuses.CampusName,
                                         cr.SchoolId,
                                         cr.SchoolInformation.SchoolName,
                                         cr.SessionId,
                                         cr.Sessions.SessionName,
                                         cr.ReferenceCode,
                                         cr.TermId,
                                         cr.Terms.TermName,
                                         cr.DepositorsAccountName,
                                         cr.InvoiceCode,
                                         cr.IsApproved,
                                         cr.IsVerified,
                                         cr.ParentId,
                                         cr.PaymentDate,
                                         cr.PaymentMethodId,
                                         cr.PaymentMethods.MethodName,
                                         cr.StudentId,
                                         Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                     };
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Payment Created Successfully", Data = getPayment.FirstOrDefault() };
                }
                else
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Reference Number has already been used by you" };
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

        public async Task<GenericResponseModel> saveCashPaymentAsync(CashPaymentRequestModel obj)
        {
            try
            {
                var checkParent = new CheckerValidation(_context).checkParentById(obj.ParentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 300, StatusMessage = "Parent Doesn't Exist" };
                }
                SchoolUsers schoolUsers = _context.SchoolUsers.Where(x => x.Id == obj.FinanceUserId).FirstOrDefault();
                if (schoolUsers == null)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "User Doesn't Exist" };
                }
                InvoiceTotal invoice = _context.InvoiceTotal.Where(x => x.InvoiceCode.ToLower().Trim() == obj.InvoiceCode.ToLower().Trim() && x.ParentId == obj.ParentId).FirstOrDefault();
                if (invoice == null)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "Invoice Code doesn't exist for this parent" };
                }

                var payment = new SchoolTemporaryPayments
                {
                    AmountPaid = obj.AmountPaid,
                    CampusId = invoice.CampusId,
                    SchoolId = invoice.SchoolId,
                    ClassId = invoice.ClassId,
                    SessionId = invoice.SessionId,
                    TermId = invoice.TermId,
                    ClassGradeId = invoice.ClassGradeId,
                    InvoiceCode = invoice.InvoiceCode,
                    IsApproved = false,
                    IsVerified = true,
                    VerifiedBy = obj.FinanceUserId,
                    ParentId = invoice.ParentId,
                    PaymentMethodId = (int)EnumUtility.PaymentMethod.Cash_Payment,
                    StudentId = invoice.StudentId,
                    PaymentDate = obj.PaymentDate,
                    LastUpdated = DateTime.Now,
                    DateCreated = DateTime.Now,
                    };

                    await _context.SchoolTemporaryPayments.AddAsync(payment);
                    await _context.SaveChangesAsync();
                
                //check for payment total record
                SchoolFeesPayments feesPayments = _context.SchoolFeesPayments.Where(x => x.InvoiceCode.ToLower().Trim() == obj.InvoiceCode.ToLower().Trim()).FirstOrDefault();
                if (feesPayments == null)
                {
                    long balance = invoice.InvoiceSubTotal - obj.AmountPaid;
                    long amountPaidRecord;
                    bool isPaymentCompleted = false;
                    if (obj.AmountPaid > invoice.InvoiceSubTotal)
                    {
                        amountPaidRecord = obj.AmountPaid + balance;
                        isPaymentCompleted = true;
                    }
                    else if (obj.AmountPaid == invoice.InvoiceSubTotal)
                    {
                        amountPaidRecord = obj.AmountPaid;
                        isPaymentCompleted = true;
                    }
                    else
                    {
                        amountPaidRecord = obj.AmountPaid;
                    }
                    //create new total record
                    SchoolFeesPayments schoolFeesPayments = new SchoolFeesPayments
                    {
                        AmountPaid = amountPaidRecord,
                        Balance = balance,
                        CampusId = invoice.CampusId,
                        ClassGradeId = invoice.ClassGradeId,
                        ClassId = invoice.ClassId,
                        DateCreated = DateTime.Now,
                        InvoiceCode = invoice.InvoiceCode,
                        InvoiceTotal = invoice.InvoiceSubTotal,
                        IsPaymentCompleted = isPaymentCompleted,
                        LastUpdated = DateTime.Now,
                        ParentId = obj.ParentId,
                        SchoolId = invoice.SchoolId,
                        SessionId = invoice.SessionId,
                        StudentId = invoice.StudentId,
                        TermId = invoice.TermId
                    };
                    await _context.SchoolFeesPayments.AddAsync(schoolFeesPayments);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    bool isPaymentCompleted = false;
                    long amountNeeded = 0;
                    long balance = 0;
                    long expectedAmount = feesPayments.AmountPaid + obj.AmountPaid;
                    if (expectedAmount > invoice.InvoiceSubTotal)
                    {
                        amountNeeded = feesPayments.InvoiceTotal - feesPayments.AmountPaid;
                        balance = obj.AmountPaid;
                        isPaymentCompleted = true;
                    }
                    else if (expectedAmount == invoice.InvoiceSubTotal)
                    {
                        amountNeeded = obj.AmountPaid;
                        balance = amountNeeded;
                        isPaymentCompleted = true;
                    }
                    else
                    {
                        amountNeeded = obj.AmountPaid;
                        balance = amountNeeded;
                    }
                    feesPayments.AmountPaid += amountNeeded;
                    feesPayments.Balance -= balance;
                    feesPayments.IsPaymentCompleted = isPaymentCompleted;
                    feesPayments.LastUpdated = DateTime.Now;
                    await _context.SaveChangesAsync();
                }


                //approve the payment
                payment.IsApproved = true;
                payment.LastUpdated = DateTime.Now;
                payment.ApprovedBy = obj.FinanceUserId;
                await _context.SaveChangesAsync();
                //return new GenericResponseModel { StatusCode = 200, StatusMessage = "Payment approved successfully" };

                //get the Payment Created
                var getPayment = from cr in _context.SchoolTemporaryPayments
                                     where cr.Id == payment.Id
                                     select new
                                     {
                                         cr.AmountPaid,
                                         cr.CampusId,
                                         cr.ClassId,
                                         cr.Classes.ClassName,
                                         cr.DateCreated,
                                         cr.Id,
                                         cr.BankName,
                                         cr.LastUpdated,
                                         cr.SchoolCampuses.CampusName,
                                         cr.SchoolId,
                                         cr.SchoolInformation.SchoolName,
                                         cr.SessionId,
                                         cr.Sessions.SessionName,
                                         cr.ReferenceCode,
                                         cr.TermId,
                                         cr.Terms.TermName,
                                         cr.DepositorsAccountName,
                                         cr.InvoiceCode,
                                         cr.IsApproved,
                                         cr.IsVerified,
                                         cr.ParentId,
                                         cr.PaymentDate,
                                         cr.PaymentMethodId,
                                         cr.PaymentMethods.MethodName,
                                         cr.StudentId,
                                         Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                     };
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Payment Created and Approved Successfully", Data = getPayment.FirstOrDefault() };
                
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

        public async Task<GenericResponseModel> getAllParentPaymentAsync(Guid parentId)
        {
            try
            {
                var checkParent = new CheckerValidation(_context).checkParentById(parentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Parent Doesn't Exist" };
                }
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.ParentId == parentId
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.BankName,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.ReferenceCode,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.DepositorsAccountName,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.ParentId,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.PaymentMethods.MethodName,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getAllParentPaymentByInvoiceCodeAsync(Guid parentId, string invoiceCode)
        {
            try
            {
                var checkParent = new CheckerValidation(_context).checkParentById(parentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Parent Doesn't Exist" };
                }
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.ParentId == parentId && cr.InvoiceCode == invoiceCode
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.BankName,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.ReferenceCode,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.DepositorsAccountName,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.ParentId,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.PaymentMethods.MethodName,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getAllSchoolPaymentAsync(long schoolId)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.SchoolId == schoolId
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.BankName,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.ReferenceCode,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.DepositorsAccountName,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.ParentId,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.PaymentMethods.MethodName,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getAllSchoolPaymentByInvoiceCodeAsync(long schoolId, string invoiceCode)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.SchoolId == schoolId && cr.InvoiceCode == invoiceCode
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.BankName,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.ReferenceCode,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.DepositorsAccountName,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.ParentId,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.PaymentMethods.MethodName,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                             };

                if (result.Count() > 0)
                {
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


        public async Task<GenericResponseModel> getAllParentSummaryPaymentAsync(Guid parentId, bool isPaymentCompleted)
        {
            try
            {
                var checkParent = new CheckerValidation(_context).checkParentById(parentId);
                if (checkParent == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Parent Doesn't Exist" };
                }
                var result = from cr in _context.SchoolFeesPayments
                             where cr.ParentId == parentId && cr.IsPaymentCompleted == isPaymentCompleted
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.InvoiceCode,
                                 cr.ParentId,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                 cr.IsPaymentCompleted,
                                 cr.InvoiceTotal,
                                 cr.Balance
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getAllSchoolSummaryPaymentAsync(long schoolId, bool isPaymentCompleted)
        {
            try
            {
                var checkSchool = new CheckerValidation(_context).checkSchoolById(schoolId);
                if (checkSchool == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "School Doesn't Exist" };
                }
                var result = from cr in _context.SchoolFeesPayments
                             where cr.SchoolId == schoolId && cr.IsPaymentCompleted == isPaymentCompleted
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.InvoiceCode,
                                 cr.ParentId,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                                 cr.IsPaymentCompleted,
                                 cr.InvoiceTotal,
                                 cr.Balance
                             };

                if (result.Count() > 0)
                {
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

        public async Task<GenericResponseModel> getPaymentByIdAsync(long paymentId)
        {
            try
            {
                var checkPayment = new CheckerValidation(_context).checkPaymentById(paymentId);
                if (checkPayment == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Payment Doesn't Exist" };
                }
                var result = from cr in _context.SchoolTemporaryPayments
                             where cr.Id == paymentId
                             orderby cr.Id descending
                             select new
                             {
                                 cr.AmountPaid,
                                 cr.CampusId,
                                 cr.ClassId,
                                 cr.Classes.ClassName,
                                 cr.DateCreated,
                                 cr.Id,
                                 cr.BankName,
                                 cr.LastUpdated,
                                 cr.SchoolCampuses.CampusName,
                                 cr.SchoolId,
                                 cr.SchoolInformation.SchoolName,
                                 cr.SessionId,
                                 cr.Sessions.SessionName,
                                 cr.ReferenceCode,
                                 cr.TermId,
                                 cr.Terms.TermName,
                                 cr.DepositorsAccountName,
                                 cr.InvoiceCode,
                                 cr.IsApproved,
                                 cr.IsVerified,
                                 cr.ParentId,
                                 cr.PaymentDate,
                                 cr.PaymentMethodId,
                                 cr.PaymentMethods.MethodName,
                                 cr.StudentId,
                                 Fullname = cr.Students.FirstName + " " + cr.Students.LastName,
                             };

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
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

        public async Task<GenericResponseModel> verifyPaymentAsync(long paymentId, Guid financeUserId)
        {
            try
            {
                var checkPayment = new CheckerValidation(_context).checkPaymentById(paymentId);
                if (checkPayment == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Payment Doesn't Exist" };
                }
                SchoolUsers schoolUsers = _context.SchoolUsers.Where(x => x.Id == financeUserId).FirstOrDefault();
                if (schoolUsers == null)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "User Doesn't Exist" };
                }
                SchoolTemporaryPayments payments = _context.SchoolTemporaryPayments.Where(x => x.Id == paymentId && x.SchoolId == schoolUsers.SchoolId).FirstOrDefault();
                if (payments == null)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Record Doesn't Exist for your school" };
                }
                if (payments.IsVerified == true)
                {
                    return new GenericResponseModel { StatusCode = 305, StatusMessage = "Payment has already been verified" };
                }
                //verify the payment
                payments.IsVerified = true;
                payments.LastUpdated = DateTime.Now;
                payments.VerifiedBy = financeUserId;
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Payment verified successfully" };
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

        public async Task<GenericResponseModel> approvePaymentAsync(long paymentId, Guid financeUserId)
        {
            try
            {
                var checkPayment = new CheckerValidation(_context).checkPaymentById(paymentId);
                if (checkPayment == false)
                {
                    return new GenericResponseModel { StatusCode = 301, StatusMessage = "Payment Doesn't Exist" };
                }
                SchoolUsers schoolUsers = _context.SchoolUsers.Where(x => x.Id == financeUserId).FirstOrDefault();
                if (schoolUsers == null)
                {
                    return new GenericResponseModel { StatusCode = 302, StatusMessage = "User Doesn't Exist" };
                }
                //check if payment exist for the school
                SchoolTemporaryPayments payments = _context.SchoolTemporaryPayments.Where(x => x.Id == paymentId && x.SchoolId == schoolUsers.SchoolId).FirstOrDefault();
                if (payments == null)
                {
                    return new GenericResponseModel { StatusCode = 303, StatusMessage = "Record Doesn't Exist for your school" };
                }
                //check if payment has been approved already
                if (payments.IsApproved == true)
                {
                    return new GenericResponseModel { StatusCode = 305, StatusMessage = "Payment has already been approved" };
                }
                //check if invoice exist
                InvoiceTotal invoiceTotal = _context.InvoiceTotal.Where(x => x.InvoiceCode.ToLower().Trim() == payments.InvoiceCode.ToLower().Trim()).FirstOrDefault();
                if(invoiceTotal == null)
                {
                    return new GenericResponseModel { StatusCode = 306, StatusMessage = "Invoice doesn't exist" };
                }
                //check for payment total record
                SchoolFeesPayments feesPayments = _context.SchoolFeesPayments.Where(x => x.InvoiceCode.ToLower().Trim() == payments.InvoiceCode.ToLower().Trim()).FirstOrDefault();
                if(feesPayments == null)
                {
                    long balance = invoiceTotal.InvoiceSubTotal - payments.AmountPaid;
                    long amountPaidRecord;
                    bool isPaymentCompleted = false;
                    if (payments.AmountPaid > invoiceTotal.InvoiceSubTotal)
                    {
                        amountPaidRecord = payments.AmountPaid + balance;
                        isPaymentCompleted = true;
                    }
                    else if(payments.AmountPaid == invoiceTotal.InvoiceSubTotal)
                    {
                        amountPaidRecord = payments.AmountPaid;
                        isPaymentCompleted = true;
                    }
                    else
                    {
                        amountPaidRecord = payments.AmountPaid;
                    }
                    //create new total record
                    SchoolFeesPayments schoolFeesPayments = new SchoolFeesPayments
                    {
                        AmountPaid = amountPaidRecord,
                        Balance = balance,
                        CampusId = payments.CampusId,
                        ClassGradeId = payments.ClassGradeId,
                        ClassId = payments.ClassId,
                        DateCreated = DateTime.Now,
                        InvoiceCode = payments.InvoiceCode,
                        InvoiceTotal = invoiceTotal.InvoiceSubTotal,
                        IsPaymentCompleted = isPaymentCompleted,
                        LastUpdated = DateTime.Now,
                        ParentId = payments.ParentId,
                        SchoolId = payments.SchoolId,
                        SessionId = invoiceTotal.SessionId,
                        StudentId = payments.StudentId,
                        TermId = invoiceTotal.TermId
                    };
                    await _context.SchoolFeesPayments.AddAsync(schoolFeesPayments);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    bool isPaymentCompleted = false;
                    long amountNeeded = 0;
                    long balance = 0;
                    long expectedAmount = feesPayments.AmountPaid + payments.AmountPaid;
                    if(expectedAmount > invoiceTotal.InvoiceSubTotal)
                    {
                        amountNeeded = feesPayments.InvoiceTotal - feesPayments.AmountPaid;
                        balance = payments.AmountPaid;
                        isPaymentCompleted = true;
                    }else if (expectedAmount == invoiceTotal.InvoiceSubTotal)
                    {
                        amountNeeded = payments.AmountPaid;
                        balance = amountNeeded;
                        isPaymentCompleted = true;
                    }
                    else
                    {
                        amountNeeded = payments.AmountPaid;
                        balance = amountNeeded;
                    }
                    feesPayments.AmountPaid += amountNeeded;
                    feesPayments.Balance -= balance;
                    feesPayments.IsPaymentCompleted = isPaymentCompleted;
                    feesPayments.LastUpdated = DateTime.Now;
                    await _context.SaveChangesAsync();
                }

                
                //approve the payment
                payments.IsApproved = true;
                payments.LastUpdated = DateTime.Now;
                payments.ApprovedBy = financeUserId;
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Payment approved successfully" };
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

        public async Task<GenericResponseModel> deletePaymentAsync(long paymentId)
        {
            try
            {
                var obj = _context.SchoolTemporaryPayments.Where(x => x.Id == paymentId).FirstOrDefault();
                if (obj != null)
                {
                    if (obj.IsApproved == false && obj.IsVerified == false)
                    {
                        _context.SchoolTemporaryPayments.Remove(obj);
                        await _context.SaveChangesAsync();

                        return new GenericResponseModel { StatusCode = 200, StatusMessage = " Record Deleted Successfully!" };
                    }
                    else
                    {
                        return new GenericResponseModel { StatusCode = 201, StatusMessage = "Record can't be deleted because it has been approved or verified!" };
                    }
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "No Template With the Specified ID!" };
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
    }
}
