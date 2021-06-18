using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class BulkTransferResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public IList<BulkTransferData> data { get; set; }
    }

    public class BulkTransferData
    {
        public string recipient { get; set; }
        public long amount { get; set; }
        public string transfer_code { get; set; }
        public string currency { get; set; }
    }
}
