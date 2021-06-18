using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CartResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object CartItems { get; set; }
        public object Data { get; set; }

    }
}
