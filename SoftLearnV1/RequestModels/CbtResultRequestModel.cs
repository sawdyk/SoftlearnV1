using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.RequestModels
{
    public class CbtResultRequestModel
    {
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public long CbtId { get; set; }
    }

    public class QuestionAndAnswerList
    {
        [Required]
        public long QuetionId { get; set; }
        [Required]
        public string Answer { get; set; }
    }

    //Inherits from the Parent Class
    public class CbtResultCreationRequestModel : CbtResultRequestModel
    {
        [Required]
        public IList<QuestionAndAnswerList> QuestionAndAnswer { get; set; }
    }
}
