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
    public class CourseRatingController : ControllerBase
    {
        private readonly ICourseRatingRepo _courseRatingRepo;
        public CourseRatingController(ICourseRatingRepo courseRatingRepo)
        {
            _courseRatingRepo = courseRatingRepo;
        }

        [HttpPost("rateCourse")]
        [AllowAnonymous]
        public async Task<IActionResult> rateCourseAsync(CourseRatingRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRatingRepo.rateCourseAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseRatingByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseRatingByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRatingRepo.getCourseRatingByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseRatingByLearnerId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseRatingByLearnerIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRatingRepo.getCourseRatingByLearnerIdAsync(learnerId);

            return Ok(result);
        }

        [HttpGet("courseRatingByRatingValue")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseRatingByRatingValueAsync(long courseId, long ratingValue)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRatingRepo.getCourseRatingByRatingValueAsync(courseId, ratingValue);

            return Ok(result);
        }

        [HttpDelete("deleteCourseRating")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseRatingAsync(long courseRatingId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRatingRepo.deleteCourseRatingAsync(courseRatingId);

            return Ok(result);
        }

        [HttpGet("courseAverageRating")]
        [AllowAnonymous]
        public async Task<IActionResult> courseAverageRatingAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRatingRepo.courseAverageRatingAsync(courseId);

            return Ok(result);
        }

    }
}
