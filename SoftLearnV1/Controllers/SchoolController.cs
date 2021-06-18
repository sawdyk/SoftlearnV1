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
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolRepo _schoolRepo;

        public SchoolController(ISchoolRepo schoolRepo)
        {
            _schoolRepo = schoolRepo;
        }

        [HttpPost("schoolSignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> schoolSignUp(SchoolSignUpRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.schoolSignUpAsync(obj);

            return Ok(result);
        }

        [HttpPut("activateAccount")]
        [AllowAnonymous]
        public async Task<IActionResult> activateAccountAsync([FromBody] AccountActivationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.activateAccountAsync(obj);

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

            var result = await _schoolRepo.resendActivationCodeAsync(email);

            return Ok(result);
        }

        //------------------------ SCHOOL CAMPUS -----------------------------------------------------------

        [HttpPost("createSchoolCampus")]
        [Authorize]
        public async Task<IActionResult> createSchoolCampusAsync(SchoolCampusCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.createSchoolCampusAsync(obj);

            return Ok(result);
        }

        [HttpGet("schoolCampus")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolCampusAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.getAllSchoolCampusAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("schoolCampusById")]
        [Authorize]
        public async Task<IActionResult> getSchoolCampusByIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.getSchoolCampusByIdAsync(campusId);

            return Ok(result);
        }

        //-------------------- SCHOOL USERS -----------------------------------------------

        [HttpPost("createSchoolUsers")]
        [AllowAnonymous]
        public async Task<IActionResult> createSchoolUsersAsync(SchoolUsersCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.createSchoolUsersAsync(obj);

            return Ok(result);
        }

        [HttpPut("resetSchoolUserPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> resetSchoolUserPasswordAsync(Guid userId, long sessionId, string newPassword, string userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.resetSchoolUserPasswordAsync(userId, sessionId, newPassword, userType);

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

            var result = await _schoolRepo.resendPasswordResetLinkAsync(email);

            return Ok(result);
        }

        [HttpPost("schoolUserLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> schoolUserLoginAsync(LoginRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.schoolUserLoginAsync(obj);

            return Ok(result);
        }


        [HttpGet("schoolUsersByRoleId")]
        [Authorize]
        public async Task<IActionResult> getSchoolUsersByRoleIdAsync(long schoolId, long campusId, long roleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.getSchoolUsersByRoleIdAsync(schoolId, campusId, roleId);

            return Ok(result);
        }

        [HttpGet("schoolRoles")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolRolesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.getAllSchoolRolesAsync();

            return Ok(result);
        }

        [HttpGet("schoolRolesByRoleId")]
        [Authorize]
        public async Task<IActionResult> getSchoolRolesByRoleIdAsync(long schoolRoleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.getSchoolRolesByRoleIdAsync(schoolRoleId);

            return Ok(result);
        }

        [HttpGet("schoolRolesForSchoolUserCreation")]
        [Authorize]
        public async Task<IActionResult> getSchoolRolesForSchoolUserCreationAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.getSchoolRolesForSchoolUserCreationAsync();

            return Ok(result);
        }

        [HttpPost("assignRolesToSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> assignRolesToSchoolUsersAsync(AssignRolesToSchoolUsersRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.assignRolesToSchoolUsersAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateSchoolUserDetails")]
        [Authorize]
        public async Task<IActionResult> updateSchoolUserDetailsAsync(Guid schoolUserId, UpdateSchoolUsersDetailsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.updateSchoolUserDetailsAsync(schoolUserId, obj);

            return Ok(result);
        }

        [HttpPut("updateSchoolDetails")]
        [Authorize]
        public async Task<IActionResult> updateSchoolDetailsAsync(long schoolId, UpdateSchoolDetailsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.updateSchoolDetailsAsync(schoolId, obj);

            return Ok(result);
        }

        [HttpPut("updateCampusDetails")]
        [Authorize]
        public async Task<IActionResult> updateCampusDetailsAsync(long campusId, SchoolCampusCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.updateCampusDetailsAsync(campusId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> deleteSchoolUsersAsync(Guid schoolUserId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.deleteSchoolUsersAsync(schoolUserId, schoolId, campusId);

            return Ok(result);
        }

        [HttpDelete("deleteRolesAssignedToSchoolUsers")]
        [Authorize]
        public async Task<IActionResult> deleteRolesAssignedToSchoolUsersAsync(DeleteRolesAssignedToSchoolUsersRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolRepo.deleteRolesAssignedToSchoolUsersAsync(obj);

            return Ok(result);
        }

    }
}
