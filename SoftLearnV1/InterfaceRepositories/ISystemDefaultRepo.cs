using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Reusables;
using SoftLearnV1.SchoolReusables;
using SoftLearnV1.Security;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISystemDefaultRepo
    {
        Task<GenericResponseModel> getAllGenderAsync();
        Task<GenericResponseModel> getGenderByIdAsync(long genderId);
        Task<GenericResponseModel> getClassOrAlumniAsync();
        Task<GenericResponseModel> getClassOrAlumniByIdAsync(long classOrAlumniId);
        Task<GenericResponseModel> getAllSchoolSubTypesBySchoolTypeIdAsync(long schoolTypeId);
        Task<GenericResponseModel> getAllAttendancePeriodAsync();
        Task<GenericResponseModel> getAttendancePeriodByIdAsync(long periodId);
        Task<GenericResponseModel> getAllStatusAsync();
        Task<GenericResponseModel> getStatusByIdAsync(long statusId);
        Task<GenericResponseModel> getAllScoreStatusAsync();
        Task<GenericResponseModel> getScoreStatusByIdAsync(long scoreStatusId);
        Task<GenericResponseModel> getActiveInActiveStatusAsync();
        Task<GenericResponseModel> getActiveInActiveStatusByIdAsync(long statusId);

    }
}
