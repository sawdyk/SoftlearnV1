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
    public class ParentController : ControllerBase
    {
        private readonly IParentRepo _parentRepo;

        public ParentController(IParentRepo parentRepo)
        {
            _parentRepo = parentRepo;
        }

        [HttpPost("parentLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> parentLoginAsync(LoginRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.parentLoginAsync(obj);

            return Ok(result);
        }

        [HttpGet("resendPasswordResetLink")]
        [Authorize]
        public async Task<IActionResult> resendPasswordResetLinkAsync(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.resendPasswordResetLinkAsync(email);

            return Ok(result);
        }

        [HttpGet("parentDetailsByEmail")]
        [Authorize]
        public async Task<IActionResult> getParentDetailsByEmailAsync(string email, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getParentDetailsByEmailAsync(email, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("parentDetailsById")]
        [Authorize]
        public async Task<IActionResult> getParentDetailsByIdAsync(Guid parentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getParentDetailsByIdAsync(parentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("parent")]
        [Authorize]
        public async Task<IActionResult> getAllParentAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getAllParentAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("parentChild")]
        [Authorize]
        public async Task<IActionResult> getAllParentChildAsync(Guid parentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getAllParentChildAsync(parentId, schoolId, campusId);

            return Ok(result);
        }
        //-------------------------------------ChildrenProfile-----------------------------------------------
        [HttpPost("childrenProfile")]
        [Authorize]
        public async Task<IActionResult> getChildrenProfileAsync([FromBody]ChildrenProfileRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenProfileAsync(obj);

            return Ok(result);
        }
        //-------------------------------------ChildrenAttendance-----------------------------------------------
        [HttpPost("childrenAttendanceBySessionId")]
        [Authorize]
        public async Task<IActionResult> getChildrenAttendanceBySessionIdAsync(IList<Guid> childrenId, Guid parentId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenAttendanceBySessionIdAsync(childrenId, parentId, sessionId);

            return Ok(result);
        }

        [HttpPost("childrenAttendanceByTermId")]
        [Authorize]
        public async Task<IActionResult> getChildrenAttendanceByTermIdAsync(IList<Guid> childrenId, Guid parentId, long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenAttendanceByTermIdAsync(childrenId, parentId, termId);

            return Ok(result);
        }

        [HttpPost("childrenAttendanceByDate")]
        [Authorize]
        public async Task<IActionResult> getChildrenAttendanceByDateAsync(IList<Guid> childrenId, Guid parentId,DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenAttendanceByDateAsync(childrenId, parentId, fromDate, toDate);

            return Ok(result);
        }

        //-------------------------------------ChildrenSubject-----------------------------------------------
        [HttpPost("childrenSubject")]
        [Authorize]
        public async Task<IActionResult> getChildrenSubjectAsync(IList<Guid> childrenId, Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildrenSubjectAsync(childrenId, parentId);

            return Ok(result);
        }
        //-------------------------------------ChildAttendance-----------------------------------------------
        [HttpGet("childAttendanceBySessionId")]
        [Authorize]
        public async Task<IActionResult> getChildAttendanceBySessionIdAsync(Guid childId, Guid parentId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildAttendanceBySessionIdAsync(childId, parentId, sessionId);

            return Ok(result);
        }

        [HttpGet("childAttendanceByTermId")]
        [Authorize]
        public async Task<IActionResult> getChildAttendanceByTermIdAsync(Guid childId, Guid parentId, long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildAttendanceByTermIdAsync(childId, parentId, termId);

            return Ok(result);
        }

        [HttpGet("childAttendanceByDate")]
        [Authorize]
        public async Task<IActionResult> getChildAttendanceByDateAsync(Guid childId, Guid parentId, DateTime fromDate, DateTime toDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildAttendanceByDateAsync(childId, parentId, fromDate, toDate);

            return Ok(result);
        }

        //-------------------------------------ChildSubject-----------------------------------------------
        [HttpGet("childSubject")]
        [Authorize]
        public async Task<IActionResult> getChildSubjectAsync(Guid childId, Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _parentRepo.getChildSubjectAsync(childId, parentId);

            return Ok(result);
        }
    }
}
