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
    public class FileUploadController : ControllerBase
    {
        private readonly IFileUploadRepo _uploadFilesRepo;

        public FileUploadController(IFileUploadRepo uploadFilesRepo)
        {
            _uploadFilesRepo = uploadFilesRepo;
        }
        [DisableRequestSizeLimit]
        [HttpPost("uploadFiles")]
        [AllowAnonymous]
        public async Task<IActionResult> uploadFilesAsync([FromForm]FileUploadRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.uploadFilesAsync(obj);

            return Ok(result);
        }

        [HttpGet("appTypes")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllAppTypesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAllAppTypesAsync();

            return Ok(result);
        }

        [HttpGet("appTypesById")]
        [AllowAnonymous]
        public async Task<IActionResult> getAppTypesByIdAsync(long appId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAppTypesByIdAsync(appId);

            return Ok(result);
        }

        [HttpGet("supportedFileExtensions")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllSupportedFileExtensionsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAllSupportedFileExtensionsAsync();

            return Ok(result);
        }

        [HttpGet("folderTypesByAppId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllFolderTypesByAppIdAsync(long appId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getAllFolderTypesByAppIdAsync(appId);

            return Ok(result);
        }

        [HttpGet("folderTypeById")]
        [AllowAnonymous]
        public async Task<IActionResult> getFolderTypeByIdAsync(long folderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _uploadFilesRepo.getFolderTypeByIdAsync(folderId);

            return Ok(result);
        }

    }
}
