using MemberTrack.Data.Entities.Quizzes;
using MemberTrack.Data.RawData;
using MemberTrack.Data.Seedings;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace MemberTrack.Data.Tests
{
    [TestClass]
    public class SeedSpiritualGiftsDiscoveryQuizTests
    {
        private DatabaseContext GetSeededDatabase()
        {
            var dbContextBuilder = new DbContextOptionsBuilder<DatabaseContext>();

            //Provide an in-memory database who has a GUID for a name, to isolate each test.
            dbContextBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

            var databaseContext = new DatabaseContext(dbContextBuilder.Options);

            var seeding = new SeedSpiritualGiftsDiscoveryQuiz(databaseContext, false);
            seeding.Seed();

            return databaseContext;
        }

        [TestMethod]
        public void AllQuestions_HaveSameAnswers()
        {
            var databaseContext = GetSeededDatabase();

            //Now that the database is seeded, verify the answers.
            foreach (Question question in databaseContext.QuizQuestions)
            {
                var answers = question.Answers.ToList();

                Assert.AreEqual(SpiritualGiftsDiscoveryQuizData.AnswersText.Count, answers.Count);

                for (int counter = 0; counter < SpiritualGiftsDiscoveryQuizData.AnswersText.Count; counter++)
                {
                    Assert.AreEqual(SpiritualGiftsDiscoveryQuizData.AnswersText[counter], answers[counter].Description);
                }

            }

        }


        [TestMethod]
        public void Questions_Verify()
        {
            var databaseContext = GetSeededDatabase();

            //Make sure that we have the expected number of questions.
            Assert.AreEqual(SpiritualGiftsDiscoveryQuizData.QuizQuestionText.Count, databaseContext.QuizQuestions.Count());

            foreach (Question question in databaseContext.QuizQuestions)
            {
                Assert.IsTrue(!string.IsNullOrEmpty(question.Description));
                Assert.AreEqual(false, question.AllowMultipleAnswers);
                Assert.AreEqual(false, question.RandomizeAnswers);
            }

        }

        [TestMethod]
        public void Topics_ContainsAnswersForRightQuestions()
        {
            var databaseContext = GetSeededDatabase();

            var expectedAnswersPerTopic = SpiritualGiftsDiscoveryQuizData.QuizQuestionText.Count * SpiritualGiftsDiscoveryQuizData.AnswersText.Count / SpiritualGiftsDiscoveryQuizData.Topics.Count;

            var numberTopics = databaseContext.Topics.Count();
            Assert.AreEqual(SpiritualGiftsDiscoveryQuizData.Topics.Count, numberTopics);


            int topicPosition = 0;
            foreach (Topic topic in databaseContext.Topics)
            {
                Assert.AreEqual(expectedAnswersPerTopic, topic.Answers.Count);

                foreach (Answer answer in topic.Answers)
                {
                    var questions = databaseContext.QuizQuestions.ToList();
                    var questionPosition = questions.IndexOf(answer.Question);

                    Assert.AreEqual(topicPosition, questionPosition % numberTopics);
                }

                topicPosition++;
            }
        }
    }
}
