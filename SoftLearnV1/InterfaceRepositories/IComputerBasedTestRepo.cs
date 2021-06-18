using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IComputerBasedTestRepo
    {
        //------------------------------------------------COMPUTER BASES TEST---------------------------------------------------------------------
        Task<GenericResponseModel> createComputerBasedTestAsync(CbtRequestModel obj);
        Task<GenericResponseModel> getComputerBasedTestByIdAsync(long cbtId);
        Task<GenericResponseModel> getComputerBasedTestAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long typeId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestByTypeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long typeId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestByCategoryIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestByTypeIdAndSubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long typeId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestByCategoryIdAndSubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestByClassIdAndGradeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestByStatusIdAsync(long schoolId, long campusId, long classId, long classGradeId, long statusId, long termId, long sessionId);
        Task<GenericResponseModel> setComputerBasedTestStatusAsync(long cbtId, long statusId); //Set Status as Active/InActive
        Task<GenericResponseModel> updateComputerBasedTestAsync(long cbtId, CbtRequestModel obj);
        Task<GenericResponseModel> deleteComputerBasedTestAsync(long cbtId);

        //--------------------------------------------------START COMPUTER BASED TEST----------------------------------------------------------------
        Task<GenericResponseModel> takeComputerBasedTestAsync(CbtResultRequestModel obj);


        //---------------------------------------------------COMPUTER BASED TEST QUESTIONS-----------------------------------------------------------
        Task<GenericResponseModel> createQuestionsAsync(CbtQuestionRequestModel obj);
        Task<GenericResponseModel> createBulkQuestionsFromExcelAsync(BulkCbtQuestionsRequestModel obj);
        Task<GenericResponseModel> getQuestionsByIdAsync(long questionId);
        Task<GenericResponseModel> getQuestionsByCbtIdAsync(long cbtId);
        Task<GenericResponseModel> getQuestionsByQuestionTypeIdAsync(long cbtId, long questionTypeId);
        Task<GenericResponseModel> updateQuestionAsync(long questionId, CbtQuestionCreationRequestModel obj);
        Task<GenericResponseModel> deleteQuestionAsync(long questionId);



        //---------------------------------------------------COMPUTER BASED TEST RESULTS-------------------------------------------------------------
        Task<GenericResponseModel> updateComputerBasedTestResultAsync(CbtResultCreationRequestModel obj);
        Task<GenericResponseModel> getComputerBasedTestResultByIdAsync(long cbtResultId);
        Task<GenericResponseModel> getComputerBasedTestResultAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long typeId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestResultByTypeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long typeId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestResultByCategoryIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestResultByCbtIdAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long typeId, long cbtId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestResultByStudentIdAsync(long schoolId, long campusId, long classId, long classGradeId, long cbtId, long categoryId, long typeId, Guid studentId, long termId, long sessionId);
        Task<GenericResponseModel> getComputerBasedTestResultByIndividualStudentIdAsync(long schoolId, long campusId, long cbtId, Guid studentId);
        Task<GenericResponseModel> getComputerBasedTestResultByClassIdAndGradeIdAsync(long schoolId, long campusId, long classId, long classGradeId, long cbtId, long termId, long sessionId);
        Task<GenericResponseModel> deleteComputerBasedTestResultAsync(long cbtResultId);


        //-------------------------------------------------SYSTEM DEFINED/DEFAULTS------------------------------------------------------------------------
        Task<GenericResponseModel> getCbtTypesAsync();
        Task<GenericResponseModel> getCbtTypesByIdAsync(long cbtTypeId);
        Task<GenericResponseModel> getCbtCategoryAsync();
        Task<GenericResponseModel> getCbtCategoryByIdAsync(long cbtCategoryId);

    }
}
