using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISchoolFeeRepo
    {
        //----------------------------FeeTemplate---------------------------------------------------------------
        Task<GenericResponseModel> createSchoolFeeAsync(SchoolFeeRequestModel obj);
        Task<GenericResponseModel> updateSchoolFeeAsync(long schoolFeeId, SchoolFeeRequestModel obj);
        Task<GenericResponseModel> deleteSchoolFeeAsync(long schoolFeeId);
        Task<GenericResponseModel> deleteSchoolFeesByTemplateIdAsync(long templateId);
        Task<GenericResponseModel> getAllSchoolFeesAsync(GetSchoolFeesRequestModel obj);
        Task<GenericResponseModel> getSchoolFeesByIdAsync(long schoolFeeId);
        Task<GenericResponseModel> getAllSchoolFeesBySchoolIdAsync(long schoolId);
        //----------------------------Invoice---------------------------------------------------------------
        Task<InvoiceResponseModel> generateInvoiceAsync(InvoiceGenerationRequestModel obj);
        Task<GenericResponseModel> deleteInvoiceAsync(string invoiceCode);
        Task<GenericResponseModel> getAllParentInvoiceAsync(Guid parentId);
        Task<GenericResponseModel> getAllSchoolInvoiceAsync(long schoolId);
        Task<GenericResponseModel> getInvoiceByIdAsync(long invoiceId);
        //----------------------------Payment---------------------------------------------------------------
        Task<GenericResponseModel> getAllPaymentMethodAsync();
        Task<GenericResponseModel> savePaymentAsync(PaymentRequestModel obj);
        Task<GenericResponseModel> saveCashPaymentAsync(CashPaymentRequestModel obj);
        Task<GenericResponseModel> getAllParentPaymentAsync(Guid parentId);
        Task<GenericResponseModel> getAllParentPaymentByInvoiceCodeAsync(Guid parentId, string invoiceCode);
        Task<GenericResponseModel> getAllSchoolPaymentAsync(long schoolId);
        Task<GenericResponseModel> getAllSchoolPaymentByInvoiceCodeAsync(long schoolId, string invoiceCode);
        Task<GenericResponseModel> getAllParentSummaryPaymentAsync(Guid parentId, bool isPaymentCompleted);
        Task<GenericResponseModel> getAllSchoolSummaryPaymentAsync(long schoolId, bool isPaymentCompleted);
        Task<GenericResponseModel> getPaymentByIdAsync(long paymentId);
        Task<GenericResponseModel> verifyPaymentAsync(long paymentId, Guid financeUserId);
        Task<GenericResponseModel> approvePaymentAsync(long paymentId, Guid financeUserId);
        Task<GenericResponseModel> deletePaymentAsync(long paymentId);
    }
}
