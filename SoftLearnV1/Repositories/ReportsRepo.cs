using SoftLearnV1.Entities;
using SoftLearnV1.Helpers;
using SoftLearnV1.InterfaceRepositories;
using SoftLearnV1.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Repositories
{
    public class ReportsRepo : IReportsRepo
    {
        private readonly AppDbContext _context;

        public ReportsRepo(AppDbContext context)
        {
            _context = context;
         
        }

        public async Task<GenericResponseModel> entityReportsAsync()
        {
            try
            {
                //Count the numbers of each entities
                var noOfCourses = (from crs in _context.Courses select crs).Count();
                var noOfFacilitators = (from fac in _context.Facilitators select fac).Count();
                var noOfLearners = (from lrn in _context.Learners select lrn).Count();

                //Return the values as an object
                var dataResponse = new EntityReportResponseModel();
                dataResponse.No_Of_Courses = noOfCourses.ToString();
                dataResponse.No_Of_Facilitators = noOfFacilitators.ToString();
                dataResponse.No_Of_Learners = noOfLearners.ToString();

                return new GenericResponseModel { StatusCode = 200, StatusMessage = "Successful", Data = dataResponse };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new GenericResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<MonthReportResponseModel> numberOfFacilitatorPerYearMonthAsync(int year)
        {
            try
            {
                //the list of the months
                var monthsData = Months.monthsList();

                //list of months to be returned
                IList<Months> monthsList = new List<Months>();

                foreach (var monthsDatas in monthsData)
                {
                    //Count the numbers of each entities
                    var noOfFacilitators = (from fac in _context.Facilitators where fac.DateCreated.Year == year && fac.DateCreated.Month == monthsDatas.Id select fac).Count();

                    //assigns a new value to the "Value" field in the monthsData class
                    monthsDatas.TotalNumber = noOfFacilitators;
                    monthsList.Add(monthsDatas);
                }

                return new MonthReportResponseModel { StatusCode = 200, StatusMessage = "Successful", ReportYear = year.ToString(), Data = monthsList };

            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new MonthReportResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<MonthReportResponseModel> numberOfLearnersPerYearMonthAsync(int year)
        {
            try
            {
                //the list of the months
                var monthsData = Months.monthsList();

                //list of months to be returned
                IList<Months> monthsList = new List<Months>();

                foreach (var monthsDatas in monthsData)
                {
                    //Count the numbers of each entities
                    var noOfLearners = (from lrn in _context.Learners where lrn.DateCreated.Year == year && lrn.DateCreated.Month == monthsDatas.Id select lrn).Count();

                    //assigns a new value to the "Value" field in the monthsData class
                    monthsDatas.TotalNumber = noOfLearners;
                    monthsList.Add(monthsDatas);
                }

                return new MonthReportResponseModel { StatusCode = 200, StatusMessage = "Successful", ReportYear = year.ToString(), Data = monthsList };
              
            }
            catch (Exception exMessage)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = exMessage.Message,
                    ErrorSource = exMessage.Source,
                    ErrorStackTrace = exMessage.StackTrace,
                    ErrorDate = DateTime.Now
                };
                await _context.ErrorLog.AddAsync(error);
                await _context.SaveChangesAsync();
                return new MonthReportResponseModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }
    }
}
