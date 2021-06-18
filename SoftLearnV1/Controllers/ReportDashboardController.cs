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
    public class ReportDashboardController : ControllerBase
    {
        private readonly IReportDashboardRepo _dashboardRepo;

        public ReportDashboardController(IReportDashboardRepo dashboardRepo)
        {
            this._dashboardRepo = dashboardRepo;
        }
        //-------------------------Parent--------------------------------------------
        [HttpGet("noOfCampusesForChildren")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfCampusesForChildrenAsync(Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfCampusesForChildrenAsync(parentId);

            return Ok(result);
        }

        [HttpGet("noOfChildren")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfChildrenAsync(Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfChildrenAsync(parentId);

            return Ok(result);
        }

        [HttpGet("noOfClassesForChildren")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfClassesForChildrenAsync(Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfClassesForChildrenAsync(parentId);

            return Ok(result);
        }

        [HttpGet("totalApprovedAmountPaidForCurrentTerm")]
        [AllowAnonymous]
        public async Task<IActionResult> getTotalAmountPaidForCurrentTermAsync(Guid parentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getTotalAmountPaidForCurrentTermAsync(parentId);

            return Ok(result);
        }
        //-------------------------Class Teacher--------------------------------------------
        [HttpGet("noOfStudentsInTeacherClasses")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfStudentsInTeacherClassAsync(Guid classTeacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfStudentsInTeacherClassAsync(classTeacherId);

            return Ok(result);
        }

        [HttpGet("noOfMaleStudentsInTeacherClasses")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfMaleStudentsInTeacherClassAsync(Guid classTeacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfMaleStudentsInTeacherClassAsync(classTeacherId);

            return Ok(result);
        }

        [HttpGet("noOfFemaleStudentsInTeacherClasses")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfFemaleStudentsInTeacherClassAsync(Guid classTeacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfFemaleStudentsInTeacherClassAsync(classTeacherId);

            return Ok(result);
        }

        [HttpGet("noOfSubjectsInTeacherClasses")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfSubjectsInTeacherClassAsync(Guid classTeacherId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfSubjectsInTeacherClassAsync(classTeacherId);

            return Ok(result);
        }
        //-------------------------Admin--------------------------------------------
        [HttpGet("noOfNonTeachingStaffsInSchool")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfNonTeachingStaffsInSchoolAsync(Guid adminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfNonTeachingStaffsInSchoolAsync(adminId);

            return Ok(result);
        }

        [HttpGet("noOfTeachersInSchool")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfTeachersInSchoolAsync(Guid adminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfTeachersInSchoolAsync(adminId);

            return Ok(result);
        }

        [HttpGet("noOfSchoolCampuses")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfSchoolCampusesAsync(Guid adminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfSchoolCampusesAsync(adminId);

            return Ok(result);
        }

        [HttpGet("noOfStudentsInSchool")]
        [AllowAnonymous]
        public async Task<IActionResult> getNoOfStudentsInSchoolAsync(Guid adminId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _dashboardRepo.getNoOfStudentsInSchoolAsync(adminId);

            return Ok(result);
        }
    }
}
