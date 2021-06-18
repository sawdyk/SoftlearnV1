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
    public class ClassTeacherReportController : ControllerBase
    {
        private readonly IClassTeacherReportRepo _reportRepo;

        public ClassTeacherReportController(IClassTeacherReportRepo reportRepo)
        {
            this._reportRepo = reportRepo;
        }
        [HttpGet("testPerformanceBySubject")]
        [Authorize]
        public async Task<IActionResult> getTestPerformanceBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTestPerformanceBySubjectAsync(sessionId, termId, schoolId, campusId, classId, subjectId);

            return Ok(result);
        }

        [HttpGet("examPerformanceBySubject")]
        [Authorize]
        public async Task<IActionResult> getExamPerformanceBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getExamPerformanceBySubjectAsync(sessionId, termId, schoolId, campusId, classId, subjectId);

            return Ok(result);
        }
    }
}
