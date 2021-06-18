using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class TransferRecipientResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public TransferRecipientData data { get; set; }
    }

    public class TransferRecipientData
    {
        public bool active { get; set; }
        public DateTime createdAt { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public string domain { get; set; }
        public string email { get; set; }
        public long id { get; set; }
        public long integration { get; set; }
        public  string metadata { get; set; }
        public string name { get; set; }
        public string recipient_code { get; set; }
        public string type { get; set; }
        public DateTime updatedAt { get; set; }
        public bool is_deleted { get; set; }
        public Details details { get; set; }
    }

    public class Details
    {
        public string authorization_code { get; set; }
        public string account_number { get; set; }
        public string account_name { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }

    }

}
