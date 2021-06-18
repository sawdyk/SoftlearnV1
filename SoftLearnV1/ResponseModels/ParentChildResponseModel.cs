using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class ParentChildResponseModel
    {
        public long StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public object ParentDetails { get; set; }
        public object childDetails { get; set; }

    }
}
