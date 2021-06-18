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
    public class CourseRequirementsController : ControllerBase
    {
        private readonly ICourseRequirementRepo _courseRequirementRepo;

        public CourseRequirementsController(ICourseRequirementRepo courseRequirementRepo)
        {
            _courseRequirementRepo = courseRequirementRepo;
        }


        [HttpPost("createCourseRequirements")]
        [AllowAnonymous]
        public async Task<IActionResult> createCourseRequirement(CourseRequirementRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRequirementRepo.createCourseRequirementAsync(obj);

            return Ok(result);
        }

        [HttpPost("createMultipleCourseRequirement")]
        [AllowAnonymous]
        public async Task<IActionResult> createMultipleCourseRequirementAsync(MultipleCourseRequirementRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRequirementRepo.createMultipleCourseRequirementAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseRequirementByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> courseRequirementByCourseId(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRequirementRepo.getCourseRequirementByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseRequirementById")]
        [AllowAnonymous]
        public async Task<IActionResult> courseRequirementById(long courseRequirementId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRequirementRepo.getCourseRequirementByIdAsync(courseRequirementId);

            return Ok(result);
        }

        [HttpDelete("deleteCourseRequirement")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseRequirementAsync(long courseRequirementId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRequirementRepo.deleteCourseRequirementAsync(courseRequirementId);

            return Ok(result);
        }
    }
}
