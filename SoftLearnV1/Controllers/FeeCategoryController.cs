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
    public class FeeCategoryController : ControllerBase
    {
        private readonly IFeeCategoryRepo _categoryRepo;

        public FeeCategoryController(IFeeCategoryRepo categoryRepo)
        {
            this._categoryRepo = categoryRepo;
        }
        //----------------------------FeeCategory---------------------------------------------------------------
        [HttpPost("createFeeCategory")]
        [Authorize]
        public async Task<IActionResult> createFeeCategoryAsync([FromBody]FeeCategoryRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.createFeeCategoryAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateFeeCategory")]
        [Authorize]
        public async Task<IActionResult> updateFeeCategoryAsync(long categoryId, [FromBody]FeeCategoryRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.updateFeeCategoryAsync(categoryId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteFeeCategory")]
        [Authorize]
        public async Task<IActionResult> deleteFeeCategoryAsync(long categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.deleteFeeCategoryAsync(categoryId);

            return Ok(result);
        }

        [HttpGet("feeCategoryByCampusId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeCategoryByCampusIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getAllFeeCategoryByCampusIdAsync(campusId);

            return Ok(result);
        }

        [HttpGet("feeCategoryBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeCategoryBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getAllFeeCategoryBySchoolIdAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("feeCategoryById")]
        [Authorize]
        public async Task<IActionResult> getFeeCategoryByIdAsync(long categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getFeeCategoryByIdAsync(categoryId);

            return Ok(result);
        }
        //----------------------------FeeSubCategory---------------------------------------------------------------
        [HttpPost("createFeeSubCategory")]
        [Authorize]
        public async Task<IActionResult> createFeeSubCategoryAsync([FromBody]FeeSubCategoryRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.createFeeSubCategoryAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateFeeSubCategory")]
        [Authorize]
        public async Task<IActionResult> updateFeeSubCategoryAsync(long subCategoryId, [FromBody]FeeSubCategoryRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.updateFeeSubCategoryAsync(subCategoryId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteFeeSubCategory")]
        [Authorize]
        public async Task<IActionResult> deleteFeeSubCategoryAsync(long subCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.deleteFeeSubCategoryAsync(subCategoryId);

            return Ok(result);
        }

        [HttpGet("feeSubCategoryByCategoryId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeSubCategoryByCategoryIdAsync(long categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getAllFeeSubCategoryByCategoryIdAsync(categoryId);

            return Ok(result);
        }

        [HttpGet("feeSubCategoryByCampusId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeSubCategoryByCampusIdAsync(long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getAllFeeSubCategoryByCampusIdAsync(campusId);

            return Ok(result);
        }

        [HttpGet("feeSubCategoryBySchoolId")]
        [Authorize]
        public async Task<IActionResult> getAllFeeSubCategoryBySchoolIdAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getAllFeeSubCategoryBySchoolIdAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("feeSubCategoryById")]
        [Authorize]
        public async Task<IActionResult> getFeeSubCategoryByIdAsync(long subCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _categoryRepo.getFeeSubCategoryByIdAsync(subCategoryId);

            return Ok(result);
        }
    }
}
