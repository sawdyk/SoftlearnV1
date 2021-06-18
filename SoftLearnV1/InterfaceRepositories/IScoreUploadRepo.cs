using Microsoft.AspNetCore.Http;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IScoreUploadRepo
    {
        //----------------------------------------------------SCORES UPLOAD-------------------------------------------------------------------------------------

        Task<UploadScoreResponseModel> uploadScoresAsync(UploadSubjectScoreRequestModel obj);
        Task<GenericResponseModel> getScoresBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> getScoresByStudentIdAndSubjectIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> getAllScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<UploadScoreResponseModel> uploadSingleStudentScoreAsync(UploadScorePerSubjectAndStudentRequestModel obj);
        Task<UploadScoreResponseModel> updateSingleStudentScoresAsync(UploadScorePerSubjectAndStudentRequestModel obj);
        Task<UploadScoreResponseModel> updateScoresAsync(UploadSubjectScoreRequestModel obj);
        Task<GenericResponseModel> deleteScoresPerSubjectForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> deleteScoresPerSubjectForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> deleteScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> deleteScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);

        //-----------------------------------------------------EXTENDED SCORES-----------------------------------------------------------------------------------------------
        Task<ExtendedScoresResponseModel> getAllStudentAndSubjectScoresExtendedAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId, IList<SubjectId> subjectId);

        //-----------------------------------------------------SCORE UPLOAD SHEET-----------------------------------------------------------------------------------------------
        Task<ScoreUploadSheetResponseModel> createScoreUploadSheetTemplateAsync(ScoreUploadSheetTemplateRequestModel obj);
        Task<GenericResponseModel> getScoreSheetTemplateByIdAsync(long scoreSheetTemplateId);
        Task<GenericResponseModel> getAllUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId);
        Task<GenericResponseModel> getAllUnUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId);


        //-----------------------------------------------------BULK UPLOAD OF SCORES(EXCEL)-----------------------------------------------------------------------------------------------
        Task<UploadScoreResponseModel> bulkScoresUploadAsync(BulkScoresUploadRequestModel obj);

        //-----------------------------------------------------STUDENT GRADE BOOK (Student Ability to View their scores per subject, category and subcategory)-----------------------------------------------------------------------------------------------
        Task<GenericResponseModel> studentGradeBookScoresPerSubjectAndCategoryAsync(Guid studentId, long schoolId, long campusId, long categoryId, long subCategoryId);

    }
}
