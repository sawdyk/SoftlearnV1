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
    public class SystemUsersController : Controller
    {
        private readonly ISystemUserRepo _userRepo;

        public SystemUsersController(ISystemUserRepo userRepo)
        {
            this._userRepo = userRepo;
        }
        [HttpPost("systemUserLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> systemUserLoginAsync([FromBody]LoginRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userRepo.systemUserLoginAsync(obj);

            return Ok(result);
        }
        [HttpPost("createSystemUser")]
        [Authorize]
        public async Task<IActionResult> createSystemUserAsync([FromBody]SystemUserRegRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userRepo.createSystemUserAsync(obj);

            return Ok(result);
        }

        [HttpGet("systemRoles")]
        [Authorize]
        public async Task<IActionResult> getAllSystemRolesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userRepo.getAllSystemRolesAsync();

            return Ok(result);
        }

        [HttpGet("systemRolesById")]
        [Authorize]
        public async Task<IActionResult> getSystemRolesByIdAsync(long systemRoleId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userRepo.getSystemRolesByIdAsync(systemRoleId);

            return Ok(result);
        }
    }
}
