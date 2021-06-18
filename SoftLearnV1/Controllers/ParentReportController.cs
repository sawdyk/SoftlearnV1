using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ParentReportController : ControllerBase
    {
        private readonly IParentReportRepo _reportRepo;

        public ParentReportController(IParentReportRepo reportRepo)
        {
            this._reportRepo = reportRepo;
        }
        //----------------------------TestPerformance---------------------------------------------------------------
        [HttpGet("testPerformanceByTerm")]
        [Authorize]
        public async Task<IActionResult> getTestPerformanceByTermAsync(Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTestPerformanceByTermAsync(childId, sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }

        [HttpGet("examPerformanceByTerm")]
        [Authorize]
        public async Task<IActionResult> getExamPerformanceByTermAsync(Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getExamPerformanceByTermAsync(childId, sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }
        //----------------------------TopScore---------------------------------------------------------------
        [HttpGet("topTestPerformanceByTerm")]
        [Authorize]
        public async Task<IActionResult> getTopTestPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTopTestPerformanceByTermAsync(topNumber, childId, sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }

        [HttpGet("topExamPerformanceByTerm")]
        [Authorize]
        public async Task<IActionResult> getTopExamPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTopExamPerformanceByTermAsync(topNumber, childId, sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }

        [HttpGet("topTotalPerformanceByTerm")]
        [Authorize]
        public async Task<IActionResult> getTopTotalPerformanceByTermAsync(int topNumber, Guid childId, int sessionId, int termId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTopTotalPerformanceByTermAsync(topNumber, childId, sessionId, termId, schoolId, classId, gradeId);

            return Ok(result);
        }
        //----------------------------Trend---------------------------------------------------------------
        [HttpGet("trendReportbySubjectTest")]
        [Authorize]
        public async Task<IActionResult> getTrendReportbySubjectTestAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTrendReportbySubjectTestAsync(childId, sessionId, subjectId, schoolId, classId, gradeId);

            return Ok(result);
        }

        [HttpGet("trendReportbySubjectExam")]
        [Authorize]
        public async Task<IActionResult> getTrendReportbySubjectExamAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTrendReportbySubjectExamAsync(childId, sessionId, subjectId, schoolId, classId, gradeId);

            return Ok(result);
        }

        [HttpGet("trendReportbySubjectTotal")]
        [Authorize]
        public async Task<IActionResult> getTrendReportbySubjectAsync(Guid childId, int sessionId, long subjectId, long schoolId, long classId, long gradeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTrendReportbySubjectAsync(childId, sessionId, subjectId, schoolId, classId, gradeId);

            return Ok(result);
        }
    }
}
