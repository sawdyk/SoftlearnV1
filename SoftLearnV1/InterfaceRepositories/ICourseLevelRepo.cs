using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.InterfaceRepositories
{
    public interface ICourseLevelRepo
    {
        Task<GenericResponseModel> getAllCourseLevelAsync();
        Task<GenericResponseModel> getCourseLevelByIdAsync(long courseLevelId);
    }
}
