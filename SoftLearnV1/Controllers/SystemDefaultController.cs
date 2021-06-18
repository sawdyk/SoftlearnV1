using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
    public class SystemDefaultController : ControllerBase
    {
        private readonly ISystemDefaultRepo _systemDefaultRepo;

        public SystemDefaultController(ISystemDefaultRepo systemDefaultRepo)
        {
            _systemDefaultRepo = systemDefaultRepo;
        }

        [HttpGet("gender")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllGenderAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllGenderAsync();

            return Ok(result);
        }

        [HttpGet("genderById")]
        [AllowAnonymous]
        public async Task<IActionResult> getGenderByIdAsync(long genderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getGenderByIdAsync(genderId);

            return Ok(result);
        }

        [HttpGet("classOrAlumni")]
        [AllowAnonymous]
        public async Task<IActionResult> getClassOrAlumniAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getClassOrAlumniAsync();

            return Ok(result);
        }

        [HttpGet("classOrAlumniById")]
        [AllowAnonymous]
        public async Task<IActionResult> getClassOrAlumniByIdAsync(long classOrAlumniId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getClassOrAlumniByIdAsync(classOrAlumniId);

            return Ok(result);
        }

        [HttpGet("schoolSubTypesBySchoolTypeId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllSchoolSubTypesBySchoolTypeIdAsync(long schoolTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllSchoolSubTypesBySchoolTypeIdAsync(schoolTypeId);

            return Ok(result);
        }

        [HttpGet("attendancePeriod")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllAttendancePeriodAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllAttendancePeriodAsync();

            return Ok(result);
        }

        [HttpGet("attendancePeriodById")]
        [AllowAnonymous]
        public async Task<IActionResult> getAttendancePeriodByIdAsync(long periodId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAttendancePeriodByIdAsync(periodId);

            return Ok(result);
        }

        [HttpGet("status")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllStatusAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllStatusAsync();

            return Ok(result);
        }

        [HttpGet("statusById")]
        [AllowAnonymous]
        public async Task<IActionResult> getStatusByIdAsync(long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getStatusByIdAsync(statusId);

            return Ok(result);
        }

        [HttpGet("scoreStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllScoreStatusAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getAllScoreStatusAsync();

            return Ok(result);
        }

        [HttpGet("scoreStatusById")]
        [AllowAnonymous]
        public async Task<IActionResult> getScoreStatusByIdAsync(long scoreStatusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getScoreStatusByIdAsync(scoreStatusId);

            return Ok(result);
        }

        [HttpGet("activeInActiveStatus")]
        [AllowAnonymous]
        public async Task<IActionResult> getActiveInActiveStatusAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getActiveInActiveStatusAsync();

            return Ok(result);
        }

        [HttpGet("activeInActiveStatusById")]
        [AllowAnonymous]
        public async Task<IActionResult> getActiveInActiveStatusByIdAsync(long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _systemDefaultRepo.getActiveInActiveStatusByIdAsync(statusId);

            return Ok(result);
        }
    }
}
