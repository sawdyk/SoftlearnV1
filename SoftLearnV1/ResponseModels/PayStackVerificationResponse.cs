using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class PayStackVerificationResponse
    {
        public long Status { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Data
    {
        public long Id { get; set; }
        public string Domain { get; set; }
        public string Status { get; set; }
        public string reference { get; set; }
        public string Amount { get; set; }
        public string Message { get; set; }
        public string Gateway_Response { get; set; }
        public string Paid_At { get; set; }
        public string Created_At { get; set; }
        public string Channel { get; set; }
        public string Currency { get; set; }
        public string Ip_Address { get; set; }
        public object Metadata { get; set; }
        public object Log { get; set; }
        public IList<object> History { get; set; }
        public string Fees { get; set; }
        public string Fees_Split { get; set; }
        public object Authorization { get; set; }
        public object Customer { get; set; }
        public string Plan { get; set; }
        public object Split { get; set; }
        public string Order_Id { get; set; }
        public string PaidAt { get; set; }
        public string CreatedAt { get; set; }
        public string Requested_Amount { get; set; }
        public string Transaction_Date { get; set; }
        public object Plan_Object { get; set; }
        public object Subaccount { get; set; }
    }
}
