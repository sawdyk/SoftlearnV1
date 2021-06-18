using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SoftLearnV1.Entities
{
    public class CourseQuizResults
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long CourseQuizId { get; set; }
        public Guid LearnerId { get; set; }
        public long NoOfQuestions { get; set; }
        public long RightAnswers { get; set; }
        public long WrongAnswers { get; set; }
        public long InvalidQuestions { get; set; }
        public long Score { get; set; }
        public decimal PercentageScore { get; set; }
        public string Status { get; set; } //PASS or FAIL
        public DateTime DateTaken { get; set; } 

        [ForeignKey("CourseQuizId")]
        public virtual CourseQuiz CourseQuiz { get; set; }

        [ForeignKey("LearnerId")]
        public virtual Learners Learners { get; set; }
    }
}
