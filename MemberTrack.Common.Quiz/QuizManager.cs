using MemberTrack.Common.Quiz.Exceptions;
using MemberTrack.Common.Quiz.Util;
using MemberTrack.Data.Entities;
using MemberTrack.Data.Entities.Quizzes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz
{
    //One goal of this library is to only deal at the Entities level in order to create unit tests that don't need a DatabaseContext.  This 
    //doesn't avoid a dependency, since the Entities and the DatabaseContext objects are in the same library, but abstracts the quiz logic
    //out from a database context.

    public class QuizManager : IQuizManager
    {
        private readonly IRandomizer<Guid> _randomizer;

        public QuizManager()
            :this(new Randomizer())
        {
        }

        public QuizManager(IRandomizer<Guid> randomizer)
        {
            _randomizer = randomizer;
        }

        public string QuestionResponse(Person person, IEnumerable<Answer> answers)
        {
            //TODO:  For typical quizzes the responses are something like:
            //          "You got it right!" 
            //          "Wrong.  The correct answer is <blah>"
            //          "The isn't the best answer, you get X points though."

            return string.Empty;
        }

        //Make sure that all answers pertain to the intended quiz and are all for the same question.
        public IEnumerable<Answer> ValidateAnswers(Data.Entities.Quizzes.Quiz quiz, IEnumerable<Answer> allAnswers, IEnumerable<long> answerIds)
        {
            var setAnswerIds = new HashSet<long>(answerIds);
            var answers = allAnswers.Where(a => setAnswerIds.Contains(a.Id)).ToList();

            long questionId = -1;
            foreach (var answer in answers)
            {
                if (questionId == -1)
                {
                    questionId = answer.QuestionId;
                    if (answer.Question.QuizId != quiz.Id)
                        throw new QuizAnswersNotForQuizException(answer.Id, quiz.Id, answer.Question.QuizId);
                }
                else if (questionId != answer.QuestionId)
                {
                    throw new QuizAnswersNotSameQuestionException(answer.Id, questionId, answer.QuestionId);
                }
            }

            return answers;
        }

        public Question NextQuestion(Data.Entities.Quizzes.Quiz quiz, Person person, IEnumerable<Question> allQuestions, IEnumerable<PersonAnswer> allPersonAnswers)
        {
            var questions = allQuestions.Where(q => q.QuizId == quiz.Id);
            var questionIdsOfPersonAnswers = new HashSet<long>(allPersonAnswers.Select(a => a.Answer.QuestionId));
            
            //We only are concerned with unanswered questions.
            var availableQuestions = questions.Where(q => !questionIdsOfPersonAnswers.Contains(q.Id));

            if (quiz.RandomizeQuestions)
            {
                availableQuestions = availableQuestions.Shuffle(_randomizer);
            }

            var question = availableQuestions.FirstOrDefault();
            if (question != null && question.RandomizeAnswers)
            {
                question.Answers = question.Answers.Shuffle(_randomizer).ToList();
            }

            return question;
        }

        public string InterpolateAnswerName(Answer answer, int index)
        {
            string result = answer.Name.Replace("{Position}", (index + 1).ToString());
            result = result.Replace("{Index}", index.ToString());
            result = result.Replace("{LetterPosition}", ((char)('A' + index)).ToString());
            result = result.Replace("{TopicWeight}", answer.TopicWeight.ToString());

            return result;            
        }
    }
}
