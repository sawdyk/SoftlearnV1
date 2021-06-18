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
    public class ComputerBasedTestController : ControllerBase
    {
        private readonly IComputerBasedTestRepo _computerBasedTestRepo;

        public ComputerBasedTestController(IComputerBasedTestRepo computerBasedTestRepo)
        {
            _computerBasedTestRepo = computerBasedTestRepo;
        }

        //------------------------------------------------COMPUTER BASES TEST---------------------------------------------------------------------

        [HttpPost("createComputerBasedTest")]
        [Authorize]
        public async Task<IActionResult> createComputerBasedTestAsync(CbtRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.createComputerBasedTestAsync(obj);

            return Ok(result);
        }

        [HttpGet("computerBasedTest")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long typeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestAsync(schoolId, campusId, classId, classGradeId, subjectId, categoryId, typeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestByCategoryIdAndSubjectId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByCategoryIdAndSubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByCategoryIdAndSubjectIdAsync(schoolId, campusId, classId, classGradeId, subjectId, categoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestByCategoryId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByCategoryIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByCategoryIdAsync(schoolId, campusId, classId, classGradeId, categoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestByClassIdAndGradeId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByClassIdAndGradeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByClassIdAndGradeIdAsync(schoolId, campusId, classId, classGradeId, termId, sessionId);

            return Ok(result);
        }


        [HttpGet("computerBasedTestById")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByIdAsync(long cbtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByIdAsync(cbtId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestBySubjectIdAsync(schoolId, campusId, classId, classGradeId, subjectId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestByTypeIdAndSubjectId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByTypeIdAndSubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long typeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByTypeIdAndSubjectIdAsync(schoolId, campusId, classId, classGradeId, subjectId, typeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestByTypeId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByTypeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long typeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByTypeIdAsync(schoolId, campusId, classId, classGradeId, typeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestByStatusId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestByStatusIdAsync(long schoolId, long campusId, long classId, long classGradeId, long statusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestByStatusIdAsync(schoolId, campusId, classId, classGradeId, statusId, termId, sessionId);

            return Ok(result);
        }

        [HttpPut("setComputerBasedTestStatus")]
        [Authorize]
        public async Task<IActionResult> setComputerBasedTestStatusAsync(long cbtId, long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.setComputerBasedTestStatusAsync(cbtId, statusId);

            return Ok(result);
        }

        [HttpPut("updateComputerBasedTest")]
        [Authorize]
        public async Task<IActionResult> updateComputerBasedTestAsync(long cbtId, CbtRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.updateComputerBasedTestAsync(cbtId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteComputerBasedTest")]
        [Authorize]
        public async Task<IActionResult> deleteComputerBasedTestAsync(long cbtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.deleteComputerBasedTestAsync(cbtId);

            return Ok(result);
        }

        //---------------------------------------------------COMPUTER BASED TEST QUESTIONS-----------------------------------------------------------

        [HttpPost("createQuestions")]
        [Authorize]
        public async Task<IActionResult> createQuestionsAsync(CbtQuestionRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.createQuestionsAsync(obj);

            return Ok(result);
        }

        [HttpPost("createBulkQuestionsFromExcel")]
        [Authorize]
        public async Task<IActionResult> createBulkQuestionsFromExcelAsync([FromForm]BulkCbtQuestionsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.createBulkQuestionsFromExcelAsync(obj);

            return Ok(result);
        }


        [HttpGet("questionsById")]
        [Authorize]
        public async Task<IActionResult> getQuestionsByIdAsync(long questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getQuestionsByIdAsync(questionId);

            return Ok(result);
        }

        [HttpGet("questionsByCbtId")]
        [Authorize]
        public async Task<IActionResult> getQuestionsByCbtIdAsync(long cbtId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getQuestionsByCbtIdAsync(cbtId);

            return Ok(result);
        }

        [HttpGet("questionsByQuestionTypeId")]
        [Authorize]
        public async Task<IActionResult> getQuestionsByQuestionTypeIdAsync(long cbtId, long questionTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getQuestionsByQuestionTypeIdAsync(cbtId, questionTypeId);

            return Ok(result);
        }


        [HttpPut("updateQuestion")]
        [Authorize]
        public async Task<IActionResult> updateQuestionAsync(long questionId, CbtQuestionCreationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.updateQuestionAsync(questionId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteQuestion")]
        [Authorize]
        public async Task<IActionResult> deleteQuestionAsync(long questionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.deleteQuestionAsync(questionId);

            return Ok(result);
        }

        //---------------------------------------------------COMPUTER BASED TEST RESULTS-------------------------------------------------------------

        [HttpPut("updateComputerBasedTestResult")]
        [Authorize]
        public async Task<IActionResult> updateComputerBasedTestResultAsync(CbtResultCreationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.updateComputerBasedTestResultAsync(obj);

            return Ok(result);
        }


        [HttpGet("computerBasedTestResultById")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByIdAsync(long cbtResultId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByIdAsync(cbtResultId);

            return Ok(result);
        }


        [HttpGet("computerBasedTestResult")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long typeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultAsync(schoolId, campusId, classId, classGradeId, categoryId, typeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestResultByTypeId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByTypeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long typeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByTypeIdAsync(schoolId, campusId, classId, classGradeId, typeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestResultByCategoryId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByCategoryIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByCategoryIdAsync(schoolId, campusId, classId, classGradeId, categoryId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestResultByCbtId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByCbtIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long typeId, long cbtId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByCbtIdAsync(schoolId, campusId, classId, classGradeId, categoryId, typeId, cbtId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestResultByStudentId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByStudentIdAsync(long schoolId, long campusId, long classId, long classGradeId, long cbtId, long categoryId, long typeId, Guid studentId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByStudentIdAsync(schoolId, campusId, classId, classGradeId, cbtId, categoryId, typeId, studentId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestResultByIndividualStudentId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByIndividualStudentIdAsync(long schoolId, long campusId, long cbtId, Guid studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByIndividualStudentIdAsync(schoolId, campusId, cbtId, studentId);

            return Ok(result);
        }

        [HttpGet("computerBasedTestResultByClassIdAndGradeId")]
        [Authorize]
        public async Task<IActionResult> getComputerBasedTestResultByClassIdAndGradeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long cbtId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getComputerBasedTestResultByClassIdAndGradeIdAsync(schoolId, campusId, classId, classGradeId, cbtId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteComputerBasedTestResult")]
        [Authorize]
        public async Task<IActionResult> deleteComputerBasedTestResultAsync(long cbtResultId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.deleteComputerBasedTestResultAsync(cbtResultId);

            return Ok(result);
        }

        //--------------------------------------------------START COMPUTER BASED TEST----------------------------------------------------------------
        //call this endpoint to start CBT, by creating the result and calling the updateCbtResult to update the result on completion of the CBT

        [HttpPost("takeComputerBasedTest")]
        [Authorize]
        public async Task<IActionResult> takeComputerBasedTestAsync(CbtResultRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.takeComputerBasedTestAsync(obj);

            return Ok(result);
        }

        //-------------------------------------------------SYSTEM DEFINED/DEFAULTS------------------------------------------------------------------------

        [HttpGet("cbtTypes")]
        [AllowAnonymous]
        public async Task<IActionResult> getCbtTypesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getCbtTypesAsync();

            return Ok(result);
        }

        [HttpGet("cbtTypesById")]
        [AllowAnonymous]
        public async Task<IActionResult> getCbtTypesByIdAsync(long cbtTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getCbtTypesByIdAsync(cbtTypeId);

            return Ok(result);
        }

        [HttpGet("cbtCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> getCbtCategoryAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getCbtCategoryAsync();

            return Ok(result);
        }


        [HttpGet("cbtCategoryById")]
        [AllowAnonymous]
        public async Task<IActionResult> getCbtCategoryByIdAsync(long cbtCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _computerBasedTestRepo.getCbtCategoryByIdAsync(cbtCategoryId);

            return Ok(result);
        }
    }
}
