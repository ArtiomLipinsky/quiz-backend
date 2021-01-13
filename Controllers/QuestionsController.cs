using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using backend.DAL;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly QuizContext context;
        private readonly IMapper mapper;

        public QuestionsController(QuizContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<QuestionDto>> Post([FromBody] QuestionDto value)
        {
            var question = mapper.Map<Question>(value);
            await context.Questions.AddAsync(question);
            await context.SaveChangesAsync();
            return Ok(question);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var a = context.Questions.ToArray();
            return Ok(context.Questions.Select(mapper.Map<QuestionDto>).AsEnumerable()) ;
        }

        [HttpGet("{quizId}")]
        public async Task<IActionResult> Get([FromRoute] int quizId)
        {
            if (context.Quizzes.Find(quizId) == null) return NotFound();
            var result = context.Questions
                .Where(x=>x.QuizId == quizId).AsEnumerable()
                .Select(mapper.Map<QuestionDto>);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] QuestionDto questionDto)
        {
            if (id != questionDto.Id) return BadRequest();

            if (await context.Quizzes.FindAsync(questionDto.QuizId) == null) return NotFound();

            var question = await context.Questions.SingleOrDefaultAsync(x => x.Id == id);

            mapper.Map(questionDto, question);

            context.Entry(question).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return Ok(mapper.Map<QuestionDto>(question));
        }
    }
}
