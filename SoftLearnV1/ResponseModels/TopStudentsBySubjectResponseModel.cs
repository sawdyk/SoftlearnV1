using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.ResponseModels
{
    public class TopStudentsBySubjectResponseModel
    {
        public Guid studentId { get; set; }
        public string studentName { get; set; }
        public string AdmissionNumber { get; set; }
        public decimal studentScore { get; set; }
        public long subjectId { get; set; }
        public string subjectName { get; set; }
    }
}
