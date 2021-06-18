using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SchoolBasicInfoLoginResponseModel
    {
        public long SchoolId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
        public string SchoolTypeName { get; set; }
        public string SchoolLogoUrl { get; set; }
        public long CampusId { get; set; }
        public string CampusName { get; set; }
        public string CampusAddress { get; set; }


    }
}
