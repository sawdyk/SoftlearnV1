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
    public class CourseTopicsController : ControllerBase
    {
        private readonly ICourseTopicsRepo _courseTopicsRepo;

        public CourseTopicsController(ICourseTopicsRepo courseTopicsRepo)
        {
            _courseTopicsRepo = courseTopicsRepo;
        }


        [HttpPost("createCourseTopics")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicsAsync([FromBody] CourseTopicsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createCourseTopicsAsync(obj);

            return Ok(result);
        }

        [HttpPost("createMultipleCourseTopics")]
        [Authorize]
        public async Task<IActionResult> createMultipleCourseTopicsAsync([FromBody] MultipleCourseTopicsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createMultipleCourseTopicsAsync(obj);

            return Ok(result);
        }

        [HttpGet("allCourseTopics")]
        [Authorize]
        public async Task<IActionResult> getAllCourseTopicsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicsAsync();

            return Ok(result);
        }

        [HttpGet("allCourseTopicsByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicsByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicsByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("allCourseTopicsByCourseIdWithApprovedData")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicsByCourseIdWithApprovedDataAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicsByCourseIdWithApprovedDataAsync(courseId);

            return Ok(result);
        }

        [HttpGet("allCourseTopicsByFacilitatorId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicsByFacilitatorIdAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicsByFacilitatorIdAsync(facilitatorId);

            return Ok(result);
        }

        [HttpGet("courseTopicsById")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicsByIdAsync(long courseTopicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicsByIdAsync(courseTopicId);

            return Ok(result);
        }

        //------------------------------------COURSE TOPIC MATERIAL ------------------------------------------------------------
        [HttpPut("approveCourseTopicMaterial")]
        [Authorize]
        public async Task<IActionResult> approveCourseTopicMaterialAsync(long courseTopicMateriaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.approveCourseTopicMaterialAsync(courseTopicMateriaId);

            return Ok(result);
        }

        [HttpPost("createCourseTopicMaterial")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicMaterialsAsync(CourseTopicsMaterialsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createCourseTopicMaterialsAsync(obj);

            return Ok(result);
        }


        [HttpPost("createMultipleCourseTopicMaterials")]
        [Authorize]
        public async Task<IActionResult> createMultipleCourseTopicMaterialsAsync(MultipleCourseTopicsMaterialsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createMultipleCourseTopicMaterialsAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseTopicMaterialsById")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicMaterialsByIdAsync(long courseTopicMaterialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicMaterialsByIdAsync(courseTopicMaterialId);

            return Ok(result);
        }

        [HttpGet("allCourseTopicMaterialsByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicMaterialsByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicMaterialsByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseTopicMaterialsByCourseTopicId")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicMaterialsByCourseTopicIdAsync(long courseTopiclId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicMaterialsByCourseTopicIdAsync(courseTopiclId);

            return Ok(result);
        }


        //------------------------------------COURSE TOPIC VIDEOS ------------------------------------------------------------

        [HttpPut("approveCourseTopicVideo")]
        [Authorize]
        public async Task<IActionResult> approveCourseTopicVideoAsync(long courseTopicVideoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.approveCourseTopicVideoAsync(courseTopicVideoId);

            return Ok(result);
        }

        [HttpPost("createCourseTopicVideo")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicVideosAsync(CourseTopicsMaterialsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createCourseTopicVideosAsync(obj);

            return Ok(result);
        }

        [HttpPost("createMultipleCourseTopicVideos")]
        [Authorize]
        public async Task<IActionResult> createMultipleCourseTopicVideosAsync(MultipleCourseTopicsMaterialsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createMultipleCourseTopicVideosAsync(obj);

            return Ok(result);
        }


        [HttpGet("courseTopicVideoById")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicVideosByIdAsync(long courseTopicVideoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicVideosByIdAsync(courseTopicVideoId);

            return Ok(result);
        }

        [HttpGet("allCourseTopicVideosByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicVideosByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicVideosByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseTopicVideosByCourseTopicId")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicVideosByCourseTopicIdAsync(long courseTopicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicVideosByCourseTopicIdAsync(courseTopicId);

            return Ok(result);
        }

        [HttpDelete("deleteCourseTopics")]
        [Authorize]
        public async Task<IActionResult> deleteCourseTopicsAsync(long courseTopicId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.deleteCourseTopicsAsync(courseTopicId);

            return Ok(result);
        }

        [HttpDelete("deleteCourseTopicMaterial")]
        [Authorize]
        public async Task<IActionResult> deleteCourseTopicMaterialAsync(long courseTopicMaterialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.deleteCourseTopicMaterialAsync(courseTopicMaterialId);

            return Ok(result);
        }

        [HttpDelete("deleteCourseTopicVideos")]
        [AllowAnonymous]
        public async Task<IActionResult> deleteCourseTopicVideosAsync(long courseTopicVideoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.deleteCourseTopicVideosAsync(courseTopicVideoId);

            return Ok(result);
        }
        //------------------------------------COURSE TOPIC VIDEO MATERIAL ------------------------------------------------------------
        [HttpPut("approveCourseTopicVideoMaterial")]
        [Authorize]
        public async Task<IActionResult> approveCourseTopicVideoMaterialAsync(long courseTopicVideoMateriaId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.approveCourseTopicVideoMaterialAsync(courseTopicVideoMateriaId);

            return Ok(result);
        }

        [HttpPost("createCourseTopicVideoMaterial")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicVideoMaterialsAsync(CourseTopicVideoMaterialsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createCourseTopicVideoMaterialsAsync(obj);

            return Ok(result);
        }


        [HttpPost("createMultipleCourseTopicVideoMaterials")]
        [Authorize]
        public async Task<IActionResult> createMultipleCourseTopicVideoMaterialsAsync(MultipleCourseTopicVideoMaterialsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createMultipleCourseTopicVideoMaterialsAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseTopicVideoMaterialsById")]
        [AllowAnonymous]
        public async Task<IActionResult> getCourseTopicVideoMaterialsByIdAsync(long courseTopicVideoMaterialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicVideoMaterialsByIdAsync(courseTopicVideoMaterialId);

            return Ok(result);
        }

        [HttpGet("allCourseTopicVideoMaterialsByVideoId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicVideoMaterialsByVideoIdAsync(long videoId, bool? isApproved)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicVideoMaterialsByVideoIdAsync(videoId, isApproved);

            return Ok(result);
        }

        [HttpGet("allCourseTopicVideoMaterialsByCourseTopicId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseTopicVideoMaterialsByCourseTopicIdAsync(long courseTopicId, bool? isApproved)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getAllCourseTopicVideoMaterialsByCourseTopicIdAsync(courseTopicId, isApproved);

            return Ok(result);
        }

        [HttpDelete("deleteCourseTopicVideoMaterial")]
        [Authorize]
        public async Task<IActionResult> deleteCourseTopicVideoMaterialsAsync(long courseTopicVideoMaterialId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.deleteCourseTopicVideoMaterialsAsync(courseTopicVideoMaterialId);

            return Ok(result);
        }
        //------------------------------- Course Topic Completed Video-----------------------------------------------------------
        [HttpPost("createCourseTopicCompletedVideo")]
        [Authorize]
        public async Task<IActionResult> createCourseTopicCompletedVideoAsync(CourseEnrolleeCompletedVideoRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.createCourseTopicCompletedVideoAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseTopicCompletedVideoByCourseId")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicCompletedVideoByCourseIdAsync(long courseId, long courseEnrolleeId, Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicCompletedVideoByCourseIdAsync(courseId, courseEnrolleeId, learnerId);

            return Ok(result);
        }

        [HttpGet("courseTopicCompletedVideoByCourseTopicId")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicCompletedVideoByCourseTopicIdAsync(long courseTopicId, long courseEnrolleeId, Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicCompletedVideoByCourseTopicIdAsync(courseTopicId, courseEnrolleeId, learnerId);

            return Ok(result);
        }

        [HttpGet("courseTopicCompletedVideoByVideoId")]
        [Authorize]
        public async Task<IActionResult> getCourseTopicCompletedVideoByVideoIdAsync(long videoId, long courseEnrolleeId, Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseTopicsRepo.getCourseTopicCompletedVideoByVideoIdAsync(videoId, courseEnrolleeId, learnerId);

            return Ok(result);
        }
    }
}
