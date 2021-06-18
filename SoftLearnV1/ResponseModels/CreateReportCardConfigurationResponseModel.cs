using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class CreateReportCardConfigurationResponseModel
    {
        public object ConfigurationDetail { get; set; }
        public IList<long> SuccessfulClassId { get; set; }
    }
}
