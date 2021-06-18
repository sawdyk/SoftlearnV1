using Microsoft.AspNetCore.Http;
using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.RequestModels;
using SoftLearnV1.ResponseModels;
using SoftLearnV1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class CourseTypeRepo : ICourseTypeRepo
    {
        private readonly AppDbContext _context;
        public CourseTypeRepo(AppDbContext context)
        {
            _context = context;

        }
        public async Task<GenericResponseModel> getAllCourseTypeAsync()
        {
            try
            {
                var result = from cl in _context.CourseType select cl;

                if (result.Count() == 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

                }

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList<object>(), };
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

        public async Task<GenericResponseModel> getCourseTypeByIdAsync(long courseTypeId)
        {
            try
            {
                var result = from cl in _context.CourseType where cl.Id == courseTypeId select cl;

                if (result.Count() > 0)
                {
                    return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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
    }
}
