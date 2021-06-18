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
    public class CourseTypeController : ControllerBase
    {
        private readonly ICourseTypeRepo _courseTypeRepo;

        public CourseTypeController(ICourseTypeRepo courseTypeRepo)
        {
            _courseTypeRepo = courseTypeRepo;
        }

        [HttpGet("allCourseType")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseType()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTypeRepo.getAllCourseTypeAsync();

            return Ok(result);
        }

        [HttpGet("courseTypeById")]
        [AllowAnonymous]
        public async Task<IActionResult> courseTypeById(long courseTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTypeRepo.getCourseTypeByIdAsync(courseTypeId);

            return Ok(result);
        }
    }
}
