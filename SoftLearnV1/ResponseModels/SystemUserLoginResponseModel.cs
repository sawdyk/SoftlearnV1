using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class SystemUserLoginResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Token { get; set; }
        public object UserDetails { get; set; }
        public IEnumerable<object> Roles { get; set; } //A list of Roles
    }
}
