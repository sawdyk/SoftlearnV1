using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class SingleTransferResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public SingleTransferResponseData data { get; set; }

    }

    public class SingleTransferResponseData
    {
        public string reference { get; set; }
        public string integration { get; set; }
        public string domain { get; set; }
        public long amount { get; set; }
        public string currency { get; set; }
        public string source { get; set; }
        public string reason { get; set; }
        public long recipient { get; set; }
        public string status { get; set; }
        public string transfer_code { get; set; }
        public long id { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }



}
