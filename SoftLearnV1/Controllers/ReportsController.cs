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
    public class ReportsController : ControllerBase
    {
        private readonly IReportsRepo _reportsRepo;

        public ReportsController(IReportsRepo reportsRepo)
        {
            _reportsRepo = reportsRepo;
        }

        [HttpGet("entityReports")]
        [Authorize]
        public async Task<IActionResult> entityReportsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportsRepo.entityReportsAsync();

            return Ok(result);
        }

        [HttpGet("numberOfFacilitatorPerYearMonth")]
        [Authorize]
        public async Task<IActionResult> numberOfFacilitatorPerYearMonthAsync(int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportsRepo.numberOfFacilitatorPerYearMonthAsync(year);

            return Ok(result);
        }

        [HttpGet("numberOfLearnersPerYearMonth")]
        [Authorize]
        public async Task<IActionResult> numberOfLearnersPerYearMonthAsync(int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportsRepo.numberOfLearnersPerYearMonthAsync(year);

            return Ok(result);
        }
    }
}
