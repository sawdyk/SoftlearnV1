using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class ChildrenProfileRequestModel
    {
        public IList<Guid> ChildrenId { get; set; }
        public Guid ParentId { get; set; }
    }
}
