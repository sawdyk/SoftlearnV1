using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SoftLearnV1.Controllers
{
    [Route("api/v1/[controller]")]
    public class FeeTemplateController : ControllerBase
    {
        private readonly IFeeTemplateRepo _feeRepo;

        public FeeTemplateController(IFeeTemplateRepo feeRepo)
        {
            this._feeRepo = feeRepo;
        }
        //----------------------------FeeTemplate---------------------------------------------------------------
        [HttpPost("createFeeTemplate")]
        [Authorize]
        public async Task<IActionResult> createFeeTemplateAsync([FromBody]FeeTemplateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.createFeeTemplateAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateFeeTemplate")]
        [Authorize]
        public async Task<IActionResult> updateFeeTemplateAsync(long templateId, [FromBody]FeeTemplateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.updateFeeTemplateAsync(templateId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteFeeTemplate")]
        [Authorize]
        public async Task<IActionResult> deleteFeeTemplateAsync(long templateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deleteFeeTemplateAsync(templateId);

            return Ok(result);
        }

        [HttpGet("feeTemplateByCampusId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeTemplateByCampusIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllFeeTemplateByCampusIdAsync(campusId);

            return Ok(result);
        }

        [HttpGet("feeTemplateBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeTemplateBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllFeeTemplateBySchoolIdAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("feeTemplateById")]
        [Authorize]
        public async Task<IActionResult> getFeeTemplateByIdAsync(long templateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getFeeTemplateByIdAsync(templateId);

            return Ok(result);
        }
        //----------------------------FeeTemplateList---------------------------------------------------------------
        [HttpPost("createFeeTemplateList")]
        [Authorize]
        public async Task<IActionResult> createFeeTemplateListAsync([FromBody]FeeTemplateListRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.createFeeTemplateListAsync(obj);

            return Ok(result);
        }

        [HttpDelete("deleteFeeTemplateList")]
        [Authorize]
        public async Task<IActionResult> deleteFeeTemplateListAsync(long templateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deleteFeeTemplateListAsync(templateId);

            return Ok(result);
        }

        [HttpDelete("deleteFeeInTemplateList")]
        [Authorize]
        public async Task<IActionResult> deleteFeeInTemplateListAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.deleteFeeInTemplateListAsync(id);

            return Ok(result);
        }

        [HttpGet("feeTemplateListByCampusId")]
        [Authorize]
        public async Task<IActionResult> getFeeTemplateListByCampusIdAsync(long campusId, long templateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getFeeTemplateListByCampusIdAsync(campusId, templateId);

            return Ok(result);
        }
        [HttpGet("allFeeTemplateListByCampusId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeTemplateListByCampusIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllFeeTemplateListByCampusIdAsync(campusId);

            return Ok(result);
        }

        [HttpGet("feeTemplateListBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getFeeTemplateListBySchoolIdAsync(long schoolId, long templateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getFeeTemplateListBySchoolIdAsync(schoolId,templateId);

            return Ok(result);
        }

        [HttpGet("allFeeTemplateListBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeTemplateListBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getAllFeeTemplateListBySchoolIdAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("feeTemplateListById")]
        [Authorize]
        public async Task<IActionResult> getFeeTemplateListByIdAsync(long templateListId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _feeRepo.getFeeTemplateListByIdAsync(templateListId);

            return Ok(result);
        }
    }
}
