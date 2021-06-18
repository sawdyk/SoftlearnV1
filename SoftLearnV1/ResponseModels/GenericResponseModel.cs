using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class GenericResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
        public object Data { get; set; }
    }
}
