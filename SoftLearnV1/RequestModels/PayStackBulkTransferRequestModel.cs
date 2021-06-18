using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class PayStackBulkTransferRequestModel
    {
        public string currency => "NGN"; //by default
        public string source => "balance"; //by default
        public IList<Transfers> transfers { get; set; }
    }


    public class PayStackSingleTransferRequestModel : Transfers
    {
        public string source => "balance"; //by default
    }

    public class Transfers
    {
        public long amount { get; set; }
        public string reason => "Course Payments"; //by default
        public string recipient { get; set; }
    }
}
