using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DAL
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Quiz")]
        public int QuizId { get; set; }
        public string Text { get; set; }
        public string CorrectAnswer { get; set; }
        public string WrongAnswerA { get; set; }
        public string WrongAnswerB { get; set; }
        public string WrongAnswerC { get; set; }
        public Quiz Quiz { get; set; }
    }
}
