using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class UpdateFacilitatorProfileRequestModel
    {
        [Required]
        public Guid FacilitatorId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string InstitutionAttended { get; set; }
        public string CourseOfStudy { get; set; }
        public string CertificateObtained { get; set; }
        public string Profession { get; set; }
        public string Bio { get; set; }
    }
}
