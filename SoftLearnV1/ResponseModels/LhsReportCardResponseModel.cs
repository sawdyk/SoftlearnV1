using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LhsReportCardResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public StudentData StudentData { get; set; }
    }

    public class StudentData
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Session { get; set; }
        public string Term { get; set; }
        public string NoInClass { get; set; }
        public string Position { get; set; }
        public IList<CognitiveAbility> CognitiveAbility { get; set; }
    }


    public class CognitiveAbility
    {
        public string Subject { get; set; }
        public decimal CW { get; set; }
        public decimal HW { get; set; }
        public decimal Exam { get; set; }
        public decimal Total { get; set; }
        public string Grade { get; set; }
        public string Comment { get; set; }

    }
}
