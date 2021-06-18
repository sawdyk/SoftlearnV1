using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class TrendReportByClassResponseModel
    {
        public long classId { get; set; }
        public string className { get; set; }
        public decimal averageScore { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }
}
