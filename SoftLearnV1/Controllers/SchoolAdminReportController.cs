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
    public class SchoolAdminReportController : ControllerBase
    {
        private readonly ISchoolAdminReportRepo _reportRepo;

        public SchoolAdminReportController(ISchoolAdminReportRepo reportRepo)
        {
            this._reportRepo = reportRepo;
        }

        [HttpGet("trendReportByClass")]
        [Authorize]
        public async Task<IActionResult> getTrendReportByClassAsync(int sessionId, int termId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTrendReportByClassAsync(sessionId, termId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("trendReportBySubject")]
        [Authorize]
        public async Task<IActionResult> getTrendReportBySubjectAsync(int sessionId, int termId, long schoolId, long campusId, long classId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportRepo.getTrendReportBySubjectAsync(sessionId, termId, schoolId, campusId, classId);

            return Ok(result);
        }
        //----------------------------Top Student---------------------------------------------------------------
        [HttpGet("topStudentsByClass")]
        [Authorize]
        public async Task<IActionResult> getTopStudentsByClassAsync(int topNumber,int sessionId, int termId, long schoolId, long campusId, long classId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _reportRepo.getTopStudentsByClassAsync(topNumber, sessionId, termId, schoolId, campusId, classId);
            return Ok(result);
        }
        [HttpGet("topStudentsBySubject")]
        [Authorize]
        public async Task<IActionResult> getTopStudentsBySubjectAsync(int topNumber, int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _reportRepo.getTopStudentsBySubjectAsync(topNumber, sessionId, termId, schoolId, campusId, classId, subjectId);
            return Ok(result);
        }
        //----------------------------Low Student---------------------------------------------------------------
        [HttpGet("lowStudentsByClass")]
        [Authorize]
        public async Task<IActionResult> getLowStudentsByClassAsync(int lowNumber, int sessionId, int termId, long schoolId, long campusId, long classId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _reportRepo.getLowStudentsByClassAsync(lowNumber, sessionId, termId, schoolId, campusId, classId);
            return Ok(result);
        }
        [HttpGet("lowStudentsBySubject")]
        [Authorize]
        public async Task<IActionResult> getLowStudentsBySubjectAsync(int lowNumber, int sessionId, int termId, long schoolId, long campusId, long classId, long subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await _reportRepo.getLowStudentsBySubjectAsync(lowNumber, sessionId, termId, schoolId, campusId, classId, subjectId);
            return Ok(result);
        }
    }
}
