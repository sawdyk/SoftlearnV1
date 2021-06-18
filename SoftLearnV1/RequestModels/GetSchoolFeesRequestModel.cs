using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class GetSchoolFeesRequestModel
    {
        public long TermId { get; set; }
        public long SessionId { get; set; }
        public Guid ChildId { get; set; }
        public Guid ParentId { get; set; }
    }
}
