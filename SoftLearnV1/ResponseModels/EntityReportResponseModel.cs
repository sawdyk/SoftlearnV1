using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EntityReportResponseModel
    {
        public string No_Of_Facilitators { get; set; }
        public string No_Of_Learners { get; set; }
        public string No_Of_Courses { get; set; }
    }
}
