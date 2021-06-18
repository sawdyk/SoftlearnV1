using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IScoresConfigRepo
    {
        //------------------------Score Grading---------------------------------------------------------//
        Task<GenericResponseModel> createScoreGradesAsync(ScoreGradeCreateRequestModel obj);
        Task<GenericResponseModel> getAllScoreGradesAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getScoreGradeByIdAsync(long scoreGradeId);
        Task<GenericResponseModel> getScoreGradeByClassIdAsync(long classId, long schoolId, long campusId);
        Task<GenericResponseModel> updateScoreGradeAsync(long scoreGradeId, ScoreGradeCreateRequestModel obj);
        Task<GenericResponseModel> deleteScoreGradeAsync(long scoreGradeId);

        //------------------------Score Category---------------------------------------------------------//

        Task<GenericResponseModel> getAllScoreCategoryAsync();
        Task<GenericResponseModel> getScoreCategoryByIdAsync(long scoreCategoryId);

        //------------------------Score Category Configuration---------------------------------------------------------//

        Task<GenericResponseModel> createScoreCategoryConfigAsync(ScoreCategoryConfigRequestModel obj);
        Task<GenericResponseModel> getAllScoreCategoryConfigAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getScoreCategoryConfigByIdAsync(long scoreCategoryConfigId);
        Task<GenericResponseModel> updateScoreCategoryConfigAsync(long scoreCategoryConfigId, ScoreCategoryConfigRequestModel obj);
        Task<GenericResponseModel> deleteScoreCategoryConfigAsync(long scoreCategoryConfigId);

        //------------------------Score SubCategory Configuration---------------------------------------------------------//

        Task<GenericResponseModel> createScoreSubCategoryConfigAsync(ScoreSubCategoryConfigRequestModel obj);
        Task<GenericResponseModel> getAllScoreSubCategoryConfigAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getScoreSubCategoryConfigByIdAsync(long scoreSubCategoryConfigId);
        Task<GenericResponseModel> updateScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId, ScoreSubCategoryConfigRequestModel obj);
        Task<GenericResponseModel> deleteScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId);


    }
}
