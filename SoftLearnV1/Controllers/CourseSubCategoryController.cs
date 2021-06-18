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
    public class CourseSubCategoryController : ControllerBase
    {
        private readonly ICourseSubCategoryRepo _courseSubCategoryRepo;

        public CourseSubCategoryController(ICourseSubCategoryRepo courseSubCategoryRepo)
        {
            _courseSubCategoryRepo = courseSubCategoryRepo;
        }


        //--------------------------------COURSE SUBCATEGORY-----------------------------------------------------------

        [HttpPost("createCourseSubCategory")]
        [Authorize]
        public async Task<IActionResult> createCourseSubCategoryAsync(CourseSubCategoryRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.createCourseSubCategoryAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseSubCategoryById")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseSubCategoryByIdAsync(long courseSubCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.getCourseSubCategoryByIdAsync(courseSubCategoryId);

            return Ok(result);
        }

        [HttpGet("allCourseSubCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseSubCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.getAllCourseSubCategoryAsync();

            return Ok(result);
        }

        [HttpPut("updateCourseSubCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> updateCourseSubCategoryByIdAsync(long courseSubCategoryId, CourseSubCategoryRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.updateCourseSubCategoryAsync(courseSubCategoryId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteCourseSubCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseSubCategoryAsync(long courseSubCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.deleteCourseSubCategoryAsync(courseSubCategoryId);

            return Ok(result);
        }

        [HttpGet("allCourseSubCategoryByCourseCategoryId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseSubCategoryByCourseCategoryIdAsync(long courseCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.getAllCourseSubCategoryByCourseCategoryIdAsync(courseCategoryId);

            return Ok(result);
        }

        [HttpGet("allCourseSubCategoryByCourseCategoryIdPagination")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseSubCategoryByCourseCategoryIdAsync(int pageNumber, int pageSize, long courseCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseSubCategoryRepo.getAllCourseSubCategoryByCourseCategoryIdAsync(pageNumber, pageSize, courseCategoryId);

            return Ok(result);
        }

    }
}
