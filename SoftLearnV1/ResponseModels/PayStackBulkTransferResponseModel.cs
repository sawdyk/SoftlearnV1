using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class PayStackBulkTransferResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object Data { get; set; }
        public object PayStackData { get; set; }
    }
}
