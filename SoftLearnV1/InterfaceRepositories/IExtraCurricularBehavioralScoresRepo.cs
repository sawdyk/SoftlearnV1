using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IExtraCurricularBehavioralScoresRepo
    {
        //----------------------------------------------------SCORES UPLOAD-------------------------------------------------------------------------------------

        Task<UploadScoreResponseModel> uploadExtraCurricularBehavioralScoresAsync(UploadScoreRequestModel obj);
        Task<GenericResponseModel> getExtraCurricularBehavioralScoresAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> getExtraCurricularBehavioralScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> getExtraCurricularBehavioralScoresByStudentIdAndCategoryIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);
        Task<UploadScoreResponseModel> uploadSingleStudentExtraCurricularBehavioralScoreAsync(UploadScorePerStudentRequestModel obj);
        Task<UploadScoreResponseModel> updateExtraCurricularBehavioralScoresAsync(UploadScoreRequestModel obj);
        Task<GenericResponseModel> deleteExtraCurricularBehavioralScoresForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> deleteExtraCurricularBehavioralScoresForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericResponseModel> deleteExtraCurricularBehavioralScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);
        Task<GenericResponseModel> deleteExtraCurricularBehavioralScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);

    }
}
