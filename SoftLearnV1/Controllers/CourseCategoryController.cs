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
    public class CourseCategoryController : ControllerBase
    {
        private readonly ICourseCategoryRepo _courseCategoryRepo;

        public CourseCategoryController(ICourseCategoryRepo courseCategoryRepo)
        {
            _courseCategoryRepo = courseCategoryRepo;
        }

        [HttpPost("createCourseCategory")]
        [Authorize]
        public async Task<IActionResult> createCourseCategoryAsync(CourseCategoryCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.createCourseCategoryAsync(obj);

            return Ok(result);
        }

        [HttpGet("allCourseCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.getAllCourseCategoryAsync();

            return Ok(result);
        }

        [HttpPut("updateCourseCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> updateCourseCategoryAsync(long courseCategoryId, CourseCategoryCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.updateCourseCategoryAsync(courseCategoryId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteCourseCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseCategoryAsync(long courseCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.deleteCourseCategoryAsync(courseCategoryId);

            return Ok(result);
        }

        //With Pagination
        [HttpGet("allCourseCategoryPagination")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseCategoryAsync(int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.getAllCourseCategoryAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("courseCategoryById")]
        [AllowAnonymous]
        public async Task<IActionResult> courseCategoryByIdAsync(long courseCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.getCourseCategoryByIdAsync(courseCategoryId);

            return Ok(result);
        }

        [HttpGet("popularCourseCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> popularCourseCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.popularCourseCategoryAsync();

            return Ok(result);
        }

        [HttpGet("topCoursesInCourseCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> topCoursesInCourseCategoryAsync(long categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseCategoryRepo.topCoursesInCourseCategoryAsync(categoryId);

            return Ok(result);
        }
    }
}
