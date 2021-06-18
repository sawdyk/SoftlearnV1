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
    public class LearnerController : ControllerBase
    {
        private readonly ILearnerRepo _learnerRepo;

        public LearnerController(ILearnerRepo learnerRepo)
        {
            _learnerRepo = learnerRepo;
        }

        //Creates a new Learner Account
        [HttpPost("signUp")]
        [AllowAnonymous]
        public async Task<IActionResult> learnerSignUpAsync([FromBody] LearnerRegRequestModel signUpObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var signUpUser = await _learnerRepo.learnerSignUpAsync(signUpObj);

            return Ok(signUpUser);
        }

        //Learners Login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> learnerLoginAsync([FromBody] LoginRequestModel loginObj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var loginUser = await _learnerRepo.learnerLoginAsync(loginObj);

            return Ok(loginUser);
        }

        //All Learners
        [HttpGet("allLearners")]
        [Authorize]
        public async Task<IActionResult> getAllLearnersAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.getAllLearnersAsync();

            return Ok(result);
        }

        [HttpPut("activateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> activateLearnerAccount([FromBody] AccountActivationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.activateAccountAsync(obj);

            return Ok(result);
        }

        //Learners By ID
        [HttpGet("learnerById")]
        [Authorize]
        public async Task<IActionResult> getLearnerByIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.getLearnerByIdAsync(learnerId);

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

            var result = await _learnerRepo.resendActivationCodeAsync(email);

            return Ok(result);
        }

        [HttpPut("updateProfileDetails")]
        [Authorize]
        public async Task<IActionResult> updateProfileDetailsAsync([FromBody] UpdateLearnerProfileRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.updateProfileDetailsAsync(obj);

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

            var result = await _learnerRepo.forgotPasswordAsync(email);

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

            var result = await _learnerRepo.changePasswordAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateProfilePicture")]
        [Authorize]
        public async Task<IActionResult> updateProfilePictureAsync(Guid learnerId, string profilePictureUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.updateProfilePictureAsync(learnerId, profilePictureUrl);

            return Ok(result);
        }

        //-------------------------Bank Account Details----------------------------------------

        [HttpPost("createAccountDetails")]
        [Authorize]
        public async Task<IActionResult> createAccountDetailsAsync([FromBody] LearnerAccountDetailsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.createAccountDetailsAsync(obj);

            return Ok(result);
        }

        [HttpGet("accountDetailsById")]
        [Authorize]
        public async Task<IActionResult> getAccountDetailsByIdAsync(Guid learnerId, long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.getAccountDetailsByIdAsync(learnerId, accountId);

            return Ok(result);
        }

        [HttpGet("accountDetailsByLearnerId")]
        [Authorize]
        public async Task<IActionResult> getAllAccountDetailsByLearnerIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.getAllAccountDetailsByLearnerIdAsync(learnerId);

            return Ok(result);
        }


        [HttpPut("updateAccountDetails")]
        [Authorize]
        public async Task<IActionResult> updateAccountDetailsAsync(LearnerAccountDetailsRequestModel obj, long accountId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _learnerRepo.updateAccountDetailsAsync(obj, accountId);

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

            var result = await _learnerRepo.deleteAccountDetailsAsync(accountId);

            return Ok(result);
        }
    }
}
