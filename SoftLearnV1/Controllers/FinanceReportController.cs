using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FinanceReportController : ControllerBase
    {
        private readonly IFinanceReportRepo _reportRepo;

        public FinanceReportController(IFinanceReportRepo reportRepo)
        {
            this._reportRepo = reportRepo;
        }
        //----------------------------SchoolFeePaymentStatus---------------------------------------------------------------
        [HttpGet("allFeePaymentStatus")]
        [Authorize]
        public async Task<IActionResult> getAllFeePaymentStatusAsync(int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getFeePaymentStatusAsync(sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }
        //----------------------------PaymentMethod---------------------------------------------------------------
        [HttpGet("allFeePaymentByMethod")]
        [Authorize]
        public async Task<IActionResult> getAllFeePaymentByMethodAsync(int methodId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getAllFeePaymentByMethodAsync(methodId, sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }
        [HttpGet("allFeePaymentTotalByMethod")]
        [Authorize]
        public async Task<IActionResult> getAllFeePaymentTotalByMethodAsync(int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getAllFeePaymentTotalByMethodAsync(sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }
    }
}
