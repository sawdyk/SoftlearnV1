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
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankRepo _bankRepo;

        public BankController(IBankRepo bankRepo)
        {
            this._bankRepo = bankRepo;
        }
        [HttpPost("createBank")]
        [Authorize]
        public async Task<IActionResult> createBankAsync(BankRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _bankRepo.createBankAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateBank")]
        [Authorize]
        public async Task<IActionResult> updateBankAsync(long id, BankRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _bankRepo.updateBankAsync(id, obj);

            return Ok(result);
        }

        [HttpDelete("deleteBank")]
        [Authorize]
        public async Task<IActionResult> deleteBankAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _bankRepo.deleteBankAsync(id);

            return Ok(result);
        }

        [HttpGet("getAllBank")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllBankAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _bankRepo.getAllBankAsync();

            return Ok(result);
        }

        [HttpGet("getBankById")]
        [AllowAnonymous]
        public async Task<IActionResult> getBankByIdAsync(long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _bankRepo.getBankByIdAsync(id);

            return Ok(result);
        }
    }
}
