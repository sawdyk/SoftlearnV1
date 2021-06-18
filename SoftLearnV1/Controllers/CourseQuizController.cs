using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CourseQuizController : ControllerBase
    {
        private readonly ICourseQuizRepo _quizRepo;

        public CourseQuizController(ICourseQuizRepo quizRepo)
        {
            _quizRepo = quizRepo;
        }
        //----------------------------CourseQuiz---------------------------------------------------------------
        [HttpPost("createCourseQuiz")]
        [Authorize]
        public async Task<IActionResult> createCourseQuizAsync([FromBody]CourseQuizRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createCourseQuizAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateCourseQuiz")]
        [Authorize]
        public async Task<IActionResult> updateCourseQuizAsync(long quizId, [FromBody]CourseQuizRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.updateCourseQuizAsync(quizId,obj);

            return Ok(result);
        }

        [HttpDelete("deleteCourseQuiz")]
        [Authorize]
        public async Task<IActionResult> deleteCourseQuizAsync(long quizId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.deleteCourseQuizAsync(quizId);

            return Ok(result);
        }

        [HttpGet("courseQuizByCourseId")]
        [Authorize]
        public async Task<IActionResult> courseQuizByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseQuizByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseQuizByFacilitatorId")]
        [Authorize]
        public async Task<IActionResult> allCourseQuizByFacilitatorIdAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseQuizByFacilitatorIdAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("courseQuizById")]
        [Authorize]
        public async Task<IActionResult> getCourseQuizByIdAsync(long quizId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseQuizByIdAsync(quizId);

            return Ok(result);
        }

        //----------------------------CourseQuizQuestion-------------------------------------------------------
        [HttpPost("createCourseQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> createCourseQuizQuestionAsync([FromBody]CourseQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createCourseQuizQuestionAsync(obj);

            return Ok(result);
        }

        [HttpPost("createBulkCourseQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> createBulkCourseQuizQuestionAsync([FromBody]BulkCourseQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createBulkCourseQuizQuestionAsync(obj);

            return Ok(result);
        }

        [HttpPost("createBulkCourseQuizQuestionFromExcel")]
        [Authorize]
        public async Task<IActionResult> createBulkCourseQuizQuestionFromExcelAsync([FromForm]BulkQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createBulkCourseQuizQuestionFromExcelAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateCourseQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> updateCourseQuizQuestionAsync(long id,[FromBody]CourseQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.updateCourseQuizQuestionAsync(id,obj);

            return Ok(result);
        }

        [HttpDelete("deleteCourseQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> deleteCourseQuizQuestionAsync(long questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.deleteCourseQuizQuestionAsync(questionId);

            return Ok(result);
        }

        [HttpGet("courseQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> getAllCourseQuizQuestionAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseQuizQuestionAsync();

            return Ok(result);
        }

        [HttpGet("courseQuizQuestionByQuizId")]
        [Authorize]
        public async Task<IActionResult> getAllCourseQuizQuestionByQuizIdAsync(long quizId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseQuizQuestionByQuizIdAsync(quizId);

            return Ok(result);
        }

        [HttpGet("courseQuizQuestionById")]
        [Authorize]
        public async Task<IActionResult> getCourseQuizQuestionByIdAsync(long questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseQuizQuestionByIdAsync(questionId);

            return Ok(result);
        }

        //----------------------------CourseQuizResult-------------------------------------------------------
        [HttpPost("createCourseQuizResult")]
        [Authorize]
        public async Task<IActionResult> createCourseQuizResultAsync([FromBody]CourseQuizResultRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createCourseQuizResultAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseQuizResultByLearnerId")]
        [Authorize]
        public async Task<IActionResult> getAllCourseQuizResultByLearnerIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseQuizResultByLearnerIdAsync(learnerId);

            return Ok(result);
        }

        [HttpGet("courseQuizResultById")]
        [Authorize]
        public async Task<IActionResult> getCourseQuizResultByIdAsync(long resultId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseQuizResultByIdAsync(resultId);

            return Ok(result);
        }
    }
}
