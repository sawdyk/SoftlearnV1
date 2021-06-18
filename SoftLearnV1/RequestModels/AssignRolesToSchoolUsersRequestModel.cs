using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class AssignRolesToSchoolUsersRequestModel
    {
        [Required]
        public Guid SchoolUserId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public IEnumerable<RoleIds> RoleIds { get; set; } //A list of Roles
    }

    public class DeleteRolesAssignedToSchoolUsersRequestModel
    {
        [Required]
        public Guid SchoolUserId { get; set; }
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        public long RoleId { get; set; }
    }
}
