using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseQuizQuestions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CourseQuizId { get; set; }
        public long QuestionTypeId { get; set; }
        public string Question { get; set; }
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Answer { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastUpdated { get; set; }

        [ForeignKey("CourseQuizId")]
        public virtual CourseQuiz CourseQuiz { get; set; }

        [ForeignKey("QuestionTypeId")]
        public virtual QuestionTypes QuestionTypes { get; set; }
    }
}
