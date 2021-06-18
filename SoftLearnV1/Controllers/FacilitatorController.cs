using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class FacilitatorController : ControllerBase
    {
        private readonly IFacilitatorRepo _facilitatorRepo;

        public FacilitatorController(IFacilitatorRepo facilitatorRepo)
        {
            _facilitatorRepo = facilitatorRepo;
        }

        //Creates a new Facilitator Account
        [HttpPost("signUp")]
        [AllowAnonymous]
        public async Task<IActionResult> facilitatorSignUp([FromBody] FacilitatorRegRequestModel signUpObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var signUpUser = await _facilitatorRepo.facilitatorSignUpAsync(signUpObj);

            return Ok(signUpUser);
        }

        //Creates a new Internal Facilitator Account
        [HttpPost("createFacilitator")]
        [Authorize]
        public async Task<IActionResult> facilitatorSignUpInternalAsync([FromBody] InternalFacilitatorRegRequestModel signUpObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var signUpUser = await _facilitatorRepo.facilitatorSignUpInternalAsync(signUpObj);

            return Ok(signUpUser);
        }

        [HttpPut("resetInternalFacilitatorPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> resetPasswordAsync(Guid userId, long sessionId, string newPassword, string userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.resetPasswordAsync(userId, sessionId, newPassword, userType);

            return Ok(result);
        }

        [HttpGet("resendPasswordResetLink")]
        [Authorize]
        public async Task<IActionResult> resendPasswordResetLinkAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.resendPasswordResetLinkAsync(email);

            return Ok(result);
        }

        //Facilitator Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> facilitatorLogin([FromBody] LoginRequestModel loginObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var loginUser = await _facilitatorRepo.facilitatorLoginAsync(loginObj);

            return Ok(loginUser);
        }

        //All facilitator
        [HttpGet("allFacilitators")]
        [Authorize]
        public async Task<IActionResult> getAllFacilitators(long? facilitatorTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllFacilitatorAsync(facilitatorTypeId);

            return Ok(result);
        }

        [HttpPut("activateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> activateFacilitatorAccount([FromBody] AccountActivationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.activateAccountAsync(obj);

            return Ok(result);
        }

        //Facilitator By ID
        [HttpGet("facilitatorById")]
        [AllowAnonymous]
        public async Task<IActionResult> getFacilitatorByIdAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getFacilitatorByIdAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("resendActivationCode")]
        [AllowAnonymous]
        public async Task<IActionResult> resendActivationCodeAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.resendActivationCodeAsync(email);

            return Ok(result);
        }

        [HttpPut("updateProfileDetails")]
        [Authorize]
        public async Task<IActionResult> updateProfileDetailsAsync([FromBody] UpdateFacilitatorProfileRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.updateProfileDetailsAsync(obj);

            return Ok(result);
        }

        [HttpGet("forgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> forgotPasswordAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.forgotPasswordAsync(email);

            return Ok(result);
        }

        [HttpPut("changePassword")]
        [AllowAnonymous]
        public async Task<IActionResult> changePasswordAsync([FromBody]ChangePasswordRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.changePasswordAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateProfilePicture")]
        [Authorize]
        public async Task<IActionResult> updateProfilePictureAsync(Guid facilitatorId, string profilePictureUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.updateProfilePictureAsync(facilitatorId, profilePictureUrl);

            return Ok(result);
        }

        //------------------------------------------ACCOUNT DETAILS-----------------------------------------------------

        [HttpPost("createAccountDetails")]
        [Authorize]
        public async Task<IActionResult> createAccountDetailsAsync(FacilitatorAccountDetailsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.createAccountDetailsAsync(obj);

            return Ok(result);
        }

        [HttpGet("accountDetailsById")]
        [Authorize]
        public async Task<IActionResult> getAccountDetailsByIdAsync(Guid facilitatorId, long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAccountDetailsByIdAsync(facilitatorId, accountId);

            return Ok(result);
        }

        [HttpGet("accountDetailsByFacilitatorId")]
        [Authorize]
        public async Task<IActionResult> getAllAccountDetailsByFacilitatorIdAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllAccountDetailsByFacilitatorIdAsync(facilitatorId);

            return Ok(result);
        }

        [HttpPut("setAccountDetailsAsDefault")]
        [Authorize]
        public async Task<IActionResult> setAccountDetailsAsDefaultAsync(Guid facilitatorId, long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.setAccountDetailsAsDefaultAsync(facilitatorId, accountId);

            return Ok(result);
        }

        [HttpPut("updateAccountDetails")]
        [Authorize]
        public async Task<IActionResult> updateAccountDetailsAsync(FacilitatorAccountDetailsRequestModel obj, long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.updateAccountDetailsAsync(obj, accountId);

            return Ok(result);
        }

        [HttpDelete("deleteAccountDetails")]
        [Authorize]
        public async Task<IActionResult> deleteAccountDetailsAsync(long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.deleteAccountDetailsAsync(accountId);

            return Ok(result);
        }


        //-------------------------Percentage Earned on Courses----------------------------------------

        [HttpGet("percentageEarnedOnCourses")]
        [Authorize]
        public async Task<IActionResult> getAllPercentageEarnedOnCoursesAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllPercentageEarnedOnCoursesAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("percentageEarnedOnCoursesByCourseId")]
        [Authorize]
        public async Task<IActionResult> getPercentageEarnedOnCoursesByCourseIdAsync(Guid facilitatorId, long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getPercentageEarnedOnCoursesByCourseIdAsync(facilitatorId, courseId);

            return Ok(result);
        }

        [HttpGet("earningsPerCourse")]
        [Authorize]
        public async Task<IActionResult> getAllEarningsPerCourseAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllEarningsPerCourseAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("earningsPerCourseByCourseId")]
        [Authorize]
        public async Task<IActionResult> getAllEarningsPerCourseByCourseIdAsync(Guid facilitatorId, long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllEarningsPerCourseByCourseIdAsync(facilitatorId, courseId);

            return Ok(result);
        }

        [HttpGet("earningsPerCourseByDateEarned")]
        [Authorize]
        public async Task<IActionResult> getAllEarningsPerCourseByDateEarnedAsync(Guid facilitatorId, DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllEarningsPerCourseByDateEarnedAsync(facilitatorId, date);

            return Ok(result);
        }

        [HttpGet("earningsPerCourseByDateEarnedAndCourseId")]
        [Authorize]
        public async Task<IActionResult> getAllEarningsPerCourseByDateEarnedAndCourseIdAsync(Guid facilitatorId, long courseId, DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllEarningsPerCourseByDateEarnedAndCourseIdAsync(facilitatorId, courseId, date);

            return Ok(result);
        }


        [HttpGet("earningsPerCourseByDateEarnedRange")]
        [Authorize]
        public async Task<IActionResult> getAllEarningsPerCourseByDateEarnedRangeAsync(Guid facilitatorId, DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllEarningsPerCourseByDateEarnedRangeAsync(facilitatorId, fromDate, toDate);

            return Ok(result);
        }

        [HttpGet("earningsPerCourseByDateEarnedRangeAndCourseId")]
        [Authorize]
        public async Task<IActionResult> getAllEarningsPerCourseByDateEarnedRangeAndCourseIdAsync(Guid facilitatorId, long courseId, DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getAllEarningsPerCourseByDateEarnedRangeAndCourseIdAsync(facilitatorId, courseId, fromDate, toDate);

            return Ok(result);
        }

        //------------------------------Total Earnings------------------------------------------------------------

        [HttpGet("totalEarningsByDateEarned")]
        [Authorize]
        public async Task<IActionResult> getTotalEarningsByDateEarnedAsync(Guid facilitatorId, DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getTotalEarningsByDateEarnedAsync(facilitatorId, date);

            return Ok(result);
        }

        [HttpGet("totalEarningsByDateEarnedRange")]
        [Authorize]
        public async Task<IActionResult> getTotalEarningsByDateEarnedRangeAsync(Guid facilitatorId, DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getTotalEarningsByDateEarnedRangeAsync(facilitatorId, fromDate, toDate);

            return Ok(result);
        }

        [HttpGet("totalEarningsSettled")]
        [Authorize]
        public async Task<IActionResult> getTotalEarningsSettledAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getTotalEarningsSettledAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("totalEarningsUnSettled")]
        [Authorize]
        public async Task<IActionResult> getTotalEarningsUnSettledAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getTotalEarningsUnSettledAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("facilitatorsSettledEarnings")]
        [Authorize]
        public async Task<IActionResult> getFacilitatorsSettledEarningsAsync(DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getFacilitatorsSettledEarningsAsync(fromDate, toDate);

            return Ok(result);
        }

        [HttpGet("facilitatorsUnSettledEarnings")]
        [Authorize]
        public async Task<IActionResult> getFacilitatorsUnSettledEarningsAsync(DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _facilitatorRepo.getFacilitatorsUnSettledEarningsAsync(fromDate, toDate);

            return Ok(result);
        }

    }
}
