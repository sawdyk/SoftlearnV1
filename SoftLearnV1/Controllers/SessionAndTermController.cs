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
    public class SessionAndTermController : ControllerBase
    {
        private readonly ISessionTermRepo _sessionTermRepo;

        public SessionAndTermController(ISessionTermRepo sessionTermRepo)
        {
            _sessionTermRepo = sessionTermRepo;
        }

        [HttpPost("createAcademicSession")]
        [Authorize]
        public async Task<IActionResult> createAcademicSessionAsync(AcademicSessionCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.createAcademicSessionAsync(obj);

            return Ok(result);
        }

        [HttpPost("createSession")]
        [Authorize]
        public async Task<IActionResult> createSessionAsync(SessionCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.createSessionAsync(obj);

            return Ok(result);
        }

        [HttpGet("academicSessions")]
        [Authorize]
        public async Task<IActionResult> getAllAcademicSessionsAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getAllAcademicSessionsAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("sessions")]
        [Authorize]
        public async Task<IActionResult> getAllSessionsAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getAllSessionsAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("terms")]
        [Authorize]
        public async Task<IActionResult> getAllTermsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getAllTermsAsync();

            return Ok(result);
        }

        [HttpGet("sessionById")]
        [Authorize]
        public async Task<IActionResult> getSessionByIdAsync(long schoolId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getSessionByIdAsync(schoolId, sessionId);

            return Ok(result);
        }

        [HttpGet("termById")]
        [Authorize]
        public async Task<IActionResult> getTermByIdAsync(long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getTermByIdAsync(termId);

            return Ok(result);
        }

        [HttpPut("setAcademicSessionAsCurrent")]
        [Authorize]
        public async Task<IActionResult> setAcademicSessionAsCurrentAsync(long schoolId, long academicSessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.setAcademicSessionAsCurrentAsync(schoolId, academicSessionId);

            return Ok(result);
        }

        [HttpPut("closeAcademicSession")]
        [Authorize]
        public async Task<IActionResult> closeAcademicSessionAsync(long schoolId, long academicSessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.closeAcademicSessionAsync(schoolId, academicSessionId);

            return Ok(result);
        }

        [HttpPut("openAcademicSession")]
        [Authorize]
        public async Task<IActionResult> openAcademicSessionAsync(long schoolId, long academicSessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.openAcademicSessionAsync(schoolId, academicSessionId);

            return Ok(result);
        }

        [HttpGet("currentSession")]
        [Authorize]
        public async Task<IActionResult> getCurrentSessionAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getCurrentSessionAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("currentTerm")]
        [Authorize]
        public async Task<IActionResult> getCurrentTermAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getCurrentTermAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("currentAcademicSession")]
        [Authorize]
        public async Task<IActionResult> getCurrentAcademicSessionAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.getCurrentAcademicSessionAsync(schoolId);

            return Ok(result);
        }

        [HttpPut("updateSession")]
        [Authorize]
        public async Task<IActionResult> updateSessionAsync(long sessionId, SessionCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.updateSessionAsync(sessionId, obj);

            return Ok(result);
        }

        [HttpPut("updateAcademicSession")]
        [Authorize]
        public async Task<IActionResult> updateAcademicSessionAsync(long academicSessionId, AcademicSessionCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.updateAcademicSessionAsync(academicSessionId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSession")]
        [Authorize]
        public async Task<IActionResult> deleteSessionAsync(long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.deleteSessionAsync(sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteAcademicSession")]
        [Authorize]
        public async Task<IActionResult> deleteAcademicSessionAsync(long academicSessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _sessionTermRepo.deleteAcademicSessionAsync(academicSessionId);

            return Ok(result);
        }
    }
}
