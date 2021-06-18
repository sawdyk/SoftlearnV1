using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SchoolTypeController : ControllerBase
    {
        private readonly ISchoolTypeRepo _schoolTypeRepo;

        public SchoolTypeController(ISchoolTypeRepo schoolTypeRepo)
        {
            _schoolTypeRepo = schoolTypeRepo;
        }


        [HttpGet("schoolType")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllSchoolType()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolTypeRepo.getAllSchoolTypeAsync();

            return Ok(result);
        }

        [HttpGet("schoolTypeById")]
        [AllowAnonymous]
        public async Task<IActionResult> schoolTypeById(long schoolTypeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _schoolTypeRepo.getSchoolTypeByIdAsync(schoolTypeId);

            return Ok(result);
        }
    }
}
