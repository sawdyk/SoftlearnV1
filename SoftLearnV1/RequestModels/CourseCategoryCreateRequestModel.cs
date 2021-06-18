using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CourseCategoryCreateRequestModel
    {
       
        [Required]
        public string CourseCategoryName { get; set; }
        public string CourseCategoryImage { get; set; }
        public string CategoryDescription { get; set; }

    }
}
