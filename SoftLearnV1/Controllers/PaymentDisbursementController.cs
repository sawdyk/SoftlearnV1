using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentDisbursementController : ControllerBase
    {
        private readonly IPaymentDisbursementRepo _paymentDisbursementRepo;

        public PaymentDisbursementController(IPaymentDisbursementRepo paymentDisbursementRepo)
        {
            _paymentDisbursementRepo = paymentDisbursementRepo;
        }

        [HttpPost("facilitatorsTotalEarnings")]
        [Authorize]
        public async Task<IActionResult> facilitatorsTotalEarningsAsync(IList<PaymentDisbursementRequestModel> objList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _paymentDisbursementRepo.facilitatorsTotalEarningsAsync(objList);

            return Ok(result);
        }

        [HttpPost("learnersCourseRefund")]
        [Authorize]
        public async Task<IActionResult> learnersCourseRefundAsync(IList<LearnerPaymentDisbursementRequestModel> objList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _paymentDisbursementRepo.learnersCourseRefundAsync(objList);

            return Ok(result);
        }
    }
}
