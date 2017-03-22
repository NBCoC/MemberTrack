using MemberTrack.Common.Quiz.Exceptions;
using MemberTrack.Common.Quiz.Tests.Util;
using MemberTrack.Common.Quiz.Util;
using MemberTrack.Data.Entities;
using MemberTrack.Data.Entities.Quizzes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Common.Quiz.Tests
{
    [TestClass]
    public class QuizManagerTests
    {

        [TestMethod]
        public void ValidateAnswers_SimpleSuccess()
        {
            var quizManager = new QuizManager();

            var quiz = new MemberTrack.Data.Entities.Quizzes.Quiz
            {
                Id = 1,
                Name = "What Star Wars character are you?",
            };

            var question = new Question { Id = 10, QuizId = quiz.Id, Quiz = quiz };
            var question2 = new Question { Id = 11, QuizId = quiz.Id, Quiz = quiz };

            var allAnswers = new List<Answer>
            {
                new Answer
                {
                    Id = 100,
                    QuestionId = question.Id,
                    Question = question
                },
                new Answer
                {
                    Id = 101,
                    QuestionId = question2.Id,
                    Question = question2
                },
                new Answer
                {
                    Id = 102,
                    QuestionId = question.Id,
                    Question = question
                },
            };

            var answerIds = new List<long> { 100, 102 };

            quizManager.ValidateAnswers(quiz, allAnswers, answerIds);
        }


        [TestMethod]
        [ExpectedException(typeof(QuizAnswersNotSameQuestionException))]
        public void ValidateAnswers_FailsWhenAnswersNotSameQuestion()
        {
            var quizManager = new QuizManager();

            var quiz = new MemberTrack.Data.Entities.Quizzes.Quiz
            {
                Id = 1,
                Name = "What Star Wars character are you?",
            };

            var question = new Question { Id = 10, QuizId = quiz.Id, Quiz = quiz };
            var question2 = new Question { Id = 11, QuizId = quiz.Id, Quiz = quiz };

            var allAnswers = new List<Answer>
            {
                new Answer
                {
                    Id = 100,
                    QuestionId = question.Id,
                    Question = question
                },
                new Answer
                {
                    Id = 101,
                    QuestionId = question2.Id,
                    Question = question2
                },
                new Answer
                {
                    Id = 102,
                    QuestionId = question.Id,
                    Question = question
                },
            };

            var answerIds = new List<long> { 100, 101 };

            quizManager.ValidateAnswers(quiz, allAnswers, answerIds);
        }

        [TestMethod]
        [ExpectedException(typeof(QuizAnswersNotForQuizException))]
        public void ValidateAnswers_FailsWhenAnswersNotForQuiz()
        {
            var quizManager = new QuizManager();

            var quiz = new MemberTrack.Data.Entities.Quizzes.Quiz { Id = 1, };
            var quiz2 = new MemberTrack.Data.Entities.Quizzes.Quiz { Id = 2, };

            var question = new Question { Id = 10, QuizId = quiz.Id, Quiz = quiz };
            var question2 = new Question { Id = 11, QuizId = quiz.Id, Quiz = quiz };

            var allAnswers = new List<Answer>
            {
                new Answer
                {
                    Id = 100,
                    QuestionId = question.Id,
                    Question = question
                },
                new Answer
                {
                    Id = 101,
                    QuestionId = question2.Id,
                    Question = question2
                },
                new Answer
                {
                    Id = 102,
                    QuestionId = question.Id,
                    Question = question
                },
            };

            var answerIds = new List<long> { 100, 102 };

            quizManager.ValidateAnswers(quiz2, allAnswers, answerIds);
        }

        [TestMethod]
        public void NextQuestion_RandomizeQuestions()
        {
            var randomizer = new FakeReverseOrderRandomizer(100);

            var quizManager = new QuizManager(randomizer);

            var quiz = new MemberTrack.Data.Entities.Quizzes.Quiz
            {
                Id = 10,                
                RandomizeQuestions = true,
            };

            var question1 = new Question
            {
                Id = 1,
                QuizId = quiz.Id,
            };
            var question2 = new Question
            {
                Id = 2,
                QuizId = quiz.Id,
            };
            var question3 = new Question
            {
                Id = 3,
                QuizId = quiz.Id,
            };

            quiz.Questions = new List<Question> { question1, question2, question3 };

            var person = new Person
            {
                Id = 100
            };

            var personAnswers = new List<PersonAnswer>();

            var choosenQuestion = quizManager.NextQuestion(quiz, person, quiz.Questions, personAnswers);
            Assert.AreSame(question3, choosenQuestion);
        }

        [TestMethod]
        public void NextQuestion_RandomizeAnswers()
        {
            var randomizer = new FakeReverseOrderRandomizer(100);

            var quizManager = new QuizManager(randomizer);

            var answer1 = new Answer
            {
                Id = 1000,
            };

            var answer2 = new Answer
            {
                Id = 1001,
            };

            var answer3 = new Answer
            {
                Id = 1002,
            };

            var quiz = new MemberTrack.Data.Entities.Quizzes.Quiz
            {
                Id = 10,
                RandomizeQuestions = false,
            };

            var question1 = new Question
            {
                Id = 1,
                QuizId = quiz.Id,
                RandomizeAnswers = true,
                Answers = new List<Answer> {  answer1, answer2, answer3 }, 
            };            

            var question2 = new Question
            {
                Id = 2,
                QuizId = quiz.Id,
            };
            var question3 = new Question
            {
                Id = 3,
                QuizId = quiz.Id,
            };

            quiz.Questions = new List<Question> { question1, question2, question3 };

            var person = new Person
            {
                Id = 100
            };

            var personAnswers = new List<PersonAnswer>();

            var choosenAnswer = quizManager.NextQuestion(quiz, person, quiz.Questions, personAnswers);
            Assert.AreSame(question1, choosenAnswer);
            Assert.AreEqual(question1.Answers.Count, choosenAnswer.Answers.Count);

            //The answers should be in reverse order.
            Assert.AreSame(answer3, question1.Answers.First());
            Assert.AreSame(answer2, question1.Answers.Skip(1).First());
            Assert.AreSame(answer1, question1.Answers.Skip(2).First());
        }


        [TestMethod]
        public void InterpolateAnswerNames_PositionSuccess()
        {
            var answer = new Answer
            {
                Id = 1000,
                Name = "{Position}."
            };

            var quizManager = new QuizManager();
            var interpolated = quizManager.InterpolateAnswerName(answer, 3);

            Assert.AreEqual("4.", interpolated);
        }

        [TestMethod]
        public void InterpolateAnswerNames_IndexSuccess()
        {
            var answer = new Answer
            {
                Id = 1000,
                Name = "Prefix{Index}."
            };

            var quizManager = new QuizManager();
            var interpolated = quizManager.InterpolateAnswerName(answer, 3);

            Assert.AreEqual("Prefix3.", interpolated);
        }

        [TestMethod]
        public void InterpolateAnswerNames_LetterPositionSuccess()
        {
            var answer = new Answer
            {
                Id = 1000,
                Name = "{LetterPosition}."
            };

            var quizManager = new QuizManager();
            var interpolated = quizManager.InterpolateAnswerName(answer, 3);

            Assert.AreEqual("D.", interpolated);
        }

        [TestMethod]
        public void InterpolateAnswerNames_TopicWeightSuccess()
        {
            var answer = new Answer
            {
                Id = 1000,
                Name = "{TopicWeight}.",
                TopicWeight = 99,
            };

            var quizManager = new QuizManager();
            var interpolated = quizManager.InterpolateAnswerName(answer, 3);

            Assert.AreEqual("99.", interpolated);
        }
        
    }
}
