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
    public class CouponCodeController : ControllerBase
    {
        private readonly ICouponCodeRepo _couponCodeRepo;

        public CouponCodeController(ICouponCodeRepo couponCodeRepo)
        {
            _couponCodeRepo = couponCodeRepo;
        }

        [HttpPost("createCouponCodes")]
        [Authorize]
        public async Task<IActionResult> createCouponCodesAsync(CouponCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _couponCodeRepo.createCouponCodesAsync(obj);

            return Ok(result);
        }

        [HttpPost("applyCouponCodes")]
        [AllowAnonymous]
        public async Task<IActionResult> applyCouponCodesAsync(ApplyCouponCodeRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _couponCodeRepo.applyCouponCodesAsync(obj);

            return Ok(result);
        }

        [HttpGet("allCouponCodes")]
        [Authorize]
        public async Task<IActionResult> getAllCouponCodesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _couponCodeRepo.getAllCouponCodesAsync();

            return Ok(result);
        }

        [HttpGet("couponCodesById")]
        [AllowAnonymous]
        public async Task<IActionResult> getCouponCodesByIdAsync(long couponCodeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _couponCodeRepo.getCouponCodesByIdAsync(couponCodeId);

            return Ok(result);
        }

        [HttpGet("couponCodesByCouponCode")]
        [AllowAnonymous]
        public async Task<IActionResult> getCouponCodesByCouponCodeAsync(string couponCode)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _couponCodeRepo.getCouponCodesByCouponCodeAsync(couponCode);

            return Ok(result);
        }
    }
}
