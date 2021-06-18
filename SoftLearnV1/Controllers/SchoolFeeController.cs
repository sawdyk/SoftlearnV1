using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchoolFeeController : ControllerBase
    {
        private readonly ISchoolFeeRepo _feeRepo;

        public SchoolFeeController(ISchoolFeeRepo feeRepo)
        {
            this._feeRepo = feeRepo;
        }
        //----------------------------SchoolFee---------------------------------------------------------------
        [HttpPost("createSchoolFee")]
        [Authorize]
        public async Task<IActionResult> createSchoolFeeAsync([FromBody]SchoolFeeRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.createSchoolFeeAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateSchoolFee")]
        [Authorize]
        public async Task<IActionResult> updateSchoolFeeAsync(long schoolFeeId, [FromBody]SchoolFeeRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.updateSchoolFeeAsync(schoolFeeId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSchoolFee")]
        [Authorize]
        public async Task<IActionResult> deleteSchoolFeeAsync(long schoolFeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deleteSchoolFeeAsync(schoolFeeId);

            return Ok(result);
        }

        [HttpDelete("deleteSchoolFeesByTemplateId")]
        [Authorize]
        public async Task<IActionResult> deleteSchoolFeesByTemplateIdAsync(long templateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deleteSchoolFeesByTemplateIdAsync(templateId);

            return Ok(result);
        }

        [HttpPost("schoolFees")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolFeesAsync([FromBody]GetSchoolFeesRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllSchoolFeesAsync(obj);

            return Ok(result);
        }

        [HttpGet("schoolFeesById")]
        [Authorize]
        public async Task<IActionResult> getSchoolFeesByIdAsync(long schoolFeeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getSchoolFeesByIdAsync(schoolFeeId);

            return Ok(result);
        }

        [HttpGet("allSchoolFeesBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolFeesBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllSchoolFeesBySchoolIdAsync(schoolId);

            return Ok(result);
        }
        //----------------------------Invoice---------------------------------------------------------------
        [HttpPost("generateInvoice")]
        [Authorize]
        public async Task<IActionResult> generateInvoiceAsync([FromBody]InvoiceGenerationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.generateInvoiceAsync(obj);

            return Ok(result);
        }
        [HttpDelete("deleteInvoice")]
        [Authorize]
        public async Task<IActionResult> deleteInvoiceAsync(string invoiceCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deleteInvoiceAsync(invoiceCode);

            return Ok(result);
        }
        [HttpGet("allParentInvoice")]
        [Authorize]
        public async Task<IActionResult> getAllParentInvoiceAsync(Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllParentInvoiceAsync(parentId);

            return Ok(result);
        }
        [HttpGet("allSchoolInvoice")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolInvoiceAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllSchoolInvoiceAsync(schoolId);

            return Ok(result);
        }
        [HttpGet("invoiceById")]
        [Authorize]
        public async Task<IActionResult> getInvoiceByIdAsync(long invoiceId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getInvoiceByIdAsync(invoiceId);

            return Ok(result);
        }
        //----------------------------Payment---------------------------------------------------------------
        [HttpGet("allPaymentMethod")]
        [Authorize]
        public async Task<IActionResult> getAllPaymentMethodAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllPaymentMethodAsync();

            return Ok(result);
        }
        [HttpPost("savePayment")]
        [Authorize]
        public async Task<IActionResult> savePaymentAsync([FromBody]PaymentRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.savePaymentAsync(obj);

            return Ok(result);
        }

        [HttpPost("saveCashPayment")]
        [Authorize]
        public async Task<IActionResult> saveCashPaymentAsync([FromBody]CashPaymentRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.saveCashPaymentAsync(obj);

            return Ok(result);
        }

        [HttpGet("allParentPayment")]
        [Authorize]
        public async Task<IActionResult> getAllParentPaymentAsync(Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllParentPaymentAsync(parentId);

            return Ok(result);
        }
        [HttpGet("allParentPaymentByInvoiceCode")]
        [Authorize]
        public async Task<IActionResult> getAllParentPaymentByInvoiceCodeAsync(Guid parentId, string invoiceCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllParentPaymentByInvoiceCodeAsync(parentId, invoiceCode);

            return Ok(result);
        }
        [HttpGet("allSchoolPayment")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolPaymentAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllSchoolPaymentAsync(schoolId);

            return Ok(result);
        }
        [HttpGet("allSchoolPaymentByInvoiceCode")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolPaymentByInvoiceCodeAsync(long schoolId, string invoiceCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllSchoolPaymentByInvoiceCodeAsync(schoolId, invoiceCode);

            return Ok(result);
        }
        [HttpGet("allParentSummaryPayment")]
        [Authorize]
        public async Task<IActionResult> getAllParentSummaryPaymentAsync(Guid parentId, bool isPaymentCompleted)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllParentSummaryPaymentAsync(parentId, isPaymentCompleted);

            return Ok(result);
        }
        [HttpGet("allSchoolSummaryPayment")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolSummaryPaymentAsync(long schoolId, bool isPaymentCompleted)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllSchoolSummaryPaymentAsync(schoolId, isPaymentCompleted);

            return Ok(result);
        }
        [HttpGet("paymentById")]
        [Authorize]
        public async Task<IActionResult> getPaymentByIdAsync(long paymentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getPaymentByIdAsync(paymentId);

            return Ok(result);
        }
        [HttpPut("verifyPayment")]
        [Authorize]
        public async Task<IActionResult> verifyPaymentAsync(long paymentId, Guid financeUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.verifyPaymentAsync(paymentId, financeUserId);

            return Ok(result);
        }
        [HttpPost("approvePayment")]
        [Authorize]
        public async Task<IActionResult> approvePaymentAsync(long paymentId, Guid financeUserId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.approvePaymentAsync(paymentId, financeUserId);

            return Ok(result);
        }
        [HttpDelete("deletePayment")]
        [Authorize]
        public async Task<IActionResult> deletePaymentAsync(long paymentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deletePaymentAsync(paymentId);

            return Ok(result);
        }
    }
}
