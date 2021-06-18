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
    public class CourseTopicQuizController : Controller
    {
        private readonly ICourseTopicQuizRepo _quizRepo;

        public CourseTopicQuizController(ICourseTopicQuizRepo quizRepo)
        {
            _quizRepo = quizRepo;
        }
        //----------------------------CourseTopicQuiz---------------------------------------------------------------
        [HttpPost("createCourseTopicQuiz")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicQuiz([FromBody]CourseTopicQuizRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createCourseTopicQuizAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateCourseTopicQuiz")]
        [Authorize]
        public async Task<IActionResult> updateCourseTopicQuizAsync(long quizId, [FromBody]CourseTopicQuizRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.updateCourseTopicQuizAsync(quizId,obj);

            return Ok(result);
        }

        [HttpDelete("deleteCourseTopicQuiz")]
        [Authorize]
        public async Task<IActionResult> deleteCourseTopicQuizAsync(long quizId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.deleteCourseTopicQuizAsync(quizId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizByTopicId")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicQuizByTopicIdAsync(long topicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseTopicQuizByTopicIdAsync(topicId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizByCourseId")]
        [Authorize]
        public async Task<IActionResult> getAllCourseTopicQuizByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseTopicQuizByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizByFacilitatorId")]
        [Authorize]
        public async Task<IActionResult> getAllCourseTopicQuizByFacilitatorIdAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseTopicQuizByFacilitatorIdAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizById")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicQuizByIdAsync(long quizId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseTopicQuizByIdAsync(quizId);

            return Ok(result);
        }

        //----------------------------CourseTopicQuizQuestion-------------------------------------------------------
        [HttpPost("createCourseTopicQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicQuizQuestionAsync([FromBody]CourseTopicQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createCourseTopicQuizQuestionAsync(obj);

            return Ok(result);
        }

        [HttpPost("createBulkCourseTopicQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> createBulkCourseTopicQuizQuestionAsync([FromBody]BulkCourseTopicQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createBulkCourseTopicQuizQuestionAsync(obj);

            return Ok(result);
        }

        [HttpPost("createBulkCourseTopicQuizQuestionFromExcel")]
        [Authorize]
        public async Task<IActionResult> createBulkCourseTopicQuizQuestionFromExcelAsync([FromForm]BulkQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createBulkCourseTopicQuizQuestionFromExcelAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateCourseTopicQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> updateCourseTopicQuizQuestionAsync(long id,[FromBody]CourseTopicQuizQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.updateCourseTopicQuizQuestionAsync(id,obj);

            return Ok(result);
        }

        [HttpDelete("deleteCourseTopicQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> deleteCourseTopicQuizQuestionAsync(long questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.deleteCourseTopicQuizQuestionAsync(questionId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizQuestion")]
        [Authorize]
        public async Task<IActionResult> getAllCourseTopicQuizQuestionAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseTopicQuizQuestionAsync();

            return Ok(result);
        }

        [HttpGet("courseTopicQuizQuestionByQuizId")]
        [Authorize]
        public async Task<IActionResult> getAllCourseTopicQuizQuestionByQuizIdAsync(long quizId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseTopicQuizQuestionByQuizIdAsync(quizId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizQuestionById")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicQuizQuestionByIdAsync(long questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseTopicQuizQuestionByIdAsync(questionId);

            return Ok(result);
        }

        //----------------------------CourseTopicQuizResult-------------------------------------------------------
        [HttpPost("createCourseTopicQuizResult")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicQuizResultAsync([FromBody]CourseTopicQuizResultRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.createCourseTopicQuizResultAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizResultByLearnerId")]
        [Authorize]
        public async Task<IActionResult> getAllCourseTopicQuizResultByLearnerIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getAllCourseTopicQuizResultByLearnerIdAsync(learnerId);

            return Ok(result);
        }

        [HttpGet("courseTopicQuizResultById")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicQuizResultByIdAsync(long resultId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _quizRepo.getCourseTopicQuizResultByIdAsync(resultId);

            return Ok(result);
        }
    }
}
