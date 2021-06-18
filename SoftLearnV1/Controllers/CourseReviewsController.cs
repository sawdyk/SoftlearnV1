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
    public class CourseReviewsController : ControllerBase
    {
        private readonly ICourseReviewsRepo _courseReviewsRepo;
        public CourseReviewsController(ICourseReviewsRepo courseReviewsRepo)
        {
            _courseReviewsRepo = courseReviewsRepo;
        }

        [HttpPost("reviewCourse")]
        [AllowAnonymous]
        public async Task<IActionResult> reviewCourseAsync(CourseReviewRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.reviewCourseAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseReviewsByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseReviewsByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.getCourseReviewsByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseReviewsByLearnerId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseReviewsByLearnerIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.getCourseReviewsByLearnerIdAsync(learnerId);

            return Ok(result);
        }

        [HttpGet("courseReviews")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseReviewsAsync(int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.getCourseReviewsAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("courseReviewsAtRandom")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseReviewsAtRandomAsync(int noOfCourseReviews)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.getCourseReviewsAtRandomAsync(noOfCourseReviews);

            return Ok(result);
        }

        [HttpDelete("deleteCourseReviews")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseReviewsAsync(long courseReviewId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.deleteCourseReviewsAsync(courseReviewId);

            return Ok(result);
        }


        [HttpGet("facilitatorCoursesReviews")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllFacilitatorCoursesReviewsAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseReviewsRepo.getAllFacilitatorCoursesReviewsAsync(facilitatorId);

            return Ok(result);
        }

    }
}
