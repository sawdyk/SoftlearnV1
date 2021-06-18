using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class MonthReportResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string ReportYear { get; set; }
        public IList<Months> Data { get; set; }
    }

    public class Months
    {
        public long Id { get; set; }
        public string MonthName { get; set; }
        public long TotalNumber { get; set; }

        //List of all the months in the year and default values
        public static IList<Months> monthsList()
        {

            var mnthList = new List<Months>
                {
                    new Months
                    {
                        Id = 1,
                        MonthName = "January",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 2,
                        MonthName = "February",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 3,
                        MonthName = "March",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 4,
                        MonthName = "April",
                        TotalNumber = 0,
                    },
                   new Months
                    {
                        Id = 5,
                        MonthName = "May",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 6,
                        MonthName = "June",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 7,
                        MonthName = "July",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 8,
                        MonthName = "August",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 9,
                        MonthName = "September",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 10,
                        MonthName = "October",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 11,
                        MonthName = "November",
                        TotalNumber=0,
                    },
                   new Months
                    {
                        Id = 12,
                        MonthName = "December",
                        TotalNumber=0,
                    },
                };
            return mnthList;
        }
    }
}
