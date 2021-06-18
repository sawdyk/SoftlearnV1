using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class VerifyAccountNoResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public AccountData data { get; set; }
    }

    public class AccountData
    {
        public string account_number { get; set; }
        public string account_name { get; set; }
        public long bank_id { get; set; }
    }
}
