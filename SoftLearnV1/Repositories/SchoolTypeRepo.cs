using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class SchoolTypeRepo : ISchoolTypeRepo
    {
        private readonly AppDbContext _context;
        public SchoolTypeRepo(AppDbContext context)
        {
            _context = context;

        }
        public async Task<GenericResponseModel> getAllSchoolTypeAsync()
        {
            try
            {
                var result = from sch in _context.SchoolType
                             select new
                             {
                                 sch.Id,
                                 sch.SchoolTypeName,
                             };
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericResponseModel> getSchoolTypeByIdAsync(long schoolTypeId)
        {
            try
            {
                var result = from sch in _context.SchoolType
                             where sch.Id == schoolTypeId
                             select new
                             {
                                 sch.Id,
                                 sch.SchoolTypeName,
                             };
                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };

                }
                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No CourseCategory with the specified ID", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
    }
}
