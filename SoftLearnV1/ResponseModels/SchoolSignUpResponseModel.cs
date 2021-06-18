using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SchoolSignUpResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
        public object AdministratorDetails { get; set; }
        public object SchoolDetails { get; set; }

    }
}
