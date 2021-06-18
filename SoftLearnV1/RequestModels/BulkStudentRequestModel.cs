using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaxApplication.Helper;

namespace SoftLearnV1.RequestModels
{
    public class BulkStudentRequestModel
    {
        [Required]
        public long SchoolId { get; set; }
        [Required]
        public long CampusId { get; set; }
        [Required]
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
