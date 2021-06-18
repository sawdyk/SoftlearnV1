using Newtonsoft.Json;
using SoftLearnV1.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class TeacherCreateResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
        public object TeacherDetails { get; set; }
        public object SchoolDetails { get; set; }
        public IEnumerable<object> TeacherRoles { get; set; } //A list of Teacher Roles (Class or Subject Teacher)
    }
}
