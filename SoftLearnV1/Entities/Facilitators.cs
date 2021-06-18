using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class Facilitators
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string Salt { get; set; }
        public string PasswordHash { get; set; }
        public long FacilitatorTypeId { get; set; }

        public string InstitutionAttended { get; set; }
        public string CourseOfStudy { get; set; }
        public string CertificateObtained { get; set; }
        public string Profession { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }

        public bool IsActive { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<Courses> Courses { get; set; }

        [ForeignKey("FacilitatorTypeId")]
        public virtual FacilitatorType FacilitatorType { get; set; }
    }
}
