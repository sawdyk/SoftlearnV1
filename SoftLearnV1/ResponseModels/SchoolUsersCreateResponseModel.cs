using Newtonsoft.Json;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SchoolUsersCreateResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object SchoolUserDetails { get; set; }
        public object SchoolDetails { get; set; }
        public IEnumerable<object> Roles { get; set; } //A list of Roles 
    }
}
