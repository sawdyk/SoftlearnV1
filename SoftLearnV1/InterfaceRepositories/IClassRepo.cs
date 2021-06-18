using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface IClassRepo
    {

        //-----------------------------Classes and Class Grades/Arms-------------------------------------------------
        Task<GenericResponseModel> createClassAsync(ClassCreateRequestModel obj);
        Task<GenericResponseModel> createClassGradesAsync(ClassGradeCreateRequestModel obj);
        Task<GenericResponseModel> getAllClassesAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getAllClassGradesAsync(long schoolId, long campusId);
        Task<GenericResponseModel> getClassByClassIdAsync(long classId, long schoolId, long campusId);
        Task<GenericResponseModel> getClassGradesByClassGradeIdAsync(long classGradeId, long schoolId, long campusId);
        Task<GenericResponseModel> getClassGradesByClassIdAsync(long classId, long schoolId, long campusId);
        Task<GenericResponseModel> updateClassAsync(long classId, ClassCreateRequestModel obj);
        Task<GenericResponseModel> updateClassGradeAsync(long classGradeId, ClassGradeCreateRequestModel obj);
        Task<GenericResponseModel> deleteClassAsync(long classId, long schoolId, long campusId);
        Task<GenericResponseModel> deleteClassGradeAsync(long classGradeId, long schoolId, long campusId);


        //-----------------------------Students In Class And ClassGrades-------------------------------------------------
        Task<GenericResponseModel> getAllStudentInClassAsync(long classId, long schoolId, long campusId, long sessionId);
        Task<GenericResponseModel> getAllStudentInClassGradeAsync(long classId, long classGradeId,long schoolId, long campusId, long sessionId);
        Task<GenericResponseModel> getAllStudentInClassForCurrentSessionAsync(long classId, long schoolId, long campusId);
        Task<GenericResponseModel> getAllStudentInClassGradeForCurrentSessionAsync(long classId, long classGradeId, long schoolId, long campusId);
    }
}
