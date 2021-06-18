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
    public class ReportCardConfigurationController : ControllerBase
    {
        private readonly IReportCardConfigurationRepo _reportCardConfigurationRepo;

        public ReportCardConfigurationController(IReportCardConfigurationRepo reportCardConfigurationRepo)
        {
            _reportCardConfigurationRepo = reportCardConfigurationRepo;
        }

        [HttpPost("createCommentsList")]
        [Authorize]
        public async Task<IActionResult> createCommentsListAsync([FromBody]CommentListRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.createCommentsListAsync(obj);

            return Ok(result);
        }

        [HttpGet("commentById")]
        [Authorize]
        public async Task<IActionResult> getCommentByIdAsync(long schoolId, long campusId, long commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getCommentByIdAsync(schoolId, campusId, commentId);

            return Ok(result);
        }

        [HttpGet("comments")]
        [Authorize]
        public async Task<IActionResult> getAllCommentsAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllCommentsAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpPut("updateComments")]
        [Authorize]
        public async Task<IActionResult> updateCommentsAsync(long commentId, [FromBody]UpdateCommentRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.updateCommentsAsync(commentId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteComments")]
        [Authorize]
        public async Task<IActionResult> deleteCommentsAsync(long commentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteCommentsAsync(commentId);

            return Ok(result);
        }

        [HttpPost("uploadReportCardSignature")]
        [Authorize]
        public async Task<IActionResult> uploadReportCardSignatureAsync([FromBody]ReportCardSignatureRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.uploadReportCardSignatureAsync(obj);

            return Ok(result);
        }

        [HttpGet("reportCardSignature")]
        [Authorize]
        public async Task<IActionResult> getReportCardSignatureAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardSignatureAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpPost("nextTermBegins")]
        [Authorize]
        public async Task<IActionResult> nextTermBeginsAsync([FromBody]NextTermBeginsRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.nextTermBeginsAsync(obj);

            return Ok(result);
        }

        [HttpGet("nextTermBegins")]
        [Authorize]
        public async Task<IActionResult> getNextTermBeginsAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getNextTermBeginsAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpPost("addCommentsOnReportCardForAllStudents")]
        [Authorize]
        public async Task<IActionResult> addCommentsOnReportCardForAllStudentsAsync([FromBody]CommentsOnReportsCardForAllStudent obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.addCommentsOnReportCardForAllStudentsAsync(obj);

            return Ok(result);
        }

        [HttpPost("addCommentsOnReportCardForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> addCommentsOnReportCardForSingleStudentAsync([FromBody]CommentsOnReportsCardForSingleStudent obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.addCommentsOnReportCardForSingleStudentAsync(obj);

            return Ok(result);
        }

        [HttpGet("commentOnReportCard")]
        [Authorize]
        public async Task<IActionResult> getAllCommentOnReportCardAsync(long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllCommentOnReportCardAsync(schoolId, campusId, commentConfigId, classId, classGradeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("commentOnReportCardById")]
        [Authorize]
        public async Task<IActionResult> getCommentOnReportCardByIdAsync(long commentOnReportCardId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getCommentOnReportCardByIdAsync(commentOnReportCardId);

            return Ok(result);
        }

        [HttpGet("commentOnReportCardByStudentId")]
        [Authorize]
        public async Task<IActionResult> getCommentOnReportCardByStudentIdAsync(Guid studentId, long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getCommentOnReportCardByStudentIdAsync(studentId, schoolId, campusId, commentConfigId, classId, classGradeId, termId, sessionId);

            return Ok(result);
        }

        [HttpGet("reportCardCommenConfig")]
        [Authorize]
        public async Task<IActionResult> getAllReportCardCommenConfigAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllReportCardCommenConfigAsync();

            return Ok(result);
        }

        [HttpGet("reportCardCommenConfigById")]
        [Authorize]
        public async Task<IActionResult> getAllReportCardCommenConfigByIdAsync(long commentConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllReportCardCommenConfigByIdAsync(commentConfigId);

            return Ok(result);
        }

        [HttpPut("updateCommentsOnReportCardForAllStudents")]
        [Authorize]
        public async Task<IActionResult> updateCommentsOnReportCardForAllStudentsAsync([FromBody]CommentsOnReportsCardForAllStudent obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.updateCommentsOnReportCardForAllStudentsAsync(obj);

            return Ok(result);
        }

        [HttpPut("updateCommentsOnReportCardForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> updateCommentsOnReportCardForSingleStudentAsync([FromBody]CommentsOnReportsCardForSingleStudent obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.updateCommentsOnReportCardForSingleStudentAsync(obj);

            return Ok(result);
        }

        [HttpDelete("deleteCommentsOnReportCardForAllStudent")]
        [Authorize]
        public async Task<IActionResult> deleteCommentsOnReportCardForAllStudentAsync(long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteCommentsOnReportCardForAllStudentAsync(schoolId, campusId, commentConfigId, classId, classGradeId, termId, sessionId);

            return Ok(result);
        }

        [HttpDelete("deleteCommentsOnReportCardForSingleStudent")]
        [Authorize]
        public async Task<IActionResult> deleteCommentsOnReportCardForSingleStudentAsync(long commentConfigId, Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteCommentsOnReportCardForSingleStudentAsync(commentConfigId, studentId, schoolId, campusId, classId, classGradeId, termId, sessionId);

            return Ok(result);
        }

        //-------------------------------------------------------REPORT CARD TEMPLATE--------------------------------------------------------------------------------

        [HttpPost("createReportCardTemplate")]
        [Authorize]
        public async Task<IActionResult> createReportCardTemplateAsync([FromBody]ReportCardTemplateRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.createReportCardTemplateAsync(obj);

            return Ok(result);
        }

        [HttpGet("reportCardTemplateById")]
        [Authorize]
        public async Task<IActionResult> getReportCardTemplateByIdAsync(long reportCardTemplateId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardTemplateByIdAsync(reportCardTemplateId);

            return Ok(result);
        }

        [HttpGet("reportCardTemplate")]
        [Authorize]
        public async Task<IActionResult> getReportCardTemplateAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardTemplateAsync(schoolId, campusId);

            return Ok(result);
        }

        //----------------------------------------------------REPORT CARD CONFIGURATION------------------------------------------------------------------------------------------------------

        [HttpPost("createReportCardConfiguration")]
        [Authorize]
        public async Task<IActionResult> createReportCardConfigurationAsync([FromBody]ReportCardConfigRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.createReportCardConfigurationAsync(obj);

            return Ok(result);
        }

        [HttpGet("reportCardConfiguration")]
        [Authorize]
        public async Task<IActionResult> getAllReportCardConfigurationAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllReportCardConfigurationAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("classesReportCardConfiguration")]
        [Authorize]
        public async Task<IActionResult> getAllClassesReportCardConfigurationAsync(long schoolId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllClassesReportCardConfigurationAsync(schoolId);

            return Ok(result);
        }

        [HttpGet("reportCardConfigurationById")]
        [Authorize]
        public async Task<IActionResult> getReportCardConfigurationByIdAsync(long schoolId, long campusId, long reportCardConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardConfigurationByIdAsync(schoolId, campusId, reportCardConfigId);

            return Ok(result);
        }

        [HttpGet("reportCardClassConfigurationById")]
        [Authorize]
        public async Task<IActionResult> getReportCardClassConfigurationByIdAsync(long reportCardClassConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardClassConfigurationByIdAsync(reportCardClassConfigId);

            return Ok(result);
        }

        [HttpGet("reportCardConfigurationByTermId")]
        [Authorize]
        public async Task<IActionResult> getReportCardConfigurationByTermIdAsync(long schoolId, long campusId, long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardConfigurationByTermIdAsync(schoolId, campusId, termId);

            return Ok(result);
        }

        [HttpPut("updateReportCardConfiguration")]
        [Authorize]
        public async Task<IActionResult> updateReportCardConfigurationAsync(ReportCardConfigRequestModel obj, long reportCardConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.updateReportCardConfigurationAsync(obj, reportCardConfigId);

            return Ok(result);
        }

        [HttpDelete("deleteReportCardConfiguration")]
        [Authorize]
        public async Task<IActionResult> deleteReportCardConfigurationAsync(long reportCardConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteReportCardConfigurationAsync(reportCardConfigId);

            return Ok(result);
        }

        [HttpDelete("deleteReportCardConfigurationForClass")]
        [Authorize]
        public async Task<IActionResult> deleteReportCardConfigurationForClassAsync(long reportCardClassConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteReportCardConfigurationForClassAsync(reportCardClassConfigId);

            return Ok(result);
        }

        [HttpPut("changeConfigurationStatusForClass")]
        [Authorize]
        public async Task<IActionResult> changeConfigurationStatusForClassAsync(long reportCardClassConfigId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.changeConfigurationStatusForClassAsync(reportCardClassConfigId);

            return Ok(result);
        }

        //----------------------------------------------------REPORT CARD CONFIGURATION (LEGEND)------------------------------------------------------------------------------------------------------

        [HttpPost("createReportCardConfigurationLegend")]
        [Authorize]
        public async Task<IActionResult> createReportCardConfigurationLegendAsync([FromBody]ReportCardConfigurationLegendRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.createReportCardConfigurationLegendAsync(obj);

            return Ok(result);
        }

        [HttpGet("reportCardConfigurationLegend")]
        [Authorize]
        public async Task<IActionResult> getAllReportCardConfigurationLegendAsync(long schoolId, long campusId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getAllReportCardConfigurationLegendAsync(schoolId, campusId);

            return Ok(result);
        }

        [HttpGet("reportCardConfigurationLegendById")]
        [Authorize]
        public async Task<IActionResult> getReportCardConfigurationLegendByIdAsync(long schoolId, long campusId, long reportCardConfigLegendId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardConfigurationLegendByIdAsync(schoolId, campusId, reportCardConfigLegendId);

            return Ok(result);
        }

        [HttpGet("reportCardConfigurationLegendByTermId")]
        [Authorize]
        public async Task<IActionResult> getReportCardConfigurationLegendByTermIdAsync(long schoolId, long campusId, long termId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.getReportCardConfigurationLegendByTermIdAsync(schoolId, campusId, termId);

            return Ok(result);
        }

        [HttpPut("updateReportCardConfigurationLegend")]
        [Authorize]
        public async Task<IActionResult> updateReportCardConfigurationLegendAsync(long reportCardConfigLegendId, UpdateLegendRequestModel obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.updateReportCardConfigurationLegendAsync(reportCardConfigLegendId, obj);

            return Ok(result);
        }

        [HttpPut("updateReportCardConfigurationLegendList")]
        [Authorize]
        public async Task<IActionResult> updateReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId, long schoolId, long campusId, LegendList obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.updateReportCardConfigurationLegendListAsync(reportCardConfigLegendId, legendListId, schoolId, campusId, obj);

            return Ok(result);
        }

        [HttpDelete("deleteReportCardConfigurationLegendList")]
        [Authorize]
        public async Task<IActionResult> deleteReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteReportCardConfigurationLegendListAsync(reportCardConfigLegendId, legendListId);

            return Ok(result);
        }



        [HttpPut("addReportCardConfigurationLegendList")]
        [Authorize]
        public async Task<IActionResult> addReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long schoolId, long campusId, IList<LegendList> legendList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.addReportCardConfigurationLegendListAsync(reportCardConfigLegendId, schoolId, campusId, legendList);

            return Ok(result);
        }

        [HttpDelete("deleteReportCardConfigurationLegend")]
        [Authorize]
        public async Task<IActionResult> deleteReportCardConfigurationLegendAsync(long reportCardConfigLegendId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _reportCardConfigurationRepo.deleteReportCardConfigurationLegendAsync(reportCardConfigLegendId);

            return Ok(result);
        }
    }
}
