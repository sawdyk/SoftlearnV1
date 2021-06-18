using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class FeeTemplateListRequestModel
    {
        public long SchoolId { get; set; }
        public long CampusId { get; set; }
        public long TemplateId { get; set; }
        public IList<long> FeeSubCategoryIdList { get; set; }
    }
}
