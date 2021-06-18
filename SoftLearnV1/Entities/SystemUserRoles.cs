using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class SystemUserRoles
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public long RoleId { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("UserId")]
        public virtual SystemUsers SystemUsers { get; set; }

        [ForeignKey("RoleId")]
        public virtual SystemRoles SystemRoles { get; set; }
    }
}
