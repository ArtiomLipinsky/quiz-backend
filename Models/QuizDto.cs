using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

namespace backend.Models
{
    public class QuizDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<QuestionDto> Questions { get; set; }
    }
}
