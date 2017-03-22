using MemberTrack.Data.Entities;
using MemberTrack.Data.Entities.Quizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz
{
    public interface IQuizManager
    {
        string QuestionResponse(Person person, IEnumerable<Answer> answers);

        IEnumerable<Answer> ValidateAnswers(Data.Entities.Quizzes.Quiz quiz, IEnumerable<Answer> allAnswers, IEnumerable<long> answerIds);

        Question NextQuestion(Data.Entities.Quizzes.Quiz quiz, Person person, IEnumerable<Question> allQuestions, IEnumerable<PersonAnswer> allPersonAnswers);

        string InterpolateAnswerName(Answer answer, int index);
    }
}
