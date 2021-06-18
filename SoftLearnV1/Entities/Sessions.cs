using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class Sessions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string SessionName { get; set; }
        public long SchoolId { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("SchoolId")]
        public virtual SchoolInformation SchoolInformation { get; set; }

    }
}
