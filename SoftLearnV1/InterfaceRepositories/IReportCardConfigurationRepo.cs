using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IReportCardConfigurationRepo
    {
        Task<GenericResponseModel> createCommentsListAsync(CommentListRequestModel obj);
        Task<GenericResponseModel> getCommentByIdAsync(long schoolId, long campusId, long commentId);
        Task<GenericResponseModel> getAllCommentsAsync(long schoolId, long campusId);
        Task<GenericResponseModel> updateCommentsAsync(long commentId, UpdateCommentRequestModel obj);
        Task<GenericResponseModel> deleteCommentsAsync(long commentId);
        Task<GenericResponseModel> uploadReportCardSignatureAsync(ReportCardSignatureRequestModel obj);
        Task<GenericResponseModel> getReportCardSignatureAsync(long schoolId, long campusId);
        Task<GenericResponseModel> nextTermBeginsAsync(NextTermBeginsRequestModel obj);
        Task<GenericResponseModel> getNextTermBeginsAsync(long schoolId, long campusId);
        Task<GenericResponseModel> addCommentsOnReportCardForAllStudentsAsync(CommentsOnReportsCardForAllStudent obj);
        Task<GenericResponseModel> getAllCommentOnReportCardAsync(long schoolId, long campusId, long CommentConfigId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericResponseModel> getCommentOnReportCardByIdAsync(long commentOnReportCardId);
        Task<GenericResponseModel> getCommentOnReportCardByStudentIdAsync(Guid studentId, long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericResponseModel> addCommentsOnReportCardForSingleStudentAsync(CommentsOnReportsCardForSingleStudent obj);
        Task<GenericResponseModel> updateCommentsOnReportCardForAllStudentsAsync(CommentsOnReportsCardForAllStudent obj);
        Task<GenericResponseModel> updateCommentsOnReportCardForSingleStudentAsync(CommentsOnReportsCardForSingleStudent obj);
        Task<GenericResponseModel> deleteCommentsOnReportCardForSingleStudentAsync(long commentConfigId, Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericResponseModel> deleteCommentsOnReportCardForAllStudentAsync(long commentConfigId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);

        //----------------------------------------------------SYSTEM DEFINED/DEFAULT------------------------------------------------------------------------------------------------------
        Task<GenericResponseModel> getAllReportCardCommenConfigAsync();
        Task<GenericResponseModel> getAllReportCardCommenConfigByIdAsync(long commentConfigId);

        //----------------------------------------------------REPORT CARD TEMPLATE------------------------------------------------------------------------------------------------------
        Task<GenericResponseModel> createReportCardTemplateAsync(ReportCardTemplateRequestModel obj);
        Task<GenericResponseModel> getReportCardTemplateByIdAsync(long reportCardTemplateId);
        Task<GenericResponseModel> getReportCardTemplateAsync(long schoolId, long campusId);

        //----------------------------------------------------REPORT CARD CONFIGURATION------------------------------------------------------------------------------------------------------
        Task<GenericResponseModel> createReportCardConfigurationAsync(ReportCardConfigRequestModel obj);
        Task<GenericResponseModel> getAllReportCardConfigurationAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllClassesReportCardConfigurationAsync(long schoolId);
        Task<GenericResponseModel> getReportCardConfigurationByIdAsync(long schoolId, long campusId, long reportCardConfigId);
        Task<GenericResponseModel> getReportCardClassConfigurationByIdAsync(long reportCardClassConfigId);
        Task<GenericResponseModel> getReportCardConfigurationByTermIdAsync(long schoolId, long campusId, long termId);
        Task<GenericResponseModel> updateReportCardConfigurationAsync(ReportCardConfigRequestModel obj, long reportCardConfigId);
        Task<GenericResponseModel> deleteReportCardConfigurationAsync(long reportCardConfigId);
        Task<GenericResponseModel> deleteReportCardConfigurationForClassAsync(long reportCardClassConfigId);
        Task<GenericResponseModel> changeConfigurationStatusForClassAsync(long reportCardClassConfigId);

        //----------------------------------------------------REPORT CARD CONFIGURATION (LEGEND)---------------------------------------------------------------------------------------------
        Task<GenericResponseModel> createReportCardConfigurationLegendAsync(ReportCardConfigurationLegendRequestModel obj);
        Task<GenericResponseModel> getAllReportCardConfigurationLegendAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getReportCardConfigurationLegendByIdAsync(long schoolId, long campusId, long reportCardConfigLegendId);
        Task<GenericResponseModel> getReportCardConfigurationLegendByTermIdAsync(long schoolId, long campusId, long termId);
        Task<GenericResponseModel> updateReportCardConfigurationLegendAsync(long reportCardConfigLegendId, UpdateLegendRequestModel obj);

        Task<GenericResponseModel> updateReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId, long schoolId, long campusId, LegendList obj);
        Task<GenericResponseModel> deleteReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId);
        Task<GenericResponseModel> addReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long schoolId, long campusId, IList<LegendList> legendList);

        Task<GenericResponseModel> deleteReportCardConfigurationLegendAsync(long reportCardConfigLegendId);

    }
}
