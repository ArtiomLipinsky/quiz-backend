using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DAL
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
