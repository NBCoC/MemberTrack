using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class QuizController : BaseController
    {
        private readonly IQuizService _quizService;
        private readonly ISystemPersonService _personService;

        public QuizController(ILoggerFactory loggerFactory, IQuizService quizService, ISystemPersonService personService)
            : base(loggerFactory.CreateLogger<UserController>())
        {
            _quizService = quizService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var data = await _quizService.Find(x => x.Id == id);

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var data = await _quizService.GetAll();

                return Ok(data);
            }
            catch (Exception e)
            {
                return Exception(e);
            }
        }

        [HttpPost("NextQuestion/{quizId}")]
        public async Task<IActionResult> NextQuestion(long quizId, [FromBody] PersonInsertOrUpdateDto personDto, [FromBody] QuizQuestionAnswerDto answersToPreviousQuestion)
        {
            var trans = await _personService.BeginTransactionAsync();

            try
            {
                //Need to determine if this person already exists in the database, and if not add him.
                var personId = await _personService.SystemEnsurePersonExists(personDto);

                var answerResponse = await _quizService.AnswerQuestion(quizId, personId, answersToPreviousQuestion);

                trans.Commit();

                var data = await _quizService.NextQuestion(quizId, personId, answersToPreviousQuestion);

                return Ok(data);
            }
            catch (Exception e)
            {
                trans.Rollback();
                return Exception(e);
            }
        }

    }
}
