using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace backend.Models
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public  int QuizId { get; set; }
        public string Text { get; set; }

        public string CorrectAnswer { get; set; }

        public string WrongAnswerA { get; set; }

        public string WrongAnswerB { get; set; }

        public string WrongAnswerC { get; set; }
    }
}
