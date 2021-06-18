using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISessionTermRepo
    {
        Task<GenericResponseModel> getAllTermsAsync();
        Task<GenericResponseModel> getTermByIdAsync(long termId);
        Task<GenericResponseModel> createSessionAsync(SessionCreateRequestModel obj);
        Task<GenericResponseModel> getAllSessionsAsync(long schoolId);
        Task<GenericResponseModel> getSessionByIdAsync(long schoolId, long sessionId);
        Task<GenericResponseModel> createAcademicSessionAsync(AcademicSessionCreateRequestModel obj);
        Task<GenericResponseModel> getAllAcademicSessionsAsync(long schoolId);
        Task<GenericResponseModel> setAcademicSessionAsCurrentAsync(long schoolId, long academicSessionId);
        Task<GenericResponseModel> closeAcademicSessionAsync(long schoolId, long academicSessionId);
        Task<GenericResponseModel> openAcademicSessionAsync(long schoolId, long academicSessionId);
        Task<GenericResponseModel> getCurrentSessionAsync(long schoolId);
        Task<GenericResponseModel> getCurrentTermAsync(long schoolId);
        Task<GenericResponseModel> getCurrentAcademicSessionAsync(long schoolId);

        Task<GenericResponseModel> updateSessionAsync(long sessionId, SessionCreateRequestModel obj);
        Task<GenericResponseModel> updateAcademicSessionAsync(long academicSessionId, AcademicSessionCreateRequestModel obj);
        Task<GenericResponseModel> deleteSessionAsync(long sessionId);  
        Task<GenericResponseModel> deleteAcademicSessionAsync(long academicSessionId);


    }
}
