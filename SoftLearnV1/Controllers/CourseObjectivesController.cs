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
    public class CourseObjectivesController : ControllerBase
    {
        private readonly ICourseObjectivesRepo _courseObjectivesRepo;

        public CourseObjectivesController(ICourseObjectivesRepo courseObjectivesRepo)
        {
            _courseObjectivesRepo = courseObjectivesRepo;
        }


        [HttpPost("createCourseObjectives")]
        [AllowAnonymous]
        public async Task<IActionResult> courseObjectives(CourseObjectivesRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseObjectivesRepo.createCourseObjectivesAsync(obj);

            return Ok(result);
        }

        [HttpPost("createMultipleCourseObjectives")]
        [AllowAnonymous]
        public async Task<IActionResult> createMultipleCourseObjectivesAsync(MultipleCourseObjectivesRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseObjectivesRepo.createMultipleCourseObjectivesAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseObjectiveById")]
        [AllowAnonymous]
        public async Task<IActionResult> courseObjectiveById([FromQuery]long courseObjectiveId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseObjectivesRepo.getCourseObjectiveByIdAsync(courseObjectiveId);

            return Ok(result);
        }

        [HttpGet("courseObjectivesByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> courseObjectivesByCourseId(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseObjectivesRepo.getCourseObjectivesByCourseIdAsync(courseId);

            return Ok(result);
        }


        [HttpDelete("deleteCourseObjective")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseObjectiveAsync(long courseObjectiveId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseObjectivesRepo.deleteCourseObjectiveAsync(courseObjectiveId);

            return Ok(result);
        }
    }
}
