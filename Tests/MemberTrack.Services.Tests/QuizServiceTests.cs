using MemberTrack.Common.Quiz;
using MemberTrack.Common.Quiz.Exceptions;
using MemberTrack.Data;
using MemberTrack.Data.Entities.Quizzes;
using MemberTrack.Services.Contracts;
using MemberTrack.Services.Dtos;
using MemberTrack.Services.Exceptions;
using MemberTrack.Services.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemberTrack.Services.Tests
{
    [TestClass]
    public class QuizServiceTests
    {

        [TestMethod]
        public async Task Find_SimpleIdCase()
        {
            var databaseContext = QuizTestData.GetPopulatedDatabase();
            var quizManager = new QuizManager();

            //Get the ID of the second quiz
            var searchForId = databaseContext.Quizzes.Skip(1).First().Id;

            var quizService = new QuizService(databaseContext, quizManager);
            var foundQuiz = await quizService.Find(q => q.Id == searchForId);

            Assert.IsNotNull(foundQuiz);
            Assert.AreEqual(QuizTestData.PersonalityQuiz.Id, foundQuiz.Id);
            Assert.AreEqual(QuizTestData.PersonalityQuiz.Name, foundQuiz.Name);
            Assert.AreEqual(QuizTestData.PersonalityQuiz.Description, foundQuiz.Description);
            Assert.AreEqual(QuizTestData.PersonalityQuiz.Instructions, foundQuiz.Instructions);

        }

        [TestMethod]
        public async Task GetAll_SimpleCase()
        {
            //Assumption:  At this point, order of the quizzes isn't important.

            var databaseContext = QuizTestData.GetPopulatedDatabase();
            var quizManager = new Mock<IQuizManager>();

            var quizService = new QuizService(databaseContext, quizManager.Object);

            var quizzes = await quizService.GetAll();

            Assert.IsNotNull(quizzes);
            Assert.AreEqual(databaseContext.Quizzes.Count(), quizzes.Count);
            Assert.AreEqual(databaseContext.Quizzes.Count(), quizzes.TotalCount);
            HashSet<long> previousIds = new HashSet<long>();
            foreach (QuizTerseDto quiz in quizzes.Data)
            {
                Assert.IsTrue(quiz.Id == QuizTestData.WeightQuiz.Id || 
                              quiz.Id == QuizTestData.PersonalityQuiz.Id || 
                              quiz.Id == QuizTestData.DisneyPrincessQuiz.Id);

                //Check for duplicates.
                Assert.IsFalse(previousIds.Contains(quiz.Id));

                previousIds.Add(quiz.Id);
            }
        }

    }
}
