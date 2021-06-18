using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class CertificateResponseModel
    {
        public string LernerFullName { get; set; }
        public string CourseName { get; set; }
        public DateTime CompletionDate { get; set; }
        public string FacilitatorFullName { get; set; }
        public string CertificateNumber { get; set; }
    }
}
