using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.DAL;
using backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly QuizContext _context;
        private readonly IMapper _mapper;

        public QuizzesController(QuizContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [Authorize]
        [HttpGet]
        public ActionResult<IEnumerable<QuizDto>> GetQuizzes()
        {
            return _context.Quizzes
                .Where(x=>x.OwnerId == HttpContext.User.Claims.First().Value)
                .Select(_mapper.Map<QuizDto>).ToList();
        }

        [HttpGet("all")]
        public ActionResult<IEnumerable<QuizDto>> GetAllQuizzes()
        {
            return _context.Quizzes.Select(_mapper.Map<QuizDto>).ToList();
        }

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizDto>> GetQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);

            if (quiz == null)
            {
                return NotFound();
            }

            return _mapper.Map<QuizDto>(quiz);
        }

        // PUT: api/Quizzes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id, QuizDto quizDto)
        {
            if (id != quizDto.Id)
            {
                return BadRequest();
            }

            var quiz = await _context.Quizzes.SingleOrDefaultAsync(x => x.Id == id);

            if (quiz.OwnerId != HttpContext.User.Claims.First().Value)
            {
                return BadRequest();
            }

            _mapper.Map(quizDto, quiz);

            _context.Entry(quiz).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuizExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Quizzes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(QuizDto quizDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var quiz = _mapper.Map<Quiz>(quizDto);

            quiz.OwnerId = HttpContext.User.Claims.First().Value;

            await _context.Quizzes.AddAsync(quiz);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuiz", new { id = quiz.Id }, _mapper.Map<QuizDto>(quiz));
        }

        // DELETE: api/Quizzes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null)
            {
                return NotFound();
            }

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool QuizExists(int id)
        {
            return _context.Quizzes.Any(e => e.Id == id);
        }
    }
}
