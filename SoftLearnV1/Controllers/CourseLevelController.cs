using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseLevelController : ControllerBase
    {
        private readonly ICourseLevelRepo _courseLevelRepo;

        public CourseLevelController(ICourseLevelRepo courseLevelRepo)
        {
            _courseLevelRepo = courseLevelRepo;
        }


        [HttpGet("allCourseLevel")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseLevel()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseLevelRepo.getAllCourseLevelAsync();

            return Ok(result);
        }

        [HttpGet("courseLevelById")]
        [AllowAnonymous]
        public async Task<IActionResult> courseLevelById(long courseLevelId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseLevelRepo.getCourseLevelByIdAsync(courseLevelId);

            return Ok(result);
        }
    }
}
