using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class TransferReceipentRequestModel
    {
        public string type => "nuban"; //by default
        public string name { get; set; }
        public string account_number { get; set; }
        public string bank_code { get; set; }
        public string currency => "NGN"; //by default
    }
}
