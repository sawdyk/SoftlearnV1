using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class EmailConfirmationCodes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public DateTime DateGenerated { get; set; }
        
    }
}
