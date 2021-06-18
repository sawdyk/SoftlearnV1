using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class TotalPaymentByMethodResponseModel
    {
        public long TotalBank { get; set; }
        public long TotalOnline { get; set; }
        public long TotalCard { get; set; }
        public long TotalCash { get; set; }
    }
}
