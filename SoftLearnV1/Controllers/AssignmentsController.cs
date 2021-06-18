﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentRepo _assignmentRepo;

        public AssignmentsController(IAssignmentRepo assignmentRepo)
        {
            _assignmentRepo = assignmentRepo;
        }

        //--------------------------------------------------------------ASSIGNMENTS------------------------------------------------------------------------------------------------------

        [HttpPost("createAssignment")]
        [Authorize]
        public async Task<IActionResult> createAssignmentAsync(AssignmentCreationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.createAssignmentAsync(obj);

            return Ok(result);
        }

        [HttpGet("assignmentById")]
        [Authorize]
        public async Task<IActionResult> getAssignmentByIdAsync(long assignmentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAssignmentByIdAsync(assignmentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("assignmentBySubjectId")]
        [Authorize]
        public async Task<IActionResult> getAssignmentBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAssignmentBySubjectIdAsync(subjectId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }
       
        [HttpPut("updateAssignment")]
        [Authorize]
        public async Task<IActionResult> updateAssignmentAsync(long assignmentId, AssignmentCreationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.updateAssignmentAsync(assignmentId, obj);

            return Ok(result);
        }


        [HttpDelete("deleteAssignment")]
        [Authorize]
        public async Task<IActionResult> deleteAssignmentAsync(long assignmentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.deleteAssignmentAsync(assignmentId, schoolId, campusId);

            return Ok(result);
        }


        //-----------------------------------------------SUBMIT AND GRADE ASSIGNMENTS-------------------------------------------------

        [HttpPost("submitAssignment")]
        [Authorize]
        public async Task<IActionResult> submitAssignmentAsync(SubmitAssignmentRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.submitAssignmentAsync(obj);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentById")]
        [Authorize]
        public async Task<IActionResult> getSubmittedAssignmentByIdAsync(long assignmentSubmittedId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getSubmittedAssignmentByIdAsync(assignmentSubmittedId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByAssignmentId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByAssignmentIdAsync(long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByAssignmentIdAsync(classId, classGradeId, assignmentId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllSubmittedAssignmentsByStudentIdAsync(studentId, classId, classGradeId, assignmentId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("unSubmittedAssignmentsByStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllUnSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllUnSubmittedAssignmentsByStudentIdAsync(studentId, classId, classGradeId, schoolId, campusId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("unSubmittedAssignmentsByIndividualStudentId")]
        [Authorize]
        public async Task<IActionResult> getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(studentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("submittedAssignmentsByIndividualStudentId")]
        [Authorize]
        public async Task<IActionResult> getSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long assignmentId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.getSubmittedAssignmentsByIndividualStudentIdAsync(studentId, assignmentId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPut("updateSubmittedAssignments")]
        [Authorize]
        public async Task<IActionResult> updateSubmittedAssignmentsAsync(long assignmentSubmittedId, SubmitAssignmentRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.updateSubmittedAssignmentsAsync(assignmentSubmittedId, obj);

            return Ok(result);
        }


        [HttpDelete("deleteSubmittedAssignments")]
        [Authorize]
        public async Task<IActionResult> deleteSubmittedAssignmentsAsync(long assignmentSubmittedId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.deleteSubmittedAssignmentsAsync(assignmentSubmittedId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPut("gradeSubmittedAssignments")]
        [Authorize]
        public async Task<IActionResult> gradeSubmittedAssignmentsAsync(GradeAssignmentsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _assignmentRepo.gradeSubmittedAssignmentsAsync(obj);

            return Ok(result);
        }
    }
}
