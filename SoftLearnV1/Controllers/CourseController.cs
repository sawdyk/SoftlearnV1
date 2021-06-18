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
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepo _courseRepo;

        public CourseController(ICourseRepo courseRepo)
        {
            _courseRepo = courseRepo;
        }

        [HttpPost("createCourse")]
        [Authorize]
        public async Task<IActionResult> createCourse(CourseCreateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.createCourseAsync(obj);

            return Ok(result);
        }

        [HttpPut("approveCourseCreation")]
        [Authorize]
        public async Task<IActionResult> approveCourseCreationAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.approveCourseCreationAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseById")]
        [AllowAnonymous]
        public async Task<IActionResult> courseById(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCourseByIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("allCourses")]
        [AllowAnonymous]
        public async Task<IActionResult> allCourses(int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCoursesAsync(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("allCourseByFacilitatorId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseByFacilitatorIdAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCourseByFacilitatorIdAsync(facilitatorId);

            return Ok(result);
        }

        [HttpDelete("deleteCourse")]
        [Authorize]
        public async Task<IActionResult> deleteCourseAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.deleteCourseAsync(courseId);

            return Ok(result);
        }

        [HttpDelete("deleteCourseAttachedToEnrollee")]
        [Authorize]
        public async Task<IActionResult> deleteCourseAttachedToEnrolleeAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.deleteCourseAttachedToEnrolleeAsync(courseId);

            return Ok(result);
        }


        [HttpPut("updateCourseVideoPreview")]
        [Authorize]
        public async Task<IActionResult> updateCourseVideoPreviewAsync(long courseId, string courseVideoPreviewUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.updateCourseVideoPreviewAsync(courseId, courseVideoPreviewUrl);

            return Ok(result);
        }

        //With Pagination
        [HttpGet("allCoursePaginationByFacilitatorId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseByFacilitatorIdAsync(Guid facilitatorId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCourseByFacilitatorIdAsync(facilitatorId, pageNumber, pageSize);

            return Ok(result);
        }

        //With pagination
        [HttpGet("coursesPaginationByTypeId")]
        [AllowAnonymous]
        public async Task<IActionResult> coursesByTypeId(long typeId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByTypeIdAsync(typeId, pageNumber, pageSize);

            return Ok(result);
        }

        //With pagination
        [HttpGet("coursesPaginationByCategoryId")]
        [AllowAnonymous]
        public async Task<IActionResult> coursesByCategoryId(long categoryId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByCategoryIdAsync(categoryId, pageNumber, pageSize);

            return Ok(result);
        }

        //With pagination
        [HttpGet("coursesPaginationByLevelId")]
        [AllowAnonymous]
        public async Task<IActionResult> coursesByLevelId(long levelId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByLevelIdAsync(levelId, pageNumber, pageSize);

            return Ok(result);
        }

        //Without pagination
        [HttpGet("allCoursesByParameters")]
        [AllowAnonymous]
        public async Task<IActionResult> AllCourses(long typeId, long categoryId, long subCategoryId, long levelId, long statusId, Guid facilitatorId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCoursesAsync(typeId, categoryId, subCategoryId, levelId, statusId, facilitatorId, pageNumber, pageSize);

            return Ok(result);
        }

        //Without pagination
        [HttpGet("coursesByTypeId")]
        [AllowAnonymous]
        public async Task<IActionResult> coursesByTypeId(long typeId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByTypeIdAsync(typeId);

            return Ok(result);
        }

        //Without pagination
        [HttpGet("coursesByCategoryId")]
        [AllowAnonymous]
        public async Task<IActionResult> coursesByCategoryId(long categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByCategoryIdAsync(categoryId);

            return Ok(result);
        }

        //Without pagination
        [HttpGet("coursesByLevelId")]
        [AllowAnonymous]
        public async Task<IActionResult> coursesByLevelId(long levelId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByLevelIdAsync(levelId);

            return Ok(result);
        }

        [HttpPost("courseEnroll")]
        [Authorize]
        public async Task<IActionResult> courseEnrollAsync(CourseEnrollRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.courseEnrollAsync(obj);

            return Ok(result);
        }

        [HttpGet("courseEnrollById")]
        [Authorize]
        public async Task<IActionResult> getCourseEnrollByIdAsync(long courseEnrollId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCourseEnrollByIdAsync(courseEnrollId);

            return Ok(result);
        }

        [HttpGet("searchCoursesEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> searchCoursesEnrolledForAsync(Guid learnerId, string courseName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.searchCoursesEnrolledForAsync(learnerId, courseName);

            return Ok(result);
        }

        [HttpGet("allCourseEnrolledForByCourseId")]
        //[Authorize]
        public async Task<IActionResult> getAllCourseEnrolledForByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCourseEnrolledForByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("allCoursesLearnerEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> getAllCoursesLearnerEnrolledForAysnc(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCoursesLearnerEnrolledForAysnc(learnerId);

            return Ok(result);
        }

        [HttpGet("allCoursesLearnerEnrolledForByPagination")]
        [Authorize]
        public async Task<IActionResult> getAllCoursesLearnerEnrolledForAysnc(Guid learnerId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCoursesLearnerEnrolledForAysnc(learnerId, pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("verifyPayment")]
        [Authorize]
        public async Task<IActionResult> verifyPaymentAysnc(long cartId, Guid learnerId, string reference)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.verifyPaymentAysnc(cartId, learnerId, reference);

            return Ok(result);
        }

        [HttpGet("searchCourseByCourseName")]
        [AllowAnonymous]
        public async Task<IActionResult> searchCourseByCourseNameAsync(string courseName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.searchCourseByCourseNameAsync(courseName);

            return Ok(result);
        }

        //With pagination
        [HttpGet("coursesPaginationBySubCategoryId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCoursesBySubCategoryIdAsync(long subCategoryId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesBySubCategoryIdAsync(subCategoryId, pageNumber, pageSize);

            return Ok(result);
        }

        //Without pagination
        [HttpGet("coursesBySubCategoryId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCoursesBySubCategoryIdAsync(long subCategoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesBySubCategoryIdAsync(subCategoryId);

            return Ok(result);
        }

        //With pagination
        [HttpGet("coursesPaginationByStatusId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCoursesByStatusIdAsync(long statusId, int pageNumber, int pageSize)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByStatusIdAsync(statusId, pageNumber, pageSize);

            return Ok(result);
        }

        //Without pagination
        [HttpGet("coursesByStatusId")]
        [AllowAnonymous]
        public async Task<IActionResult> getCoursesByStatusIdAsync(long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesByStatusIdAsync(statusId);

            return Ok(result);
        }


        [HttpGet("popularCourses")]
        [AllowAnonymous]
        public async Task<IActionResult> popularCoursesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.popularCoursesAsync();

            return Ok(result);
        }

        [HttpPost("createMostViewedCourses")]
        [AllowAnonymous]
        public async Task<IActionResult> createMostViewedCourses(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.createMostViewedCoursesAsync(courseId);

            return Ok(result);
        }

        [HttpGet("mostViewedCourses")]
        [AllowAnonymous]
        public async Task<IActionResult> mostViewedCoursesAsyn()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.mostViewedCoursesAsync();

            return Ok(result);
        }

        [HttpGet("recommendedCourses")]
        [AllowAnonymous]
        public async Task<IActionResult> recommendedCoursesAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.recommendedCoursesAsync(learnerId);

            return Ok(result);
        }

        [HttpGet("coursesByFacilitatorLearnersEnrolledFor")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCoursesByFacilitatorLearnersEnrolledForAsync(Guid facilitatorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCoursesByFacilitatorLearnersEnrolledForAsync(facilitatorId);

            return Ok(result);
        }

        [HttpDelete("deleteAllCoursesEnrolledForByCourseId")]
        [Authorize]
        public async Task<IActionResult> deleteAllCoursesEnrolledForByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.deleteAllCoursesEnrolledForByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpDelete("deleteCourseEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> deleteCourseEnrolledForAsync(Guid learnerId, long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.deleteCourseEnrolledForAsync(learnerId, courseId);

            return Ok(result);
        }

        [HttpGet("courseRatingAndReviewByCourseId")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseRatingAndReviewByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCourseRatingAndReviewByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("courseAndAverageRating")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllCourseAndAverageRatingAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllCourseAndAverageRatingAsync();

            return Ok(result);
        }

        //--------------------Course Archiving, Activation and deactivaton by Learners--------------------------------

        [HttpPut("archiveOrUnArchiveCourseEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> archiveOrUnArchiveCourseEnrolledForAsync(Guid learnerId, long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.archiveOrUnArchiveCourseEnrolledForAsync(learnerId, courseId);

            return Ok(result);
        }

        [HttpGet("allArchivedCoursesEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> getAllArchivedCoursesEnrolledForAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllArchivedCoursesEnrolledForAsync(learnerId);

            return Ok(result);
        }

        [HttpGet("allUnArchivedCoursesEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> getAllUnArchivedCoursesEnrolledForAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllUnArchivedCoursesEnrolledForAsync(learnerId);

            return Ok(result);
        }

        [HttpPut("updateStatusForCourseEnrolledFor")]
        [Authorize]
        public async Task<IActionResult> updateStatusForCourseEnrolledForAsync(Guid learnerId, long courseId, long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.updateStatusForCourseEnrolledForAsync(learnerId, courseId, statusId);

            return Ok(result);
        }

        [HttpGet("coursesEnrolledForByStatusId")]
        [Authorize]
        public async Task<IActionResult> getCoursesEnrolledForByStatusIdAsync(Guid learnerId, long statusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCoursesEnrolledForByStatusIdAsync(learnerId, statusId);

            return Ok(result);
        }

        //----------------------Earning Percentage on Courses (SuperAdmin/Admin)---------------------------------------------------------

        [HttpGet("defaultPercentageEarningsPerCourse")]
        [Authorize]
        public async Task<IActionResult> getDefaultPercentageEarningsPerCourseAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getDefaultPercentageEarningsPerCourseAsync();

            return Ok(result);
        }

        [HttpPut("updateDefaultPercentageEarningsPerCourse")]
        [Authorize]
        public async Task<IActionResult> updateDefaultPercentageEarningsPerCourseAsync(long Id, long percentage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.updateDefaultPercentageEarningsPerCourseAsync(Id, percentage);

            return Ok(result);
        }


        [HttpGet("percentageEarnedOnCourses")]
        [Authorize]
        public async Task<IActionResult> getAllPercentageEarnedOnCoursesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllPercentageEarnedOnCoursesAsync();

            return Ok(result);
        }

        [HttpGet("percentageEarnedOnCoursesById")]
        [Authorize]
        public async Task<IActionResult> getPercentageEarnedOnCoursesByIdAsync(long Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getPercentageEarnedOnCoursesByIdAsync(Id);

            return Ok(result);
        }


        [HttpGet("percentageEarnedOnCoursesByCourseId")]
        [Authorize]
        public async Task<IActionResult> getPercentageEarnedOnCoursesByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getPercentageEarnedOnCoursesByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpPut("updatePercentageEarnedOnCourses")]
        [Authorize]
        public async Task<IActionResult> updatePercentageEarnedOnCoursesAsync(long courseId, long percentage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.updatePercentageEarnedOnCoursesAsync(courseId, percentage);

            return Ok(result);
        }

        //------------------------------- Course Return -------------------------------------------------------------

        [HttpPost("refundCourse")]
        [Authorize]
        public async Task<IActionResult> refundCourseAsync(CourseRefundRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.refundCourseAsync(obj);

            return Ok(result);
        }

        [HttpDelete("deleteRefundCourse")]
        [Authorize]
        public async Task<IActionResult> deleteRefundCourseAsync(long refundCourseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.deleteRefundCourseAsync(refundCourseId);

            return Ok(result);
        }

        [HttpGet("refundCourses")]
        [Authorize]
        public async Task<IActionResult> getAllRefundCoursesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getAllRefundCoursesAsync();

            return Ok(result);
        }

        [HttpGet("refundCoursesById")]
        [Authorize]
        public async Task<IActionResult> getRefundCoursesByIdAsync(long refundCourseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getRefundCoursesByIdAsync(refundCourseId);

            return Ok(result);
        }


        [HttpGet("refundCourseByCourseId")]
        [Authorize]
        public async Task<IActionResult> getRefundCourseByCourseIdAsync(long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getRefundCourseByCourseIdAsync(courseId);

            return Ok(result);
        }

        [HttpGet("refundCourseByLearnerId")]
        [Authorize]
        public async Task<IActionResult> getRefundCourseByLearnerIdAsync(Guid learnerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getRefundCourseByLearnerIdAsync(learnerId);

            return Ok(result);
        }


        [HttpPost("courseProgress")]
        [Authorize]
        public async Task<IActionResult> courseProgressAsync(Guid learnerId, long courseId, long videoId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.courseProgressAsync(learnerId, courseId, videoId);

            return Ok(result);
        }


        [HttpGet("courseProgress")]
        [Authorize]
        public async Task<IActionResult> getCourseProgressAsync(Guid learnerId, long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCourseProgressAsync(learnerId, courseId);

            return Ok(result);
        }

        [HttpGet("courseCertificate")]
        [Authorize]
        public async Task<IActionResult> getCourseCertificateAsync(Guid learnerId, long courseId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _courseRepo.getCourseCertificateAsync(learnerId, courseId);

            return Ok(result);
        }


    }
}
