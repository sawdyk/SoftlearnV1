using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ILessonNoteRepo
    {
        //-----------------------------------LESSONNOTES---------------------------------------------------------------
        Task<GenericResponseModel> createLessonNotesAsync(LessonNoteCreateRequestModel obj);
        Task<GenericResponseModel> getLessonNotesByIdAsync(long lessonNoteId, long schoolId, long campusId);
        Task<GenericResponseModel> getLessonNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getLessonNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getLessonNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> deleteLessonNotesAsync(long lessonNoteId, long schoolId, long campusId);
        Task<GenericResponseModel> updateLessonNotesAsync(long lessonNoteId, LessonNoteCreateRequestModel obj);
        Task<GenericResponseModel> approveLessonNotesAsync(long lessonNoteId, long statusId, long schoolId, long campusId);


        //-------------------------------------------------SUBJECTNOTES---------------------------------------------------------------
        Task<GenericResponseModel> createSubjectNotesAsync(SubjectNoteCreateRequestModel obj);
        Task<GenericResponseModel> getSubjectNotesByIdAsync(long subjectNoteId, long schoolId, long campusId);
        Task<GenericResponseModel> getSubjectNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getSubjectNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> getSubjectNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericResponseModel> deleteSubjectNotesAsync(long subjectNoteId, long schoolId, long campusId);
        Task<GenericResponseModel> updateSubjectNotesAsync(long subjectNoteId, SubjectNoteCreateRequestModel obj);

    }
}
