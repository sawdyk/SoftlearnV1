﻿using System;
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
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectRepo _subjectRepo;

        public SubjectController(ISubjectRepo subjectRepo)
        {
            _subjectRepo = subjectRepo;
        }

        [HttpPost("createSubject")]
        [Authorize]
        public async Task<IActionResult> createSubjectAsync(SubjectCreationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.createSubjectAsync(obj);

            return Ok(result);
        }

        [HttpGet("subjectById")]
        [Authorize]
        public async Task<IActionResult> getSubjectByIdAsync(long subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getSubjectByIdAsync(subjectId);

            return Ok(result);
        }

        [HttpGet("classSubjects")]
        [Authorize]
        public async Task<IActionResult> getAllClassSubjectsAsync(long classId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllClassSubjectsAsync(classId, schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("schoolSubjects")]
        [Authorize]
        public async Task<IActionResult> getAllSchoolSubjectsAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllSchoolSubjectsAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("assignedSubjects")]
        [Authorize]
        public async Task<IActionResult> getAllAssignedSubjectsAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllAssignedSubjectsAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("unAssignedSubjects")]
        [Authorize]
        public async Task<IActionResult> getAllUnAssignedSubjectsAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllUnAssignedSubjectsAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpPost("assignSubjectToTeacher")]
        [Authorize]
        public async Task<IActionResult> assignSubjectToTeacherAsync(AssignSubjectToTeacherRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.assignSubjectToTeacherAsync(obj);

            return Ok(result);
        }

        [HttpGet("subjectsAssignedToTeacher")]
        [Authorize]
        public async Task<IActionResult> getAllSubjectsAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllSubjectsAssignedToTeacherAsync(teacherId, schoolId, campusId);

            return Ok(result);
        }

        [HttpPost("createSubjectDepartment")]
        [Authorize]
        public async Task<IActionResult> createSubjectDepartmentAsync(SubjectDepartmentCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.createSubjectDepartmentAsync(obj);

            return Ok(result);
        }

        [HttpGet("subjectDepartment")]
        [Authorize]
        public async Task<IActionResult> getAllSubjectDepartmentAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllSubjectDepartmentAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("subjectDepartmentById")]
        [Authorize]
        public async Task<IActionResult> getSubjectDepartmentByIdAsync(long subjectDepartmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getSubjectDepartmentByIdAsync(subjectDepartmentId);

            return Ok(result);
        }


        [HttpPut("assignSubjectToDepartment")]
        [Authorize]
        public async Task<IActionResult> assignSubjectToDepartmentAsync(AssignSubjectToDepartmentRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.assignSubjectToDepartmentAsync(obj);

            return Ok(result);
        }

        [HttpGet("subjectsAssignedToDepartment")]
        [Authorize]
        public async Task<IActionResult> getAllSubjectsAssignedToDepartmentAsync(long subjectDepartmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.getAllSubjectsAssignedToDepartmentAsync(subjectDepartmentId);

            return Ok(result);
        }

        [HttpPut("orderOfSubjects")]
        [Authorize]
        public async Task<IActionResult> orderOfSubjectsAsync(long classId, IEnumerable<OrderOfSubjectsRequestModel> obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.orderOfSubjectsAsync(classId, obj);

            return Ok(result);
        }

        [HttpPut("updateSubject")]
        [Authorize]
        public async Task<IActionResult> updateSubjectAsync(long subjectId, SubjectCreationRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.updateSubjectAsync(subjectId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteSubject")]
        [Authorize]
        public async Task<IActionResult> deleteSubjectAsync(long subjectId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.deleteSubjectAsync(subjectId);

            return Ok(result);
        }

        [HttpDelete("deleteAssignedSubjects")]
        [Authorize]
        public async Task<IActionResult> deleteAssignedSubjectsAsync(long subjectAssignedId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.deleteAssignedSubjectsAsync(subjectAssignedId);

            return Ok(result);
        }

        [HttpDelete("deleteSubjectDepartment")]
        [Authorize]
        public async Task<IActionResult> deleteSubjectDepartmentAsync(long subjectDepartmentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _subjectRepo.deleteSubjectDepartmentAsync(subjectDepartmentId);

            return Ok(result);
        }

    }
}
