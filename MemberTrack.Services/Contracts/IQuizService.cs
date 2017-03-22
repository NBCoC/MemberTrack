using MemberTrack.Data.Entities.Quizzes;
using MemberTrack.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MemberTrack.Services.Contracts
{
    public interface IQuizService
    {
        Task<QuizDto> Find(Expression<Func<Quiz, bool>> predicate);

        Task<SearchResultDto<QuizTerseDto>> GetAll();

        Task<QuizAnswerResponseDto> AnswerQuestion(long quizId, long personId, QuizQuestionAnswerDto questionAnswerDto);

        Task<QuizNextQuestionDto> NextQuestion(long quizId, long personId, QuizQuestionAnswerDto answersToPreviousQuestion);

    }
}
