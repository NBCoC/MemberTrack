using MemberTrack.Common.Contracts;
using MemberTrack.Common.Quiz;
using MemberTrack.Data;
using MemberTrack.Data.Entities.Quizzes;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using MemberTrack.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MemberTrack.Services
{
    public class QuizService : IQuizService
    {
        private readonly DatabaseContext _context;
        private readonly IQuizManager _quizManager;

        public QuizService(DatabaseContext context, IQuizManager quizManager)
        {
            _context = context;
            _quizManager = quizManager;
        }

        public async Task<QuizDto> Find(Expression<Func<Quiz, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            var entity = await _context.Quizzes.FirstOrDefaultAsync(predicate);

            if (entity == null)
            {
                throw new EntityNotFoundException();
            }

            return entity.ToDto();
        }

        public async Task<SearchResultDto<QuizTerseDto>> GetAll()
        {
            var query = await _context.Quizzes.ToListAsync();
            var count = query.Count();

            return new SearchResultDto<QuizTerseDto>(query.ToTerseDtos(), count);
        }


        public async Task<QuizAnswerResponseDto> AnswerQuestion(long quizId, long personId, QuizQuestionAnswerDto questionAnswerDto)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId);
            if (quiz == null)
            {
                throw new QuizNotFoundException(quizId);
            }

            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
            {
                throw new PersonNotFoundException(quizId);
            }

            var allAnswers = await _context.QuizAnswers.ToListAsync();

            var answers = _quizManager.ValidateAnswers(quiz, allAnswers, questionAnswerDto.AnswerIds);
            _context.PersonAnswers.AddRange(answers.Select(a => new PersonAnswer() { AnswerId = a.Id, PersonId = person.Id }));

            await _context.SaveChangesAsync();

            var answerResponse = _quizManager.QuestionResponse(person, answers);
            return new QuizAnswerResponseDto
            {
                PersonId = person.Id,
                AnswerResponseText = answerResponse,
            };
        }

        public async Task<QuizNextQuestionDto> NextQuestion(long quizId, long personId, QuizQuestionAnswerDto answersToPreviousQuestion)
        {
            var quiz = await _context.Quizzes.FirstOrDefaultAsync(q => q.Id == quizId);
            if (quiz == null)
            {
                throw new QuizNotFoundException(quizId);
            }

            var person = await _context.People.FirstOrDefaultAsync(p => p.Id == personId);
            if (person == null)
            {
                throw new PersonNotFoundException(quizId);
            }

            var answerIds = new HashSet<long>(answersToPreviousQuestion.AnswerIds);
            var answers = _context.QuizAnswers.Where(a => answerIds.Contains(a.Id));

            //Record the person's answer(s)
            var responseString = _quizManager.QuestionResponse(person, answers);


            //Get the next question for the person
            var questions = await _context.QuizQuestions.ToListAsync();
            var personAnswers = await _context.PersonAnswers.ToListAsync();

            var question = _quizManager.NextQuestion(quiz, person, questions, personAnswers);
            var questionDto = question.ToDto(_quizManager);

            return new QuizNextQuestionDto { NextQuestion = questionDto, PreviousQuestionAnswerResponse = responseString };
        }
    }
}
