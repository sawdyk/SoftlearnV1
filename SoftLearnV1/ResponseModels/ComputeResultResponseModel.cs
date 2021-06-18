using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ComputeResultResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object ComputedResultPerSubjects { get; set; }
        public object CumulativeComputedResult { get; set; }
    }
}
