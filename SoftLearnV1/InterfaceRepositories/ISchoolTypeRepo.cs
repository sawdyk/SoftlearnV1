using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ISchoolTypeRepo
    {
        Task<GenericResponseModel> getAllSchoolTypeAsync();
        Task<GenericResponseModel> getSchoolTypeByIdAsync(long schoolTypeId);
    }
}
